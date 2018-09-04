using System;
using UnityEngine;

public static class JsonUtilityExtensions
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Array;
    }
    
    public static T[] FromJsonArray<T>(string json)
    {
        var newJson = "{ \"Array\": " + json + "}";
        var wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.Array;
    }
}