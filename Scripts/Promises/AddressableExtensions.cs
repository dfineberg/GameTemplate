using System.Collections.Generic;
using GameTemplate.Promises;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using GameTemplate;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using System;

public static class AddressableExtensions
{
    public static IPromise Init()
    {
        var request = Addressables.InitializeAsync();
        return WaitForRequest(request, "Initialized Addressables");
    }

    public static IPromise LoadAsync<T>(string address)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            var promise = Promise.Create();
            var load = Addressables.LoadAssetAsync<T>(address);
            load.Completed += (result) => promise.Resolve(result);
            return promise;
        }
#endif
        
        var request = Addressables.LoadAssetAsync<T>(address);        
        return WaitForRequest(request, "Loaded addressable at: " + address);
    }

    public static IPromise LoadAllAsync<T>(string address, Action<T> callbackPerAsset = null)
    {
        var request = Addressables.LoadAssetsAsync<T>(address, callbackPerAsset);
        return WaitForRequest(request, "Loaded addressables at: " + address);
    }

    public static IPromise LoadAllAsync<T>(IList<string> addresses, Action<T> callbackPerAsset = null)
    {
        var request = Addressables.LoadAssetsAsync<T>(addresses, callbackPerAsset, Addressables.MergeMode.Union);
        return WaitForRequest(request, "Loaded multiple addressables");
    }

    public static IPromise LoadAllFromStringLibrary<T>(StringLibrary lib, Action<T> callbackPerAsset = null)
    {
        return LoadAllAsync<T>(lib.StringsWithResourcesDirectory, callbackPerAsset);
    }

    public static void Release<T>(T obj)
    {
        Addressables.Release<T>(obj);
    }

    public static IPromise LoadSceneAsync(string address, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        var request = Addressables.LoadSceneAsync(address, mode);
        return WaitForRequest(request, "Loaded addressable scene: " + address);
    }

    public static IPromise UnloadSceneAsync(SceneInstance scene)
    {
        string name = scene.Scene.name;
        var request = Addressables.UnloadSceneAsync(scene);
        return WaitForRequest(request, "Unloaded addressable scene: " + name);
    }

    private static IPromise WaitForRequest<T>(AsyncOperationHandle<T> request, string message)
    {
        var p = Promise.Create();
        
        CoroutineExtensions.WaitUntil(() => request.IsDone)
            .ThenDo(() => {
                if (!request.IsValid()) // request was auto-collected
                {
                    Debug.Log(message);
                    p.Resolve();
                    return;
                }

                if (request.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log(message);
                    p.Resolve(request.Result);
                }
                else if (request.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogException(request.OperationException);
                    p.Reject(request.OperationException);
                }
            });

        return p;
    }
}