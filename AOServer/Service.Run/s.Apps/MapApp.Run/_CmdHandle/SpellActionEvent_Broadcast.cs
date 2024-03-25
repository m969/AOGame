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
    public class SpellActionEvent_Broadcast : AEvent<SpellActionEvent>
    {
        protected override async ETTask Run(Entity source, SpellActionEvent a)
        {
            var unit = a.SpellAction.Creator.Unit;
            if (a.Type == SpellActionEvent.SpellStart)
            {
                MessageHelper.Broadcast(unit.MapUnit(), new M2C_SpellStart() { Position = unit.MapUnit().Position, UnitId = unit.Id, SkillId = a.SpellAction.SkillAbility.SkillConfig.Id });
            }
            if (a.Type == SpellActionEvent.SpellEnd)
            {
                MessageHelper.Broadcast(unit.MapUnit(), new M2C_SpellEnd() { Position = unit.MapUnit().Position, UnitId = unit.Id, SkillId = a.SpellAction.SkillAbility.SkillConfig.Id });
            }
        }
    }
}
