using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    /// <summary>
    /// 状态指示接口
    /// 
    /// 这里的状态指示不是事件，而是代表一种状态指示信号，一般用作全局临时状态的表示
    /// 比如场景出现Boss，就可以设置一个场景危机状态指示信号，角色的行为动作都会因为这个状态指示改变
    /// 比如红绿灯状态指示信号，红灯停绿灯行，汽车的行为也因为这个状态指示改变
    /// 
    /// 为什么用接口和类来表示状态指示，而不是用枚举，因为类和接口更方便扩展
    /// 比如危机状态信号可以分级别，高级危机状态信号可以继承低级危机状态信号
    /// 还有高级危机状态信号可以扩展出Boss高级危机状态信号、地震高级危机状态信号、病毒高级危机状态信号等等
    /// </summary>
    public interface IStateTag
    {

    }
}
