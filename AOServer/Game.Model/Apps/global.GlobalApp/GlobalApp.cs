namespace AO
{
    using AO;
    using ET;

    public class GlobalApp : Entity, IApp, IAwake, IUpdate
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