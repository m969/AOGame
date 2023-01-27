using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AO
{
    public static class AvatarControlComponentSystem
    {
        [ObjectSystem]
        public class AvatarControlComponentAwakeSystem : AwakeSystem<AvatarControlComponent>
        {
            protected override void Awake(AvatarControlComponent self)
            {
                self.UnitViewComp = self.Parent.GetComponent<UnitViewComponent>();
            }
        }

        [ObjectSystem]
        public class AvatarControlComponentUpdateSystem : UpdateSystem<AvatarControlComponent>
        {
            protected override void Update(AvatarControlComponent self)
            {
                if (self.UnitViewComp != null)
                {
                    var h = Input.GetAxis("Horizontal");
                    var v = Input.GetAxis("Vertical");
                    self.UnitViewComp.UnitObj.transform.Translate(new Vector3(h, 0, v) * Time.deltaTime);
                    //Avatar.MyAvatarCall.C2G_EnterMap(new C2G_EnterMap()).Coroutine();
                }
            }
        }
    }
}