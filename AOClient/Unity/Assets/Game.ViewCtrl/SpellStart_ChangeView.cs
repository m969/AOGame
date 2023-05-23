namespace AO
{
    using ET;
    using UnityEngine;

    [Event(SceneType.Process)]
    public class SpellStart_ChangeView : AEvent<EventType.SpellStart>
    {
        protected override async ETTask Run(Entity source, EventType.SpellStart args)
        {
            //Log.Debug($"UnitMove_SetTransform Run {args.Type}");

            var unit = args.Unit;
            if (unit.IsDisposed)
            {
                return;
            }
            if (unit.GetComponent<UnitAnimationComponent>() == null)
            {
                return;
            }
            unit.GetComponent<UnitAnimationComponent>().Play(AnimationType.Attack);

            await ETTask.CompletedTask;
        }
    }
}