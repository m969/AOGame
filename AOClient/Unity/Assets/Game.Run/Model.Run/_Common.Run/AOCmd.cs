using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    /// <summary>
    /// Command指令机制的开放方法类
    /// </summary>
    public static class AOCmd
    {
        public static void Dispatch<T>(T cmd) where T : struct, ICommand
        {
            AOGame.Root.GetComponent<EventComponent>().DispatchCommands.Enqueue(cmd);
        }

        public static void Execute<T>(T cmd) where T : AExecuteCommand
        {
            AOGame.Root.GetComponent<EventComponent>().ExecuteCommands.Enqueue(cmd);
        }
    }
}
