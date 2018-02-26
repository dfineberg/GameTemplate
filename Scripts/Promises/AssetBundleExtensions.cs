using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameTemplate.Promises
{
    public static class AssetBundleExtensions
    {
        private static readonly Dictionary<string, AssetBundle> LoadedBundles = new Dictionary<string, AssetBundle>();
        
        private static IPromise LoadFromFileAsync(string bundlePath)
        {
            if (LoadedBundles.ContainsKey(bundlePath))
                return Promise.Resolved(LoadedBundles[bundlePath]);
            
            var p = new Promise();
            var path = Path.Combine(Application.streamingAssetsPath, bundlePath).Replace("\\", "/");
            var request = AssetBundle.LoadFromFileAsync(path);
            
            CoroutineExtensions.WaitUntil(request)
                .ThenDo(() =>
                {
                    Debug.Log($"Loaded AssetBundle at: {path}");
                    LoadedBundles.Add(bundlePath, request.assetBundle);
                    p.Resolve(request.assetBundle);
                });
            
            return p;
        }

        public static IPromise Load(string bundlePath)
        {
#if UNITY_EDITOR
            return Promise.Resolved();
#else
            return LoadFromFileAsync(bundlePath);
#endif
        }

        public static IPromise LoadAllAssets(string bundlePath)
        {
#if UNITY_EDITOR
            var paths = AssetDatabase.GetAssetPathsFromAssetBundle(bundlePath);
            var assets = paths.Select(AssetDatabase.LoadAssetAtPath<Object>).ToArray();
            return Promise.Resolved(assets);
#else
            return LoadFromFileAsync(bundlePath)
                .Then<AssetBundle>(b => b.LoadAllAssetsPromise());
#endif
        }

        public static IPromise LoadAllAssetNames(string bundlePath)
        {
#if UNITY_EDITOR
            return Promise.Resolved(AssetDatabase.GetAssetPathsFromAssetBundle(bundlePath));
#else
            return LoadFromFileAsync(bundlePath)
                .Then<AssetBundle>(b => Promise.Resolved(b.GetAllAssetNames()));
#endif
        }

        public static IPromise LoadAsset(string bundlePath, string assetName)
        {
#if UNITY_EDITOR
            var assetPath = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(bundlePath, assetName)[0];
            return Promise.Resolved(AssetDatabase.LoadAssetAtPath<Object>(assetPath));
#else       
            return LoadFromFileAsync(bundlePath)
                .Then<AssetBundle>(b => b.LoadAssetPromise(assetName));
#endif
        }

        public static void Unload(string bundlePath, bool unloadAllLoadedObjects)
        {
            if (!LoadedBundles.ContainsKey(bundlePath))
                return;
            
            LoadedBundles[bundlePath].Unload(unloadAllLoadedObjects);
        }

        public static void UnloadAll(bool unloadAllObjects)
        {
            LoadedBundles.Clear();
            AssetBundle.UnloadAllAssetBundles(unloadAllObjects);
        }

        
        // --------- AssetBundle extension methods --------- //
        
        public static IPromise LoadAllAssetsPromise(this AssetBundle bundle)
        {
            var p = new Promise();
            var request = bundle.LoadAllAssetsAsync();

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