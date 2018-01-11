using UnityEngine;

namespace GameTemplate
{
    public static class TransformExtensions
    {
        public static void LerpPosition(this Transform transform, Vector3 from, Vector3 to, float f)
        {
            transform.position = Vector3.LerpUnclamped(from, to, f);
        }

        public static void LerpScale(this Transform transform, Vector3 from, Vector3 to, float f)
        {
            transform.localScale = Vector3.LerpUnclamped(from, to, f);
        }

        public static void LerpScale(this Transform transform, float from, float to, float f)
        {
            transform.LerpScale(new Vector3(from, from, from), new Vector3(to, to, to), f);
        }

        public static void LerpRotation(this Transform transform, Quaternion from, Quaternion to, float f)
        {
            transform.localRotation = Quaternion.SlerpUnclamped(from, to, f);
        }

        public static void LerpRotation(this Transform transform, Vector3 from, Vector3 to, float f)
        {
            transform.LerpRotation(Quaternion.Euler(from), Quaternion.Euler(to), f);
        }

        public static void FillParent(this RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;

            rt.offsetMin = rt.offsetMax = Vector2.zero;
        }
    }
}