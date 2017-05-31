using System.Linq;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public LayerMask TouchHandlerLayerMask = ~0;
    
    public delegate void TouchEventHandler(Vector2 touchPosition);

    public delegate void DeltaTouchEventHandler(Vector2 touchPosition, Vector2 touchPositionDelta);

    public event TouchEventHandler OnTouchDown;
    public event DeltaTouchEventHandler OnTouchUpdate;
    public event TouchEventHandler OnTouchUp;
    public event TouchEventHandler OnTouchTap;

    private bool _wasTouching;
    private Vector2 _oldTouchPos;
    private float _lastMouseDownTime;

    private IDragInputHandler _dragHandler;

    private const float ClickTime = 0.3f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastMouseDownTime = Time.time;

            var hits = RaycastTouchPosition();
            var handler = hits.Select(h => h.collider.GetComponent<ITouchInputHandler>()).FirstOrDefault(h => h != null);
            
            _dragHandler = hits.Select(h => h.collider.GetComponent<IDragInputHandler>())
                .FirstOrDefault(h => h != null);
            
            if(handler != null)
                handler.HandleTouchDown(Input.mousePosition);
            
            if(_dragHandler != default(IDragInputHandler))
                _dragHandler.HandleBeginDrag(Input.mousePosition);
            
            if (OnTouchDown != null)
                OnTouchDown(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            var touchPositionDelta = (Vector2) Input.mousePosition - _oldTouchPos;

            var handler = RaycastTouchHandler();
            
            if(handler != null)
                handler.HandleTouchUpdate(Input.mousePosition, touchPositionDelta);
            
            if(_dragHandler != default(IDragInputHandler))
            {
                _dragHandler.HandleUpdateDrag(Input.mousePosition, touchPositionDelta);

                if (_dragHandler.ForceDrop())
                {
                    _dragHandler.HandleEndDrag(Input.mousePosition);
                    _dragHandler = default(IDragInputHandler);
                }
            }            
            if (OnTouchUpdate != null)
            {
                OnTouchUpdate(Input.mousePosition, touchPositionDelta);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            var handler = RaycastTouchHandler();
            
            if(handler != null)
                handler.HandleTouchUp(Input.mousePosition);

            if (_dragHandler != default(IDragInputHandler))
            {
                _dragHandler.HandleEndDrag(Input.mousePosition);
                _dragHandler = null;
            }
            
            if (OnTouchUp != null)
                OnTouchUp(_oldTouchPos);

            if (Time.time - _lastMouseDownTime <= ClickTime)
            {
                if(handler != null)
                    handler.HandleTouchTap(Input.mousePosition);
                
                if (OnTouchTap != null)
                    OnTouchTap(Input.mousePosition);
            }
        }

        _oldTouchPos = Input.mousePosition;
    }

    public RaycastHit[] RaycastTouchPosition(float maxDistance = Mathf.Infinity)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.RaycastAll(ray, maxDistance, TouchHandlerLayerMask);
    }

    private ITouchInputHandler RaycastTouchHandler()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit, Mathf.Infinity, TouchHandlerLayerMask) ? hit.collider.GetComponent<ITouchInputHandler>() : null;
    }
}