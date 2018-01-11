using GameTemplate.Promises;
using UnityEngine;

namespace GameTemplate
{
    public static class AnimatorExtensions
    {
        public static IPromise WaitUntilState(this Animator animator, string state, int layer = 0)
        {
            return CoroutineExtensions.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(layer).IsName(state));
        }

        public static IPromise WaitUntilState(this Animator animator, int state, int layer = 0)
        {
            return CoroutineExtensions.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(layer).shortNameHash == state);
        }

        public static IPromise WaitForEndOfState(this Animator animator, int layer = 0)
        {
            return CoroutineExtensions.WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= 0.999f);
        }

        public static IPromise WaitForEndOfState(this Animator animator, string state, int layer = 0)
        {
            return WaitUntilState(animator, state, layer)
                .Then(() => WaitForEndOfState(animator, layer));
        }
    }
}
