using AO;
using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ET
{
    public static class TimeUtils
    {
        public static async ETTask WaitAsync(long time)
        {
            await TimerComponent.Instance.WaitAsync(time);
        }
    }
}
