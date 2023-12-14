using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    /// <summary>
    /// 指示信号接口
    /// 
    /// 这里的信号不是事件，而是代表一种指示状态，一般用作全局临时状态的表示
    /// 比如场景出现Boss，就可以设置一个场景危机信号状态，角色的行为动作都会因为这个信号状态改变
    /// 比如红绿灯信号，红灯停绿灯行，汽车的行为也因为这个信号状态改变
    /// 
    /// 为什么用接口和类来表示信号，而不是用枚举，因为类和接口更方便扩展
    /// 比如危机信号可以分级别，高级危机信号可以继承低级危机信号
    /// 还有高级危机信号可以扩展出Boss高级危机信号、地震高级危机信号、病毒高级危机信号等等
    /// </summary>
    public interface ISignal
    {
        
    }
}
