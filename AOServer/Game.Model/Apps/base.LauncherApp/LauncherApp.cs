namespace AO
{
    using AO;
    using ET;

    /// <summary>
    /// 启动器（第一个启动的进程App，由此App再去启动其他类型的App）
    /// </summary>
    public class LauncherApp : Entity, IApp, IAwake
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