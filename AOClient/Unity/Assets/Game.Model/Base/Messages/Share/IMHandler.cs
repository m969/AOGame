using System;

namespace ET
{
    public interface IMHandler
    {
        void Handle(object message);
        Type GetMessageType();

        Type GetResponseType();
    }
}