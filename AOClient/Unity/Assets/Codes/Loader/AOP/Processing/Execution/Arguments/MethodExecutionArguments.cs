using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ITnnovative.AOP.Processing.Execution.Arguments
{
    public class MethodExecutionArguments : BaseExecutionArgs
    {

        /// <summary>
        /// Method that is executed (original method with custom name), includes DeclaringType information, so it's ommited
        /// </summary>
        public MethodInfo method;
        
        /// <summary>
        /// Method that is root executor - original method without custom name
        /// </summary>
        public MethodInfo rootMethod;
        
        /// <summary>
        /// Arguments for this method
        /// </summary>
        public List<MethodArgument> arguments = new List<MethodArgument>();

        /// <summary>
        /// Return value
        /// </summary>
        public object returnValue;

      

        /// <summary>
        /// Gets argument for method execution
        /// </summary>
        public T GetArgument<T>(string name)
        {
            return (T) arguments.FirstOrDefault(a => a.name.Equals(name))?.value;
        }

     
    }
}