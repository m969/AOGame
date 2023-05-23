using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ET;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using BundleFile;

namespace AssetFile
{
    [Invoke(1333)]
    public class AssetTimer : ATimer<Asset>
    {
        protected override void Run(Asset self)
        {
            try
            {
                self.Task.SetResult(self);
                self.OnComplete?.Invoke(self);
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    public enum AssetLoadType
    {
        Editor,
        LocalData
    }

    public class Asset : Entity, IAwake
    {
        public static BinaryFileList BinaryFileList;
        public readonly static Dictionary<string, string> AssetName2Paths = new();
        public readonly static Dictionary<string, string> Path2BundleNames = new();
        public readonly static Dictionary<string, AssetBundle> BundleName2Bundles = new();
        public readonly static Dictionary<string, int> Bundle2RefCounters = new();
        public string BundleName;
        public string AssetPath;
        public static AssetLoadType AssetLoadType;

        private ETTask<Asset> task;
        public ETTask<Asset> Task
        {
            get
            {
                if (task == null)
                {
                    task = ETTask<Asset>.Create();
                }
                return task;
            }
        }

        public Action<Asset> OnComplete { get; set; }

        public UnityEngine.Object Object { get; set; }
        public GameObject GameObjectPrefab => Object as GameObject;

        public T Get<T>() where T : UnityEngine.Object
        {
            return Object as T;
        }

        public AssetBundle GetAssetBundle()
        {
            BundleName2Bundles.TryGetValue(BundleName, out var bundle);
            return bundle;
        }

        /// <summary> 增加bundle引用 </summary>
        public static int AddRefCounter(string bundleName, int counter)
        {
            if (!Bundle2RefCounters.ContainsKey(bundleName))
            {
                Bundle2RefCounters.Add(bundleName, 0);
            }
            var c = Bundle2RefCounters[bundleName] + counter;
            c = Math.Max(c, 0);
            Bundle2RefCounters[bundleName] = c;
            return c;
        }

        /// <summary> 释放bundle引用 </summary>
        private void Release()
        {
            var counter = AddRefCounter(BundleName, -1);
            if (counter == 0 && BundleName2Bundles.ContainsKey(BundleName))
            {
                BundleName2Bundles[BundleName].Unload(true);
                BundleName2Bundles.Remove(BundleName);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (IsDisposed)
            {
                return;
            }
            Release();
        }

        public static string ArtDataPath { get; set; } = Application.persistentDataPath + "/Bundles/artdata";
        public static string StreamingDataPath { get; set; } = Application.streamingAssetsPath + "/Bundles/artdata";
        public static string StreamingArtDataBinFilePath { get; set; } = Application.streamingAssetsPath + "/Bundles/artdata.bin";
        
        public static Asset LoadAsset(string path)
        {
            var assetName = Path.GetFileName(path);
            Asset asset;
            if (ETRoot.Root != null)
            {
                asset = ETRoot.Root.AddChild<Asset>();
            }
            else
            {
                asset = new Asset();
            }
            try
            {
                if (AssetName2Paths.ContainsKey(path))
                {
                    AssetName2Paths.TryGetValue(path, out path);
                }
                //else
                //{
                //    Debug.LogError($"LoadAssetAsync not found {path}");
                //}
                asset.AssetPath = path;
                if (!Path2BundleNames.TryGetValue(path, out string bundleName))
                {
                    Debug.LogError($"LoadAssetAsync not found bundle {path}");
                }
                UnityEngine.Object obj = null;

#if UNITY_EDITOR
                if (AssetLoadType == AssetLoadType.Editor)
                {
                    obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                }
                else
#endif
                {
                    AssetBundle ab = null;
                    BundleName2Bundles.TryGetValue(bundleName, out ab);
                    if (ab == null)
                    {
                        var loadPath = string.Empty;
                        if (File.Exists(ArtDataPath + $"/{bundleName}"))
                        {
                            loadPath = ArtDataPath + $"/{bundleName}";
                        }
                        else
                        {
                            loadPath = StreamingDataPath + $"/{bundleName}";
                        }
                        if (BinaryFileList.File2OffsetDict.TryGetValue(bundleName, out var offset))
                        {
                            ab = AssetBundle.LoadFromFile(StreamingArtDataBinFilePath, 0, (ulong)offset);
                        }
                        else
                        {
                            ab = AssetBundle.LoadFromFile(loadPath);
                        }
                        BundleName2Bundles.Add(bundleName, ab);
                    }
                    AddRefCounter(bundleName, 1);
                    obj = ab.LoadAsset(path);
                }

                asset.BundleName = bundleName;
                Debug.Log($"LoadAsset {path} {bundleName} {obj}");
                asset.Object = obj;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return asset;
        }

        public static Asset LoadAssetAsync(string path)
        {
            var assetName = Path.GetFileName(path);
            Asset asset;
            if (ETRoot.Root != null)
            {
                asset = ETRoot.Root.AddChild<Asset>();
            }
            else
            {
                asset = new Asset();
            }
            try
            {
                if (AssetName2Paths.ContainsKey(path))
                {
                    AssetName2Paths.TryGetValue(path, out path);
                }
                //else
                //{
                //    Debug.LogError($"LoadAssetAsync not found {path}");
                //}
                asset.AssetPath = path;
                Path2BundleNames.TryGetValue(path, out string bundleName);
                UnityEngine.Object obj = null;

#if UNITY_EDITOR
                if (AssetLoadType == AssetLoadType.Editor)
                {
                    obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                    asset.Object = obj;
                    asset.BundleName = bundleName;
                    if (TimerComponent.Instance != null)
                    {
                        TimerComponent.Instance.NewOnceTimer(TimeHelper.ClientFrameTime() + 10, 1333, asset);
                    }
                    else
                    {
                        asset.Task.SetResult(asset);
                        asset.OnComplete?.Invoke(asset);
                    }
                }
                else
#endif
                {
                    AssetBundle ab = null;
                    BundleName2Bundles.TryGetValue(bundleName, out ab);
                    if (ab == null)
                    {
                        var loadPath = string.Empty;
                        if (File.Exists(ArtDataPath + $"/{bundleName}"))
                        {
                            loadPath = ArtDataPath + $"/{bundleName}";
                        }
                        else
                        {
                            loadPath = StreamingDataPath + $"/{bundleName}";
                        }
                        AssetBundleCreateRequest request;
                        if (BinaryFileList.File2OffsetDict.TryGetValue(bundleName, out var offset))
                        {
                            request = AssetBundle.LoadFromFileAsync(StreamingArtDataBinFilePath, 0, (ulong)offset);
                        }
                        else
                        {
                            request = AssetBundle.LoadFromFileAsync(loadPath);
                        }
                        request.completed += (op) =>
                        {
                            ab = request.assetBundle;
                            BundleName2Bundles.Add(bundleName, ab);
                            AddRefCounter(bundleName, 1);
                            asset.BundleName = bundleName;

                            if (assetName.EndsWith(".unity"))
                            {
                                //asset.task?.SetResult(asset);
                                asset.OnComplete?.Invoke(asset);
                            }
                            else
                            {
                                var assetRequest = ab.LoadAssetAsync(path);
                                assetRequest.completed += (assetop) =>
                                {
                                    asset.Object = assetRequest.asset;
                                    asset.task?.SetResult(asset);
                                    asset.OnComplete?.Invoke(asset);
                                };
                            }
                        };
                    }
                    else
                    {
                        AddRefCounter(bundleName, 1);
                        asset.BundleName = bundleName;

                        if (assetName.EndsWith(".unity"))
                        {
                            //asset.task?.SetResult(asset);
                            asset.OnComplete?.Invoke(asset);
                        }
                        else
                        {
                            var assetRequest = ab.LoadAssetAsync(path);
                            assetRequest.completed += (assetop) =>
                            {
                                asset.Object = assetRequest.asset;
                                asset.task?.SetResult(asset);
                                asset.OnComplete?.Invoke(asset);
                            };
                        }
                    }
                }

                Debug.Log($"LoadAssetAsync {path} {bundleName} {obj}");
            }
            catch (Exception e)
            {
                Debug.Log($"LoadAssetAsync {path} {assetName}");
                Debug.LogError(e);
            }
            return asset;
        }

        public static AsyncOperation LoadSceneAsync(Asset asset, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            Debug.Log($"LoadSceneAsync {asset.AssetPath}");
#if UNITY_EDITOR
            var parameters = new LoadSceneParameters { loadSceneMode = loadSceneMode };
            var op = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(asset.AssetPath, parameters);
#else
            var op =  SceneManager.LoadSceneAsync(Path.GetFileNameWithoutExtension(asset.AssetPath), loadSceneMode);
#endif
            return op;
        }
    }
}
