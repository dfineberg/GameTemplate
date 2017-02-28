using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInput : MonoBehaviour
{
    public delegate void TouchEventHandler(Vector2 touchPosition);

    public delegate void DeltaTouchEventHandler(Vector2 touchPosition, Vector2 touchPositionDelta);

    public event TouchEventHandler OnTouchDown;
    public event DeltaTouchEventHandler OnTouchUpdate;
    public event TouchEventHandler OnTouchUp;
    public event TouchEventHandler OnTouchTap;

    private bool _wasTouching;
    private Vector2 _oldTouchPos;
    private float _lastMouseDownTime;

    private const float ClickTime = 0.1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastMouseDownTime = Time.time;

            if (OnTouchDown != null)
                OnTouchDown(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            if (OnTouchUpdate != null)
                OnTouchUpdate(Input.mousePosition, (Vector2)Input.mousePosition - _oldTouchPos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (OnTouchUp != null)
                OnTouchUp(_oldTouchPos);

            if (Time.time - _lastMouseDownTime <= ClickTime && OnTouchTap != null)
                OnTouchTap(Input.mousePosition);
        }

        _oldTouchPos = Input.mousePosition;
    }
}
