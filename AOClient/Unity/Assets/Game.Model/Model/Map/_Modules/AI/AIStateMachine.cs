using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public class AIStateMachine : Entity, IAwake
    {
        public List<IAIState> ActiveStates { get; set; }
    }
}
