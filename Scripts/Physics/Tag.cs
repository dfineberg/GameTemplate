using System.Collections.Generic;
using UnityEngine;

namespace GameTemplate
{
    public class Tag : MonoBehaviour
    {
        public List<string> Tags;

        private static readonly Dictionary<string, List<GameObject>> ObjectLists = new Dictionary<string, List<GameObject>>();

        private void OnEnable()
        {
            foreach (var t in Tags)
            {
                if (!ObjectLists.ContainsKey(t))
                {
                    var newList = ObjectPool.Pop<List<GameObject>>();
                    ObjectLists.Add(t, newList);
                }

                ObjectLists[t].Add(gameObject);
            }
        }

        private void OnDisable()
        {
            foreach (var t in Tags)
            {
                var objectList = ObjectLists[t];
                objectList.Remove(gameObject);

                if (objectList.Count == 0)
                {
                    ObjectLists.Remove(t);
                    ObjectPool.Push(objectList);
                }
            }
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
            if(!HasTag(newTag)) Tags.Add(newTag);
        }

        public void RemoveTag(string removeTag)
        {
            if (HasTag(removeTag)) Tags.Remove(removeTag);
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