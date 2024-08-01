using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameUtils;
using System;
using NaughtyBezierCurves;
using ET;
#if EGAMEPLAY_ET
using AO;
using Unity.Mathematics;
using Vector3 = Unity.Mathematics.float3;
using ET.EventType;

#else
using float3 = UnityEngine.Vector3;
#endif
#if UNITY
using DG.Tweening;
#endif

namespace EGamePlay.Combat
{
    /// <summary>
    /// 
    /// </summary>
    public class AbilityItemPathMoveComponent : Component
    {
        public IPosition PositionEntity { get; set; }
        public float RotateAgree { get; set; }
        public float Duration { get; set; }
        public float Speed { get; set; } = 0.05f;
        public float Progress { get; set; }
        public BezierCurve3D BezierCurve { get; set; }
        public IPosition OriginEntity { get; set; }
        public Vector3 InputPoint { get; set; }
        public Vector3 OriginPoint
        {
            get
            {
                if (OriginEntity != null) return OriginEntity.Position;
                else return InputPoint;
            }
        }


        public override void Update()
        {
            var abilityItem = GetEntity<AbilityItem>();
            //abilityItem.Position = abilityItem.LocalPosition + OriginPoint;
#if EGAMEPLAY_ET
            var itemUnit = abilityItem.GetComponent<CombatUnitComponent>().Unit;
            if (itemUnit != null && !PositionEntity.Position.Equals(itemUnit.MapUnit().Position))
            {
                PositionEntity.Position = itemUnit.MapUnit().Position;
            }
#endif
        }

        public float3[] GetPathPoints()
        {
            var abilityItem = GetEntity<AbilityItem>();
            var points = GetPathLocalPoints();
            for (var i = 0; i < points.Length; i++)
            {
                var point = points[i];
                points[i] = point + OriginPoint;
            }
            return points;
        }

        public float3[] GetPathLocalPoints()
        {
            var abilityItem = GetEntity<AbilityItem>();
            var pathPoints = new float3[BezierCurve.Sampling];
            var perc = 1f / BezierCurve.Sampling;
            for (int i = 1; i <= BezierCurve.Sampling; i++)
            {
                var progress = perc * i;
                var endValue = BezierCurve.GetPoint(progress);
                pathPoints[i - 1] = endValue;
                //ET.Log.Console($"{progress} {endValue}");
            }

            var duration = Duration;
            var length = math.distance(pathPoints[0], abilityItem.LocalPosition);
            for (int i = 0; i < pathPoints.Length; i++)
            {
                if (i == pathPoints.Length - 1)
                {
                    break;
                }
                var dist = math.distance(pathPoints[i + 1], pathPoints[i]);
                length += dist;
            }
            var speed = length / duration;
            Speed = speed;

            return pathPoints;
        }

        public void FollowMove()
        {
            var localPos = GetEntity<AbilityItem>().LocalPosition;
            var endValue = OriginPoint + localPos;
            var startPos = PositionEntity.Position;
            var dist = math.distance(endValue, startPos);
            if (dist < 0.1f)
            {
                return;
            }
#if EGAMEPLAY_ET
            var itemUnit = GetEntity<AbilityItem>().GetComponent<CombatUnitComponent>().Unit;
            AOGame.Publish(new UnitPathMoveEvent() { Unit = itemUnit.MapUnit(), PathPoints = new Vector3[] { startPos, endValue } });
#else
#if UNITY
            var duration = dist / 2;
            DOTween.To(() => startPos, (x) => PositionEntity.Position = x, endValue, duration).SetEase(DG.Tweening.Ease.Linear);
#endif
#endif
        }

        public void DOMove()
        {
            Progress = 1f / BezierCurve.Sampling;

            var localPos = BezierCurve.GetPoint(Progress);
            GetEntity<AbilityItem>().LocalPosition = localPos;
            var endValue = OriginPoint + localPos;
            var startPos = PositionEntity.Position;
#if UNITY
            var duration = math.distance(endValue, startPos) / Speed;
            DOTween.To(() => startPos, (x) => PositionEntity.Position = x, endValue, duration).SetEase(Ease.Linear).OnComplete(DOMoveNext);
#else
            //EventSystem.Instance.Publish()
#endif
        }

        private void DOMoveNext()
        {
            if (Progress >= 1f)
            {
                return;
            }
            Progress += 1f / BezierCurve.Sampling;
            Progress = System.Math.Min(1f, Progress);
            var endValue = BezierCurve.GetPoint(Progress);
            var startPos = PositionEntity.Position;
#if UNITY
            var duration = math.distance(endValue, startPos) / Speed;
            DOTween.To(() => startPos, (x) => PositionEntity.Position = x, endValue, duration).SetEase(Ease.Linear).OnComplete(DOMoveNext);
#endif
        }
    }
}