using GameTemplate;
using UnityEngine;

public class AnimatorIntListener : GenericValueListener<IntValue, int>
{
    public string ParameterName;        
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void Listener(int arg)
    {
        _animator.SetInteger(ParameterName, arg);
    }
}
