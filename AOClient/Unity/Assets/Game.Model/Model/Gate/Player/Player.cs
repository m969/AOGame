using ET;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public sealed class Player : Entity, IAwake<string>
    {
        [BsonIgnore]
        public long SessionId { get; set; }

        [NotifySelf]
        public string Account { get; set; }

        [NotifySelf]
        public long UnitId { get; set; }
    }
}
