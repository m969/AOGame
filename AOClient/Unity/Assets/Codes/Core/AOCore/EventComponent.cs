using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public class EventComponent : Entity, IAwake, IUpdate
    {
        public List<IEventRun> RunningEvents { get; set; } = new();
        public Queue<ICommand> DispatchCommands { get; set; } = new();
        public Queue<IExecuteCommand> ExecuteCommands { get; set; } = new();
        public Dictionary<Type, List<ICommandHandler>> CommandHandlers { get; set; } = new();
    }
}
