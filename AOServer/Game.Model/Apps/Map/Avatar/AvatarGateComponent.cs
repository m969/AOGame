using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public class AvatarGateComponent : Entity, IAwake<long>, ITransfer
    {
        public long GateSessionActorId { get; set; }
    }
}
