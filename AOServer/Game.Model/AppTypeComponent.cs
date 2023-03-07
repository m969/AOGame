namespace AO
{
    using AO;
    using ET;

    public class AppTypeComponent : Entity, IAwake<string>
    {
        public string AppType { get; set; }
    }
}