using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    public AnimateOnOffGroup Animator { get; private set; }

    public bool CanFireEvents { get; set; }

    public delegate void UiScreenEventHandler(int i);

    public event UiScreenEventHandler ButtonPressedEvent;

    private void Awake()
    {
        Animator = GetComponentInChildren<AnimateOnOffGroup>();
        CanFireEvents = true;
    }

    public void ButtonPress(int i)
    {
        if (!CanFireEvents)
            return;

        if (ButtonPressedEvent != null)
            ButtonPressedEvent(i);
    }
}