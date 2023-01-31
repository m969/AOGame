namespace AO
{
    using ET;
    using Unity.Mathematics;

    public class AvatarCall : Entity
    {
        public void OnStartMove() { EventSystem.Instance.Publish(this.DomainScene(), 0); }
        public async ETTask<G2C_EnterMap> C2G_EnterMap(C2G_EnterMap message)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(message);
            return msg.Response as G2C_EnterMap;
        }
    }
}