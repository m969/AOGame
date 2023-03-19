using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ITnnovative.AOP.Attributes;
using ITnnovative.AOP.Attributes.Event;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Attributes.Property;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using UnityEditor;
using UnityEngine;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using MethodBody = Mono.Cecil.Cil.MethodBody;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;

namespace ITnnovative.AOP.Processing.Editor
{
    [InitializeOnLoad]
    public static class CodeProcessor
    {
        static CodeProcessor()
        {
            AssemblyReloadEvents.beforeAssemblyReload += WeaveEditorAssemblies; 
        }
        
        /// <summary>
        /// Cache for Attribute Types
        /// </summary>
        private static Dictionary<Type, List<Type>> _typeCache = new Dictionary<Type, List<Type>>();
    
        public static void WeaveAssembly(AssemblyDefinition assembly, string path)
        {
            //Debug.Log($"WeaveAssembly  {assembly.Name}");

            // For all types in every module
            foreach (var module in assembly.Modules)
            {
                foreach (var type in module.Types)
                {
                    // For all constructors
                    for (var index = 0; index < type.Methods.Count; index++)
                    {
                        var method = type.Methods[index];

                        if (!HasAttributeOfType<AOPGeneratedAttribute>(method))
                        {
                            // Register AOP Processor by encapsulating method
                            if (HasAttributeOfType<IMethodAspect>(method))
                            { 
                                MarkAsProcessed(module, method);
                                EncapsulateMethod(assembly, module, type, method);
                            }
                        }
                    }

                    for (var index = 0; index < type.Events.Count; index++)
                    {
                        var evt = type.Events[index];
                        if (!HasAttributeOfType<AOPGeneratedAttribute>(evt))
                        {
                            // Register AOP Processor by encapsulating method
                            if (HasAttributeOfType<IEventAspect>(evt))
                            {
                                MarkAsProcessed(module, evt);
                                if (HasAttributeOfType<IEventAddedListenerAspect>(evt))
                                {
                                    EncapsulateMethod(assembly, module, type, evt.AddMethod, evt.Name, 
                                        nameof(AOPProcessor.OnEventListenerAdded));    
                                }
                                if (HasAttributeOfType<IEventRemovedListenerAspect>(evt))
                                {
                                    EncapsulateMethod(assembly, module, type, evt.RemoveMethod, evt.Name, 
                                        nameof(AOPProcessor.OnEventListenerRemoved));
                                }

                                if (HasAttributeOfType<IEventInvokedAspect>(evt))
                                {
                                    EncapsulateEventExecution(module, type, evt);
                                }
                            }
                        }

                        
                    }

                    for (var index = 0; index < type.Properties.Count; index++)
                    {
                        var property = type.Properties[index];
                        if (!HasAttributeOfType<AOPGeneratedAttribute>(property))
                        {  
                            // Register AOP Processor by encapsulating method
                            if (HasAttributeOfType<IPropertyAspect>(property))
                            { 
                                MarkAsProcessed(module, property);
                                if (HasAttributeOfType<IPropertyGetAspect>(property))
                                {
                                    EncapsulateMethod(assembly, module, type, property.GetMethod, property.Name, 
                                        nameof(AOPProcessor.OnPropertyGet));    
                                }
                                if (HasAttributeOfType<IPropertySetAspect>(property))
                                {
                                    //Debug.Log($"WeaveAssembly EncapsulateMethod OnPropertySet {property.Name}");
                                    EncapsulateMethod(assembly, module, type, property.SetMethod, property.Name, 
                                        nameof(AOPProcessor.OnPropertySet));    
                                }
                                
                            } 
                        }
                    }
                }
            }
            
            assembly.Write(path);
            //Debug.Log($"assembly.Write {DateTime.Now.ToLongTimeString()}");
        }

