using System;

namespace ET
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class NotifySelfAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NotifyAOIAttribute : Attribute
    {
    }
}