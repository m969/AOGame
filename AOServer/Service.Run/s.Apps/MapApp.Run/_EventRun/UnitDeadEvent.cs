using AO.Events;
using ET;
using ET.EventType;
using ET.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    /// <summary>
    /// 单位死亡事件
    /// </summary>
    public class UnitDeadEvent : AEventRun<Actor, Actor>, ICombatEvent
    {
        protected override async ETTask Run(Actor deadUnit, Actor causeUnit)
        {
            if (causeUnit.IsPlayer())
            {
                //var resComp = causeUnit.GetComponent<UnitResourcesComponent>();
                //if (resComp != null)
                //{
                //    resComp.AddResourceItem(ItemConst.ManaGemPieces, 10);
                //}
            }

            if (deadUnit.IsPlayer())
            {
                //actor.LeaveOutScene();
                //actor.DestroySelf();
            }
        }
    }

    public class UnitDeadEvent2 : AEventRun<Actor>
    {
        protected override async ETTask Run(Actor actor)
        {

        }
    }
}
