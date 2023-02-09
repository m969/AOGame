namespace AO
{
    using AO;
    using ET;

    public class MapApp : Entity, IApp, IAwake, IUpdate, IDomain
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