using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomRig : MonoBehaviour
{
    [Serializable]
    public struct AxisInfo
    {
        public string Label;
        [HorizontalGroup, ListDrawerSettings(Expanded = true)] public string[] NegativeAxis;
        [HorizontalGroup, ListDrawerSettings(Expanded = true)] public string[] PositiveAxis;

        public float GetValue(SkinnedMeshRenderer renderer)
        {
            if (renderer == null || renderer.sharedMesh == null)
                return 0f;
            
            var posIndex = PositiveAxis != null && PositiveAxis.Length > 0 ? renderer.sharedMesh.GetBlendShapeIndex(PositiveAxis[0]) : -1;
            var negIndex = NegativeAxis != null && NegativeAxis.Length > 0 ? renderer.sharedMesh.GetBlendShapeIndex(NegativeAxis[0]) : -1;
            
            var posValue = posIndex > -1 ? renderer.GetBlendShapeWeight(posIndex) : 0f;
            var negValue = negIndex > -1 ? renderer.GetBlendShapeWeight(negIndex) : 0f;
            return posValue - negValue;
        }

        public void SetValue(SkinnedMeshRenderer renderer, float newValue)
        {
            var posWeight = Mathf.Max(newValue, 0f);
            var negWeight = Mathf.Abs(Math.Min(newValue, 0f));

            if (PositiveAxis != null)
            {
                for (var i = 0; i < PositiveAxis.Length; i++)
                {
                    var shapeIndex = renderer.sharedMesh.GetBlendShapeIndex(PositiveAxis[i]);
                    if (shapeIndex != -1) renderer.SetBlendShapeWeight(shapeIndex, posWeight);
                }
            }

            if (NegativeAxis != null)
            {
                for (var i = 0; i < NegativeAxis.Length; i++)
                {
                    var shapeIndex = renderer.sharedMesh.GetBlendShapeIndex(NegativeAxis[i]);
                    if (shapeIndex != -1) renderer.SetBlendShapeWeight(shapeIndex, negWeight);
                }
            }
        }
    }
    
    [Serializable]
    public class HandleInfo
    {
        public Transform HandleTransform;
        public float Scale = 0.1f;
        public float MaxBlendShapeValue = 100f;
        public bool DrawHandle = true;
        public AxisInfo X;
        public AxisInfo Y;
        public AxisInfo Z;

        public Vector3 GetValue(SkinnedMeshRenderer renderer)
        {
            return new Vector3(X.GetValue(renderer), Y.GetValue(renderer), Z.GetValue(renderer));
        }

        public void SetValue(SkinnedMeshRenderer renderer, Vector3 newValue)
        {
            X.SetValue(renderer, Mathf.Clamp(newValue.x, -MaxBlendShapeValue, MaxBlendShapeValue));
            Y.SetValue(renderer, Mathf.Clamp(newValue.y, -MaxBlendShapeValue, MaxBlendShapeValue));
            Z.SetValue(renderer, Mathf.Clamp(newValue.z, -MaxBlendShapeValue, MaxBlendShapeValue));
        }
    }

    [Serializable]
    public class RotationHandle
    {
        public Vector3 NeutralPosition;
        public Transform BoneTransform;
        public bool DrawHandle = true;
    }

    [FormerlySerializedAs("PositionHandles")] [FormerlySerializedAs("Handles")] public HandleInfo[] BlendShapeHandles;
    [FormerlySerializedAs("RotationHandles")] public RotationHandle[] BoneHandles;
    [InlineEditor] public SkinnedMeshRenderer Renderer;
}
