using UnityEngine;

namespace GameTemplate
{
    public interface IDragInputHandler
    {
        bool CanDrag { get; }
        void HandleBeginDrag(Vector2 touchPos);
        void HandleUpdateDrag(Vector2 touchPos, Vector2 touchPosDelta);
        void HandleEndDrag(Vector2 touchPos);
        bool ForceDrop();
    }
}