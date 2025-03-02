using AO;
using Puerts;
using System;
using UnityEngine;

namespace ET
{
    public class CameraGroup : Singleton<CameraGroup>
    {
        public Transform RootTrans { get; private set; }
        public Camera MainCam { get; private set; }
        public Camera StageCam { get; private set; }

        public CameraGroup()
        {
            RootTrans = GameObject.Find($"[ {this.GetType().Name} ]").transform;
            MainCam = RootTrans.Find("Main Camera").GetComponent<Camera>();
            StageCam = RootTrans.Find("Stage Camera").GetComponent<Camera>();
        }
    }
}