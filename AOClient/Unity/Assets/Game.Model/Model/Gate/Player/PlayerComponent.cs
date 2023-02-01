using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public class PlayerComponent : Entity, IAwake, IDestroy
    {
        public readonly Dictionary<long, Player> idPlayers = new Dictionary<long, Player>();
    }
}
