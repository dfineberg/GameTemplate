using GameTemplate;
using UnityEngine;

public class AnimatorBoolListener : GenericValueListener<BoolValue, bool>
{
    public string ParameterName;    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void Listener(bool arg)
    {
        _animator.SetBool(ParameterName, arg);
    }
}
