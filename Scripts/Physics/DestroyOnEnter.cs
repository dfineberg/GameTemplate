using UnityEngine;

public class DestroyOnEnter : MonoBehaviour
{
    public LayerMask DestroyLayers;

    private void OnTriggerEnter(Collider other)
    {
        if (!DestroyLayers.ContainsLayer(other.gameObject.layer))
            return;

        Destroy(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!DestroyLayers.ContainsLayer(other.gameObject.layer))
            return;
        
        Destroy(other.gameObject);
    }
}
