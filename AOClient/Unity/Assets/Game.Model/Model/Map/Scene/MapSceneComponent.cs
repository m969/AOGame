using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public class MapSceneComponent : Entity, IAwake, IDestroy
    {
        public readonly Dictionary<long, Scene> idScenes = new();
        public readonly UnOrderMultiMap<string, Scene> typeScenes = new();
    }
}
