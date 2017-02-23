using System;
using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    public event Action ETestComplete;

    public Transform TweenTransform;

    private GameObject _cube;

    private void Start()
    {
        /*ResourceExtensions.LoadAllAsync<GameObject>(
			new [] { "Cube", "Capsule" },
			resources => {
				_cube = Instantiate(resources[0], new Vector3(-2f, 0f, 0f), Quaternion.identity);
				Instantiate(resources[1], new Vector3(2f, 0f, 0f), Quaternion.identity);
				}
		)
		.ThenLog("load async complete")
		.ThenWaitForSeconds(3f)
		.ThenTween(
			2f,
			Easing.Functions.CubicEaseInOut,
			t => _cube.transform.LerpPosition(Vector3.zero, new Vector3(0f, 5f, 0f), t)
		)
		.ThenLog("cube animation complete");*/

        CoroutineExtensions.WaitForCoroutine(TestRoutine())
            .ThenLog("wait for coroutine complete")
            .ThenTween(
                2f,
                Easing.Functions.BackEaseOut,
                new Vector3(-5f, 0f, 0f),
                new Vector3(5f, 0f, 0f),
                TweenTransform.LerpPosition
            )
            .ThenLog("anim 1")
            .ThenWaitForSeconds(1f)
            .ThenTween(
                2f,
                Easing.Functions.CircularEaseInOut,
                1f,
                3f,
                TweenTransform.LerpScale
            )
            .ThenLog("anim 2")
            .ThenWaitForSeconds(1f)
            .ThenAll(
                () => CoroutineExtensions.Tween(
                    1f,
                    Easing.Functions.BounceEaseOut,
                    3f,
                    0.5f,
                    TweenTransform.LerpScale
                ),
                () => CoroutineExtensions.Tween(
                    3f,
                    Easing.Functions.ExponentialEaseOut,
                    new Vector3(5f, 0f, 0f),
                    new Vector3(-5f, 3f, 0f),
                    TweenTransform.LerpPosition
                )
            )
            .ThenLog("anim 3")
            .ThenDo(() =>
            {
                if (ETestComplete != null)
                    ETestComplete();
            });
    }

    private static IEnumerator TestRoutine()
    {
        yield return new WaitForSeconds(3f);
    }
}