using AssetFile;
using ET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AO
{
    public class UIPackageComponent : Entity, IAwake, IDestroy, IUpdate
    {
        public FairyGUI.Stage Stage { get; set; }
    }
}
