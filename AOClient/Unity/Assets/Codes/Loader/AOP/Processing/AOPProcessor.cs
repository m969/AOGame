using System;
using System.Collections.Generic;
using System.Linq;
using ITnnovative.AOP.Attributes.Event;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Attributes.Property;
using ITnnovative.AOP.Processing.Execution;
using ITnnovative.AOP.Processing.Execution.Arguments;
using ITnnovative.AOP.Processing.Execution.Arguments.Enums;
using UnityEngine;

namespace ITnnovative.AOP.Processing
{
    /// <summary>
    /// AOP Processor is a static class responsible for processing AOP Operations
    /// </summary>
    public static class AOPProcessor
    {
      
        public const string APPENDIX = "_UAOP";

        public static void BeforeEventInvoked(object instance, Type type, string eventName)
        {
            var arguments = new EventExecutionArguments();
            var evt = type?.GetEvent(eventName);
            arguments.executionType = EventExecutionType.Invoke;
            arguments.eventObject = evt;
            arguments.arguments = null;
            
            var startAspects = evt.GetCustomAttributes(typeof(IBeforeEventInvokedAspect), true);
            
            foreach (var aspect in startAspects)
            {
                ((IBeforeEventInvokedAspect) aspect).BeforeEventInvoked(arguments);
            }
        }

        public static void AfterEventInvoked(object instance, Type type, string eventName)
        {
            var arguments = new EventExecutionArguments();
            var evt = type?.GetEvent(eventName);
            arguments.executionType = EventExecutionType.Invoke;
            arguments.eventObject = evt;
            arguments.arguments = null;
            
            var startAspects = evt.GetCustomAttributes(typeof(IAfterEventInvokedAspect), true);
            
            foreach (var aspect in startAspects)
            {
                ((IAfterEventInvokedAspect) aspect).BeforeEventInvoked(arguments);
            }
        } 
        
        public static object OnEventListenerAdded(object instance, Type type, string eventName, object[] args)
        {
            var arguments = new EventExecutionArguments();
            var evt = type?.GetEvent(eventName);
            arguments.executionType = EventExecutionType.AddListener;
            arguments.eventObject = evt;
            arguments.arguments = args;
            
            var method = type.GetMethod("add_" + eventName + APPENDIX);
            // Get start aspects and process them
            var startAspects = evt.GetCustomAttributes(typeof(IEventBeforeListenerAddedAspect), true);
            
            foreach (var aspect in startAspects)
            {
                ((IEventBeforeListenerAddedAspect) aspect).BeforeEventListenerAdded(arguments);
            }

            if (!arguments.HasErrored)
            {
                try
                {
                    method.Invoke(instance, args);
                }
                catch (Exception ex)
                {
                    arguments.exception = ex.InnerException;
                }
            }

            // Get start aspects and process them
            var exitAspects = evt.GetCustomAttributes(typeof(IEventAfterListenerAddedAspect), true);
            foreach (var aspect in exitAspects)
            {
                ((IEventAfterListenerAddedAspect) aspect).AfterEventListenerAdded(arguments);
            }

            // If had error throw it again after processing
            if (arguments.HasErrored)
            {
                // Invoke event if exists
                arguments.onException?.Invoke(arguments.exception);
                
                // Invoke exception aspects
                var exceptionAspects= evt.GetCustomAttributes(typeof(IEventExceptionThrownAspect), true);
                foreach (var aspect in exceptionAspects)
                {
                    ((IEventExceptionThrownAspect) aspect).OnExceptionThrown(arguments.exception, arguments);
                }
                
                // Rethrow exception
                // ReSharper disable once PossibleNullReferenceException, already checked using HasErrored
                throw arguments.exception;
            }
            
            // Return value
            return null;
        }

        
        public static object OnEventListenerRemoved(object instance, Type type, string eventName, object[] args)
        {
            var arguments = new EventExecutionArguments();
            var evt = type?.GetEvent(eventName);
            arguments.executionType = EventExecutionType.RemoveListener;
            arguments.eventObject = evt;
            arguments.arguments = args;
            
