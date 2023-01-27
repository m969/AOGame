using System;

namespace ET
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class NotifyAttribute : Attribute
    {
    }
}