using AssetFile;
using ET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AO
{
    public class UnitViewComponent : Entity, IAwake<Asset>, IDestroy
    {
        public GameObject UnitObj { get; set; }
    }
}
