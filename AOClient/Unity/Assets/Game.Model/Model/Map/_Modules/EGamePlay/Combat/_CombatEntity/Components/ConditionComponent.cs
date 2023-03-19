using System.Collections.Generic;
using System;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 条件管理组件，在这里管理一个战斗实体所有条件达成事件的添加监听、移除监听、触发流程
    /// </summary>
    public sealed class ConditionEventComponent : Component
    {
        private Dictionary<Action, Entity> ConditionEvents { get; set; } = new Dictionary<Action, Entity>();


        public void AddListener(ConditionEventType conditionType, Action action, object paramObj = null)
        {
            switch (conditionType)
            {
                case ConditionEventType.WhenInTimeNoDamage:
                    {
                        var time = (float)paramObj;
                        var condition = Entity.AddChild<ConditionEvent>();
                        var comp = condition.AddComponent<ConditionWhenInTimeNoDamageComponent>(time);
                        ConditionEvents.Add(action, condition);
                        comp.StartListen(action);
                        break;
                    }
                case ConditionEventType.WhenHPLower:
                    break;
                case ConditionEventType.WhenHPPctLower:
                    break;
                case ConditionEventType.WhenIntervalTime:
                    {
                        var time = (float)paramObj;
                        var condition = Entity.AddChild<ConditionTimeIntervalEvent>();
                        ConditionEvents.Add(action, condition);
                        break;
                    }
                default:
                    break;
            }
        }

        public void RemoveListener(ConditionEventType conditionType, Action action)
        {
            if (ConditionEvents.ContainsKey(action))
            {
                Entity.Destroy(ConditionEvents[action]);
                ConditionEvents.Remove(action);
            }
        }
    }
}