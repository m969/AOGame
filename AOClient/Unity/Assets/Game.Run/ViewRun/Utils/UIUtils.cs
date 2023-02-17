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

        public static Asset LoadPackage(string name)
        {
            Log.Debug($"LoadPackage {name}");
            var filePath = "Assets/Bundles/UIRes/" + name + "_fui.bytes";
            var asset = Asset.LoadAsset(filePath);
            if (Define.IsEditor)
            {
                UIPackage.AddPackage("Assets/Bundles/UIRes/" + name);
            }
            else
            {
                UIPackage.AddPackage(asset.GetAssetBundle());
            }
            return asset;
        }

        public static async ETTask<Asset> LoadPackageAsync(string name)
        {
            Log.Debug($"LoadPackageAsync {name}");
            var filePath = "Assets/Bundles/UIRes/" + name + "_fui.bytes";
            var asset = Asset.LoadAssetAsync(filePath);
            await asset.Task;
            if (Define.IsEditor)
            {
                UIPackage.AddPackage("Assets/Bundles/UIRes/" + name);
            }
            else
            {
                UIPackage.AddPackage(asset.GetAssetBundle());
            }
            return asset;
        }
    }
}