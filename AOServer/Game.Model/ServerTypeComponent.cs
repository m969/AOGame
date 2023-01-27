namespace AO
{
    using AO;
    using ET;

    public class ServerTypeComponent : Entity, IAwake<string>
    {
        public string ServerType { get; set; }
    }
}