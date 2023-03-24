namespace AO
{
    using ET;

    public partial class AttributeSpellWillpowerComponent : Entity, IAwake, IUnitAttribute, IUnitDBComponent
    {
        /// <summary>
        /// 施法念力属性值
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int AttributeValue { get; set; }

        /// <summary>
        /// 可用的施法念力
        /// </summary>
        [NotifyAOI, PropertyChanged]
        public int AvailableValue { get; set; }
    }
}