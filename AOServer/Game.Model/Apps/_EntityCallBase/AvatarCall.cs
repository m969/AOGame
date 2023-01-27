namespace AO
{
    using ET;

    public class AvatarCall : Entity, IAwake
    {
        public ClientCall Client { get; private set; } = new ClientCall();

        public class ClientCall : Entity, IAwake
        {
            public void OnStartMove() { EventSystem.Instance.Publish(this.DomainScene(), 0); }
        }

        public class InnerCall : Entity, IAwake
        {
        }
    }
}