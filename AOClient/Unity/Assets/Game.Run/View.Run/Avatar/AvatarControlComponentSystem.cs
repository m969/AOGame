using ET;
using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AO
{
    public static class AvatarControlComponentSystem
    {
        [ObjectSystem]
        public class AwakeHandler : AwakeSystem<AvatarControlComponent>
        {
            protected override void Awake(AvatarControlComponent self)
            {
                
            }
        }

        [ObjectSystem]
        public class UpdateHandler : UpdateSystem<AvatarControlComponent>
        {
            protected override void Update(AvatarControlComponent self)
            {
                if (GRoot.inst.touchTarget != null)
                {
                    return;
                }

                if (Input.GetMouseButtonDown(((int)MouseButton.RightMouse)))
                {
                    //Log.Debug($"GetMouseButtonDown {MouseButton.RightMouse}");
                    if (RaycastUtils.CastMapPoint(out var hitPoint))
                    {
                        //Log.Debug($"Raycast {hitPoint}");
                        AvatarCall.C2M_PathfindingResult(new C2M_PathfindingResult() { Position = hitPoint });
                        //Avatar.Main.MoveToAsync(hitPoint).Coroutine();
                        //GameObject.Find("Cube").transform.position = hitPoint;
                    }
                }

                if (Input.GetMouseButtonUp(((int)MouseButton.LeftMouse)))
                {
                    //Log.Debug($"GetMouseButtonUp {MouseButton.LeftMouse}");
                    if (RaycastUtils.CastMapPoint(out var hitPoint))
                    {
                        AvatarCall.C2M_SpellRequest(new C2M_SpellRequest() { CastPoint = hitPoint, SkillId = 1002 }).Coroutine();
                    }               
                }

                //var h = Input.GetAxis("Horizontal");
                //var v = Input.GetAxis("Vertical");
            }
        }
    }
}