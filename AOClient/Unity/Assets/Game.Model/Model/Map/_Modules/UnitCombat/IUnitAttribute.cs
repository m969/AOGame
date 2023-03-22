namespace AO
{
    using ET;

    public interface IUnitAttribute
    {
        /// <summary>
        /// 属性值，角色的等级属性值，只受等级影响
        /// </summary>
        public int AttributeValue { get; set; }

        /// <summary>
        /// 可用值，真正实际可用的值，受装备、战斗、buff等影响
        /// </summary>
        public int AvailableValue { get; set; }
    }
}