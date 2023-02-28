using System.Reflection;

namespace ITnnovative.AOP.Processing.Execution.Arguments
{
    public class PropertyExecutionArguments : BaseExecutionArgs
    {
        /// <summary>
        /// True if setting property
        /// </summary>
        public bool isSetArguments;

        public object instance;

        /// <summary>
        /// New property value
        /// </summary>
        public object newValue;

        /// <summary>
        /// Property
        /// </summary>
        public PropertyInfo property;

        /// <summary>
        /// Property return value
        /// </summary>
        public object returnValue;
    }
}