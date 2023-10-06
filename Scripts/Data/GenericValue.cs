#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GameTemplate
{
    public abstract class GenericValue<T> : GenericEvent<T>
    {
        [SerializeField] protected T _value;

        [Header("Debug Command Options")]
        public bool IsDebugCommand;
        public string DebugCommandName;
        [TextArea] public string DebugCommandDescription;

        public T Value
        {
            get { return _value; }
            set
            {
                if (Equals(value)) return;
                if (!Application.isPlaying) _defaultValue = value;
                SetValue(value);
            }
        }

        protected virtual void OnEnable()
        {
            ResetToDefault();
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
#endif
        }

        protected virtual void SetValue(T value)
        {
            _value = value;
#if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return; // don't invoke if we're leaving play mode
#endif
            Invoke(_value);
        }

#if UNITY_EDITOR
        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= HandlePlayModeStateChanged;
        }

        private void HandlePlayModeStateChanged(PlayModeStateChange change)
        {
            if(change == PlayModeStateChange.ExitingPlayMode) ResetToDefault();
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
                _defaultValue = _value; // _defaultValue can only be set in edit mode
            
            Invoke(_value);
        }
#endif

        public virtual void ResetToDefault()
        {
            Value = _defaultValue;
        }

        public void UpdateDefaultValue()
        {
            _defaultValue = _value;
        }

        protected abstract bool Equals(T other);
    }
}
