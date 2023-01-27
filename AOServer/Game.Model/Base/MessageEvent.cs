using AspectInjector.Broker;
using ET;
using System.ComponentModel;

namespace AO
{
    namespace EventType
    {
        public struct MessageEvent
        {

        }

        public struct SessionEvent
        {
            public Entity Session;
            public IMessage Message;
        }

        public struct ActorReplyEvent
        {
            public int FromProcess;
            public IActorResponse Message;
        }

        public struct ActorSendEvent
        {
            public long ActorId;
            public IMessage Message;
        }

        public struct ActorCallEvent
        {
            public ETTask<IActorResponse> Task;
            public long ActorId;
            public IActorRequest Message;
        }
    }
}
