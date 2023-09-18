using UnityEngine;

namespace GameTemplate
{
    [CreateAssetMenu(menuName = "Data/ReferenceCountedBoolValue")]
    public class ReferenceCountedBoolValue : BoolValue
    {
        private ReferenceCountedToggle<string> toggle;

        protected override void OnEnable()
        {
            Init();
            base.OnEnable();
        }

        public void SetValue(bool value, string key)
        {
            Init();

            bool active = (value != _defaultValue);
            toggle.Set(key, active);
        }

        protected override void SetValue(bool value)
        {
            Debug.LogError($"You cannot set reference counted value directly ({name})");
        }

        public override void ResetToDefault()
        {
            Init();

            toggle.Reset();
        }

        private void ReferenceToggle(bool active)
        {
            _value = active ? !_defaultValue : _defaultValue;

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return; // don't invoke if we're leaving play mode
#endif
            Invoke(_value);
        }

        private void Init()
        {
            if (toggle != null) return;
            toggle = new ReferenceCountedToggle<string>(ReferenceToggle, false);
        }
    }
}
