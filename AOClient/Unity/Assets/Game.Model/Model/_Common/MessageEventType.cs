using ET;
using System.Diagnostics;

namespace AO
{
    namespace EventType
    {
        public struct SessionEvent
        {
            public Entity Session;
            public IMessage Message;
        }

        public struct SessionRemoveAcceptTimeoutComponentEvent
        {
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

        public struct ActorLocationSendEvent
        {
            public long EntityId;
            public IActorLocationMessage Message;
        }

        public struct BroadcastEvent
        {
            public IMapUnit Unit;
            public IMessage Message;
        }

        public struct ActorCallEvent
        {
            public ETTask<IActorResponse> Task;
            public long ActorId;
            public IActorRequest Message;
        }

        public struct ActorLocationCallEvent
        {
            public ETTask<IActorResponse> Task;
            public long EntityId;
            public IActorLocationRequest Message;
        }
    }
}
