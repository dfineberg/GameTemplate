using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StringLibrary))]
public class StringLibraryEditor : Editor
{
    private string _folder;
    private SerializedProperty _stringArray;

    private void OnEnable()
    {
        _folder = AssetDatabase.GetAssetPath(target) // asset path
            .Split(new[]{'/'}, 2)[1] // remove the 'Assets/' from the start
            .Replace(target.name, "") // remove the asset's name
            .Split('.')[0] // clean up the file extension
            .TrimEnd('/'); // remove the last trailing slash
        
        _stringArray = serializedObject.FindProperty("Strings");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Auto-fill Options", EditorStyles.boldLabel);
        _folder = EditorGUILayout.TextField("Folder", _folder);

        if (GUILayout.Button("Auto-fill from folder"))
        {
            if (!AssetDatabase.IsValidFolder($"Assets/{_folder}"))
            {
                Debug.LogError($"Invalid folder: {_folder}");
                return;
            }

            // full file path to the folder
            var fullPath = $"{Application.dataPath}/{_folder}";

            var files = Directory.GetFiles(fullPath) // get the file names in the folder
                .Where(f =>
                    !f.Contains(".meta") && // remove the metafiles
                    !f.Contains(target.name)) // remove this asset file if its in there
                .Select(f =>
                    f.Replace($"{fullPath}\\", "") // take just the file name
                    .Split('.')[0]) // remove the file extension
                .OrderBy(f => f) // order alphabetically
                .ToArray();

            // populate the string array
            _stringArray.ClearArray();
            _stringArray.arraySize = files.Length;

            for (var i = 0; i < files.Length; i++)
                _stringArray.GetArrayElementAtIndex(i).stringValue = files[i];

            serializedObject.ApplyModifiedProperties();
        }
    }
}
