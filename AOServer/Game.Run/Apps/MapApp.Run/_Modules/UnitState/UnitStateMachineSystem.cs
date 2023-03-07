namespace AO
{
    using AO;
    using ET;
    using TComp = AO.UnitStateMachine;

    public static partial class UnitStateMachineSystem
    {
        [ObjectSystem]
        public class AwakeHandler : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {

            }
        }

        public static void EnterState<T>(this IMapUnit self) where T : Entity, IUnitState, IAwake, new()
        {
            self.Entity().AddComponent<T>();
        }
    }
}