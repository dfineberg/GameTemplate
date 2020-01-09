using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameTemplate
{
    [Serializable]
    public class ColliderEvent : UnityEvent<Collider>
    {
    }

    public class TriggerReporter : MonoBehaviour
    {
        public enum TriggerType { PhysicsLayers, TriggerTag }

        public TriggerType Type = TriggerType.PhysicsLayers;
        public LayerMask TriggerLayers = Physics.AllLayers;
        public string TriggerTag;
        public ColliderEvent OnTriggerEnterEvent;
        public ColliderEvent OnTriggerExitEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (!CheckObject(other.gameObject)) 
                return;
        
            OnTriggerEnterEvent.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!CheckObject(other.gameObject)) 
                return;
        
            OnTriggerExitEvent.Invoke(other);
        }

        private bool CheckObject(GameObject obj)
        {
            if (Type == TriggerType.PhysicsLayers)
                return TriggerLayers.ContainsLayer(obj.layer);

            return obj.HasTag(TriggerTag);
        }
    }
}