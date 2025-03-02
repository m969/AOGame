using AssetFile;
using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ET
{
	public class CodeLoader : Singleton<CodeLoader>
	{
		public Dictionary<string, Type> types = new();
		public Assembly model;
		public Assembly hotfix;

		public void Start()
		{
			if (!Define.EnableDllLoad)
			{
				//GlobalConfig globalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
				//if (globalConfig.CodeMode != CodeMode.ClientServer)
				//{
				//	throw new Exception("ENABLE_CODES mode must use ClientServer code mode!");
				//}

				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly ass in assemblies)
				{
					string name = ass.GetName().Name;
					if (name == "Game.Model")
					{
						this.model = ass;
					}
				}

				Dictionary<string, Type> types = AssemblyHelper.GetAssemblyTypes(assemblies);
				EventSystem.Instance.Add(types);

				IStaticMethod start = new StaticMethod(this.model, "ET.Entry", "Start");
				start.Run();
			}
			else
			{
				byte[] assBytes;
				byte[] pdbBytes;
				if (!Define.IsEditor)
				{
					var dllAsset = ETRoot.Root.AddAssetChild("Game.Model.dll.bytes");
					var pdbAsset = ETRoot.Root.AddAssetChild("Game.Model.pdb.bytes");
                    dllAsset.Load();
                    pdbAsset.Load();
                    assBytes = dllAsset.Get<TextAsset>().bytes;
					pdbBytes = pdbAsset.Get<TextAsset>().bytes;

					var assets = Asset.AssetName2Paths;
                    foreach (var kv in assets)
                    {
						if (kv.Key.EndsWith(".dll.bytes") && kv.Value.Contains("AOTDllCode"))
						{
							var asset = ETRoot.Root.AddAssetChild(kv.Key);
                            asset.Load();
                            byte[] bytes = asset.Get<TextAsset>().bytes;
                            RuntimeApi.LoadMetadataForAOTAssembly(bytes, HomologousImageMode.SuperSet);
                        }
                    }
                }
				else
				{
					assBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, "Game.Model.dll"));
					pdbBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, "Game.Model.pdb"));
				}

				this.model = Assembly.Load(assBytes, pdbBytes);
				this.LoadHotfix();

				IStaticMethod start = new StaticMethod(this.model, "ET.Entry", "Start");
				start.Run();
			}
		}

		// 热重载调用该方法
		public void LoadHotfix()
		{
			byte[] assBytes;
			byte[] pdbBytes;
			//byte[] assBytes2;
			//byte[] pdbBytes2;
			if (!Define.IsEditor)
			{
                var dllAsset = ETRoot.Root.AddAssetChild("Game.Run.dll.bytes");
                var pdbAsset = ETRoot.Root.AddAssetChild("Game.Run.pdb.bytes");
                dllAsset.Load();
                pdbAsset.Load();
                assBytes = dllAsset.Get<TextAsset>().bytes;
                pdbBytes = pdbAsset.Get<TextAsset>().bytes;
            }
			else
			{
				// 傻屌Unity在这里搞了个傻逼优化，认为同一个路径的dll，返回的程序集就一样。所以这里每次编译都要随机名字
				string[] logicFiles = Directory.GetFiles(Define.BuildOutputDir, "Game.Run_*.dll");
				if (logicFiles.Length != 1)
				{
					throw new Exception("Logic dll count != 1");
				}
				string logicName = Path.GetFileNameWithoutExtension(logicFiles[0]);
				assBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"{logicName}.dll"));
				pdbBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"{logicName}.pdb"));
                //assBytes2 = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"Client.Outer.dll"));
                //pdbBytes2 = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"Client.Outer.pdb"));
            }

            Assembly hotfixAssembly = Assembly.Load(assBytes, pdbBytes);
			hotfix = hotfixAssembly;
			//Assembly hotfixAssembly2 = Assembly.Load(assBytes2, pdbBytes2);
			Dictionary<string, Type> types = AssemblyHelper.GetAssemblyTypes(typeof(Game).Assembly, typeof(Init).Assembly, this.model, hotfixAssembly);

			EventSystem.Instance.Add(types);
		}
	}
}