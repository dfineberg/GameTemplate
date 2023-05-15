using System.Linq;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), "rig-gizmos", "Rig Gizmos")]
public class RigGizmosOverlay : Overlay
{
    private readonly Toggle[] _toggles = new Toggle[10];
    private Button _allButton;
    private VisualElement _root;

    private CustomRig SelectedRig => Selection.activeGameObject
            ? Selection.activeGameObject.GetComponent<CustomRig>()
            : null;

    public override VisualElement CreatePanelContent()
    {
        _root = new VisualElement { name = "Rig Gizmos" };

        for (var i = 0; i < _toggles.Length; i++)
        {
            var t = new Toggle
            {
                style = { display = DisplayStyle.None }
            };
            
            var toggleIndex = i;
            t.RegisterValueChangedCallback(b => ToggleValueChanged(toggleIndex, b.newValue));
            _root.Add(t);
            _toggles[i] = t;
        }
        
        _allButton = new Button(ButtonClicked)
        {
            text = "Toggle All"
        };
        _root.Add(_allButton);
        
        Selection.selectionChanged += UpdateToggles;
        Undo.undoRedoPerformed += UpdateToggles;
        return _root;
    }

    private void ButtonClicked()
    {
        var rig = SelectedRig;

        if (!rig)
            return;

        var any = rig.BlendShapeHandles.Any(h => h.DrawHandle) || rig.BoneHandles.Any(h => h.DrawHandle);
        Undo.RecordObject(rig, "Updated rig gizmo visibility");

        foreach (var handle in rig.BlendShapeHandles) 
            handle.DrawHandle = !any;

        foreach (var handle in rig.BoneHandles) 
            handle.DrawHandle = !any;
        
        UpdateToggles();
    }

    private void UpdateToggles()
    {
        var rig = SelectedRig;

        if (rig)
        {
            _allButton.style.display = DisplayStyle.Flex;

            for (var i = 0; i < _toggles.Length; i++)
            {
                var toggle = _toggles[i];

                if (i < rig.BlendShapeHandles.Length)
                {
                    var handle = rig.BlendShapeHandles[i];

                    toggle.style.display = DisplayStyle.Flex;
                    toggle.label = handle.HandleTransform.gameObject.name;
                    toggle.value = handle.DrawHandle;
                }
                else if (i < rig.BlendShapeHandles.Length + rig.BoneHandles.Length)
                {
                    var handle = rig.BoneHandles[i - rig.BlendShapeHandles.Length];

                    toggle.style.display = DisplayStyle.Flex;
                    toggle.label = handle.BoneTransform.gameObject.name;
                    toggle.value = handle.DrawHandle;
                }
                else
                {
                    toggle.style.display = DisplayStyle.None;
                }
            }
        }
        else
        {
            _allButton.style.display = DisplayStyle.None;

            foreach (var toggle in _toggles)
                toggle.style.display = DisplayStyle.None;
        }
    }

    private void ToggleValueChanged(int toggleIndex, bool newValue)
    {
        var rig = SelectedRig;
        
        if (rig == null)
            return;

        Undo.RecordObject(rig, "Updated rig gizmo visibility");

        if (toggleIndex < rig.BlendShapeHandles.Length)
        {
            rig.BlendShapeHandles[toggleIndex].DrawHandle = newValue;
        }
        else if (toggleIndex < rig.BlendShapeHandles.Length + rig.BoneHandles.Length)
        {
            rig.BoneHandles[toggleIndex - rig.BlendShapeHandles.Length].DrawHandle = newValue;
        }
    }
}
