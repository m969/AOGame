using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public class GateSessionIdComponent : Entity, IAwake<long>, ITransfer
    {
        public long GateSessionId { get; set; }
    }
}
