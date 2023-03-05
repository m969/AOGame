using System;

namespace ET
{
    public interface IMHandler
    {
        void Handle(Entity session, object message);

        Type GetMessageType();

        Type GetResponseType();
    }
}