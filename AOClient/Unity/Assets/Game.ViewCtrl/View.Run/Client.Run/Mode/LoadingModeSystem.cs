using ET;
using Puerts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ET.DisposeActionComponent;

namespace AO
{
    public static class LoadingModeSystem
    {
        [ObjectSystem]
        public class AwakeHandler : AwakeSystem<LoadingModeComponent>
        {
            protected override void Awake(LoadingModeComponent self)
            {
                self.LoadingProgress = 0;
            }
        }

        public static void SetProgressValue(this LoadingModeComponent self, float progress)
        {
            self.LoadingProgress = progress;
        }
    }
}