using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public static class CmdUtils
    {
        public static void Dispatch<T>(T cmd) where T : struct, ICommand
        {
            AOGame.Root.GetComponent<AOEventComponent>().DispatchCommands.Enqueue(cmd);
        }

        public static void Execute<T>(T cmd) where T : struct, IExecuteCommand
        {
            AOGame.Root.GetComponent<AOEventComponent>().ExecuteCommands.Enqueue(cmd);
        }
    }
}