        public static void EncapsulateEventExecution(ModuleDefinition module, TypeDefinition type, EventDefinition evt)
        {
            foreach (var method in type.Methods)
            {
                var body = method.Body;
                for (var pos = body.Instructions.Count - 1; pos >= 0; pos--)
                {
                    var instr = body.Instructions[pos];
                    if (instr.OpCode == OpCodes.Ldfld)
                    {
                        if (!HasAttributeOfType<IBeforeEventInvokedAspect>(evt)) continue;
                        
                        if (instr.Operand is FieldDefinition) 
                        { 
                            var opObj = (FieldDefinition) instr.Operand;
                            if (method.Name.StartsWith("add_" + evt.Name) ||
                                method.Name.StartsWith("remove_" + evt.Name)) continue; 
                            
                            if (opObj.Name == evt.Name)
                            {
                                var newMethodBody = new List<Instruction>();
                                newMethodBody.Add(Instruction.Create(method.IsStatic ? OpCodes.Ldnull : OpCodes.Ldarg_0));
                                newMethodBody.Add(Instruction.Create(OpCodes.Ldtoken, type));
                                newMethodBody.Add(Instruction.Create(OpCodes.Call, typeof(Type).GetMonoMethod(module, 
                                    nameof(Type.GetTypeFromHandle))));
                                newMethodBody.Add(Instruction.Create(OpCodes.Ldstr, evt.Name));
 
                                if(module.HasType(typeof(AOPProcessor))){
                                    newMethodBody.Add(Instruction.Create(OpCodes.Call,
                                        module.GetType(typeof(AOPProcessor))
                                            .GetMethod(nameof(AOPProcessor.BeforeEventInvoked))));
                                }
                                else 
                                {
                                    newMethodBody.Add(Instruction.Create(OpCodes.Call, typeof(AOPProcessor).GetMonoMethod(module, 
                                        nameof(AOPProcessor.BeforeEventInvoked))));
                                }

                                var index = method.Body.Instructions.IndexOf(instr) - 1;
                                foreach (var i in newMethodBody)
                                {  
                                    index++;
                                    method.Body.Instructions.Insert(index, i);
                                }
                            }
                        } 
                    }
                    else if (instr.OpCode == OpCodes.Callvirt)
                    {
                        if (!HasAttributeOfType<IAfterEventInvokedAspect>(evt)) continue;
                        
                        //evt.
                        var operand = instr.Operand as MethodReference;
                        if(operand == null) throw new Exception("[Unity AOP] Unknown error, please report with source code.");
                        
                        var opName = operand?.DeclaringType.FullName;
                        if (opName == evt.EventType.FullName)
                        {
                            var newMethodBody = new List<Instruction>();
                            newMethodBody.Add(Instruction.Create(method.IsStatic ? OpCodes.Ldnull : OpCodes.Ldarg_0));
                            newMethodBody.Add(Instruction.Create(OpCodes.Ldtoken, type));
                            newMethodBody.Add(Instruction.Create(OpCodes.Call, typeof(Type).GetMonoMethod(module, 
                                nameof(Type.GetTypeFromHandle))));
                            newMethodBody.Add(Instruction.Create(OpCodes.Ldstr, evt.Name)); 

                            if(module.HasType(typeof(AOPProcessor))){
                                newMethodBody.Add(Instruction.Create(OpCodes.Call,
                                    module.GetType(typeof(AOPProcessor))
                                        .GetMethod(nameof(AOPProcessor.AfterEventInvoked))));
                            }
                            else 
                            {
                                newMethodBody.Add(Instruction.Create(OpCodes.Call, typeof(AOPProcessor).GetMonoMethod(module, 
                                    nameof(AOPProcessor.AfterEventInvoked))));
                            }

                            var index = method.Body.Instructions.IndexOf(instr);
                            foreach (var i in newMethodBody)
                            {  
                                index++;
                                method.Body.Instructions.Insert(index, i);
                            }
                        }
                    }
                }
            }
        }
        
        private static void EncapsulateMethod(AssemblyDefinition assembly, ModuleDefinition module, 
            TypeDefinition type, MethodDefinition method, string overrideName = null, string overrideMethod =
                nameof(AOPProcessor.OnMethod))
        {
            // New body for current method (capsule)
            var newMethodBody = new List<Instruction>();
            newMethodBody.Add(Instruction.Create(method.IsStatic ? OpCodes.Ldnull : OpCodes.Ldarg_0)); // ldnull / ldarg_0
            newMethodBody.Add(Instruction.Create(OpCodes.Ldtoken, type));
            newMethodBody.Add(Instruction.Create(OpCodes.Call, typeof(Type).GetMonoMethod(module, nameof(Type.GetTypeFromHandle))));

            var mName = method.Name;
            if (!string.IsNullOrEmpty(overrideName))
                mName = overrideName;
            
            newMethodBody.Add(Instruction.Create(OpCodes.Ldstr, mName));
            newMethodBody.Add(Instruction.Create(OpCodes.Ldc_I4, method.Parameters.Count));
            newMethodBody.Add(Instruction.Create(OpCodes.Newarr, module.ImportReference(typeof(object))));

            for (var num = 0; num < method.Parameters.Count; num++)
            {
                var param = method.Parameters[num];
                var pType = param.ParameterType;

                newMethodBody.Add(Instruction.Create(OpCodes.Dup));
                newMethodBody.Add(Instruction.Create(OpCodes.Ldc_I4, num));
                newMethodBody.Add(Instruction.Create(OpCodes.Ldarg, param));
                if(param.ParameterType.IsValueType || param.ParameterType.IsGenericParameter)
                    newMethodBody.Add(Instruction.Create(OpCodes.Box, pType));
                newMethodBody.Add(Instruction.Create(OpCodes.Stelem_Ref));
            }
            
            
            if(module.HasType(typeof(AOPProcessor))){
                newMethodBody.Add(Instruction.Create(OpCodes.Call,
                    module.GetType(typeof(AOPProcessor))
                    .GetMethod(overrideMethod)));
            }
            else
            {
                newMethodBody.Add(Instruction.Create(OpCodes.Call, typeof(AOPProcessor).GetMonoMethod(module, 
                    overrideMethod)));
            }
 
            if (method.ReturnType.FullName != typeof(void).FullName)
            {
                if (method.ReturnType.IsValueType)
                {
                    newMethodBody.Add(Instruction.Create(OpCodes.Unbox_Any, method.ReturnType));
                }
            }
            else
            {
                newMethodBody.Add(Instruction.Create(OpCodes.Pop));
            }
                
            newMethodBody.Add(Instruction.Create(OpCodes.Ret));

            // Create new method
            var internalMethod = new MethodDefinition(method.Name + AOPProcessor.APPENDIX, method.Attributes, method.ReturnType);
            foreach (var param in method.Parameters)
            {
                var newParam = new ParameterDefinition(param.Name, param.Attributes, param.ParameterType);
                newParam.HasDefault = false;
                newParam.IsOptional = false; 
                internalMethod.Parameters
                    .Add(newParam);
            }  

            // Copy generic parameters
            foreach (var genericParameter in method.GenericParameters)
            {
                internalMethod.GenericParameters
                    .Add(new GenericParameter(genericParameter.Name, internalMethod));
            }
            
            var bodyClone = new MethodBody(method);
            bodyClone.AppendInstructions(newMethodBody);
            bodyClone.MaxStackSize = 8;
            
            // Replace method bodies
            internalMethod.Body = method.Body;
            method.Body = bodyClone;
            type.Methods.Add(internalMethod);

            //Debug.Log($"EncapsulateMethod Replace method bodies {internalMethod.Name}");
        }
        
