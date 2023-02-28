using System;
using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Method
{
    public interface IMethodEnterAspect : IMethodAspect
    {
        /// <summary>
        /// Invoked when the method is entered
        /// </summary>
        void OnMethodEnter(MethodExecutionArguments args);

    }
}