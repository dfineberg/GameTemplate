using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
    public static IEnumerable<GameObject> Clone(this GameObject prefab, int length)
    {
        var clones = new GameObject[length];
        clones[0] = prefab;

        for (var i = 1; i < length; i++)
            clones[i] = Object.Instantiate(prefab, prefab.transform.parent);

        return clones;
    }

    public static IEnumerable<T> Clone<T>(this T prefab, int length) where T : Component
    {
        return prefab.gameObject.Clone(length).SelectEach(o => o.GetComponent<T>());
    }
}