namespace AO
{
    using ET;
    using ET.EventType;
    using Unity.Mathematics;
    using UnityEngine;

    [Event(SceneType.Process)]
    public class ChangePosition_SetTransform : AEvent<ChangePosition>
    {
        protected override async ETTask Run(Entity source, ChangePosition args)
        {
            var unit = args.Unit;
            if (unit == null)
            {
                return;
            }
            //Log.Debug($"ChangePosition_SetTransform Run {unit.MapUnit().Position}");
            var viewComp = unit.GetComponent<UnitViewComponent>();
            if (viewComp == null || viewComp.UnitObj == null)
            {
                return;
            }
            var unitObj = viewComp.UnitObj;
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