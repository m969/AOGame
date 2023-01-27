using ET;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ET
{
    public partial class Entity
    {
        public T AddChildWithId<T>(long id, long instanceId, bool isFromPool = false) where T : Entity, IAwake, new()
        {
            Type type = typeof(T);
            T component = Entity.Create(type, isFromPool) as T;
            component.Id = id;
            component.InstanceId = instanceId;
            component.Parent = this;
            EventSystem.Instance.Awake(component);
            return component;
        }

        public Entity AddChildWithId(Type type, long id, long instanceId, bool isFromPool = false)
        {
            var component = Entity.Create(type, isFromPool);
            component.Id = id;
            component.InstanceId = instanceId;
            component.Parent = this;
            EventSystem.Instance.Awake(component);
            return component;
        }
    }
}
