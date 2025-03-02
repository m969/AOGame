using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetFile;
using UnityEngine.SceneManagement;

namespace AO
{
    public static class AssetUtils
    {
        //public static Asset ReleaseWith(this Asset asset, Entity parent)
        //{
        //    parent.AddChild(asset);
        //    return asset;
        //}

        //public static Asset LoadAssetWithParent(string path, Entity releaseWithParent)
        //{
        //    var asset = Asset.LoadAsset(path);
        //    if (releaseWithParent != null)
        //    {
        //        asset.ReleaseWith(releaseWithParent);
        //    }
        //    return asset;
        //}

        public static Asset LoadAssetWithParentAsync(string path, Entity releaseWithParent)
        {
            //var asset = Asset.LoadAssetAsync(path);
            //if (releaseWithParent != null)
            //{
            //    asset.ReleaseWith(releaseWithParent);
            //}
            var asset = releaseWithParent.AddAssetChild(path);
            return asset;
        }

        //public static AsyncOperation LoadSceneAsync(this Asset asset, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        //{
        //    return Asset.LoadSceneAsync(asset, loadSceneMode);
        //}

        //public static async ETTask<Asset> LoadSceneAsyncWithParent(string path, Entity releaseWithParent)
        //{
        //    var asset = await Asset.LoadSceneAsync(path);
        //    asset.ReleaseWith(releaseWithParent);
        //    return asset;
        //}
    }
}