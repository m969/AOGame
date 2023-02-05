using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetFile;

namespace AO
{
    public static class AssetUtils
    {
        public static Asset ReleaseWith(this Asset asset, Entity entity)
        {
            //entity.AddDisposeAction(() => { asset.Release(); });
            entity.AddChild(asset);
            return asset;
        }

        public static Asset LoadAsset(string path)
        {
            return Asset.LoadAsset(path);
        }

        public static Asset LoadAssetAsync(string path)
        {
            return Asset.LoadAssetAsync(path);
        }

        public static ETTask<Asset> LoadSceneAsync(string path)
        {
            return Asset.LoadSceneAsync(path);
        }
    }
}