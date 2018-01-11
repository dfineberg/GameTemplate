using System;
using UnityEngine.Events;

namespace GameTemplate
{
    [Serializable]
    public class UnityEventBool : UnityEvent<bool> {}

    [Serializable]
    public class UnityEventFloat : UnityEvent<float> {}

    [Serializable]
    public class UnityEventInt : UnityEvent<int> {}
}