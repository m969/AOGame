using System;

namespace ET
{
    public abstract class AMHandler<Message>: IMHandler where Message : class
    {
        //public ETTask<IResponse> Task { get; set; }

        protected abstract ETTask Run(Entity session, Message message);

        public void Handle(Entity session, object msg)
        {
            Message message = msg as Message;
            if (message == null)
            {
                Log.Error($"消息类型转换错误: {msg.GetType().Name} to {typeof (Message).Name}");
                return;
            }

            if (session.IsDisposed)
            {
                Log.Error($"session disconnect {msg}");
                return;
            }

            this.Run(session, message).Coroutine();
        }

        public Type GetMessageType()
        {
            return typeof (Message);
        }

        public Type GetResponseType()
        {
            return null;
        }
    }
}