using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    /// <summary>
    /// Event事件机制的开放方法类
    /// </summary>
    public static class AOEvent
    {
        public static void Run<T>() where T : IEventRun
        {
            var eventRun = Activator.CreateInstance<T>();
            AOGame.Root.GetComponent<EventComponent>().RunningEvents.Add(eventRun);
            eventRun.Run();
            AOGame.Root.GetComponent<EventComponent>().RunningEvents.Remove(eventRun);
        }
    }
}
