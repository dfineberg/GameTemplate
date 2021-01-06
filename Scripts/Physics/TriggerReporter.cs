using System;
using System.Collections.Generic;
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
        [SerializeField] private ColliderEvent OnTriggerEnterEvent;
        [SerializeField] private ColliderEvent OnTriggerExitEvent;

        public Action<Collider> TriggerEnterEvent;
        public Action<Collider> TriggerExitEvent;

        public Action<Collider, TriggerReporter> TriggerEnterReporterEvent;
        public Action<Collider, TriggerReporter> TriggerExitReporterEvent;

        /// <summary>
        /// A list of the rigidbodies inside this trigger. Used as part of the AbstractPlayerTrigger system.
        ///
        /// If this reporter is attached to an object with a rigidbody, it means we're looking to get callbacks when
        /// it enters triggers. For that, we don't need to keep track of rigidbodies, though this means the reporter
        /// won't play nicely with nested triggers.
        /// </summary>
        private readonly List<Rigidbody> _rigidbodies = new List<Rigidbody>();

        private void OnTriggerEnter(Collider other)
        {
            var rb = other.attachedRigidbody;
            
            if (!CheckObject(other.gameObject) || _rigidbodies.Contains(rb)) 
                return;
        
            if (rb != null) _rigidbodies.Add(rb);
            OnTriggerEnterEvent?.Invoke(other);
            TriggerEnterEvent?.Invoke(other);
            TriggerEnterReporterEvent?.Invoke(other, this);
        }

        private void OnTriggerExit(Collider other)
        {
            var rb = other.attachedRigidbody;
            
            if (!CheckObject(other.gameObject) || rb != null && !_rigidbodies.Contains(rb)) 
                return;

            _rigidbodies.Remove(rb);
            OnTriggerExitEvent?.Invoke(other);
            TriggerExitEvent?.Invoke(other);
            TriggerExitReporterEvent?.Invoke(other, this);
        }

        private bool CheckObject(GameObject obj)
        {
            if (Type == TriggerType.PhysicsLayers)
                return TriggerLayers.ContainsLayer(obj.layer);

            return obj.HasTag(TriggerTag);
        }

        public void ForceRemoveRigidbody(Rigidbody rb)
        {
            _rigidbodies.Remove(rb);
        }
    }
}