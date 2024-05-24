using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CustomRig : MonoBehaviour
{
    #region Animation Handles

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

    #endregion

    [FormerlySerializedAs("PositionHandles")] [FormerlySerializedAs("Handles")] public HandleInfo[] BlendShapeHandles;
    [FormerlySerializedAs("RotationHandles")] public RotationHandle[] BoneHandles;
    [InlineEditor] public SkinnedMeshRenderer Renderer;

    public AnimationClip[] ConsonantClips;
    public AnimationClip[] VowelClips;
    public bool Talk;
    public float MouthShapeTime = 0.1f;
    
    private PlayableGraph _talkingGraph;
    private AnimationMixerPlayable _talkingMixerPlayable;

    private int MouthClipCount => ConsonantClips.Length + VowelClips.Length;

    private void Start()
    {
        InitTalkingGraph();
    }

    private void Update()
    {
        UpdateTalkingGraph();
    }

    private void InitTalkingGraph()
    {
        if (ConsonantClips.Length == 0 || VowelClips.Length == 0)
            return;

        var animator = GetComponent<Animator>();
        _talkingMixerPlayable = AnimationPlayableUtilities.PlayMixer(animator, MouthClipCount, out _talkingGraph);

        for (var i = 0; i < ConsonantClips.Length; i++)
        {
            var clipPlayable = AnimationClipPlayable.Create(_talkingGraph, ConsonantClips[i]);
            _talkingGraph.Connect(clipPlayable, 0, _talkingMixerPlayable, i);
        }

        for (var i = 0; i < VowelClips.Length; i++)
        {
            var clipPlayable = AnimationClipPlayable.Create(_talkingGraph, VowelClips[i]);
            _talkingGraph.Connect(clipPlayable, 0, _talkingMixerPlayable, ConsonantClips.Length + i);
        }
    }


    private void UpdateTalkingGraph()
    {
        if (!_talkingGraph.IsValid())
            return;
        
        if (!Talk)
        {
            if (_talkingGraph.IsPlaying()) 
                _talkingGraph.Stop();
            
            return;
        }

        if (!_talkingGraph.IsPlaying()) 
            _talkingGraph.Play();

        var shapeIndex = (int) (Time.time / MouthShapeTime);
        var isVowel = shapeIndex % 2 == 1;
        Random.InitState(shapeIndex);
        shapeIndex = Random.Range(0, isVowel ? VowelClips.Length : ConsonantClips.Length);
        if (isVowel) shapeIndex += ConsonantClips.Length;

        for (var i = 0; i < MouthClipCount; i++)
            _talkingMixerPlayable.SetInputWeight(i, i == shapeIndex ? 1f : 0f);
    }
}
