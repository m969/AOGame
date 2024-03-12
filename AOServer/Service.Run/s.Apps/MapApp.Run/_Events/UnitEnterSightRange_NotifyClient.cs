using AO;

namespace ET.Server
{
    // 进入视野通知
    [Event(SceneType.Map)]
    public class UnitEnterSightRange_NotifyClient: AEvent<EventType.UnitEnterSightRange>
    {
        protected override async ETTask Run(Entity scene, EventType.UnitEnterSightRange args)
        {
            AOIEntity a = args.A;
            AOIEntity b = args.B;
            if (a.Id == b.Id)
            {
                return;
            }

            var ua = a.Unit;
            if (ua is not Actor)
            {
                return;
            }

            var ub = b.Unit;

            MessageHelper.NoticeUnitAdd(ua as Actor, ub.MapUnit());
            
            await ETTask.CompletedTask;
        }
    }
}