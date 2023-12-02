using System;
using System.Diagnostics;
using System.Text;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Attributes.Property;
using ITnnovative.AOP.Processing.Execution.Arguments;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AO
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyChangedAttribute : Attribute, IPropertySetExitAspect
    {
        public void OnPropertySetExit(PropertyExecutionArguments args)
        {
            Debug.Log($"PropertyChangedAttribute OnPropertySetExit");
            if (args.instance is ET.Entity entity && entity.InstanceId == 0)
            {
                return;
            }
            Debug.Log($"PropertyChangedAttribute OnPropertySetExit {args.instance.GetType().Name} {args.property.Name} {args.newValue} {args.returnValue}");
            AOGame.Publish(new EventType.PropertyChangedEvent() { Instance = args.instance, PropertyName = args.property.Name });
        }
    }
}
