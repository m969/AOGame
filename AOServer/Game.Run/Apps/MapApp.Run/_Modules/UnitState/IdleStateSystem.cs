namespace AO
{
    using AO;
    using ET;
    using TComp = AO.IdleState;

    public static partial class IdleStateSystem
    {
        [ObjectSystem]
        public class AwakeHandler : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {

            }
        }

        public class DestroyHandler : DestroySystem<TComp>
        {
            protected override void Destroy(TComp self)
            {

            }
        }
    }
}