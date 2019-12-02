using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameTemplate
{
    public class Tag : MonoBehaviour
    {
        public List<string> Tags;

        private static readonly Dictionary<string, List<GameObject>> ObjectLists = new Dictionary<string, List<GameObject>>();
        private static readonly Dictionary<string, Action<string>> Callbacks = new Dictionary<string, Action<string>>();
        
        private void OnEnable()
        {
            foreach (var t in Tags)
            {
                RegisterGameObject(t, gameObject);
            }
        }

        private void OnDisable()
        {
            foreach (var t in Tags)
            {
                UnregisterGameObject(t, gameObject);
            }
        }

        private static void RegisterGameObject(string tag, GameObject gameObject)
        {
            if (!ObjectLists.ContainsKey(tag))
            {
                var newList = ObjectPool.Pop<List<GameObject>>();
                ObjectLists.Add(tag, newList);
            }

            ObjectLists[tag].Add(gameObject);
            if (Callbacks.ContainsKey(tag)) Callbacks[tag]?.Invoke(tag);
        }

        private static void UnregisterGameObject(string tag, GameObject gameObject)
        {
            var objectList = ObjectLists[tag];
            objectList.Remove(gameObject);

            if (objectList.Count == 0)
            {
                ObjectLists.Remove(tag);
                ObjectPool.Push(objectList);
            }

            if (Callbacks.ContainsKey(tag)) Callbacks[tag]?.Invoke(tag);
        }

        public bool HasTag(string compareTag)
        {
            foreach (var thisTag in Tags)
                if (thisTag == compareTag)
                    return true;

            return false;
        }

        public void AddTag(string newTag)
        {
            if(!HasTag(newTag)) RegisterGameObject(newTag, gameObject);
        }

        public void RemoveTag(string removeTag)
        {
            if (HasTag(removeTag)) UnregisterGameObject(removeTag, gameObject);
        }

        public static GameObject FindGameObject(string tag)
        {
            if (ObjectLists.ContainsKey(tag))
                return ObjectLists[tag][0];
            
            return GameObject.FindWithTag(tag);
        }

        public static GameObject[] FindGameObjects(string tag)
        {
            if (ObjectLists.ContainsKey(tag))
                return ObjectLists[tag].ToArray();

            return GameObject.FindGameObjectsWithTag(tag);
        }

        public static int FindGameObjectsNonAlloc(string tag, GameObject[] container)
        {
            if (!ObjectLists.ContainsKey(tag))
                return 0;

            var objectList = ObjectLists[tag];
            for (var i = 0; i < objectList.Count && i < container.Length; i++) container[i] = objectList[i];
            return objectList.Count;
        }

        public static void RegisterTagUpdatedCallback(string tag, Action<string> callback)
        {
            if (Callbacks.ContainsKey(tag)) Callbacks[tag] += callback;
            else Callbacks.Add(tag, callback);
        }

        public static void UnregisterTagUpdatedCallback(string tag, Action<string> callback)
        {
            if (Callbacks.ContainsKey(tag)) Callbacks[tag] -= callback;
        }
    }

    public static class TagExtensions
    {
        public static bool HasTag(this GameObject gameObject, string tag)
        {
            if (gameObject.TryGetComponent<Tag>(out var t) && t.HasTag(tag)) return true;
            return gameObject.CompareTag(tag);
        }
    }
}