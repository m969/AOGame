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
        public static void RemovePackage(this UIStage stage, string packageName)
        {
            Log.Debug($"RemovePackage {packageName}");
            UIPackage.RemovePackage(packageName);
            stage.PackageAssets[packageName].Dispose();
            stage.PackageAssets.Remove(packageName);
        }

        public static Asset LoadPackage(this UIStage stage, string name)
        {
            Log.Debug($"LoadPackage {name}");
            var filePath = "Assets/Bundles/UIRes/" + name + "_fui.bytes";
            var asset = stage.AddChild<Asset, string>(filePath);
            stage.PackageAssets.Add(name, asset);
            asset.Load();
            //var asset = Asset.LoadAsset(filePath);
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

        public static async ETTask<Asset> LoadPackageAsync(this UIStage stage, string name)
        {
            Log.Debug($"LoadPackageAsync {name}");
            var filePath = "Assets/Bundles/UIRes/" + name + "_fui.bytes";
            var asset = stage.AddChild<Asset, string>(filePath);
            stage.PackageAssets.Add(name, asset);
            await asset.LoadAsync();
            //var asset = Asset.LoadAssetAsync(filePath);
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