using UnityEngine;

namespace GameTemplate
{
    public class StaticUpdate : MonoBehaviour
    {
        public delegate void UpdateDelegate();
            
        private static StaticUpdate _instance;

        private event UpdateDelegate Callback;
        
        private static void Init()
        {
            var obj = new GameObject("StaticUpdate") {hideFlags = HideFlags.HideInHierarchy};
            DontDestroyOnLoad(obj);
            _instance = obj.AddComponent<StaticUpdate>();
        }

        public static void Subscribe(UpdateDelegate callback)
        {
            if (!Application.isPlaying)
                return;
            
            if (!_instance)
                Init();

            _instance.Callback += callback;
        }

        public static void Unsubscribe(UpdateDelegate callback)
        {
            if (!Application.isPlaying)
                return;
            
            if (!_instance)
                Init();

            _instance.Callback -= callback;
        }

        private void Update()
        {
            Callback?.Invoke();
        }
    }
}