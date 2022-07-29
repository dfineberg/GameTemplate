using UnityEditor;
using UnityEngine;

namespace GameTemplate
{
    [CustomEditor(typeof(Vector2Event))]
    public class Vector2EventEditor : Editor
    {
        private Vector2 _testValue;

        public override void OnInspectorGUI()
        {
            _testValue = EditorGUILayout.Vector2Field("Test Value", _testValue);

            if (GUILayout.Button("Invoke"))
            {
                var e = (Vector2Event) target;
                e.Invoke(_testValue);
            }
        }
    }
}