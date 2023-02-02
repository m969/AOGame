using AO.EventType;
using AspectInjector.Broker;
using ET;
using System.ComponentModel;

namespace AO
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    [Injection(typeof(NotifyAspect))]
    public class NotifySelfAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    [Injection(typeof(NotifyAspect))]
    public class NotifyAOIAttribute : Attribute
    {
    }

    //[AttributeUsage(AttributeTargets.Property)]
    //[Injection(typeof(NotifyAspect))]
    //public class NotifyAlsoAttribute : Attribute
    //{
    //    public NotifyAlsoAttribute(params string[] notifyAlso) => NotifyAlso = notifyAlso;
    //    public string[] NotifyAlso { get; }
    //}


    [Mixin(typeof(INotifyPropertyChanged))]
    [Aspect(Scope.Global)]
    public class NotifyAspect : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };

        [Advice(Kind.After, Targets = Target.Public | Target.Setter)]
        public void AfterSetter(
            [Argument(Source.Instance)] object source,
            [Argument(Source.Name)] string propName,
            [Argument(Source.Triggers)] Attribute[] triggers
            )
        {
            if (triggers.OfType<NotifyAOIAttribute>().Any())
            {
                Log.Console($"NotifyAOIAttribute {source.GetType()} {propName}");
                PropertyChanged(source, new PropertyChangedEventArgs(propName));
                if (source is Entity entity)
                {
                    var sourceType = source.GetType();
                    var property = sourceType.GetProperty(propName);
                    var value = property.GetValue(source);
                    var valueBytes = ProtobufHelper.Serialize(value);
                    if (entity.Parent is IMapUnit unit)
                    {
                        var unitId = entity.Parent.Id;
                        var componentName = sourceType.Name;
                        var msg = new M2C_ComponentPropertyNotify() { UnitId = unitId, ComponentName = componentName, PropertyName = propName, PropertyBytes = valueBytes };
                        AOGame.Publish(new AO.EventType.BroadcastEvent() { Unit = unit, Message = msg });
                    }
                }
                return;
            }

            if (triggers.OfType<NotifySelfAttribute>().Any())
            {
                Log.Console($"NotifySelfAttribute {source.GetType()} {propName}");
                PropertyChanged(source, new PropertyChangedEventArgs(propName));
                if (source is Entity entity)
                {
                    var sourceType = source.GetType();
                    var property = sourceType.GetProperty(propName);
                    var value = property.GetValue(source);
                    var valueBytes = ProtobufHelper.Serialize(value);
                    if (entity.Parent is IUnit unit)
                    {
                        var unitId = entity.Parent.Id;
                        var componentName = sourceType.Name;
                        var msg = new M2C_ComponentPropertyNotify() { UnitId = unitId, ComponentName = componentName, PropertyName = propName, PropertyBytes = valueBytes };
                        AOGame.Publish(new ActorSendEvent() { ActorId = unitId, Message = msg });
                    }
                }
            }

            //foreach (var attr in triggers.OfType<NotifyAlsoAttribute>())
            //    foreach (var additional in attr.NotifyAlso ?? new string[] { })
            //        PropertyChanged(source, new PropertyChangedEventArgs(additional));
        }
    }
}
