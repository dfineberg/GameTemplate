using UnityEngine;

namespace GameTemplate
{
    public interface ITouchInputHandler
    {
        void HandleTouchDown(Vector2 touchPosition);
        void HandleTouchUpdate(Vector2 touchPosition, Vector2 touchPositionDelta);
        void HandleTouchUp(Vector2 touchPosition);
        void HandleTouchTap(Vector2 touchPosition);
    }
}
