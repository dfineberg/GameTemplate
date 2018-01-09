using UnityEngine;

public class AlignToTransformEvent : MonoBehaviour
{
    public TransformEvent TransformEvent;

    private void OnEnable()
    {
        TransformEvent?.Subscribe(Align);
    }

    private void OnDisable()
    {
        TransformEvent?.Unsubscribe(Align);
    }

    private void Align(Transform t)
    {
        if (t == null)
            return;
        
        transform.position = t.position;
        transform.rotation = t.rotation;
    }
}
