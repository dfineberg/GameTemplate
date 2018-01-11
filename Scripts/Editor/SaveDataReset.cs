using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameTemplate
{
    public class SaveDataReset {

        [MenuItem("Tools/Reset Save Files")]
        private static void ResetSaveFile()
        {
            foreach (var file in Directory.GetFiles(Application.persistentDataPath))
                File.Delete(file);

            foreach(var directory in Directory.GetDirectories(Application.persistentDataPath))
                Directory.Delete(directory, true);
        }
    }
}
