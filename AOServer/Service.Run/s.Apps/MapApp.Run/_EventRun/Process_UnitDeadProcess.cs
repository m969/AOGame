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
    /// 角色死亡处理
    /// </summary>
    public class Process_UnitDeadProcess : AFuncProcess<Actor, Actor>
    {
        public override async ETTask Execute(Actor causeUnit, Actor deadUnit)
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
}
