using System;
using System.Collections.Generic;
using System.Reflection;

namespace ITnnovative.AOP.Processing.Execution
{
    public static class Tools
    {

        /// <summary>
        /// Gets method from type using parameters
        /// </summary>
        public static MethodInfo GetMethod(this Type t, string methodName, List<Type> paramTypes, out List<Type> genericTypes)
        {
            genericTypes = new List<Type>();
            
            var pTypes = paramTypes?.ToArray();
            var method = t.GetMethod(methodName, pTypes);

            if (method == null)
            {
                // Get all methods
                var methods = t.GetMethods((BindingFlags) int.MaxValue);
                foreach (var m in methods)
                {
                    genericTypes.Clear();
                    // If name differs, continue
                    if (m.Name != methodName) continue;

                    // Get params 
                    var methodParams = m.GetParameters();
 
                    // Ignore if params length does not match
                    if (methodParams.Length != pTypes.Length) continue;
                    
                    for (var index = 0; index < methodParams.Length; index++)
                    {
                        var param = methodParams[index];
                        // Check if params match
                        if (param.ParameterType != pTypes[index])
                        {
                            // Compare type to arguments (check if is generic)
                            if(!param.ParameterType.IsGenericParameter)
                                goto continue_job;

                            // Register generic type
                            genericTypes.Add(pTypes[index]);
                        }
                    }

                    method = m;
                    break;
                    continue_job: ;
                }

            }

            return method;
        }

    }
}