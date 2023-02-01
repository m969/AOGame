namespace AO
{
    using AO;
    using ET;

    /// <summary>
    /// 验证服（这里是检验身份证、拿门票的地方）
    /// </summary>
    public class RealmApp : Entity, IApp, IAwake
    {
        /// <summary>
        /// 区
        /// </summary>
        public int Zone
        {
            get;
        }

        //public RealmApp(long id, long instanceId, int zone, Entity parent)
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