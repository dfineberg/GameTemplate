using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BasicEvent))]
public class PersistentEventEditor : Editor {
    public override void OnInspectorGUI()
    {
        var e = (BasicEvent) target;
        
        if(GUILayout.Button("Invoke"))
            e.Invoke();
    }
}
