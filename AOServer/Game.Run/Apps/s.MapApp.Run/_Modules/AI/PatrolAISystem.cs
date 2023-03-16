namespace AO
{
    using AO;
    using ET;
    using Unity.Mathematics;
    using TComp = AO.PatrolAI;

    public static partial class PatrolAISystem
    {
        public class AwakeHandler : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                self.RunPatrol().Coroutine();
            }
        }

        public class DestroyHandler : DestroySystem<TComp>
        {
            protected override void Destroy(TComp self)
            {

            }
        }

        public static async ETTask RunPatrol(this TComp self)
        {
            var unit = self.Parent.MapUnit();
            while (!self.IsDisposed)
            {
                if (math.distance(unit.Position, unit.GetSpawnPoint()) > 6)
                {
                    await unit.MoveToAsync(unit.GetSpawnPoint());
                }
                else
                {
                    await unit.RandomMove();
                }
                await TimerComponent.Instance.WaitAsync(5000);
            }
        }
    }
}