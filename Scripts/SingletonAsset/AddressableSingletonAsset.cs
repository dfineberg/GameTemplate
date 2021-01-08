
using GameTemplate.Promises;

public abstract class AddressableSingletonAsset : SingletonAsset
{
    protected override IPromise OnAssetsUnloaded()
    {
        AddressableExtensions.Release(this);
        return Promise.Resolved();
    }
}