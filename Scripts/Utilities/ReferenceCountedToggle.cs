using System;
using System.Collections.Generic;

namespace GameTemplate
{
    public class ReferenceCountedToggle<T>
    {
        private HashSet<T> references;
        private Action<bool> onToggle;

        public bool IsActive => references.Count > 0;

        public IEnumerable<T> References => references;

        public ReferenceCountedToggle(Action<bool> onToggle, bool callToInit)
        {
            references = new HashSet<T>();
            this.onToggle = onToggle;

            if (callToInit)
            {
                onToggle?.Invoke(IsActive);
            }
        }

        public void SetActive(T key)
        {
            Set(key, true);
        }

        public void SetInactive(T key)
        {
            Set(key, false);            
        }

        public void Reset()
        {
            if (!IsActive) return;

            references.Clear();
            onToggle?.Invoke(IsActive);
        }

        public void Set(T key, bool active)
        {
            bool wasActive = IsActive;
            
            if (active) references.Add(key);
            else        references.Remove(key);

            if (IsActive != wasActive)
            {
                onToggle?.Invoke(IsActive);
            }
        }
    }
}