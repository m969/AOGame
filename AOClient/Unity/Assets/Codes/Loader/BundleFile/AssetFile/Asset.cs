using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ET;
using System;
using System.IO;

namespace AssetFile
{
    public class Asset : Entity
    {
        public readonly static Dictionary<string, string> AssetName2Paths = new();
        public readonly static Dictionary<string, string> Path2BundleNames = new();
        public readonly static Dictionary<string, AssetBundle> BundleName2Bundles = new();
        public readonly static Dictionary<string, int> Bundle2RefCounters = new();
        public string BundleName;

        private ETTask<Asset> task;
        public ETTask<Asset> Task
        {
            get
            {
                task = ETTask<Asset>.Create();
#if UNITY_EDITOR
                task.SetResult(this);
#endif
                return task;
            }
        }

        public UnityEngine.Object Object { get; set; }
        public GameObject GameObject => Object as GameObject;

        public T Get<T>() where T : UnityEngine.Object
        {
            return Object as T;
        }

        public AssetBundle GetAssetBundle()
        {
            BundleName2Bundles.TryGetValue(BundleName, out var bundle);
            return bundle;
        }

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

        public void Release()
        {
            var counter = AddRefCounter(BundleName, -1);
            if (counter == 0 && BundleName2Bundles.ContainsKey(BundleName))
            {
                BundleName2Bundles[BundleName].Unload(true);
                BundleName2Bundles.Remove(BundleName);
            }
        }

        public static string ArtDataPath { get; set; } = Application.persistentDataPath + "/Bundles/artdata";
        public static string StreamingDataPath { get; set; } = Application.streamingAssetsPath + "/Bundles/artdata";

        public static Asset LoadAsset(string path)
        {
            var assetName = Path.GetFileName(path);
            var asset = new Asset();
            try
            {
                if (AssetName2Paths.ContainsKey(path))
                {
                    AssetName2Paths.TryGetValue(path, out path);
                }
                Path2BundleNames.TryGetValue(path, out string bundleName);
                UnityEngine.Object obj = null;
#if UNITY_EDITOR
                obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
#else
            AssetBundle ab = null;
            BundleName2Bundles.TryGetValue(bundleName, out ab);
            if (ab == null)
            {
                if (File.Exists(ArtDataPath + $"/{bundleName}"))
                {
                    ab = AssetBundle.LoadFromFile(ArtDataPath + $"/{bundleName}");
                }
                else
                {
                    ab = AssetBundle.LoadFromFile(StreamingDataPath + $"/{bundleName}");
                }
                BundleName2Bundles.Add(bundleName, ab);
            }
            AddRefCounter(bundleName, 1);
            obj = ab.LoadAsset(path);
#endif
                asset.BundleName = bundleName;
                Debug.Log($"LoadAsset {path} {bundleName} {obj}");
                asset.Object = obj;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return asset;
        }

        public static Asset LoadAssetAsync(string path)
        {
            var assetName = Path.GetFileName(path);
            var asset = new Asset();
            try
            {
                if (AssetName2Paths.ContainsKey(path))
                {
                    AssetName2Paths.TryGetValue(path, out path);
                }
                Path2BundleNames.TryGetValue(path, out string bundleName);
                UnityEngine.Object obj = null;
#if UNITY_EDITOR
                obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                asset.Object = obj;
                asset.BundleName = bundleName;
#else
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
                var request = AssetBundle.LoadFromFileAsync(loadPath);
                request.completed += (op) => {
                    ab = request.assetBundle;
                    BundleName2Bundles.Add(bundleName, ab);
                    AddRefCounter(bundleName, 1);
                    asset.BundleName = bundleName;
                    var assetRequest = ab.LoadAssetAsync(path);
                    assetRequest.completed += (assetop) => {
                        asset.Object = assetRequest.asset;
                        asset.Task.SetResult(asset);
                    };
                };
            }
            else
            {
                AddRefCounter(bundleName, 1);
                asset.BundleName = bundleName;
                var assetRequest = ab.LoadAssetAsync(path);
                assetRequest.completed += (assetop) => {
                    asset.Object = assetRequest.asset;
                    asset.Task.SetResult(asset);
                };
            }
#endif
                Debug.Log($"LoadAssetAsync {path} {bundleName} {obj}");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return asset;
        }
    }
}
