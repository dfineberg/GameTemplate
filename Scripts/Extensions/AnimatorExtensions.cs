using Promises;
using UnityEngine;

public static class AnimatorExtensions
{
    public static IPromise WaitUntilState(this Animator animator, string state, int layer = 0)
    {
        return CoroutineExtensions.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(layer).IsName(state));
    }
}
