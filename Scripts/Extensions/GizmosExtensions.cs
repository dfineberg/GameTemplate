using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class GizmosExtensions
{
    private static GUIStyle _style;
    public static void DrawLabel(Vector3 position, string text, TextAnchor textAnchor, float fontSize = 150f, Color textColour = default)
    {
#if UNITY_EDITOR
        var cameraPos = Camera.current.transform.InverseTransformPoint(position);
        var scaledSize = Mathf.RoundToInt(fontSize / cameraPos.magnitude);
        if (_style == null) _style = new GUIStyle(GUI.skin.label);
        _style.fontSize = scaledSize;
        _style.alignment = textAnchor;
        _style.contentOffset = new Vector2(10f, 0f);

        if (scaledSize > 0)
        {
            GUI.color = textColour == default ? Color.white : textColour;
            Handles.Label(position, text, _style);
        }
#endif
    }
}
