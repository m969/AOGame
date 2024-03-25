using ET;
using ET.EventType;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
            var pathMoveComp = entity.GetComponent<UnitPathMoveComponent>();
            if (pathMoveComp.PathPoints != null)
            {
                pathMoveComp.PathPoints.Clear();
            }
            pathMoveComp.PathPoints = new List<float3>(pathPoints);
            //Log.Console($"MovePathAsync {pathMoveComp.PathPoints.Count} {unit.Position}");
            AOGame.PublishServer(new EventType.BroadcastEvent() { Unit = unit, Message = new M2C_PathfindingResult() { Id = entity.Id, Position = unit.Position, Points = new List<float3>(pathPoints) } });
            AOGame.Publish(new UnitMove() { Unit = entity, Type = UnitMove.MoveStart });

            while (pathMoveComp.PathPoints.Count > 0)
            {
                var path = pathMoveComp.PathPoints;
                var item = path[0];
                //Log.Console($"MovePathAsync TranslateAsync {path.Count} {item}");
                await unit.TranslateAsync(item);
                path.RemoveAt(0);
            }
            AOGame.Publish(new UnitMove() { Unit = entity, Type = UnitMove.MoveEnd });
        }
    }
}

//public static async ETTask MovePathAsync(this IMapUnit unit, float3[] pathPoints, long arriveTime)
//{
//    var entity = unit as Entity;
//    var duration = arriveTime - TimeHelper.ServerNow();
//    var length = math.distance(pathPoints[0], unit.Position);
//    for (int i = 0; i < pathPoints.Length; i++)
//    {
//        if (i == pathPoints.Length - 1)
//        {
//            break;
//        }
//        var dist = math.distance(pathPoints[i + 1], pathPoints[i]);
//        length += dist;
//    }
//    var speed = length / duration;
//    entity.GetComponent<UnitTranslateComponent>().Speed = speed;
//    var pathMoveComp = entity.GetComponent<UnitPathMoveComponent>();
//    if (pathMoveComp.PathPoints != null)
//    {
//        pathMoveComp.PathPoints.Clear();
//    }
//    pathMoveComp.PathPoints = new List<float3>(pathPoints);
//    AOGame.PublishServer(new EventType.BroadcastEvent() { Unit = unit, Message = new M2C_PathfindingResult() { Id = entity.Id, Position = unit.Position, Points = new List<float3>(pathPoints), ArriveTime = arriveTime } });
//    AOGame.Publish(new EventType.UnitMove() { Unit = entity, Type = EventType.UnitMove.MoveStart });

//    while (pathMoveComp.PathPoints.Count > 0)
//    {
//        var path = pathMoveComp.PathPoints;
//        var item = path[0];
//        await unit.TranslateAsync(item);
//        path.RemoveAt(0);
//        //Log.Debug($"{path.Count} {item}");
//    }
//    AOGame.Publish(new EventType.UnitMove() { Unit = entity, Type = EventType.UnitMove.MoveEnd });
//}