using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public interface IUnitState
    {

    }

    //public interface IUnitStateSystem : ISystemType
    //{
    //    void Run(Entity o);
    //}

    //[ObjectSystem]
    //public abstract class UnitStateSystem<T> : IUnitStateSystem where T : Entity, IDestroy
    //{
    //    void IUnitStateSystem.Run(Entity o)
    //    {
    //        this.Destroy((T)o);
    //    }

    //    Type ISystemType.SystemType()
    //    {
    //        return typeof(IUnitStateSystem);
    //    }

    //    InstanceQueueIndex ISystemType.GetInstanceQueueIndex()
    //    {
    //        return InstanceQueueIndex.None;
    //    }

    //    Type ISystemType.Type()
    //    {
    //        return typeof(T);
    //    }

    //    protected abstract void Destroy(T self);
    //}
}
