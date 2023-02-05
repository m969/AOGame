namespace AO
{
    using AO;
    using ET;

    public class ServerAppTypeComponent : Entity, IAwake<string>
    {
        public string ServerType { get; set; }
    }
}