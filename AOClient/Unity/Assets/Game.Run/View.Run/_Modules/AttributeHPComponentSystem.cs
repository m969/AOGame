using System;
using ET;
using GameUtils;
using Unity.Mathematics;
using TComp = AO.AttributeHPComponent;

namespace AO
{
    public static partial class AttributeHPComponentSystem
    {
        public static void AvailableValue_Changed(this TComp self)
        {
            Log.Console($"AttributeHPComponentSystem AvailableValue_Changed {self.AvailableValue}");
            if (self.Parent.GetComponent<UnitPanelComponent>() == null )
            {
                return;
            }
            self.Parent.GetComponent<UnitPanelComponent>().SetHP(self.AvailableValue);
        }
    }
}