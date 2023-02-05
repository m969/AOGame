using ET;
using System.Linq;
using Unity.Mathematics;
using TComp = AO.UnitPathMoveComponent;

namespace AO
{
    public static class UnitPathMoveComponentSystem
    {
        public static async ETTask MovePathAsync(this IMapUnit unit, float3[] pathPoints)
        {
            var entity = unit as Entity;
            entity.GetComponent<UnitTranslateComponent>().Speed = entity.GetComponent<TComp>().Speed;
            AOGame.Publish(new EventType.UnitMove() { Unit = entity, Type = EventType.UnitMove.MoveStart });
            foreach (var item in pathPoints)
            {
                await unit.TranslateAsync(item);
            }
            AOGame.Publish(new EventType.UnitMove() { Unit = entity, Type = EventType.UnitMove.MoveEnd });
        }
    }
}