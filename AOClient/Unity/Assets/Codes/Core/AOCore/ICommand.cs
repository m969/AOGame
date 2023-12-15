using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public interface ICommand
    {
        
    }

    public abstract class AExecuteCommand : ICommand
    {
        public Action ExecuteAction { get; set; }
    }
}
