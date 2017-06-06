using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityTouchEvent : UnityEvent<Vector2>{}

[Serializable]
public class UnityTouchUpdateEvent : UnityEvent<Vector2, Vector2>{}

public class TouchInputReporter : MonoBehaviour, ITouchInputHandler
{
    public delegate void TouchEventHandler(Vector2 touchPosition);

    public delegate void DeltaTouchEventHandler(Vector2 touchPosition, Vector2 touchPositionDelta);

    public event TouchEventHandler OnTouchDown;
    public event DeltaTouchEventHandler OnTouchUpdate;
    public event TouchEventHandler OnTouchUp;
    public event TouchEventHandler OnTouchTap;

    public UnityTouchEvent OnTouchDownUnityEvent;
    public UnityTouchUpdateEvent OnTouchUpdateUnityEvent;
    public UnityTouchEvent OnTouchUpUnityEvent;
    public UnityTouchEvent OnTouchTapUnityEvent;

    public void HandleTouchDown(Vector2 touchPosition)
    {
        if (OnTouchDown != null)
            OnTouchDown(touchPosition);
        
        OnTouchDownUnityEvent.Invoke(touchPosition);
    }

    public void HandleTouchUpdate(Vector2 touchPosition, Vector2 touchPositionDelta)
    {
        if (OnTouchUpdate != null)
            OnTouchUpdate(touchPosition, touchPositionDelta);
        
        OnTouchUpdateUnityEvent.Invoke(touchPosition, touchPositionDelta);
    }

    public void HandleTouchUp(Vector2 touchPosition)
    {
        if (OnTouchUp != null)
            OnTouchUp(touchPosition);
        
        OnTouchUpUnityEvent.Invoke(touchPosition);
    }

    public void HandleTouchTap(Vector2 touchPosition)
    {
        if (OnTouchTap != null)
            OnTouchTap(touchPosition);
        
        OnTouchTapUnityEvent.Invoke(touchPosition);
    }
}