            var method = type.GetMethod("remove_" + eventName + APPENDIX);
            // Get start aspects and process them
            var startAspects = evt.GetCustomAttributes(typeof(IEventBeforeListenerRemovedAspect), true);
            
            foreach (var aspect in startAspects)
            {
                ((IEventBeforeListenerRemovedAspect) aspect).BeforeEventListenerRemoved(arguments);
            }

            if (!arguments.HasErrored)
            {
                try
                {
                    method.Invoke(instance, args);
                }
                catch (Exception ex)
                {
                    arguments.exception = ex.InnerException;
                }
            }

            // Get start aspects and process them
            var exitAspects = evt.GetCustomAttributes(typeof(IEventAfterListenerRemovedAspect), true);
            foreach (var aspect in exitAspects)
            {
                ((IEventAfterListenerRemovedAspect) aspect).AfterEventListenerRemoved(arguments);
            }

            // If had error throw it again after processing
            if (arguments.HasErrored)
            {
                // Invoke event if exists
                arguments.onException?.Invoke(arguments.exception);
                
                // Invoke exception aspects
                var exceptionAspects= evt.GetCustomAttributes(typeof(IEventExceptionThrownAspect), true);
                foreach (var aspect in exceptionAspects)
                {
                    ((IEventExceptionThrownAspect) aspect).OnExceptionThrown(arguments.exception, arguments);
                }
                
                // Rethrow exception
                // ReSharper disable once PossibleNullReferenceException, already checked using HasErrored
                throw arguments.exception;
            }
            
            // Return value
            return null;
        }

        public static object OnPropertyGet(object instance, Type type, string propertyName, object[] args)
        { 
            var arguments = new PropertyExecutionArguments();
            arguments.isSetArguments = false;
            var property = type?.GetProperty(propertyName);
            arguments.property = property;

            var method = type.GetMethod("get_" + propertyName + APPENDIX);
            
            // Get start aspects and process them
            var startAspects = property.GetCustomAttributes(typeof(IPropertyGetEnterAspect), true);
            foreach (var aspect in startAspects)
            {
                ((IPropertyGetEnterAspect) aspect).OnPropertyGetEnter(arguments);
            }

            if (!arguments.HasErrored)
            {
                try
                {
                    arguments.returnValue = method.Invoke(instance, args);
                }
                catch (Exception ex)
                {
                    arguments.exception = ex.InnerException;
                }
            }

            // Get start aspects and process them
            var exitAspects = property.GetCustomAttributes(typeof(IPropertyGetExitAspect), true);
            foreach (var aspect in exitAspects)
            {
                ((IPropertyGetExitAspect) aspect).OnPropertyGetExit(arguments);
            }

            // If had error throw it again after processing
            if (arguments.HasErrored)
            {
                // Invoke event if exists
                arguments.onException?.Invoke(arguments.exception);
                
                // Invoke exception aspects
                var exceptionAspects= property.GetCustomAttributes(typeof(IPropertyExceptionThrownAspect), true);
                foreach (var aspect in exceptionAspects)
                {
                    ((IPropertyExceptionThrownAspect) aspect).OnExceptionThrown(arguments.exception, arguments);
                }
                
                // Rethrow exception
                // ReSharper disable once PossibleNullReferenceException, already checked using HasErrored
                throw arguments.exception;
            }
            
            // Return value
            return arguments.returnValue;
        }
        
        public static object OnPropertySet(object instance, Type type, string propertyName, object[] args)
        {
            //Debug.Log($"AOPProcessor OnPropertySet");
            var arguments = new PropertyExecutionArguments();
            arguments.isSetArguments = false;
            arguments.instance = instance;
            var property = type?.GetProperty(propertyName);
            arguments.property = property;
            arguments.newValue = args[0]; // Set property value

            var method = type.GetMethod("set_" + propertyName + APPENDIX);
            
