using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public static class CanvasExtensions
{
    private static Transform _loadingScreenTransform;

    public static void SetLoadingScreenTransform(Transform t)
    {
        _loadingScreenTransform = t;
    }

    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public static GameObject InstantiateBehindLoadingScreen(this Canvas canvas, GameObject prefab)
    {
        var newScreenObject = Object.Instantiate(prefab, canvas.transform);

        if (_loadingScreenTransform)
            newScreenObject.transform.SetSiblingIndex(Mathf.Max(_loadingScreenTransform.GetSiblingIndex() - 1, 0));
        else
            Debug.LogError("No loading screen set in CanvasExtensions!");


        var prefabRt = prefab.transform as RectTransform;
        var rt = newScreenObject.transform as RectTransform;

        if (rt == null) return newScreenObject;

        rt.anchorMin = prefabRt.anchorMin;
        rt.anchorMax = prefabRt.anchorMax;

        rt.sizeDelta = prefabRt.sizeDelta;
        rt.anchoredPosition = prefabRt.anchoredPosition;

        return newScreenObject;
    }
}