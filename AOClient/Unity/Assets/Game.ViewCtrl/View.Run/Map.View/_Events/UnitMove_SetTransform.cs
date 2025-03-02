namespace AO
{
    using ET;
    using ET.EventType;
    using UnityEngine;

    [Event(SceneType.Process)]
    public class UnitMove_SetTransform : AEvent<UnitMove>
    {
        protected override async ETTask Run(Entity source, UnitMove args)
        {
            //Log.Debug($"UnitMove_SetTransform Run {args.Type}");

            var unit = args.Unit;
            if (unit == null)
            {
                return;
            }
            if (unit.GetComponent<UnitAnimationComponent>() == null)
            {
                return;
            }
            if (args.Type == UnitMove.MoveStart)
            {
                unit.GetComponent<UnitAnimationComponent>().Play(AnimationType.Run);
            }
            if (args.Type == UnitMove.MoveEnd)
            {
                unit.GetComponent<UnitAnimationComponent>().Play(AnimationType.Idle);
            }

            await ETTask.CompletedTask;
        }
    }
}