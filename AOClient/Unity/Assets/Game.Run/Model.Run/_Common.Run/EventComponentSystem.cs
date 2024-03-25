using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public static class EventComponentSystem
    {
        public class EventComponentAwakeSystem : AwakeSystem<EventComponent>
        {
            protected override void Awake(EventComponent self)
            {
                self.CommandHandlers.Clear();
                var types = EventSystem.Instance.GetTypes(typeof(CommandAttribute));
                foreach (var item in types)
                {
                    var handler = Activator.CreateInstance(item) as ICommandHandler;
                    var cmdType = handler.Type;
                    self.CommandHandlers.TryGetValue(cmdType, out var handlers);
                    if (handlers == null)
                    {
                        handlers = new List<ICommandHandler>();
                        self.CommandHandlers[cmdType] = handlers;
                    }
                    handlers.Add(handler);
                }
            }
        }

        public class EventComponentUpdateSystem : UpdateSystem<EventComponent>
        {
            protected override void Update(EventComponent self)
            {
                while (self.DispatchCommands.Count > 0)
                {
                    var cmd = self.DispatchCommands.Dequeue();
                    if (self.CommandHandlers.TryGetValue(cmd.GetType(), out var handlers))
                    {
                        foreach (var handler in handlers)
                        {
                            handler.HandleCmd(cmd);
                        }
                    }
                }
            }
        }
    }
}
