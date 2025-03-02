using AO;
using Puerts;
using System;
using UnityEngine;

namespace ET
{
    public class UnitGroup : Singleton<UnitGroup>
    {
        public Transform RootTrans { get; private set; }
        public Transform ActorTrans { get; private set; }

        public UnitGroup()
        {
            RootTrans = GameObject.Find($"[ {this.GetType().Name} ]").transform;
            ActorTrans = RootTrans.Find("Actor");
        }
    }
}