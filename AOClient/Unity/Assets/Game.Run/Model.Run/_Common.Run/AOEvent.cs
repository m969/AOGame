using AO.Events;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    /// <summary>
    /// Event事件机制的开放方法类
    /// 事件是执行类，不保存自身状态，只通过传入的参数执行对应的逻辑
    /// 事件是Run层的机制，无法在Model层直接调用
    /// </summary>
    public static class AOEvent
    {
        private static void BeforeRun<T>(T eventRun) where T : IEventRun
        {
            AOGame.Root.GetComponent<EventComponent>().RunningEvents.Add(eventRun);
            AOCmd.Dispatch(new BeforeRunEventCmd() { EventRun = eventRun });
        }

        private static void AfterRun<T>(T eventRun) where T : IEventRun
        {
            AOCmd.Dispatch(new AfterRunEventCmd() { EventRun = eventRun });
            AOGame.Root.GetComponent<EventComponent>().RunningEvents.Remove(eventRun);
        }

        public static async ETTask Run<T>(T eventRun) where T : AEventRun
        {
            BeforeRun(eventRun);
            await eventRun.Handle();
            AfterRun(eventRun);
        }

        public static async ETTask Run<T, A>(T eventRun, A a) where T : AEventRun<A>
        {
            BeforeRun(eventRun);
            await eventRun.Handle(a);
            AfterRun(eventRun);
        }

        public static async ETTask Run<T, A1, A2>(T eventRun, A1 a1, A2 a2) where T : AEventRun<A1, A2>
        {
            BeforeRun(eventRun);
            await eventRun.Handle(a1, a2);
            AfterRun(eventRun);
        }
    }
}
