using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public interface ICommandHandler
    {
        void Handle<T>(T cmd) where T : ICommand;
    }
}
