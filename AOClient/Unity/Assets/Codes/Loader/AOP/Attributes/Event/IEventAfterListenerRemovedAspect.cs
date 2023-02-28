using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IEventAfterListenerRemovedAspect : IEventRemovedListenerAspect
    {
        void AfterEventListenerRemoved(EventExecutionArguments arguments);
    }
}