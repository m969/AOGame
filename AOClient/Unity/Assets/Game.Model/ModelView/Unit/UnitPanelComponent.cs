using AssetFile;
using ET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AO
{
    public class UnitPanelComponent : Entity, IAwake, IDestroy, IUpdate
    {
        public FairyGUI.UIPanel UnitPanel { get; set; }
    }
}
