using AO;
using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ET
{
    public static class EntitySystem
    {
        public static Entity Entity(this IMapUnit entity)
        {
            return (entity as Entity);
        }

        public static IMapUnit MapUnit(this Entity entity)
        {
            return (entity as IMapUnit);
        }

        public static Entity GetOrAdd(this Entity entity, Type type)
        {
            if (entity.GetComponent(type) == null)
            {
                entity.AddComponent(type);
            }
            return entity.GetComponent(type);
        }

        public static T GetOrAdd<T>(this Entity entity) where T : Entity
        {
            return entity.GetOrAdd(typeof(T)) as T;
        }

        public static Entity[] GetNotifyComponents(this Entity entity)
        {
            var list = new List<Entity>();
            foreach (var kv in entity.Components)
            {
                foreach (var item in kv.Key.GetProperties())
                {
                    if (item.GetCustomAttribute<NotifyAOIAttribute>() != null || item.GetCustomAttribute<NotifySelfAttribute>() != null)
                    {
                        list.Add(kv.Value);
                        break;
                    }
                }
            }
            return list.ToArray();
        }

        public static Entity[] GetNotifySelfComponents(this Entity entity)
        {
            var list = new List<Entity>();
            foreach (var kv in entity.Components)
            {
                foreach (var item in kv.Key.GetProperties())
                {
                    if (item.GetCustomAttribute<NotifySelfAttribute>() != null)
                    {
                        list.Add(kv.Value);
                        break;
                    }
                }
            }
            return list.ToArray();
        }

        public static Entity[] GetNotifyAOIComponents(this Entity entity)
        {
            var list = new List<Entity>();
            foreach (var kv in entity.Components)
            {
                foreach (var item in kv.Key.GetProperties())
                {
                    if (item.GetCustomAttribute<NotifyAOIAttribute>() != null)
                    {
                        list.Add(kv.Value);
                        break;
                    }
                }
            }
            return list.ToArray();
        }

        public static Entity[] DeserializeComponents(UnitInfo unitInfo)
        {
            var list = new List<Entity>();
            if (unitInfo.ComponentInfos == null)
            {
                return list.ToArray();
            }
            foreach (var componentInfo in unitInfo.ComponentInfos)
            {
                var compType = Type.GetType($"{componentInfo.ComponentName}");
                if (compType == null)
                {
                    continue;
                }
                var comp = MongoHelper.Deserialize(compType, componentInfo.ComponentBytes) as Entity;
                list.Add(comp);
            }
            return list.ToArray();
        }
    }
}
