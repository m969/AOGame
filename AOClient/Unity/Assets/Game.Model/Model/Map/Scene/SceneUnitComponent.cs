using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public class SceneUnitComponent : Entity, IAwake, IDestroy, IUpdate
    {
        public readonly Dictionary<long, Entity> idUnits = new();
        public List<Entity> CollisionUnitCache = new();
        public readonly Dictionary<long, Avatar> idAvatars = new();
    }
}
