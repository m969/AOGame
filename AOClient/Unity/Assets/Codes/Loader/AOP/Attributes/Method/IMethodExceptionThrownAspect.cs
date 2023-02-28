using System;
using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Method
{
    public interface IMethodExceptionThrownAspect : IMethodAspect
    {

        void OnExceptionThrown(Exception exception, MethodExecutionArguments args);

    }
}