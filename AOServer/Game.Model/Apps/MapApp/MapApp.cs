namespace AO
{
    using AO;
    using ET;

    public class MapApp : Entity, IApp, IAwake, IUpdate
    {
        /// <summary>
        /// 区
        /// </summary>
        public int Zone
        {
            get;
        }
    }
}