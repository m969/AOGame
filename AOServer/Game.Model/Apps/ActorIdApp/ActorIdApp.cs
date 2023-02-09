namespace AO
{
    using ET;

    /// <summary>
    /// ActorId缓存服（从这里请求ActorId）
    /// </summary>
    public class ActorIdApp : Entity, IApp, IAwake, IDomain
    {
        public int DomainIndex => Zone;
        /// <summary>
        /// 区
        /// </summary>
        public int Zone
        {
            get;
        }
    }
}