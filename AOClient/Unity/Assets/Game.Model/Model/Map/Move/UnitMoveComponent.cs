using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public class UnitMoveComponent : Entity, IAwake, IDestroy
    {
        public IMapUnit Unit { get; set; }
    }
}
