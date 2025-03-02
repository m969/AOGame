using AssetFile;
using ET;
using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AO
{
    public class UIStage : Entity, IAwake, IDestroy, IUpdate
    {
        public FairyGUI.Stage Stage { get; set; }
        public Dictionary<string, UIPackage> Packages { get; set; }
        public Dictionary<string, Asset> PackageAssets { get; set; }
    }
}
