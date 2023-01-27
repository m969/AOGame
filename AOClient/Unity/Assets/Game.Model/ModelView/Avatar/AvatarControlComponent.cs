using ET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AO
{
    public class AvatarControlComponent : Entity, IAwake, IUpdate
    {
        public UnitViewComponent UnitViewComp { get; set; }
    }
}
