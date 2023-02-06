using ET;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
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
                self.Unit = self.Parent as IMapUnit;
            }
        }

        [ObjectSystem]
        public class UnitTranslateComponentUpdateSystem : UpdateSystem<TComp>
        {
            protected override void Update(TComp self)
            {
                if (self.TranslateFinish)
                {
                    return;
                }
                if (math.abs(math.distance(self.Unit.Position, self.TargetPosition)) > 0.01f)
                {
                    self.Unit.Position += self.TargetPositionNormalize * self.Speed * Time.unscaledDeltaTime;
                    //Log.Console($"Update Translate {self.Unit.Position} {self.TargetPosition} {self.Speed} {Time.unscaledDeltaTime}");
                }
                else
                {
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