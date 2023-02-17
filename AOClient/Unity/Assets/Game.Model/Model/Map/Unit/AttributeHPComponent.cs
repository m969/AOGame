namespace AO
{
    using ET;

    public partial class AttributeHPComponent : Entity, IAwake
    {
        /// <summary>
        /// 属性生命力
        /// </summary>
        [NotifyAOI]
        public int Attribute_HP { get; set; }

        /// <summary>
        /// 可用的生命值
        /// </summary>
        [NotifyAOI]
        public int Available_HP { get; set; }
        public int HP => Available_HP;
    }
}