using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    [Event(SceneType.Map)]
    public class UnitPathMoveEvent_Move : AEvent<EventType.UnitPathMoveEvent>
    {
        protected override async ETTask Run(Entity source, EventType.UnitPathMoveEvent a)
        {
            //if (a.ArriveTime > 0)
            //{
            //    a.Unit.MovePathAsync(a.PathPoints, a.ArriveTime).Coroutine();
            //}
            //else
            {
                a.Unit.MovePathAsync(a.PathPoints).Coroutine();
            }
        }
    }
}
