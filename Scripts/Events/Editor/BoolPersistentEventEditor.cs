using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoolEvent))]
public class BoolPersistentEventEditor : Editor
{
    private bool _testValue;

    public override void OnInspectorGUI()
    {
        _testValue = EditorGUILayout.Toggle("Test Value", _testValue);

        if (GUILayout.Button("Invoke"))
        {
            var e = (BoolEvent) target;
            e.Invoke(_testValue);
        }
    }
}
