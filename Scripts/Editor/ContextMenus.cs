using UnityEditor;
using UnityEngine;

public class ContextMenus : MonoBehaviour
{
    [MenuItem("CONTEXT/RectTransform/Align Anchors")]
    private static void AlignAnchors(MenuCommand command)
    {
        var rt = (RectTransform) command.context;
        var parent = (RectTransform) rt.parent;
        
        Undo.RecordObject(rt, "Changed anchor positions");

        // calculate current offset as a proportion of parent size
        var min = new Vector2(rt.offsetMin.x / parent.rect.width, rt.offsetMin.y / parent.rect.height);
        var max = new Vector2(rt.offsetMax.x / parent.rect.width, rt.offsetMax.y / parent.rect.height);

        // add it to current anchor positions
        rt.anchorMin += min;
        rt.anchorMax += max;

        // set offset to zero
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}