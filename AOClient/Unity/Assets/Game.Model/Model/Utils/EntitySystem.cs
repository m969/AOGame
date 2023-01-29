using ET;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ET
{
    public static class EntitySystem
    {
        public static Entity Get<T>(this Entity entity) where T : Entity
        {
            return entity.GetComponent<T>();
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
    }
}
