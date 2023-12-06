using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    public class SignalData
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public long StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long EndTime { get; set; }

        /// <summary>
        /// 信号等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;
    }

    public class AOSignalComponent : Entity, IAwake, IUpdate
    {
        public Dictionary<ISignal, SignalData> Signals { get; set; } = new();
    }
}
