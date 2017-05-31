using System;
using UnityEngine;

public class OnTriggerEnterReporter : MonoBehaviour
{
    public Action<Collider> OnTriggerEnterEvent;
    public Action<Collider2D> OnTriggerEnter2DEvent;

    public Action<Collider> OnTriggerExitEvent;
    public Action<Collider2D> OnTriggerExit2DEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (OnTriggerEnterEvent != null)
            OnTriggerEnterEvent(other);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (OnTriggerEnter2DEvent != null)
            OnTriggerEnter2DEvent(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if(OnTriggerExitEvent != null)
            OnTriggerExitEvent(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(OnTriggerExit2DEvent != null)
            OnTriggerEnter2DEvent(other);
    }
}