using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class SessionPlayerComponent : Entity, IAwake, IDestroy
    {
        public long PlayerId { get; set; }
        public Dictionary<Type, long> MessageType2Actor { get; set; } = new();
    }
}
