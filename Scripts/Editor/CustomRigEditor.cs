using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomRig))]
public class CustomRigEditor : OdinEditor
{
    private void OnSceneGUI()
    {
        var faceRig = (CustomRig) target;
        var renderer = faceRig.Renderer;
        
        if (renderer == null)
            return;

        foreach (var handle in faceRig.BlendShapeHandles)
        {
            var transform = handle.HandleTransform;

            if (!handle.DrawHandle || transform == null)
                continue;

            var rotation = transform.rotation;
            var currentHandleValue = handle.GetValue(renderer);
            var newHandleValue = currentHandleValue;

            // draw the drag handle
            var worldHandlePos = transform.TransformPoint(currentHandleValue / (100f / handle.Scale));
            var newWorldHandlePos = Handles.PositionHandle(worldHandlePos, rotation);

            if (newWorldHandlePos != worldHandlePos)
                newHandleValue = transform.InverseTransformPoint(newWorldHandlePos) * (100f / handle.Scale);

            // draw the labels and the reset buttons
            var handleSize = HandleUtility.GetHandleSize(newWorldHandlePos);
            var buttonSize = handleSize * 0.1f;
            var style = GUI.skin.label;
            style.alignment = TextAnchor.MiddleRight;
            style.contentOffset = new Vector2(-15f, 0f);

            // Draw x axis label + button
            var buttonPos = newWorldHandlePos + transform.right * (handleSize * 1.5f);
            Handles.color = Color.red;
            Handles.Label(buttonPos, handle.X.Label, style);

            if (Handles.Button(buttonPos, rotation, buttonSize, buttonSize * 2f, Handles.DotHandleCap))
                newHandleValue.x = 0f;

            // Draw y axis label + button
            buttonPos = newWorldHandlePos + transform.up * (handleSize * 1.5f);
            Handles.color = Color.green;
            Handles.Label(buttonPos, handle.Y.Label, style);

            if (Handles.Button(buttonPos, rotation, buttonSize, buttonSize * 2f, Handles.DotHandleCap))
                newHandleValue.y = 0f;

            // Draw z axis label + button
            buttonPos = newWorldHandlePos + transform.forward * (handleSize * 1.5f);
            Handles.color = Color.blue;
            Handles.Label(buttonPos, handle.Z.Label, style);

            if (Handles.Button(buttonPos, rotation, buttonSize, buttonSize * 2f, Handles.DotHandleCap))
                newHandleValue.z = 0f;

            // record the change into the Undo system
            if (newHandleValue != currentHandleValue)
            {
                Undo.RecordObject(faceRig.Renderer, $"Updated Blend Shapes: {faceRig.Renderer.gameObject.name}");
                handle.SetValue(renderer, newHandleValue);
            }
        }

        foreach (var handle in faceRig.BoneHandles)
        {
            if (!handle.DrawHandle || handle.BoneTransform == null)
                continue;
            
            handle.BoneTransform.GetPositionAndRotation(out var position, out var rotation);
            var newRotation = Handles.RotationHandle(rotation, position);
            
            if (newRotation != rotation)
            {
                Undo.RecordObject(handle.BoneTransform, $"Rotated Bone: {handle.BoneTransform.gameObject.name}");
                handle.BoneTransform.rotation = newRotation;
            }
            
            var buttonSize = HandleUtility.GetHandleSize(position) * 0.1f;
            Handles.color = Color.white;

            if (Handles.Button(position, rotation, buttonSize, buttonSize * 2f, Handles.DotHandleCap))
            {
                Undo.RecordObject(handle.BoneTransform, $"Reset Bone Rotation: {handle.BoneTransform.gameObject.name}");
                handle.BoneTransform.localEulerAngles = handle.NeutralPosition;
            }
        }
    }
}
