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
        public long PlayerInstanceId { get; set; }
        public Dictionary<Type, long> MessageType2EntityId { get; set; } = new();
        public Dictionary<Type, long> MessageType2ActorId { get; set; } = new();
    }
}
