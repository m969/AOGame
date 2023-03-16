using ET;
using ET.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    [Event(SceneType.Map)]
    public class UnitAttributeNumericChanged_ChangeUnit : AEvent<EventType.UnitAttributeNumericChanged>
    {
        protected override async ETTask Run(Entity source, EventType.UnitAttributeNumericChanged a)
        {
            var unit = a.Unit;
            if (a.AttributeNumeric.AttributeType == EGamePlay.Combat.AttributeType.HealthPointMax)
            {
                unit.GetComponent<AttributeHPComponent>().Attribute_HP = (int)a.AttributeNumeric.Value;
            }
            if (a.AttributeNumeric.AttributeType == EGamePlay.Combat.AttributeType.HealthPoint)
            {
                unit.GetComponent<AttributeHPComponent>().Available_HP = (int)a.AttributeNumeric.Value;
            }
        }
    }
}
