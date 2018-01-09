using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransformEvent))]
public class TransformPersistentEventEditor : Editor
{
    private Transform _testTransform;
    
    public override void OnInspectorGUI()
    {
        _testTransform = (Transform) EditorGUILayout.ObjectField("Test Transform", _testTransform,  typeof(Transform), true);

        if (GUILayout.Button("Invoke"))
        {
            var e = (TransformEvent) target;
            e.Invoke(_testTransform);
        }
    }
}
