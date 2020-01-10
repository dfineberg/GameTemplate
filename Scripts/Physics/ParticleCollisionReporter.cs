using GameTemplate;
using UnityEngine;

public class ParticleCollisionReporter : MonoBehaviour
{
    [SerializeField] private UnityEventGameObject Callback;
    [SerializeField] private string Tag;
    
    private void OnParticleCollision(GameObject other)
    {
        if (string.IsNullOrEmpty(Tag) || other.HasTag(Tag)) Callback.Invoke(other);
    }
}
