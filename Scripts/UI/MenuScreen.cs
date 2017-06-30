using UnityEngine;
using UnityEngine.EventSystems;

public class MenuScreen : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler,
    IPointerDownHandler, IPointerUpHandler
{
    private RectTransform _rt;

    public AnimateOnOffGroup Animator { get; private set; }

    public bool CanFireEvents { get; set; }

    public delegate void MenuScreenButtonEventHandler(int i);
    public delegate void MenuScreenBackButtonEventHandler();
    public delegate void MenuScreenPointerEventHandler(PointerEventData eventData);

    public event MenuScreenButtonEventHandler ButtonPressedEvent;
    public event MenuScreenBackButtonEventHandler BackButtonPressedEvent;
    public event MenuScreenPointerEventHandler DragEvent;
    public event MenuScreenPointerEventHandler BeginDragEvent;
    public event MenuScreenPointerEventHandler EndDragEvent;
    public event MenuScreenPointerEventHandler PointerClickEvent;
    public event MenuScreenPointerEventHandler PointerDownEvent;
    public event MenuScreenPointerEventHandler PointerUpEvent;


    private void Awake()
    {
        Animator = GetComponentInChildren<AnimateOnOffGroup>();
        CanFireEvents = true;

        _rt = transform as RectTransform;
    }

    public void ButtonPress(int i)
    {
        if (!CanFireEvents)
            return;

        if (ButtonPressedEvent != null)
            ButtonPressedEvent(i);
    }

    public void BackButtonPress()
    {
        if (!CanFireEvents)
            return;

        if (BackButtonPressedEvent != null)
            BackButtonPressedEvent();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DragEvent != null)
            DragEvent(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (BeginDragEvent != null)
            BeginDragEvent(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EndDragEvent != null)
            EndDragEvent(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PointerClickEvent != null)
            PointerClickEvent(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PointerDownEvent != null)
            PointerDownEvent(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PointerUpEvent != null)
            PointerUpEvent(eventData);
    }

    public bool ContainsScreenPoint(Vector2 screenPoint)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(_rt, screenPoint);
    }

    public bool ContainsScreenPoint(PointerEventData eventData)
    {
        return ContainsScreenPoint(eventData.position);
    }
}