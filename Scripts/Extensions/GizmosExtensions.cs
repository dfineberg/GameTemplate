using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GizmosExtensions
{
    public static void DrawLabel(Vector3 position, string text, GUIStyle style = default)
    {
#if UNITY_EDITOR
        if (style == default) Handles.Label(position, text);
        else Handles.Label(position, text, style);
#endif
    }
}
