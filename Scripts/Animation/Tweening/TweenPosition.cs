using UnityEngine;

namespace GameTemplate
{
    [AddComponentMenu("Animation/Position Tween")]
    public class TweenPosition : AbstractTween<Vector3>
    {
        public bool AnimateInWorldPosition;

        public virtual Vector3 GetValue(float normalisedPoint)
        {
            return Vector3.Lerp(FromValue, ToValue, normalisedPoint);
        }

        protected override void SetValue(float normalisedPoint)
        {
            if (AnimateInWorldPosition)
                transform.position = GetValue(normalisedPoint);
            else
                transform.localPosition = GetValue(normalisedPoint);
        }
    }
}