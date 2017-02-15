using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CanvasExtensions {

    static Transform _loadingScreenTransform;

    public static void SetLoadingScreenTransform(Transform t)
    {
        _loadingScreenTransform = t;
    }

	public static GameObject InstantiateBehindLoadingScreen(this Canvas canvas, GameObject prefab)
    {
        GameObject newScreenObject = Object.Instantiate<GameObject>(prefab, canvas.transform);

        if(_loadingScreenTransform)
            newScreenObject.transform.SetSiblingIndex(Mathf.Max(_loadingScreenTransform.GetSiblingIndex() - 1, 0));
        else
            Debug.LogError("No loading screen set in CanvasExtensions!");

        if (newScreenObject.transform is RectTransform)
        {
            RectTransform prefabRt = prefab.transform as RectTransform;
            RectTransform rt = newScreenObject.transform as RectTransform;

            rt.anchorMin = prefabRt.anchorMin;
            rt.anchorMax = prefabRt.anchorMax;

            rt.sizeDelta = prefabRt.sizeDelta;
            rt.anchoredPosition = prefabRt.anchoredPosition;
        }

        return newScreenObject;
    }
}
