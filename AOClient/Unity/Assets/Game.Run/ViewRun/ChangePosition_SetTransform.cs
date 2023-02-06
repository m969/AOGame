namespace AO
{
    using ET;
    using Unity.Mathematics;
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
            var forward = unit.MapUnit().Position - args.OldPos;
            if ((Vector3)forward != Vector3.zero)
            {
                unitObj.transform.forward = forward;
            }

            await ETTask.CompletedTask;
        }
    }
}