            // Get start aspects and process them
            var startAspects = property.GetCustomAttributes(typeof(IPropertySetEnterAspect), true);
            foreach (var aspect in startAspects)
            {
                ((IPropertySetEnterAspect) aspect).OnPropertySetEnter(arguments);
            }

            if (!arguments.HasErrored)
            {
                try
                {
                    arguments.returnValue = method.Invoke(instance, args);
                }
                catch (Exception ex)
                {
                    arguments.exception = ex.InnerException;
                }
            }

            // Get start aspects and process them
            var exitAspects = property.GetCustomAttributes(typeof(IPropertySetExitAspect), true);
            foreach (var aspect in exitAspects)
            {
                ((IPropertySetExitAspect) aspect).OnPropertySetExit(arguments);
            }

            // If had error throw it again after processing
            if (arguments.HasErrored)
            {
                // Invoke event if exists
                arguments.onException?.Invoke(arguments.exception);
                
                // Invoke exception aspects
                var exceptionAspects= property.GetCustomAttributes(typeof(IPropertyExceptionThrownAspect), true);
                foreach (var aspect in exceptionAspects)
                {
                    ((IPropertyExceptionThrownAspect) aspect).OnExceptionThrown(arguments.exception, arguments);
                }
                
                // Rethrow exception
                // ReSharper disable once PossibleNullReferenceException, already checked using HasErrored
                throw arguments.exception;
            }
            
            // Return value
            return arguments.returnValue;
        }

        public static object OnMethod(object instance, Type type, string methodName, object[] args)
        {
            // Create arguments for aspects
            var arguments = new MethodExecutionArguments();

            // Get types for args
            var typeArray = new List<Type>();
            foreach (var o in args)
            {
                if (o != null)
                {
                    typeArray.Add(o.GetType());
                }
            }
   
            
            // Get methods and generate generic versions for invocation
            var genericTypes = null as List<Type>;
            var method = type?.GetMethod(methodName + APPENDIX, typeArray, out genericTypes);
            if(genericTypes.Count > 0)
                method = method.MakeGenericMethod(genericTypes.ToArray());
            
            var containingMethod = type?.GetMethod(methodName, typeArray, out genericTypes);
            if(genericTypes.Count > 0)
                containingMethod = containingMethod.MakeGenericMethod(genericTypes.ToArray());
            
            // Load data
            arguments.method = method;
            arguments.rootMethod = containingMethod;
            var mParam = method.GetParameters();

            // Generate arguments for execution args
            for (var index = 0; index < args.Length; index++)
            {
                var pValue = args[index];
                var pName = mParam[index].Name;
                arguments.arguments.Add(new MethodArgument(pName, pValue));
            }

            // Get start aspects and process them
            var startAspects = containingMethod.GetCustomAttributes(typeof(IMethodEnterAspect), true);

            
            foreach (var aspect in startAspects)
            {
                ((IMethodEnterAspect) aspect).OnMethodEnter(arguments);
            }

            if (!arguments.HasErrored)
            {
                try
                {
                    arguments.returnValue = method.Invoke(instance, args);
                }
                catch (Exception ex)
                {
                    arguments.exception = ex.InnerException;
                }
            }

            // Get start aspects and process them
            var exitAspects = containingMethod.GetCustomAttributes(typeof(IMethodExitAspect), true);
            foreach (var aspect in exitAspects)
            {
                ((IMethodExitAspect) aspect).OnMethodExit(arguments);
            }
 
            // If had error throw it again after processing
            if (arguments.HasErrored)
            {
                // Invoke event if exists
                arguments.onException?.Invoke(arguments.exception);

                // Invoke exception aspects
                var exceptionAspects = containingMethod.GetCustomAttributes(typeof(IMethodExceptionThrownAspect), true);
                foreach (var aspect in exceptionAspects)
                {
                    ((IMethodExceptionThrownAspect) aspect).OnExceptionThrown(arguments.exception, arguments);
                }

                // Rethrow exception
                // ReSharper disable once PossibleNullReferenceException, already checked using HasErrored
                throw arguments.exception;
            }


            // Return value
            return arguments.returnValue;
        }




    }
}