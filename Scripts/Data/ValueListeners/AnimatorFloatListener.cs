using GameTemplate;
using UnityEngine;

public class AnimatorFloatListener : GenericValueListener<FloatValue, float>
{
    public string ParameterName;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void Listener(float arg)
    {
        _animator.SetFloat(ParameterName, arg);
    }
}
