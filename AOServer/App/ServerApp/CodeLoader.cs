using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace ET
{
    public class CodeLoader: Singleton<CodeLoader>
    {
        private AssemblyLoadContext? assemblyLoadContext;

        private Assembly? model;

        public void Start()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly.GetName().Name == "Game.Model")
                {
                    this.model = assembly;
                    break;
                }
            }
            this.LoadHotfix();
            
            //IStaticMethod start = new StaticMethod(this.model, "ET.Entry", "Start");
            //start.Run();
        }

        public void LoadHotfix()
        {
            assemblyLoadContext?.Unload();
            GC.Collect();
            assemblyLoadContext = new AssemblyLoadContext("Hotfix", true);
            byte[] dllBytes = File.ReadAllBytes("./Game.Run.dll");
            byte[] pdbBytes = File.ReadAllBytes("./Game.Run.pdb");
            Assembly hotfixAssembly = assemblyLoadContext.LoadFromStream(new MemoryStream(dllBytes), new MemoryStream(pdbBytes));
            byte[] dllBytes2 = File.ReadAllBytes("./Server.Outer.dll");
            byte[] pdbBytes2 = File.ReadAllBytes("./Server.Outer.pdb");
            Assembly hotfixAssembly2 = assemblyLoadContext.LoadFromStream(new MemoryStream(dllBytes2), new MemoryStream(pdbBytes2));

            Assembly? ass = Assembly.GetEntryAssembly();
            Dictionary<string, Type> types = AssemblyHelper.GetAssemblyTypes(typeof (Game).Assembly, this.model, hotfixAssembly, hotfixAssembly2);
			
            EventSystem.Instance.Add(types);
        }
    }
}