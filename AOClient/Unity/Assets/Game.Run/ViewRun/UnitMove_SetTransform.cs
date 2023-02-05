namespace AO
{
    using ET;
    using UnityEngine;

    [Event(SceneType.Process)]
    public class UnitMove_SetTransform : AEvent<EventType.UnitMove>
    {
        protected override async ETTask Run(Scene scene, EventType.UnitMove args)
        {
            Log.Debug("UnitMove_SetTransform Run");

            var unit = args.Unit;
            if (unit == null)
            {
                return;
            }
            if (unit.GetComponent<UnitAnimationComponent>() == null)
            {
                return;
            }
            if (args.Type == EventType.UnitMove.MoveStart)
            {
                unit.GetComponent<UnitAnimationComponent>().Play(AnimationType.Run);
            }
            if (args.Type == EventType.UnitMove.MoveEnd)
            {
                unit.GetComponent<UnitAnimationComponent>().Play(AnimationType.Idle);
            }

            await ETTask.CompletedTask;
        }
    }
}