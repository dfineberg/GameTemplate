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

    public static IPromise LoadAsync<T>(string path)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            var promise = Promise.Create();
            var load = Addressables.LoadAssetAsync<T>(path);
            load.Completed += (result) => promise.Resolve(result);
            return promise;
        }
#endif
        
        var request = Addressables.LoadAssetAsync<T>(path);        
        return WaitForRequest(request, "Loaded addressable at: " + path);
    }

    public static IPromise LoadAllAsync<T>(string path, Action<T> callbackPerAsset = null)
    {
        var request = Addressables.LoadAssetsAsync<T>(path, callbackPerAsset);
        return WaitForRequest(request, "Loaded addressables at: " + path);
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