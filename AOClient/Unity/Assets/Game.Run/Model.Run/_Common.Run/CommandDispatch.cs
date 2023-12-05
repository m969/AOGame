using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public static class CommandDispatch
    {
        public static void Dispatch<T>(T cmd) where T : struct, ICommand
        {
            AOGame.Root.GetComponent<AOEventComponent>().DispatchCommands.Enqueue(cmd);
        }
    }
}
