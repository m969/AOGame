using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    /// <summary>
    /// 过程接口
    /// </summary>
    public interface IFuncProcess
    {
        
    }

    public abstract class AFuncProcess : IFuncProcess
    {
        public abstract ETTask Execute();
    }

    public abstract class AFuncProcess<A> : IFuncProcess
    {
        public abstract ETTask Execute(A a);
    }

    public abstract class AFuncProcess<A1, A2> : IFuncProcess
    {
        public abstract ETTask Execute(A1 a1, A2 a2);
    }
}
