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

    public interface IExecuteCommand : ICommand
    {
        Action ExecuteAction { get; set; }
    }
}
