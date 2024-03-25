using ET;
using ET.EventType;
using ET.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    [Event(SceneType.Map)]
    public class UnitAttributeNumericChanged_ChangeUnit : AEvent<UnitAttributeNumericChanged>
    {
        protected override async ETTask Run(Entity source, UnitAttributeNumericChanged a)
        {
            //Log.Console($"UnitAttributeNumericChanged_ChangeUnit {a.AttributeNumeric.AttributeType}");
            var unit = a.Unit;
            if (a.AttributeNumeric.AttributeType == EGamePlay.Combat.AttributeType.HealthPointMax)
            {
                unit.GetComponent<AttributeHPComponent>().AttributeValue = (int)a.AttributeNumeric.Value;
            }
            if (a.AttributeNumeric.AttributeType == EGamePlay.Combat.AttributeType.HealthPoint)
            {
                unit.GetComponent<AttributeHPComponent>().AvailableValue = (int)a.AttributeNumeric.Value;
            }
        }
    }
}
