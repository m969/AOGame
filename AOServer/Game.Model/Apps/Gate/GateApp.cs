namespace AO
{
    using AO;
    using ET;

    /// <summary>
    /// 网关，进入游戏世界的关口（检验门票和健康码的地方，有票且绿码才能进）
    /// </summary>
    public class GateApp : Entity, IApp, IAwake
    {
        /// <summary>
        /// 区
        /// </summary>
        public int Zone
        {
            get;
        }

        //public GateApp(long id, long instanceId, int zone, Entity parent)
        //{
        //    this.Id = id;
        //    this.InstanceId = instanceId;
        //    this.Zone = zone;
        //    this.IsCreated = true;
        //    this.IsNew = true;
        //    this.Parent = parent;
        //    this.Domain = this;
        //    this.IsRegister = true;
        //}

        //public new Entity Domain
        //{
        //    get => this.domain;
        //    private set => this.domain = value;
        //}

        //public new Entity Parent
        //{
        //    get
        //    {
        //        return this.parent;
        //    }
        //    private set
        //    {
        //        if (value == null)
        //        {
        //            return;
        //        }

        //        this.parent = value;
        //        this.parent.Children.Add(this.Id, this);
        //    }
        //}
    }
}
