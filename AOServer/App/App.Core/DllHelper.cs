using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace ET
{
    public static class DllHelper
    {
        private static AssemblyLoadContext assemblyLoadContext;
        
        public static Assembly GetHotfixAssembly()
        {
            assemblyLoadContext?.Unload();
            System.GC.Collect();
            assemblyLoadContext = new AssemblyLoadContext("Game.Run", true);
            byte[] dllBytes = File.ReadAllBytes("./Game.Run.dll");
            byte[] pdbBytes = File.ReadAllBytes("./Game.Run.pdb");
            Assembly assembly = assemblyLoadContext.LoadFromStream(new MemoryStream(dllBytes), new MemoryStream(pdbBytes));
            return assembly;
        }
    }
}