using System.Collections.Generic;
using UnityEngine;

namespace GameTemplate
{
    public class Tag : MonoBehaviour
    {
        public List<string> Tags;

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