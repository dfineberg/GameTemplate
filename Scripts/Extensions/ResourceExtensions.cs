using System.Collections.Generic;
using System.Linq;
using Promises;
using UnityEngine;

public static class ResourceExtensions
{
    public static IPromise LoadAsync(string path)
    {
        var p = new Promise();
        var resourceRequest = Resources.LoadAsync(path);

        CoroutineExtensions.WaitUntil(resourceRequest)
            .ThenLog("Loaded resource at: " + path)
            .ThenDo(() => p.Resolve(resourceRequest.asset));

        return p;
    }

    public static IPromise LoadAllAsync(IEnumerable<string> paths)
    {
        var p = new Promise();
        var promises = paths.SelectEach(LoadAsync);

        Promise.All(promises)
            .ThenDo<object>(o => p.Resolve(o));

        return p;
    }

    public static IPromise LoadAllAsync(params string[] paths)
    {
        return LoadAllAsync(paths as IEnumerable<string>);
    }

    public static IPromise LoadAllAsync(string prefix, params string[] paths)
    {
        var prefixedPaths = paths.SelectEach(p => string.Concat(prefix, p));
        return LoadAllAsync(prefixedPaths);
    }
}