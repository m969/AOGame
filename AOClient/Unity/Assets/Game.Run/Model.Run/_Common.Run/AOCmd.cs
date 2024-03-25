using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    /// <summary>
    /// Command指令机制的开放方法类
    /// 指令是结构体，指令生成后数据不可更改
    /// 指令是跨层级的机制，不同层级或模块之间可以通过指令进行通信
    /// </summary>
    public static class AOCmd
    {
        public static void Dispatch<T>(T cmd) where T : struct, ICommand
        {
            AOGame.Root.GetComponent<EventComponent>().DispatchCommands.Enqueue(cmd);
        }

        public static void Execute<T>(T cmd) where T : struct, IExecuteCommand
        {
            AOGame.Root.GetComponent<EventComponent>().ExecuteCommands.Enqueue(cmd);
        }
    }
}
