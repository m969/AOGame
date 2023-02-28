using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IAfterEventInvokedAspect : IEventInvokedAspect
    {
        void BeforeEventInvoked(EventExecutionArguments args);
    }
}