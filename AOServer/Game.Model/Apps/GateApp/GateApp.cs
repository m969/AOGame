namespace AO
{
    using AO;
    using ET;

    /// <summary>
    /// 网关，进入游戏世界的关口（检验门票和健康码的地方，有票且绿码才能进）
    /// </summary>
    public class GateApp : Entity, IApp, IAwake
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
