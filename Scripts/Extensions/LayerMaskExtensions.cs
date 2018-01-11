using UnityEngine;

namespace GameTemplate
{
    public static class LayerMaskExtensions {

        public static int BuildMask(params int[] layers)
        {
            var mask = 0;

            foreach (var layer in layers)
                mask = mask | (1 << layer);

            return mask;
        }

        public static int BuildInverseMask(params int[] layers)
        {
            var mask = ~0;

            foreach (var layer in layers)
                mask = mask & (~(1 << layer));

            return mask;
        }

        public static bool ContainsLayer(int mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static bool ContainsLayer(this LayerMask mask, int layer)
        {
            return ContainsLayer(mask.value, layer);
        }
    }
}
