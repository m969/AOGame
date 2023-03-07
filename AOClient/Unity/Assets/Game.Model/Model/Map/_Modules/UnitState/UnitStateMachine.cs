using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public class UnitStateMachine : Entity, IAwake
    {
        public List<IUnitState> ActiveStates { get; set; }
    }
}
