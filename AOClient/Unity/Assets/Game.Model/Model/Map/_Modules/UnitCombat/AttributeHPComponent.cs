namespace AO
{
    using ET;

    public partial class AttributeHPComponent : Entity, IAwake, IUnitAttribute, IUnitDBComponent
    {
        /// <summary>
        /// 属性生命力
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int AttributeValue { get; set; }

        /// <summary>
        /// 可用的生命值
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int AvailableValue { get; set; }
    }
}