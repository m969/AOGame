using AO;

namespace ET.Server
{
    // 离开视野
    [Event(SceneType.Map)]
    public class UnitLeaveSightRange_NotifyClient: AEvent<EventType.UnitLeaveSightRange>
    {
        protected override async ETTask Run(Entity scene, EventType.UnitLeaveSightRange args)
        {
            await ETTask.CompletedTask;
            AOIEntity a = args.A;
            AOIEntity b = args.B;
            if (a.Unit is not Actor)
            {
                return;
            }

            if (a.Unit.GetComponent<MailBoxComponent>() == null)
            {
                return;
            }

            MessageHelper.NoticeUnitRemove(a.Unit as Actor, b.Unit.MapUnit());
        }
    }
}