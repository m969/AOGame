using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 行动点，一次战斗行动<see cref="IActionExecution"/>会触发战斗实体一系列的行动点<see cref="ActionPoint"/>
    /// </summary>
    public sealed class ActionPoint
    {
        public List<Action<Entity>> Listeners { get; set; } = new List<Action<Entity>>();


        public void AddListener(Action<Entity> action)
        {
            Listeners.Add(action);
        }

        public void RemoveListener(Action<Entity> action)
        {
            Listeners.Remove(action);
        }

        public void TriggerAllActions(Entity actionExecution)
        {
            if (Listeners.Count == 0)
            {
                return;
            }
            for (int i = Listeners.Count - 1; i >= 0; i--)
            {
                var item = Listeners[i];
                item.Invoke(actionExecution);
            }
        }
    }
}
