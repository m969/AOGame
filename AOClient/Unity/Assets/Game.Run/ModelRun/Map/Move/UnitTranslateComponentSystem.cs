using ET;
using System.Linq;
using Unity.Mathematics;
using TComp = AO.UnitTranslateComponent;

namespace AO
{
    public static class UnitTranslateComponentSystem
    {
        [ObjectSystem]
        public class UnitTranslateComponentAwakeSystem : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                self.PreTime = TimeHelper.ClientNow();
                self.Unit = self.Parent as IMapUnit;
            }
        }

        [ObjectSystem]
        public class UnitTranslateComponentUpdateSystem : UpdateSystem<TComp>
        {
            protected override void Update(TComp self)
            {
                self.DeltaTime = (TimeHelper.ClientNow() - self.PreTime) / 1000f;
                self.PreTime = TimeHelper.ClientNow();

                if (self.TranslateFinish)
                {
                    return;
                }
                if (math.abs(math.distance(self.Unit.Position, self.TargetPosition)) > 0.01f)
                {
                    self.Unit.Position += self.TargetPositionNormalize * self.Speed * self.DeltaTime;
                    //Log.Console($"Update Translate {self.TargetPosition} {self.TargetPositionNormalize} {self.Speed} {self.DeltaTime}");
                }
                else
                {
                    //Log.Console($"TranslateFinish {self.Unit.Position}");
                    self.TranslateFinish = true;
                    self.TranslateTask?.SetResult();
                    self.TranslateTask = null;
                }
            }
        }

        public static void Translate(this IMapUnit unit, float3 point)
        {
            var translateComp = unit.Entity().GetComponent<TComp>();
            translateComp.TargetPosition = point;
        }

        public static ETTask TranslateAsync(this IMapUnit unit, float3 point)
        {
            var translateComp = unit.Entity().GetComponent<TComp>();
            translateComp.TranslateTask = ETTask.Create();
            translateComp.TargetPosition = point;
            return translateComp.TranslateTask;
        }
    }
}