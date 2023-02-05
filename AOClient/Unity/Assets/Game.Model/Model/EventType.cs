using ET;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

namespace AO.EventType
{
    public struct CreateUnit
    {
        public UnitInfo Unit;
        public bool IsMainAvatar;
    }

    public struct UnitMove
    {
        public Entity Unit;
        public int Type;

        public const int MoveStart = 1;
        public const int MoveEnd = 2;
    }

    public struct ChangePosition
    {
        public Entity Unit;
        public float3 OldPos;
    }

    public struct ChangeRotation
    {
        public Entity Unit;
    }
}
