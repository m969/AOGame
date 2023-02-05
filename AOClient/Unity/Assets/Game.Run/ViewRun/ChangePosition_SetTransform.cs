namespace AO
{
    using ET;
    using UnityEngine;

    [Event(SceneType.Process)]
    public class ChangePosition_SetTransform : AEvent<EventType.ChangePosition>
    {
        protected override async ETTask Run(Scene scene, EventType.ChangePosition args)
        {
            //Log.Debug("ChangePosition_SetTransform Run");

            var unit = args.Unit;
            if (unit == null)
            {
                return;
            }
            if (unit.GetComponent<UnitViewComponent>() == null )
            {
                return;
            }
            var unitObj = unit.GetComponent<UnitViewComponent>().UnitObj;
            unitObj.transform.position = unit.MapUnit().Position;
            unitObj.transform.forward = unit.MapUnit().Position - args.OldPos;

            await ETTask.CompletedTask;
        }
    }
}