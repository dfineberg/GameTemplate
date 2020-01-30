using System;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorStateListener : MonoBehaviour
{
    [Serializable]
    public class AnimatorStateListenerEvent
    {
        public string StateName;
        public int LayerIndex;
        public UnityEvent Callback;
        internal bool OldStateCheck;
    }

    public AnimatorStateListenerEvent[] Listeners;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Listeners == null || Listeners.Length == 0) return;
        
        var layerCache = -1;
        var stateInfo = default(AnimatorStateInfo);
        
        foreach (var listener in Listeners)
        {
            if (listener.LayerIndex != layerCache)
                stateInfo = _animator.GetCurrentAnimatorStateInfo(listener.LayerIndex);

            layerCache = listener.LayerIndex;
            
            var stateCheck = stateInfo.shortNameHash == Animator.StringToHash(listener.StateName);
            if (stateCheck && !listener.OldStateCheck) listener.Callback.Invoke();
            listener.OldStateCheck = stateCheck;
        }
    }
}