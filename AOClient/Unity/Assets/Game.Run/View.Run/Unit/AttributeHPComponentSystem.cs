using System;
using ET;
using GameUtils;
using Unity.Mathematics;
using TComp = AO.AttributeHPComponent;

namespace AO
{
    public static partial class AttributeHPComponentSystem
    {
        public static void Available_HP_Changed(this TComp self)
        {
            Log.Console($"AttributeHPComponentSystem Available_HP_Changed {self.Available_HP}");
            if (self.Parent.GetComponent<UnitPanelComponent>() == null )
            {
                return;
            }
            self.Parent.GetComponent<UnitPanelComponent>().SetHP(self.Available_HP);
        }
    }
}