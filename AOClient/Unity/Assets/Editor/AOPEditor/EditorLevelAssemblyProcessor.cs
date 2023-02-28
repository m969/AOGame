using System.IO;
using UnityEditor;
using UnityEngine;

namespace ITnnovative.AOP.Processing.Editor
{
    public class EditorLevelAssemblyProcessor : UnityEditor.Build.IFilterBuildAssemblies
    {
        public int callbackOrder { get; }
        public string[] OnFilterAssemblies(BuildOptions buildOptions, string[] assemblies)
        {
            CodeProcessor.WeavePlayerAssemblies();     
            
            return assemblies;
        }
    }
}