
using GameTemplate.Promises;

public abstract class AddressableSingletonAsset : SingletonAsset
{
    public static IPromise InstanceAsync<T>() where T : AddressableSingletonAsset
    {
        if (AssetDictionary.TryGetValue(typeof(T), out var asset))
        {
            return Promise.Resolved((T) asset);
        }

        return AddressableExtensions.LoadAsync<T>(typeof(T).Name);
    }
}