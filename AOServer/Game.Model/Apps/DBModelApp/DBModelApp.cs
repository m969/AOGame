namespace AO
{
    using ET;

    /// <summary>
    /// Model数据缓存服（从这里拿实体数据，这里没有再去数据库服反序列化）
    /// </summary>
    public class DBModelApp : Entity, IApp, IAwake
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