using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Method
{
    public interface IMethodExitAspect : IMethodAspect
    {
        /// <summary>
        /// Invoked when the method is exiting
        /// </summary>
        void OnMethodExit(MethodExecutionArguments args);

    }
}