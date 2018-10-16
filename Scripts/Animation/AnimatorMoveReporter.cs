using System;
using UnityEngine;

public class AnimatorMoveReporter : MonoBehaviour
{
    public event Action OnAnimatorMoveEvent;

    private void OnAnimatorMove()
    {
        OnAnimatorMoveEvent?.Invoke();
    }
}
