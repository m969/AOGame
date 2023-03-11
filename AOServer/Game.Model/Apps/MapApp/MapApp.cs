namespace AO
{
    using AO;
    using ET;

    public class MapApp : Entity, IApp, IAwake, IUpdate
    {
        public int DomainIndex => Zone;
        /// <summary>
        /// 区
        /// </summary>
        public int Zone
        {
            get; set;
        }
    }
}