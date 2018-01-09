#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GenericValue<T> : GenericEvent<T>
{
    [SerializeField] private T _value;
    public T Value
    {
        get { return _value; }
        set
        {
            _value = value;
            Invoke(_value);
        }
    }

    protected void OnEnable()
    {
        ResetToDefault();
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
#endif
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
        if (Application.isPlaying) // if value is changed at runtime in the inspector, invoke the event
            Invoke(_value);
        else
            _value = _defaultValue; // don't allow value to be changed in the inspector while the game isn't running
    }
#endif

    public void ResetToDefault()
    {
        Value = _defaultValue;
    }

    public void Set(T newValue)
    {
        Value = newValue;
    }
}
