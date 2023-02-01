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
        public class AvatarGateComponentAwakeSystem : AwakeSystem<AvatarGateComponent, long>
        {
            protected override void Awake(AvatarGateComponent self, long a)
            {
                self.GateSessionActorId = a;
            }
        }
    }
}
