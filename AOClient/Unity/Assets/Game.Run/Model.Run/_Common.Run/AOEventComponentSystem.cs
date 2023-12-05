using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public static class AOEventComponentSystem
    {
        public class AOEventComponentAwakeSystem : AwakeSystem<AOEventComponent>
        {
            protected override void Awake(AOEventComponent self)
            {
            }
        }

        public class AOEventComponentUpdateSystem : UpdateSystem<AOEventComponent>
        {
            protected override void Update(AOEventComponent self)
            {
                while (self.DispatchCommands.Count > 0)
                {
                    var cmd = self.DispatchCommands.Dequeue();
                    if (self.CommandHandlers.TryGetValue(cmd.GetType(), out var handlers))
                    {
                        foreach (var handler in handlers)
                        {
                            handler.Handle(cmd);
                        }
                    }
                }
            }
        }
    }
}
