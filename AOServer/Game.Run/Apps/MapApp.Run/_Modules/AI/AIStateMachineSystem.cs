namespace AO
{
    using AO;
    using ET;
    using TComp = AO.AIStateMachine;

    public static partial class AIStateMachineSystem
    {
        [ObjectSystem]
        public class AwakeHandler : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {

            }
        }

        public static void EnterAI<T>(this IMapUnit self) where T : Entity, IAIState, IAwake, new()
        {
            self.Entity().AddComponent<T>();
        }
    }
}