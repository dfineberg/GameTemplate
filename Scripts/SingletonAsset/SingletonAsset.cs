using System;
using System.Collections.Generic;
using System.Linq;
using GameTemplate.Promises;
using UnityEngine;

/// <summary>
/// The SingletonAsset system is a way of creating singleton classes that are ScriptableObjects.
/// To make a new singleton, just create a new class that inherits from SingletonAsset, and the SingletonAssetResolver will
/// create a new instance of it in the Resources folder.
/// </summary>
public abstract class SingletonAsset : ScriptableObject
{
    protected static readonly Dictionary<Type, SingletonAsset> AssetDictionary = new Dictionary<Type, SingletonAsset>();
    private static Promise LoadedPromise = Promise.Create();

    public static bool Loaded => LoadedPromise.CurrentState == EPromiseState.Resolved;
    public static IPromise WaitUntilLoaded => LoadedPromise;

    public static IPromise PreloadAll()
    {
        AssetDictionary.Clear();
        var types = GetTypes();
        var preloadPromises = types.Select(t =>
        {
            var argName = t.Name;
            if (t.IsSubclassOf(typeof(AddressableSingletonAsset)))
            {
                return AddressableExtensions.LoadAsync<AddressableSingletonAsset>(argName);
            }
            else
            {
                return ResourceExtensions.LoadAsync(argName);
            }
        });

        return Promise.All(preloadPromises)
            .ThenDo<object[]>(assets =>
            {
                for (var i = 0; i < assets.Length; i++)
                {
                    var singletonAsset = (SingletonAsset) assets[i];
                    AssetDictionary.Add(types[i], singletonAsset);
                }
            });
    }

    public static IPromise LoadAll()
    {
        return Promise.All(AssetDictionary.Values.Select(asset => asset.OnAssetsLoaded()))
            .ThenDo(() => LoadedPromise.Resolve());
    }

    public static IPromise UnloadAll()
    {
        if (!Loaded) return Promise.Resolved();
        
        return Promise.All(AssetDictionary.Values.Select(o => o.OnAssetsUnloaded()))
            .ThenDo(() =>
            {
                LoadedPromise = Promise.Create();
            });
    }

    public static Type[] GetTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies() // in all the currently loaded assemblies,
            .SelectMany(s => s.GetTypes()) // get all the types
            .Where(t => t.IsClass && !t.IsGenericTypeDefinition && !t.IsAbstract && t.IsSubclassOf(typeof(SingletonAsset))) // that inherit from SingletonAsset
            .ToArray();
    }

    public static T Instance<T>() where T : SingletonAsset
    {
        if (AssetDictionary.TryGetValue(typeof(T), out var asset))
        {
            return (T) asset;
        }

        if (typeof(T).IsSubclassOf(typeof(AddressableSingletonAsset)))
        {
            Debug.LogError(typeof(T).Name + " was not preloaded, unable to find it's instance!");
            return null;
        } 

        asset = Resources.Load<T>(typeof(T).Name);
        AssetDictionary[typeof(T)] = asset;
        return (T) asset;
    }
    
    /// <summary>
    /// Called after all SingletonAssets have been loaded.
    /// The order in which the assets are initialised cannot be guaranteed,
    /// so make sure no asset relies on another asset being initialised.
    /// </summary>
    protected virtual IPromise OnAssetsLoaded()
    {
        return Promise.Resolved();
    }

    protected virtual IPromise OnAssetsUnloaded()
    {
        return Promise.Resolved();
    }
}
