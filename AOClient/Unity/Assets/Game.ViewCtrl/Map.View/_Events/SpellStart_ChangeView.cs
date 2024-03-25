namespace AO
{
    using ET;
    using ET.EventType;
    using UnityEngine;

    [Event(SceneType.Process)]
    public class SpellStart_ChangeView : AEvent<SpellStart>
    {
        protected override async ETTask Run(Entity source, SpellStart args)
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