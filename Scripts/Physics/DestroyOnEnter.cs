using UnityEngine;

namespace GameTemplate
{
    public class DestroyOnEnter : MonoBehaviour
    {
        public LayerMask DestroyLayers;

#if PHYSICS3D_ENABLED
        private void OnTriggerEnter(Collider other)
        {
            if (!DestroyLayers.ContainsLayer(other.gameObject.layer))
                return;

            Destroy(other.gameObject);
        }
#endif

#if PHYSICS2D_ENABLED
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!DestroyLayers.ContainsLayer(other.gameObject.layer))
                return;
        
            Destroy(other.gameObject);
        }
#endif
    }
}
