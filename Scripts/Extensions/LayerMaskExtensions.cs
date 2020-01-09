using System.Linq;
using UnityEngine;

namespace GameTemplate
{
    public static class LayerMaskExtensions {

        public static int BuildMask(params int[] layers)
        {
            return layers.Aggregate(0, (current, layer) => current | (1 << layer));
        }

        public static int BuildMask(params string[] layerNames)
        {
            return BuildMask(layerNames.Select(LayerMask.NameToLayer).ToArray());
        }

        public static int BuildInverseMask(params int[] layers)
        {
            return layers.Aggregate(~0, (current, layer) => current & ~(1 << layer));
        }

        public static int BuildInverseMask(params string[] layerNames)
        {
            return BuildInverseMask(layerNames.Select(LayerMask.NameToLayer).ToArray());
        }

        public static bool ContainsLayer(int mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static bool ContainsLayer(this LayerMask mask, int layer)
        {
            return ContainsLayer(mask.value, layer);
        }

        public static bool ContainsLayer(this LayerMask mask, string layerName)
        {
            return ContainsLayer(mask.value, LayerMask.NameToLayer(layerName));
        }

        public static int GetFirstLayer(this LayerMask mask)
        {
            for (var i = 0; i < 32; i++)
                if (mask.ContainsLayer(i))
                    return i;

            return -1;
        }

        public static string GetFirstLayerName(this LayerMask mask)
        {
            var firstLayer = mask.GetFirstLayer();
            return firstLayer == -1 ? null : LayerMask.LayerToName(firstLayer);
        }
    }
}
