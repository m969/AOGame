namespace AO
{
    using AO;
    using ET;

    /// <summary>
    /// 验证服（这里是检验身份证、拿门票的地方）
    /// </summary>
    public class RealmApp : Entity, IApp, IAwake
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