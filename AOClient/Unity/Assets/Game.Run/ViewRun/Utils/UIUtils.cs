using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using AssetFile;

namespace AO
{
    public static class UIUtils
    {
        public static void RemovePackage(string packageName)
        {
            Log.Debug($"RemovePackage {packageName}");
            UIPackage.RemovePackage(packageName);
        }

        public static Asset LoadPackage(string path)
        {
            Log.Debug($"LoadPackage {path}");
            var filePath = path + "_fui.bytes";
            var asset = Asset.LoadAsset(filePath);
            if (Define.IsEditor)
            {
                UIPackage.AddPackage(path);
            }
            else
            {
                UIPackage.AddPackage(asset.GetAssetBundle());
            }
            return asset;
        }

        public static async ETTask<Asset> LoadPackageAsync(string path)
        {
            Log.Debug($"LoadPackageAsync {path}");
            var filePath = path + "_fui.bytes";
            var asset = Asset.LoadAssetAsync(filePath);
            if (Define.IsEditor)
            {
                UIPackage.AddPackage(path);
            }
            else
            {
                await asset.Task;
                UIPackage.AddPackage(asset.GetAssetBundle());
            }
            return asset;
        }
    }
}