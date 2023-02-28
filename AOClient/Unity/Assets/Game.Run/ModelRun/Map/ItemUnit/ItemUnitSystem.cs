using EGamePlay.Combat;
using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TComp = AO.ItemUnit;

namespace AO
{
    public static partial class ItemUnitSystem
    {
#if UNITY
        public class AwakeSystemObject : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                
            }
        }
#endif
    }
}