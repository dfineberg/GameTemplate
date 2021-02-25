using System.IO;
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
        if (EditorApplication.isCompiling) return;

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
            AssetDatabase.Refresh();
        }

        var singletonTypes = SingletonAsset.GetTypes();
        var assetCreated = false;

        foreach (var type in singletonTypes)
        {
            string assetPath;
            if (type.IsSubclassOf(typeof(AddressableSingletonAsset)))
            {
               assetPath = $"Assets/Resources_moved/{type.Name}.asset";
            }
            else
            {
               assetPath = $"Assets/Resources/{type.Name}.asset";
            }
            if (AssetDatabase.LoadAssetAtPath(assetPath, type) != null) continue;
            if (File.Exists(assetPath)) continue;

            string oldPath = assetPath.Replace("Resources_moved", "Resources");
            if (type.IsSubclassOf(typeof(AddressableSingletonAsset)) && File.Exists(oldPath))
            {
                Debug.Log($"Moving AddressableSingletonAsset: {type.Name}");
                AssetDatabase.MoveAsset(oldPath, assetPath);
                assetCreated = true;
            }
            else
            {
                Debug.Log($"Creating new SingletonAsset: {type.Name}");
                assetCreated = true;
                var newAsset = ScriptableObject.CreateInstance(type);
                AssetDatabase.CreateAsset(newAsset, assetPath);
            }
        }

        if (assetCreated) AssetDatabase.Refresh();
    }
}
