using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace ITnnovative.AOP.Processing.Editor
{
    public static class DefinitionExtensions
    {
        
        public static bool HasType(this ModuleDefinition definition, Type type)
        {
            return definition.Types.Any(t => t.FullName.Equals(type.FullName));
        }
        
        public static TypeDefinition GetType(this ModuleDefinition definition, Type type)
        {
            return definition.Types.FirstOrDefault(t => t.FullName.Equals(type.FullName));
        }

        public static MethodDefinition GetMethod(this TypeDefinition type, string name)
        {
            return type.Methods.FirstOrDefault(m => m.Name.Equals(name));
        }
        
        public static MethodReference GetMonoMethod(this Type type, ModuleDefinition module, string name)
        {
            return module.ImportReference(type.GetMethod(name));
        }

        public static MethodBody AppendInstructions(this MethodBody body, List<Instruction> instructions)
        {
            instructions.ForEach(body.Instructions.Add);
            return body;
        }
    }
}