using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public interface IEventRun
    {
    }

    public abstract class AEventRun : IEventRun
    {
        protected abstract ETTask Run();

        public async ETTask Handle()
        {
            try
            {
                await Run();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }

    public abstract class AEventRun<A> : IEventRun
    {
        protected abstract ETTask Run(A a);
        public async ETTask Handle(A a)
        {
            try
            {
                await Run(a);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }

    public abstract class AEventRun<A1, A2> : IEventRun
    {
        protected abstract ETTask Run(A1 a1, A2 a2);
        public async ETTask Handle(A1 a1, A2 a2)
        {
            try
            {
                await Run(a1, a2);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
