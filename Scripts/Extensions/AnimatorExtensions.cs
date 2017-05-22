using Promises;
using UnityEngine;

public static class AnimatorExtensions
{
    public static IPromise WaitUntilState(this Animator animator, string state)
    {
        return CoroutineExtensions.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state));
    }
}
