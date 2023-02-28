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
    public class PublishNewUnitEvent_Broadcast : AEvent<EventType.PublishNewUnitEvent>
    {
        protected override async ETTask Run(Entity source, EventType.PublishNewUnitEvent a)
        {
            a.Unit.Entity().GetParent<Scene>().GetComponent<SceneUnitComponent>().Add(a.Unit.Entity());

            var createUnits = new M2C_CreateUnits() { Units = new List<UnitInfo>() };
            createUnits.Units.Add(a.Unit.CreateUnitInfo());
            MessageHelper.Broadcast(a.Unit, createUnits);
        }
    }
}
