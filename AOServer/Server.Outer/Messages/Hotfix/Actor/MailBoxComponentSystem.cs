using System;

namespace ET.Server
{
    [ObjectSystem]
    public class MailBoxComponentAwakeSystem: AwakeSystem<MailBoxComponent>
    {
        protected override void Awake(MailBoxComponent self)
        {
            self.MailboxType = MailboxType.MessageDispatcher;
            self.Parent.AddLocation().Coroutine();
        }
    }

    [ObjectSystem]
    public class MailBoxComponentAwake1System: AwakeSystem<MailBoxComponent, MailboxType>
    {
        protected override void Awake(MailBoxComponent self, MailboxType mailboxType)
        {
            self.MailboxType = mailboxType;
            if (mailboxType != MailboxType.GateSession)
            {
                self.Parent.AddLocation().Coroutine();
            }
        }
    }
}