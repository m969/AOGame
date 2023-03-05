namespace AO
{
    using AO;
    using ET;

    public class WebGMApp : Entity, IApp, IAwake, IUpdate
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