using AO;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public static class GateSessionIdComponentSystem
    {
        public class GateSessionIdComponentAwakeSystem : AwakeSystem<GateSessionIdComponent, long>
        {
            protected override void Awake(GateSessionIdComponent self, long a)
            {
                self.GateSessionId = a;
            }
        }
    }
}
