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
    }
}