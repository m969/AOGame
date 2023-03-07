using System;
using ET;
using GameUtils;
using Unity.Mathematics;
using TComp = AO.UnitLifeTimeComponent;

namespace AO
{
    public static class UnitLifeTimeComponentSystem
    {
        public class AwakeSystemObject : AwakeSystem<TComp, float>
        {
            protected override void Awake(TComp self, float time)
            {
                self.LifeTimer = new GameTimer(time);
            }
        }

        public class UpdateSystemObject : UpdateSystem<TComp>
        {
            protected override void Update(TComp self)
            {
                if (self.LifeTimer.IsRunning)
                {
                    self.LifeTimer.UpdateAsFinish(UnityEngine.Time.deltaTime);
                    //Log.Debug($"UnitLifeTimeComponentSystem Update {self.LifeTimer.Time}");
                }
                else
                {
                    self.DestroyEntity();
                }
            }
        }

        public static void DestroyEntity(this TComp self)
        {
            //Log.Debug($"UnitLifeTimeComponentSystem DestroyEntity {self.LifeTimer.Time}");
            //self.GetParent<Scene>().GetComponent<SceneUnitComponent>().Remove(self.Id);
            self.Parent.Dispose();
        }
    }
}