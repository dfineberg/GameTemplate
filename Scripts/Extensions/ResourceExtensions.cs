using System.Collections.Generic;
using System.Linq;
using Promises;
using UnityEngine;

public static class ResourceExtensions
{
    public static IPromise LoadAsync(string path)
    {
        var resourceRequest = Resources.LoadAsync(path);

        return CoroutineExtensions.WaitUntil(resourceRequest)
            .ThenLog("Loaded resource at: " + path)
            .ThenSetPromised(() => resourceRequest.asset);
    }

    public static IPromise LoadAllAsync(IEnumerable<string> paths)
    {
        var pathsArray = paths as string[] ?? paths.ToArray();
        var loadedObjects = new Object[pathsArray.Length];

        var promises = pathsArray.SelectEach((path, i) => LoadAsync(path).ThenDo<Object>(t => loadedObjects[i] = t));

        return Promise.All(promises)
            .ThenSetPromised(() => loadedObjects);
    }

    public static IPromise LoadAllAsync(params string[] paths)
    {
        return LoadAllAsync(paths as IEnumerable<string>);
    }
}