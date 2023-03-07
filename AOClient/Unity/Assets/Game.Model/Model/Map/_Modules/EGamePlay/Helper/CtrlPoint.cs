using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace EGamePlay
{
    public enum BezierPointType
    {
        corner,
        smooth,
        bezierCorner,
    }

    [System.Serializable]
    public class CtrlPoint
    {
        public BezierPointType type;
        public float3 position;
        [SerializeField]
        float3 inTangent;
        [SerializeField]
        float3 outTangent;

        public float3 InTangent
        {
            get
            {
                if (type == BezierPointType.corner) return float3.zero;
                else return inTangent;
            }
            set
            {
                if (type != BezierPointType.corner) inTangent = value;
                if (math.length(value) > 0.001 && type == BezierPointType.smooth)
                {
                    outTangent = math.normalize(value) * (-1) * math.length(outTangent);
                }
            }
        }

        public float3 OutTangent
        {
            get
            {
                if (type == BezierPointType.corner) return float3.zero;
                if (type == BezierPointType.smooth)
                {
                    if (math.length(inTangent) > 0.001)
                    {
                        return math.normalize(inTangent) * (-1) * math.length(outTangent);
                    }
                }
                return outTangent;
            }
            set
            {
                if (type == BezierPointType.smooth)
                {
                    if (math.length(value) > 0.001)
                    {
                        inTangent = math.normalize(value) * (-1) * math.length(inTangent);
                    }
                    outTangent = value;
                }
                if (type == BezierPointType.bezierCorner) outTangent = value;
            }
        }
    }
}
