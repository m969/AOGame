using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public class SceneUnitComponent : Entity, IAwake, IDestroy
    {
        public readonly Dictionary<long, Entity> idUnits = new();
        public readonly Dictionary<long, Avatar> idAvatars = new();
    }
}
