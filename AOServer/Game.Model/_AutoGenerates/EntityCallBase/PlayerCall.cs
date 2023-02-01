namespace AO
{
    using ET;
    using ActorSendEvent = AO.EventType.ActorSendEvent;

    public class PlayerCall : Entity, IAwake
    {
        public ClientCall Client { get; private set; } = new ClientCall();

        public class ClientCall : Entity, IAwake
        {

        }

        public class InnerCall : Entity, IAwake
        {

        }
    }
}