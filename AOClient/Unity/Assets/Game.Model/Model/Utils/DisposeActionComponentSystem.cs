using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using static ET.DisposeActionComponent;

namespace ET
{
    public class DisposeActionComponent : Entity, IDestroy, IAwake
    {
        public delegate void DisposeCallback();
        public DisposeCallback DisposeAction;
    }

    public static class DisposeActionComponentSystem
    {
        [ObjectSystem]
        public class DisposeActionComponentAwakeSystem : AwakeSystem<DisposeActionComponent>
        {
            protected override void Awake(DisposeActionComponent self)
            {

            }
        }

        [ObjectSystem]
        public class DisposeActionComponentDestroySystem : DestroySystem<DisposeActionComponent>
        {
            protected override void Destroy(DisposeActionComponent self)
            {
                if (self.DisposeAction != null)
                {
                    self.DisposeAction();
                }
                self.DisposeAction = null;
            }
        }

        public static void AddDisposeAction(this Entity self, DisposeCallback action)
        {
            var comp = self.GetOrAdd<DisposeActionComponent>();
            comp.DisposeAction += action;
        }
    }
}
