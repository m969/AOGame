using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public static class EventRunner
    {
        public static void Run<T>() where T : IEventRun
        {
            var eventRun = Activator.CreateInstance<T>();
            //AOGame.Root.GetComponent<AOEventComponent>().RunningEvents.Add(eventRun);
            eventRun.Run();
        }
    }
}
