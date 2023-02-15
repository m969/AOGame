using ET;
using ET.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    [Event(SceneType.Map)]
    public class BroadcastUnitEvent_Broadcast : AEvent<EventType.BroadcastUnitEvent>
    {
        protected override async ETTask Run(Entity source, EventType.BroadcastUnitEvent a)
        {
            var createUnits = new M2C_CreateUnits() { Units = new List<UnitInfo>() };
            createUnits.Units.Add(a.Unit.CreateUnitInfo());
            MessageHelper.Broadcast(a.Unit, createUnits);
        }
    }
}