        /// <summary>
        /// Marks member as processed by AOP
        /// </summary>
        public static void MarkAsProcessed(ModuleDefinition module, IMemberDefinition obj)
        {
            // For Assembly-CSharp load AOPGeneratedAttribute..ctor from local, otherwise reference ..ctor
            if(module.HasType(typeof(AOPGeneratedAttribute)))
            {
                var attribute = module.GetType(typeof(AOPGeneratedAttribute))
                    .GetConstructors().First();
                obj.CustomAttributes.Add(new CustomAttribute(attribute));
            }
            else
            {
                var attribute = module.ImportReference(
                    typeof(AOPGeneratedAttribute).GetConstructors((BindingFlags) int.MaxValue)
                        .First());
                obj.CustomAttributes.Add(new CustomAttribute(attribute));
            }
        }
 
        
        public static Instruction InsertInstructionAfter(this ILProcessor processor, Instruction afterThis, Instruction insertThis)
        {
            processor.InsertAfter(afterThis, insertThis);
            return insertThis;
        }
        
        /// <summary>
        /// Wave all assemblies
        /// </summary>
        public static void WeaveAssemblies(bool isPlayer)
        {
            var dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var tick = (DateTime.UtcNow.Ticks - dt1970.Ticks) / 10000;
            var nowSeconds = (int)(tick / 1000);
            var weaveTime = PlayerPrefs.GetInt("weavetime", 0);
            if (nowSeconds - weaveTime <= 2)
            {
                Debug.Log($"WeaveAssemblies {nowSeconds} {weaveTime}");
                return;
            }
            PlayerPrefs.SetInt("weavetime", nowSeconds);
            Debug.Log($"[Unity AOP] Weaving {(isPlayer ? "player" : "editor")} assemblies..."); 

            var path = Application.dataPath + $"/../Library/{(isPlayer ? "Player" : "")}ScriptAssemblies/Game.Model.dll";
            //var copyPath = path.Replace("Game.Model", "Game.ModelWeave");
            //var bytes = File.ReadAllBytes(path);
            //File.WriteAllBytes(path, bytes);
            var unityDefaultAssembly = AssemblyDefinition.ReadAssembly(path, new ReaderParameters() { ReadWrite = true });
            WeaveAssembly(unityDefaultAssembly, path);
        }

        /// <summary>
        /// Weave assemblies in editor
        /// </summary>
        public static void WeaveEditorAssemblies() => WeaveAssemblies(false);//Debug.Log("WeaveAssemblies");//

        /// <summary>
        /// Weave assemblies in player
        /// </summary>
        public static void WeavePlayerAssemblies() => WeaveAssemblies(true);

        /// <summary>
        /// Check if member has attribute
        /// </summary>
        public static bool HasAttributeOfType<T>(IMemberDefinition member)
        {
            var subtypes= FindSubtypesOf<T>();
            foreach (var attribute in member.CustomAttributes)
            {
                foreach (var st in subtypes)
                {
                    if (attribute.AttributeType.FullName.Equals(st.FullName)) return true;
                }

            }  
            return false;
        }

        /// <summary>
        /// Find subtypes of T, used for finding children attributes for aspects
        /// </summary>
        public static List<Type> FindSubtypesOf<T>()
        {
            var outObj = new List<Type>();

            // Check cache for result (improves efficiency)
            var mainType = typeof(T);
            if (_typeCache.ContainsKey(mainType))
                return _typeCache[mainType];
            
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in a.GetTypes())
                { 
                    if (mainType.IsAssignableFrom(t))
                    {
                        outObj.Add(t);
                    }
                }
            }

            _typeCache[mainType] = outObj;

            return outObj;
        }
    }
}

