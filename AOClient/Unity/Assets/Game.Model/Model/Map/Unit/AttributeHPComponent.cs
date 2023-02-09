namespace AO
{
    using ET;

    public partial class AttributeHPComponent : Entity, IAwake
    {
        [NotifyAOI]
        public int HPAttributeValue { get; set; }

        [NotifyAOI]
        public int HPValue { get; set; }
    }
}