using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SceneExtensions
{
    public static T FindObjectOfType<T>(this Scene scene)
    {
        foreach (var gameObject in scene.GetRootGameObjects())
        {
            var t = gameObject.GetComponentInChildren<T>(true);
            if (t != null) return t;
        }

        return default(T);
    }

    public static T[] FindObjectsOfType<T>(this Scene scene)
    {
        var list = new List<T>();

        foreach (var gameObject in scene.GetRootGameObjects())
            list.AddRange(gameObject.GetComponentsInChildren<T>(true));

        return list.ToArray();
    }
}
