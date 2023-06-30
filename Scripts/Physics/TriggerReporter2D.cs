using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameTemplate
{
    public class TriggerReporter2D : MonoBehaviour
    {
        public enum TriggerType { PhysicsLayers, TriggerTag }

        public TriggerType Type = TriggerType.PhysicsLayers;
        public LayerMask TriggerLayers = Physics.AllLayers;
        public string TriggerTag;
        [SerializeField] private UnityEvent<Collider2D> OnTriggerEnterEvent;
        [SerializeField] private UnityEvent<Collider2D> OnTriggerExitEvent;

        public Action<Collider2D> TriggerEnterEvent;
        public Action<Collider2D> TriggerExitEvent;

        public Action<Collider2D, TriggerReporter2D> TriggerEnterReporterEvent;
        public Action<Collider2D, TriggerReporter2D> TriggerExitReporterEvent;

        public static bool IgnoreEnterAndExitEvents { get; set; }
        
        /// <summary>
        /// A list of the rigidbodies inside this trigger. Used as part of the AbstractPlayerTrigger system.
        ///
        /// If this reporter is attached to an object with a rigidbody, it means we're looking to get callbacks when
        /// it enters triggers. For that, we don't need to keep track of rigidbodies, though this means the reporter
        /// won't play nicely with nested triggers.
        /// </summary>
        private readonly List<Rigidbody2D> _rigidbodies = new List<Rigidbody2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IgnoreEnterAndExitEvents) return;
            
            var rb = other.attachedRigidbody;
            var objectToCheck = rb ? rb.gameObject : other.gameObject;
            
            if (!CheckObject(objectToCheck) || _rigidbodies.Contains(rb)) 
                return;
        
            if (rb != null) _rigidbodies.Add(rb);
            OnTriggerEnterEvent?.Invoke(other);
            TriggerEnterEvent?.Invoke(other);
            TriggerEnterReporterEvent?.Invoke(other, this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IgnoreEnterAndExitEvents) return;
            
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

        public void ForceRemoveRigidbody(Rigidbody2D rb)
        {
            _rigidbodies.Remove(rb);
        }
    }
}