using System;

namespace ET
{
    public abstract class AMHandler<Message>: IMHandler where Message : class
    {
        protected abstract ETTask Run(Message message);

        public void Handle(object msg)
        {
            Message message = msg as Message;
            if (message == null)
            {
                Log.Error($"消息类型转换错误: {msg.GetType().Name} to {typeof (Message).Name}");
                return;
            }

            this.Run(message).Coroutine();
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