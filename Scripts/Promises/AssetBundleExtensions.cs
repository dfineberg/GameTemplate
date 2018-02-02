using System.IO;
using UnityEngine;

namespace GameTemplate.Promises
{
    public static class AssetBundleExtensions
    {
        public static IPromise LoadFromFileAsync(string streamingAssetsPath)
        {
            var p = new Promise();
            var path = Path.Combine(Application.streamingAssetsPath, streamingAssetsPath).Replace("\\", "/");
            var request = AssetBundle.LoadFromFileAsync(path);
            
            CoroutineExtensions.WaitUntil(request)
                .ThenLog("Loaded AssetBundle at: " + path)
                .ThenDo(() => p.Resolve(request.assetBundle));
            
            return p;
        }

        public static IPromise LoadFromFileAndUnpack(string streamingAssetsPath)
        {
            return LoadFromFileAsync(streamingAssetsPath)
                .Then<AssetBundle>(b => b.LoadAllAssetsPromise());
        }

        public static IPromise LoadAllAssetsPromise(this AssetBundle bundle)
        {
            return LoadAllAssetsPromise<object>(bundle);
        }

        public static IPromise LoadAllAssetsPromise<T>(this AssetBundle bundle)
        {
            var p = new Promise();
            var request = bundle.LoadAllAssetsAsync(typeof(T));

            CoroutineExtensions.WaitUntil(request)
                .ThenDo(() => p.Resolve(request.allAssets));

            return p;
        }

        public static IPromise LoadAssetPromise(this AssetBundle bundle, string assetName)
        {
            var p = new Promise();
            var request = bundle.LoadAssetAsync(assetName);

            CoroutineExtensions.WaitUntil(request)
                .ThenDo(() => p.Resolve(request.asset));

            return p;
        }
    }
}