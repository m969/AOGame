using ET;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using TComp = AO.UnitPathMoveComponent;

namespace AO
{
    public static class UnitPathMoveComponentSystem
    {
        public static async ETTask MoveToAsync(this IMapUnit unit, float3 pathPoint)
        {
            await unit.MovePathAsync(new float3[] { pathPoint });
        }

        public static async ETTask MovePathAsync(this IMapUnit unit, float3[] pathPoints)
        {
            var entity = unit as Entity;
            entity.GetComponent<UnitTranslateComponent>().Speed = entity.GetComponent<TComp>().Speed;
            AOGame.PublishServer(new EventType.BroadcastEvent() { Unit = unit, Message = new M2C_PathfindingResult() { Id = entity.Id, Position = unit.Position, Points = new List<float3>(pathPoints) } });
            AOGame.Publish(new EventType.UnitMove() { Unit = entity, Type = EventType.UnitMove.MoveStart });
            foreach (var item in pathPoints)
            {
                await unit.TranslateAsync(item);
            }
            AOGame.Publish(new EventType.UnitMove() { Unit = entity, Type = EventType.UnitMove.MoveEnd });
        }
    }
}