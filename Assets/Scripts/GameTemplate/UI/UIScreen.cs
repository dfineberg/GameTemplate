using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour {

	public AnimateOnOffGroup animator { get; private set; }

    public bool canFireEvents { get; set; }

    public delegate void UIScreenEventHandler(int i);

    public event UIScreenEventHandler eButtonPressed;

    void Awake()
    {
        animator = GetComponentInChildren<AnimateOnOffGroup>();
        canFireEvents = true;
    }

    public void ButtonPress(int i)
    {
        if (!canFireEvents)
            return;

        if (eButtonPressed != null)
            eButtonPressed(i);
    }
}
