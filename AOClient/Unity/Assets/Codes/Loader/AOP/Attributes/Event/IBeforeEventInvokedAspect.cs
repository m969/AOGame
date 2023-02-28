using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IBeforeEventInvokedAspect : IEventInvokedAspect
    {
        void BeforeEventInvoked(EventExecutionArguments args);
    }
}