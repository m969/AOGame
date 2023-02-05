using ET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AO
{
    public enum AnimationType
    {
        None = 0,
        Idle = 1,
        Walk = 2,
        Run = 3,
        Die = 4,
        Attack = 5,
        Gethit = 6,
    }

    public class UnitAnimationComponent : Entity, IAwake
    {
        public Animation Animation { get; set; }
    }
}
