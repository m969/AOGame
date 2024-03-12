using ET;

namespace AO
{
    public class GateSessionIdComponent : Entity, IAwake<long>, ITransfer
    {
        public long GateSessionId { get; set; }
    }
}
