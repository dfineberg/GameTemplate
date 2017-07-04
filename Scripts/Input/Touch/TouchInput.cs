using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public enum RaycastMode
    {
        Physics2D,
        Physics3D
    }

    public LayerMask TouchHandlerLayerMask = ~0;

    public RaycastMode Mode = RaycastMode.Physics3D;

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

    private static Vector2 ScreenToWorldPoint
    {
        get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); }
    }

    private const float ClickTime = 0.3f;

    private void Update()
    {
        // POINTER DOWN
        if (Input.GetMouseButtonDown(0))
        {
            // cache the pointer down time to test for tapping later
            _lastMouseDownTime = Time.time;

            // find an ITouchInputHandler and also search for an IDragInputHandler
            var handler = TouchInputHandler(true);

            // handle events
            if (handler != null)
                handler.HandleTouchDown(Input.mousePosition);

            if (_dragHandler != default(IDragInputHandler))
                _dragHandler.HandleBeginDrag(Input.mousePosition);

            if (OnTouchDown != null)
                OnTouchDown(Input.mousePosition);
        }
        // POINTER PRESSED
        else if (Input.GetMouseButton(0))
        {
            // calculate the difference between pointer position this frame and last frame
            var touchPositionDelta = (Vector2) Input.mousePosition - _oldTouchPos;

            // find an ITouchInputHandler but don't search for a new IDragInputHandler
            var handler = TouchInputHandler(false);

            //handle events
            if (handler != null)
                handler.HandleTouchUpdate(Input.mousePosition, touchPositionDelta);

            if (_dragHandler != default(IDragInputHandler))
            {
                _dragHandler.HandleUpdateDrag(Input.mousePosition, touchPositionDelta);

                // if this is true, stop dragging even before the pointer up event
                if (_dragHandler.ForceDrop())
                {
                    _dragHandler.HandleEndDrag(Input.mousePosition);
                    _dragHandler = default(IDragInputHandler);
                }
            }
            if (OnTouchUpdate != null)
                OnTouchUpdate(Input.mousePosition, touchPositionDelta);
        }
        // POINTER UP
        else if (Input.GetMouseButtonUp(0))
        {
            // find an ITouchInputHandler but don't search for a new IDragInputHandler
            var handler = TouchInputHandler(false);

            // handle events
            if (handler != null)
                handler.HandleTouchUp(Input.mousePosition);

            if (_dragHandler != default(IDragInputHandler))
            {
                _dragHandler.HandleEndDrag(Input.mousePosition);
                _dragHandler = null;
            }

            if (OnTouchUp != null)
                OnTouchUp(_oldTouchPos);

            // if pointer up happened soon after pointer down, fire the tap event as well
            if (Time.time - _lastMouseDownTime <= ClickTime)
            {
                if (handler != null)
                    handler.HandleTouchTap(Input.mousePosition);

                if (OnTouchTap != null)
                    OnTouchTap(Input.mousePosition);
            }
        }

        // cache this to work out the delta position in the next frame
        _oldTouchPos = Input.mousePosition;
    }

    private ITouchInputHandler TouchInputHandler(bool setDragHandler)
    {
        var hitComponent = Mode == RaycastMode.Physics3D ? (Component) RaycastTouchPosition() : OverlapTouchPosition2D();

        if (!hitComponent) 
            return null;
        
        var handler = hitComponent.GetComponent<ITouchInputHandler>();

        if (setDragHandler)
            _dragHandler = hitComponent.GetComponent<IDragInputHandler>();

        return handler;
    }

    public Collider RaycastTouchPosition(float maxDistance = Mathf.Infinity)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, maxDistance);

        return hit.collider;
    }

    public Collider2D OverlapTouchPosition2D()
    {
        return Physics2D.OverlapPoint(ScreenToWorldPoint);
    }

    public Collider2D[] OverlapAllTouchPosition2D()
    {
        return Physics2D.OverlapPointAll(ScreenToWorldPoint);
    }
}