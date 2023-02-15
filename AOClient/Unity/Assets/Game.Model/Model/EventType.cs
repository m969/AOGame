using EGamePlay.Combat;
using ET;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

namespace AO.EventType
{
    public struct CreateUnit
    {
        public UnitInfo Unit;
        public IMapUnit MapUnit;
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

    public struct SpellActionEvent
    {
        public SpellAction SpellAction;
        public int Type;
        public int SkillId;

        public const int SpellStart = 1;
        public const int SpellEnd = 2;
    }

    public struct BroadcastUnitEvent
    {
        public IMapUnit Unit;
    }

    public struct UnitPathMoveEvent
    {
        public IMapUnit Unit;
        public float3[] PathPoints;
        public long ArriveTime;
    }
}
