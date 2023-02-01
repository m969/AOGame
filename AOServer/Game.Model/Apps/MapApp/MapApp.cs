namespace AO
{
    using AO;
    using ET;

    public class MapApp : Entity, IApp, IAwake
    {
        /// <summary>
        /// 区
        /// </summary>
        public int Zone
        {
            get;
        }

        //public MapApp(long id, long instanceId, int zone, Entity parent)
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