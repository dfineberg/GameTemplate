using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PersistentEvent))]
public class PersistentEventEditor : Editor {
    public override void OnInspectorGUI()
    {
        var e = (PersistentEvent) target;
        
        if(GUILayout.Button("Invoke"))
            e.Invoke();
    }
}
