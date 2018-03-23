using UnityEngine;

public class AnimationParameterWatcher : MonoBehaviour
{
    public delegate void ParameterCrossedZero(string parameter, bool negativeToPositive);
    public event ParameterCrossedZero OnZeroCross;
    
    public string[] Parameters;

    private Animator _animator;
    private float[] _lastValues;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _lastValues = new float[Parameters.Length];

        for (var i = 0; i < Parameters.Length; i++)
            _lastValues[i] = _animator.GetFloat(Parameters[i]);
    }

    private void LateUpdate()
    {
        for (var i = 0; i < Parameters.Length; i++)
        {
            var thisValue = _animator.GetFloat(Parameters[i]);
            var lastValue = _lastValues[i];
            
            if(lastValue < 0 && thisValue > 0)
                OnZeroCross?.Invoke(Parameters[i], true);
            else if(lastValue > 0 && thisValue < 0)
                OnZeroCross?.Invoke(Parameters[i], false);

            _lastValues[i] = thisValue;
        }
    }
}
