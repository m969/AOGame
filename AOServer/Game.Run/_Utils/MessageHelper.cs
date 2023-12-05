﻿

using AO;
using System.Collections.Generic;
using System.IO;

namespace ET.Server
{
    public static class MessageHelper
    {
        public static void NoticeUnitAdd(Avatar unit, IMapUnit sendUnit)
        {
            M2C_CreateUnits createUnits = new M2C_CreateUnits() { Units = new List<UnitInfo>() };
            createUnits.Units.Add(sendUnit.CreateUnitInfo());
            MessageHelper.SendToClient(unit, createUnits);
        }

        public static void NoticeUnitRemove(Avatar unit, IMapUnit sendUnit)
        {
            M2C_RemoveUnits removeUnits = new M2C_RemoveUnits() { Units = new List<long>() };
            removeUnits.Units.Add(sendUnit.Entity().Id);
            MessageHelper.SendToClient(unit, removeUnits);
        }

        public static void Broadcast(Entity unit, IActorMessage message) => Broadcast(unit.MapUnit(), message);
        public static void Broadcast(IMapUnit unit, IActorMessage message)
        {
            AOGame.Publish(new AO.EventType.BroadcastEvent() { Unit = unit, Message = message });
            //Dictionary<long, AOIEntity> dict = unit.GetBeSeePlayers();
            //// 网络底层做了优化，同一个消息不会多次序列化
            //foreach (AOIEntity u in dict.Values)
            //{
            //    ActorMessageSenderComponent.Instance.Send(u.Unit.GetComponent<UnitGateComponent>().GateSessionActorId, message);
            //}
        }

        public static void SendToClient(Avatar unit, IActorMessage message)
        {
            SendActor(unit.GetComponent<GateSessionIdComponent>().GateSessionId, message);
        }
        
        
        /// <summary>
        /// 发送协议给ActorLocation
        /// </summary>
        /// <param name="id">注册Actor的Id</param>
        /// <param name="message"></param>
        public static void SendToLocationActor(long id, IActorLocationMessage message)
        {
            //ActorLocationSenderComponent.Instance.Send(id, message);
            AOGame.Publish(new AO.EventType.ActorLocationSendEvent() { EntityId = id, Message = message });
        }

        /// <summary>
        /// 发送协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        public static void SendActor(long actorId, IActorMessage message)
        {
            //ActorMessageSenderComponent.Instance.Send(actorId, message);
            AOGame.Publish(new AO.EventType.ActorSendEvent() { ActorId = actorId, Message = message });
        }

        /// <summary>
        /// 发送RPC协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallActor(long actorId, IActorRequest message)
        {
            //return await ActorMessageSenderComponent.Instance.Call(actorId, message);
            var task = ETTask<IActorResponse>.Create();
            AOGame.Publish(new AO.EventType.ActorCallEvent() { ActorId = actorId, Message = message, Task = task });
            return await task;
        }

        /// <summary>
        /// 发送RPC协议给Entity，底层会通过Entityid查询(定位)到ActorId(即InstanceId)，再通过ActorId把消息发出去
        /// </summary>
        /// <param name="entityId">注册Actor的实体Id</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallEntity(long entityId, IActorLocationRequest message)
        {
            //return await ActorLocationSenderComponent.Instance.Call(id, message);
            var task = ETTask<IActorResponse>.Create();
            AOGame.Publish(new AO.EventType.ActorLocationCallEvent() { EntityId = entityId, Message = message, Task = task });
            return await task;
        }
    }
}