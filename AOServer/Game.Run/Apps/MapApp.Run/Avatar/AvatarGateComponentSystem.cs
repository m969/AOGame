using AO;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public static class AvatarGateComponentSystem
    {
        public class AvatarGateComponentAwakeSystem : AwakeSystem<GateSessionIdComponent, long>
        {
            protected override void Awake(GateSessionIdComponent self, long a)
            {
                self.GateSessionId = a;
            }
        }
    }
}
