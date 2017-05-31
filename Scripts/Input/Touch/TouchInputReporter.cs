using UnityEngine;

public class TouchInputReporter : MonoBehaviour, ITouchInputHandler
{
    public delegate void TouchEventHandler(Vector2 touchPosition);

    public delegate void DeltaTouchEventHandler(Vector2 touchPosition, Vector2 touchPositionDelta);

    public event TouchEventHandler OnTouchDown;
    public event DeltaTouchEventHandler OnTouchUpdate;
    public event TouchEventHandler OnTouchUp;
    public event TouchEventHandler OnTouchTap;

    public void HandleTouchDown(Vector2 touchPosition)
    {
        if (OnTouchDown != null)
            OnTouchDown(touchPosition);
    }

    public void HandleTouchUpdate(Vector2 touchPosition, Vector2 touchPositionDelta)
    {
        if (OnTouchUpdate != null)
            OnTouchUpdate(touchPosition, touchPositionDelta);
    }

    public void HandleTouchUp(Vector2 touchPosition)
    {
        if (OnTouchUp != null)
            OnTouchUp(touchPosition);
    }

    public void HandleTouchTap(Vector2 touchPosition)
    {
        if (OnTouchTap != null)
            OnTouchTap(touchPosition);
    }
}