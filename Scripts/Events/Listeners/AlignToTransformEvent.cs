using UnityEngine;

namespace GameTemplate
{
    public class AlignToTransformEvent : MonoBehaviour
    {
        public TransformEvent TransformEvent;

        private void OnEnable()
        {
            if(TransformEvent) TransformEvent.Subscribe(Align);
        }

        private void OnDisable()
        {
            if(TransformEvent) TransformEvent.Unsubscribe(Align);
        }

        private void Align(Transform t)
        {
            if (t == null) return;
            transform.SetPositionAndRotation(t.position, t.rotation);
        }
    }
}
