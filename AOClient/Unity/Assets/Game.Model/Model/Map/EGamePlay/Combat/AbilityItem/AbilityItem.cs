using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ET;
using GameUtils;
using Unity.Mathematics;
using AO.EventType;
using AO;
using System;
using System.Linq;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 能力单元体
    /// </summary>
    public class AbilityItem : Entity, IPosition
    {
        public ET.Entity ItemUnit { get; set; }
        public Entity AbilityEntity => AbilityExecution.AbilityEntity;
        public IAbilityExecution AbilityExecution { get; set; }
        public EffectApplyType EffectApplyType { get; set; }
        public float3 Position { get; set; }
        public float3 Rotation { get; set; }
        public Transform AbilityItemTrans { get; set; }
        public CombatEntity TargetEntity { get; set; }


        public override void Awake(object initData)
        {
            AbilityExecution = initData as IAbilityExecution;
            if (AbilityEntity == null)
            {
                return;
            }
            var abilityEffects = AbilityEntity.GetComponent<AbilityEffectComponent>().AbilityEffects;
            foreach (var abilityEffect in abilityEffects)
            {
                if (abilityEffect.EffectConfig.Decorators != null)
                {
                    foreach (var effectDecorator in abilityEffect.EffectConfig.Decorators)
                    {
                        if (effectDecorator is DamageReduceWithTargetCountDecorator reduceWithTargetCountDecorator)
                        {
                            AddComponent<AbilityItemTargetCounterComponent>();
                        }
                    }
                }
            }

            //AddComponent<UpdateComponent>();
        }

        /// 结束单元体
        public void DestroyItem()
        {
            Log.Debug("AbilityItem DestroyItem");
            Destroy(this);
        }

        public override void OnDestroy()
        {
            Log.Debug("AbilityItem OnDestroy");
            var clipData = GetComponent<AbilityItemCollisionExecuteComponent>().ExecuteClipData;
            if (clipData.ExecuteClipType == ExecuteClipType.CollisionExecute && clipData.ActionEventData != null && clipData.ActionEventData.FireType == FireType.EndTrigger)
            {
                OnCollision(null);
            }
        }

        public void OnCollision(CombatEntity otherCombatEntity)
        {
            if (TargetEntity != null)
            {
                if (otherCombatEntity != TargetEntity)
                {
                    return;
                }
            }

            var collisionExecuteData = GetComponent<AbilityItemCollisionExecuteComponent>().CollisionExecuteData;

            if (AbilityEntity != null)
            {
                if (collisionExecuteData.ActionData.ActionEventType == FireEventType.AssignEffect)
                {
                    if (EffectApplyType == EffectApplyType.AllEffects)
                    {
                        AbilityEntity.GetComponent<AbilityEffectComponent>().TryAssignAllEffectsToTargetWithAbilityItem(otherCombatEntity, this);
                    }
                    else
                    {
                        AbilityEntity.GetComponent<AbilityEffectComponent>().TryAssignEffectByIndex(otherCombatEntity, (int)EffectApplyType - 1);
                    }
                }
            }

            if (AbilityExecution != null)
            {
                if (collisionExecuteData.ActionData.ActionEventType == FireEventType.TriggerNewExecution)
                {
                    OnTriggerNewExecution(collisionExecuteData.ActionData);
                }
            }

            if (TryGet(out AbilityItemTargetCounterComponent targetCounterComponent))
            {
                targetCounterComponent.TargetCounter++;
            }

            if (TargetEntity != null)
            {
                DestroyItem();
            }
        }

        public void OnTriggerNewExecution(ActionEventData ActionEventData)
        {
            Log.Debug($"AbilityItem OnTriggerNewExecution");
            var executionObject = AssetUtils.Load<ExecutionObject>(ActionEventData.NewExecution);
            if (executionObject == null)
            {
                Log.Error($"Can not find {ActionEventData.NewExecution}");
                return;
            }
            var sourceExecution = AbilityExecution as SkillExecution;
            var execution = sourceExecution.OwnerEntity.AddChild<SkillExecution>(sourceExecution.SkillAbility);
            execution.ExecutionObject = executionObject;
            //execution.InputTarget = sourceExecution.InputTarget;
            execution.InputPoint = Position;
            execution.LoadExecutionEffects();
            execution.BeginExecute();
            if (executionObject != null)
            {
                execution.AddComponent<UpdateComponent>();
            }
        }

        /// <summary>   目标飞行碰撞体     </summary>
        public void TargetFlyProcess(CombatEntity InputTarget)
        {
            var abilityItem = this;
            var clipData = abilityItem.Get<AbilityItemCollisionExecuteComponent>().ExecuteClipData;
            abilityItem.TargetEntity = InputTarget;
            abilityItem.Position = AbilityExecution.OwnerEntity.Position;
#if UNITY
            abilityItem.AddComponent<MoveWithDotweenComponent>().DoMoveToWithTime(InputTarget, clipData.Duration);
#endif
        }

        /// <summary>   前向飞行碰撞体     </summary>
        public void ForwardFlyProcess(float InputDirection)
        {
            var abilityItem = this;
            abilityItem.Position = AbilityExecution.OwnerEntity.Position;
            var x = System.Math.Sin(System.Math.PI / 180f * InputDirection);
            var z = System.Math.Cos(System.Math.PI / 180f * InputDirection);
            var destination = abilityItem.Position + new float3((float)x, 0, (float)z) * 30;
#if UNITY
            abilityItem.AddComponent<MoveWithDotweenComponent>().DoMoveTo(destination, 1f).OnMoveFinish(() => { Entity.Destroy(abilityItem); });
#endif
        }

        /// <summary>   路径飞行     </summary>
        public void PathFlyProcess()
        {
            var skillExecution = (AbilityExecution as SkillExecution);
            var abilityItem = this;
            var clipData = abilityItem.GetComponent<AbilityItemCollisionExecuteComponent>().ExecuteClipData;
            abilityItem.AddComponent<LifeTimeComponent>(clipData.Duration);
            var tempPoints = clipData.CollisionExecuteData.GetCtrlPoints();

            if (tempPoints.Count == 0)
            {
                return;
            }
            abilityItem.Position = AbilityExecution.OwnerEntity.Position + (float3)tempPoints[0].Position;
            var moveComp = abilityItem.AddComponent<AbilityItemBezierMoveComponent>();
            moveComp.PositionEntity = abilityItem;
            moveComp.BezierCurve = new NaughtyBezierCurves.BezierCurve3D();
            moveComp.BezierCurve.Position = AbilityExecution.OwnerEntity.Position;
            moveComp.BezierCurve.Sampling = clipData.CollisionExecuteData.BezierCurve.Sampling;
            moveComp.BezierCurve.KeyPoints = tempPoints;
            foreach (var item in tempPoints)
            {
                item.Curve = moveComp.BezierCurve;
            }
            if (skillExecution.ExecutionObject.TargetInputType == ExecutionTargetInputType.Point)
            {
                abilityItem.Position = (float3)skillExecution.InputPoint + (float3)tempPoints[0].Position;
                //Log.Debug($"InputPoint={InputPoint} {tempPoints[0].Position}");
                moveComp.BezierCurve.Position = skillExecution.InputPoint;
            }
            //moveComp.OriginPosition = OwnerEntity.Position;
            moveComp.RotateAgree = 0;
            moveComp.Duration = clipData.Duration;
            //moveComp.Speed = clipData.Duration / moveComp.BezierCurve.Sampling;
            //moveComp.DOMove();
        }

        /// <summary>   路径飞行     </summary>
        public void DirectionPathFlyProcess()
        {
            var abilityItem = this;
            var clipData = abilityItem.GetComponent<AbilityItemCollisionExecuteComponent>().ExecuteClipData;
            abilityItem.AddComponent<LifeTimeComponent>(clipData.Duration);
            var tempPoints = clipData.CollisionExecuteData.GetCtrlPoints();

            var angle = AbilityExecution.OwnerEntity.Rotation.y - 90;
            var agree = angle * MathF.PI / 180;

            if (tempPoints.Count == 0)
            {
                return;
            }
            abilityItem.Position = AbilityExecution.OwnerEntity.Position + (float3)tempPoints[0].Position;
            var moveComp = abilityItem.AddComponent<AbilityItemBezierMoveComponent>();
            moveComp.PositionEntity = abilityItem;
            moveComp.BezierCurve = new NaughtyBezierCurves.BezierCurve3D();
            moveComp.BezierCurve.Position = AbilityExecution.OwnerEntity.Position;
            moveComp.BezierCurve.Sampling = clipData.CollisionExecuteData.BezierCurve.Sampling;
            moveComp.BezierCurve.KeyPoints = tempPoints;
            foreach (var item in tempPoints)
            {
                item.Curve = moveComp.BezierCurve;
            }
            //moveComp.ctrlPoints = tempPoints;
            //moveComp.OriginPosition = OwnerEntity.Position;
            moveComp.RotateAgree = agree;
            moveComp.Speed = clipData.Duration / moveComp.BezierCurve.Sampling;
            moveComp.DOMove();
        }

        /// <summary>   固定位置碰撞体     </summary>
        public void FixedPositionProcess(float3 InputPoint)
        {
            var abilityItem = this;
            var clipData = abilityItem.Get<AbilityItemCollisionExecuteComponent>().ExecuteClipData;
            abilityItem.Position = (float3)InputPoint;
            abilityItem.AddComponent<LifeTimeComponent>(clipData.Duration);
        }

        /// <summary>   固定方向碰撞体     </summary>
        public void FixedDirectionProcess()
        {
            var abilityItem = this;
            var clipData = abilityItem.Get<AbilityItemCollisionExecuteComponent>().ExecuteClipData;
            abilityItem.Position = AbilityExecution.OwnerEntity.Position;
            abilityItem.Rotation = AbilityExecution.OwnerEntity.Rotation;
            abilityItem.AddComponent<LifeTimeComponent>(clipData.Duration);
        }

        /// <summary>   创建技能碰撞体     </summary>
        public ItemUnit AddCollisionComponent()
        {
            var abilityItem = this;
            var scene = AbilityExecution.OwnerEntity.Unit.GetParent<Scene>();
            var itemUnit = scene.AddChild<ItemUnit>();
            //scene.GetComponent<SceneUnitComponent>().
            itemUnit.OwnerUnit = AbilityExecution.OwnerEntity.Unit;
            itemUnit.AbilityItem = abilityItem;
            itemUnit.Position = abilityItem.Position;
            abilityItem.ItemUnit = itemUnit;
            if (AbilityEntity != null && AbilityEntity.As<SkillAbility>().SkillConfig != null)
            {
                itemUnit.ConfigId = AbilityEntity.As<SkillAbility>().SkillConfig.Id;
            }
            itemUnit.AddComponent<UnitCollisionComponent>();
            var moveComp = abilityItem.GetComponent<AbilityItemBezierMoveComponent>();
            if (moveComp != null)
            {
                itemUnit.AddComponent<UnitTranslateComponent>();
                itemUnit.AddComponent<UnitPathMoveComponent>();
                var points = moveComp.GetPathPoints();
                itemUnit.GetComponent<UnitPathMoveComponent>().Speed = moveComp.Speed;
                itemUnit.GetComponent<UnitPathMoveComponent>().PathPoints = points.ToList();
                var lifeTime = abilityItem.GetComponent<LifeTimeComponent>().LifeTimer.MaxTime * 1000;
                AOGame.PublishServer(new BroadcastUnitEvent() { Unit = itemUnit.MapUnit() });
#if UNITY
                AOGame.Publish(new CreateUnit() { MapUnit = itemUnit, IsMainAvatar = false });
#endif
                AOGame.Publish(new UnitPathMoveEvent() { Unit = itemUnit.MapUnit(), PathPoints = points, ArriveTime = (long)(TimeHelper.ServerNow() + lifeTime) });
            }
            else
            {
                AOGame.PublishServer(new BroadcastUnitEvent() { Unit = itemUnit.MapUnit() });
            }
            return itemUnit;
        }

#if UNITY
        /// <summary>   创建技能碰撞体     </summary>
        public GameObject CreateAbilityItemProxyObj()
        {
            var abilityItem = this;
            var proxyObj = new GameObject("AbilityItemProxy");
            proxyObj.transform.position = abilityItem.Position;
            proxyObj.transform.eulerAngles = abilityItem.Rotation;
            //abilityItem.AbilityItemTrans = proxyObj.transform;
            abilityItem.AddComponent<AbilityItemViewComponent>().AbilityItem = abilityItem;
            abilityItem.GetComponent<AbilityItemViewComponent>().AbilityItemTrans = proxyObj.transform;
            var clipData = abilityItem.GetComponent<AbilityItemCollisionExecuteComponent>().CollisionExecuteData;

            if (clipData.Shape == CollisionShape.Sphere)
            {
                proxyObj.AddComponent<SphereCollider>().enabled = false;
                proxyObj.GetComponent<SphereCollider>().radius = (float)clipData.Radius;
            }
            if (clipData.Shape == CollisionShape.Box)
            {
                proxyObj.AddComponent<BoxCollider>().enabled = false;
                proxyObj.GetComponent<BoxCollider>().center = clipData.Center;
                proxyObj.GetComponent<BoxCollider>().size = clipData.Size;
            }

            if (clipData.ActionData.FireType == FireType.CollisionTrigger)
            {
                proxyObj.AddComponent<OnTriggerEnterCallback>().OnTriggerEnterCallbackAction = (other) =>
                {
                    var combatEntity = CombatContext.Instance.Object2Entities[other.gameObject];
                    abilityItem.OnCollision(combatEntity);
                };
            }

            proxyObj.GetComponent<Collider>().enabled = true;

            if (clipData.ObjAsset != null)
            {
                abilityItem.Name = clipData.ObjAsset.name;
                var effectObj = GameObject.Instantiate(clipData.ObjAsset, proxyObj.transform);
                effectObj.transform.localPosition = Vector3.zero;
                effectObj.transform.localRotation = Quaternion.identity;
            }

            return proxyObj;
        }
#endif
    }
}