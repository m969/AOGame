using ET;
using ET.EventType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    [Event(SceneType.Map)]
    public class UnitPathMoveEvent_Move : AEvent<UnitPathMoveEvent>
    {
        protected override async ETTask Run(Entity source, UnitPathMoveEvent a)
        {
            //if (a.ArriveTime > 0)
            //{
            //    a.Unit.MovePathAsync(a.PathPoints, a.ArriveTime).Coroutine();
            //}
            //else
            {
                //Log.Console($"UnitPathMoveEvent_Move {a.PathPoints.Length}");
                a.Unit.MovePathAsync(a.PathPoints).Coroutine();
            }
        }
    }
}
