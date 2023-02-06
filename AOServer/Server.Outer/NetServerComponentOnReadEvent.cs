using System.Reflection;

namespace ET.Server
{
    [Event(SceneType.Process)]
    public class NetServerComponentOnReadEvent: AEvent<NetServerComponentOnRead>
    {
        protected override async ETTask Run(Scene scene, NetServerComponentOnRead args)
        {
            Session session = args.Session;
            object message = args.Message;

            if (message is IResponse response)
            {
                session.OnResponse(response);
                return;
            }

            var msgType = message.GetType();
            long actorId = 0;
            if (session.GetComponent<SessionPlayerComponent>() != null)
            {
                foreach (var item in session.GetComponent<SessionPlayerComponent>().MessageType2Actor)
                {
                    if (msgType.GetInterface(item.Key.FullName) != null)
                    {
                        actorId = item.Value;
                        break;
                    }
                }
            }

            // 根据消息接口判断是不是Actor消息，不同的接口做不同的处理,比如需要转发给Chat Scene，可以做一个IChatMessage接口
            switch (message)
            {
                case IActorLocationRequest actorLocationRequest: // gate session收到actor rpc消息，先向actor 发送rpc请求，再将请求结果返回客户端
                    {
                        //Log.Console($"NetServerComponentOnReadEvent actorId={actorId}");
                        int rpcId = actorLocationRequest.RpcId; // 这里要保存客户端的rpcId
                        long instanceId = session.InstanceId;
                        IResponse iResponse = await ActorLocationSenderComponent.Instance.Call(actorId, actorLocationRequest);
                        iResponse.RpcId = rpcId;
                        // session可能已经断开了，所以这里需要判断
                        if (session.InstanceId == instanceId)
                        {
                            session.Send(iResponse);
                        }
                        break;
                    }
                case IActorLocationMessage actorLocationMessage:
                    {
                        ActorLocationSenderComponent.Instance.Send(actorId, actorLocationMessage);
                        break;
                    }
                //case IActorRequest actorRequest:  // 分发IActorRequest消息，目前没有用到，需要的自己添加
                //{
                //    break;
                //}
                //case IActorMessage actorMessage:  // 分发IActorMessage消息，目前没有用到，需要的自己添加
                //{
                //    break;
                //}

                default:
                    {
                        Log.Console($"{message}");
                        // 非Actor消息
                        MessageDispatcherComponent.Instance.Handle(session, message);
                        break;
                    }
            }
        }
    }
}