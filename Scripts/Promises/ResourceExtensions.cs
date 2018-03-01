using System.Collections.Generic;
using UnityEngine;

namespace GameTemplate.Promises
{
    public static class ResourceExtensions
    {
        public static IPromise LoadAsync(string path)
        {
            var p = Promise.Create();
            var resourceRequest = Resources.LoadAsync(path);

            CoroutineExtensions.WaitUntil(resourceRequest)
                .ThenLog("Loaded resource at: " + path)
                .ThenDo(() => p.Resolve(resourceRequest.asset));

            return p;
        }

        public static IPromise LoadAllAsync(IEnumerable<string> paths)
        {
            var p = Promise.Create();
            var promises = paths.SelectEach(LoadAsync);

            Promise.All(promises)
                .ThenResolvePromise(p);

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

        public static IPromise LoadAllFromStringLibrary(StringLibrary lib)
        {
            return LoadAllAsync(lib.StringsWithResourcesDirectory);
        }

        public static IPromise LoadAllFromStringLibrary(string path)
        {
            return LoadAsync(path)
                .Then<StringLibrary>(LoadAllFromStringLibrary);
        }
    }
}