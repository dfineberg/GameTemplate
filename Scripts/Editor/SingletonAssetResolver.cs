using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class SingletonAssetResolver
{
    static SingletonAssetResolver()
    {
        Resolve();
    }

    private static void Resolve()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
            AssetDatabase.Refresh();
        }

        var singletonTypes = SingletonAsset.GetTypes();
        var assetCreated = false;

        foreach (var type in singletonTypes)
        {
            var assetPath = $"Assets/Resources/{type.Name}.asset";
            if (AssetDatabase.LoadAssetAtPath(assetPath, type) != null) continue;

            Debug.Log($"Creating new SingletonAsset: {type.Name}");
            assetCreated = true;
            var newAsset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(newAsset, assetPath);
        }

        if (assetCreated) AssetDatabase.Refresh();
    }
}
