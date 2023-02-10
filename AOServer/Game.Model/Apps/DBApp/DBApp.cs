namespace AO
{
    using ET;

    /// <summary>
    /// 数据库服（从这里连接数据库获取数据）
    /// </summary>
    public class DBApp : Entity, IApp, IAwake
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