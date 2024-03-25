using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    /// <summary>
    /// 普通指令接口
    /// </summary>
    public interface ICommand
    {
        
    }

    /// <summary>
    /// 执行指令接口，在发出指令的同时可携带一个可传参的Action，用于处理指令的时候按需调用
    /// </summary>
    public interface IExecuteCommand : ICommand
    {
        public Action<object> ExecuteAction { get; set; }
    }
}
