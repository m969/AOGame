using AO;
using System;

namespace ET.Server
{
    public abstract class AMRpcHandler<Request, Response> : IMHandler where Request : class, IRequest where Response : class, IResponse
    {
        protected abstract ETTask Run(Entity session, Request request, Response response);

        public void Handle(Entity session, object message)
        {
            HandleAsync(session, message).Coroutine();
        }

        public async ETTask HandleAsync(Entity session, object message)
        {
            try
            {
                Request request = message as Request;
                if (request == null)
                {
                    throw new Exception($"消息类型转换错误: {message.GetType().Name} to {typeof(Request).Name}");
                }

                int rpcId = request.RpcId;
                long instanceId = session.InstanceId;

                Response response = Activator.CreateInstance<Response>();

                try
                {
                    await this.Run(session, request, response);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                    response.Error = ErrorCore.ERR_RpcFail;
                    response.Message = exception.ToString();
                }

                // 等回调回来,session可以已经断开了,所以需要判断session InstanceId是否一样
                if (session.InstanceId != instanceId)
                {
                    return;
                }

                response.RpcId = rpcId; // 在这里设置rpcId是为了防止在Run中不小心修改rpcId字段
                //session.Send(response);
                EventSystem.Instance.Publish(Root.Instance.Scene, new AO.EventType.SessionEvent() { Session = session, Message = response });
                //Task.SetResult(response);
            }
            catch (Exception e)
            {
                throw new Exception($"解释消息失败: {message.GetType().FullName}", e);
            }
        }

        public Type GetMessageType()
        {
            return typeof(Request);
        }

        public Type GetResponseType()
        {
            return typeof(Response);
        }
    }
}