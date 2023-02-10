using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using EGamePlay.Combat;
using ET;
using Log = EGamePlay.Log;
using Unity.Mathematics;
using System;
using AO;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 技能执行体，执行体就是控制角色表现和技能表现的，包括角色动作、移动、变身等表现的，以及技能生成碰撞体等表现
    /// </summary>
    [EnableUpdate]
    public sealed partial class SkillExecution : Entity, IAbilityExecution
    {
        public Entity AbilityEntity { get; set; }
        public CombatEntity OwnerEntity { get; set; }
        public SkillAbility SkillAbility { get { return AbilityEntity as SkillAbility; } }
        public ExecutionObject ExecutionObject { get; set; }
        public List<CombatEntity> SkillTargets { get; set; } = new List<CombatEntity>();
        public CombatEntity InputTarget { get; set; }
        public float3? InputPoint { get; set; }
        public float InputDirection { get; set; }
        public long OriginTime { get; set; }
        /// 行为占用
        public bool ActionOccupy { get; set; } = true;


        public override void Awake(object initData)
        {
            AbilityEntity = initData as SkillAbility;
            OwnerEntity = GetParent<CombatEntity>();
            OriginTime = ET.TimeHelper.ServerNow();
        }

        public void LoadExecutionEffects()
        {
            AddComponent<ExecutionClipComponent>();
        }

        public override void Update()
        {
            //if (SkillAbility.Spelling == false)
            //{
            //    return;
            //}

            var nowSeconds = (double)(ET.TimeHelper.ClientNow() - OriginTime) / 1000;

            if (nowSeconds >= ExecutionObject.TotalTime)
            {
                EndExecute();
            }
        }

        public void BeginExecute()
        {
            GetParent<CombatEntity>().SpellingExecution = this;
            if (SkillAbility != null)
            {
                SkillAbility.Spelling = true;
            }

            Get<ExecutionClipComponent>().BeginExecute();

            FireEvent(nameof(BeginExecute));
        }

        public void EndExecute()
        {
            //Log.Debug("SkillExecution EndExecute");
            GetParent<CombatEntity>().SpellingExecution = null;
            if (SkillAbility != null)
            {
                SkillAbility.Spelling = false;
            }
            SkillTargets.Clear();
            Entity.Destroy(this);
            //base.EndExecute();
        }

        /// <summary>   技能碰撞体生成事件   </summary>
        public void SpawnCollisionItem(ExecuteClipData clipData)
        {
            //Log.Debug($"SkillExecution SpawnCollisionItem {clipData.StartTime} {clipData.Duration}");

            var abilityItem = Entity.Create<AbilityItem>(this);
            abilityItem.AddComponent<AbilityItemCollisionExecuteComponent>(clipData);

            if (clipData.CollisionExecuteData.MoveType == CollisionMoveType.PathFly) PathFlyProcess(abilityItem);
            if (clipData.CollisionExecuteData.MoveType == CollisionMoveType.SelectedDirectionPathFly) PathFlyProcess(abilityItem);
            if (clipData.CollisionExecuteData.MoveType == CollisionMoveType.TargetFly) TargetFlyProcess(abilityItem);
            if (clipData.CollisionExecuteData.MoveType == CollisionMoveType.ForwardFly) ForwardFlyProcess(abilityItem);
            if (clipData.CollisionExecuteData.MoveType == CollisionMoveType.SelectedPosition) FixedPositionProcess(abilityItem);
            if (clipData.CollisionExecuteData.MoveType == CollisionMoveType.SelectedDirection) FixedDirectionProcess(abilityItem);
            AddCollisionComponent(abilityItem);

#if UNITY
            CreateAbilityItemProxyObj(abilityItem);
#endif
        }

        /// <summary>   目标飞行碰撞体     </summary>
        private void TargetFlyProcess(AbilityItem abilityItem)
        {
            var clipData = abilityItem.Get<AbilityItemCollisionExecuteComponent>().ExecuteClipData;
            abilityItem.TargetEntity = InputTarget;
            abilityItem.Position = OwnerEntity.Position;
#if UNITY
            abilityItem.AddComponent<MoveWithDotweenComponent>().DoMoveToWithTime(InputTarget, clipData.Duration);
#endif
        }

        /// <summary>   前向飞行碰撞体     </summary>
        private void ForwardFlyProcess(AbilityItem abilityItem)
        {
            abilityItem.Position = OwnerEntity.Position;
            var x = System.Math.Sin(System.Math.PI / 180f * InputDirection);
            var z = System.Math.Cos(System.Math.PI / 180f * InputDirection);
            var destination = abilityItem.Position + new float3((float)x, 0, (float)z) * 30;
#if UNITY
            abilityItem.AddComponent<MoveWithDotweenComponent>().DoMoveTo(destination, 1f).OnMoveFinish(() => { Entity.Destroy(abilityItem); });
#endif
        }

        /// <summary>   路径飞行     </summary>
        private void PathFlyProcess(AbilityItem abilityItem)
        {
            var clipData = abilityItem.Get<AbilityItemCollisionExecuteComponent>().ExecuteClipData;
            var tempPoints = clipData.CollisionExecuteData.GetCtrlPoints();

            var angle = OwnerEntity.Rotation.y - 90;
            var agree = angle * MathF.PI / 180;

            abilityItem.Position = OwnerEntity.Position + tempPoints[0].position;
            var moveComp = abilityItem.Add<AbilityItemBezierMoveComponent>();
            moveComp.PositionEntity = abilityItem;
            moveComp.ctrlPoints = tempPoints;
            moveComp.OriginPosition = OwnerEntity.Position;
            moveComp.RotateAgree = agree;
            moveComp.Speed = clipData.Duration / 10;
            moveComp.DOMove();
            abilityItem.Add<LifeTimeComponent>(clipData.Duration);
        }

        /// <summary>   固定位置碰撞体     </summary>
        private void FixedPositionProcess(AbilityItem abilityItem)
        {
            var clipData = abilityItem.Get<AbilityItemCollisionExecuteComponent>().ExecuteClipData;
            abilityItem.Position = (float3)InputPoint;
            abilityItem.AddComponent<LifeTimeComponent>(clipData.Duration);
        }

        /// <summary>   固定方向碰撞体     </summary>
        private void FixedDirectionProcess(AbilityItem abilityItem)
        {
            var clipData = abilityItem.Get<AbilityItemCollisionExecuteComponent>().ExecuteClipData;
            abilityItem.Position = OwnerEntity.Position;
            abilityItem.Rotation = OwnerEntity.Rotation;
            abilityItem.AddComponent<LifeTimeComponent>(clipData.Duration);
        }

        /// <summary>   创建技能碰撞体     </summary>
        public AbilityUnit AddCollisionComponent(AbilityItem abilityItem)
        {
            var itemUnit = OwnerEntity.Unit.GetParent<Scene>().AddChild<AbilityUnit>();
            itemUnit.OwnerUnit = OwnerEntity.Unit;
            itemUnit.AbilityItem = abilityItem;
            itemUnit.Position = abilityItem.Position;
            itemUnit.AddComponent<UnitCollisionComponent>();
            return itemUnit;
        }

#if UNITY
        /// <summary>   创建技能碰撞体     </summary>
        public GameObject CreateAbilityItemProxyObj(AbilityItem abilityItem)
        {
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

            //proxyObj.AddComponent<OnTriggerEnterCallback>().OnTriggerEnterCallbackAction = (other) => {
            //    var combatEntity = CombatContext.Instance.Object2Entities[other.gameObject];
            //    abilityItem.OnCollision(combatEntity);
            //};

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