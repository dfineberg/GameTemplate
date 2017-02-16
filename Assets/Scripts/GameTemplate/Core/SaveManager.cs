using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public struct SaveFile
{
    public bool testBool;
    public int testInt;
    public string testString;
}

public class SaveManager : MonoBehaviour {

    public SaveFile saveFile;

    public bool useDebugSaveFile;

    public int loadedSaveFileNo { get; private set; }

    string directoryPath;

    void Awake()
    {
        loadedSaveFileNo = -1;

        directoryPath = directoryPath = Application.persistentDataPath + "/saveFiles";
    }

    public void LoadSaveFile(int fileNo = 0)
    {
        loadedSaveFileNo = fileNo;
        string filePath = directoryPath + "/saveFile" + fileNo;

        if (useDebugSaveFile)
            return;

        if(!File.Exists(filePath))
        {
            saveFile = new SaveFile();
            return;
        }

        var binaryFormatter = new BinaryFormatter();
        var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);

        saveFile = (SaveFile)binaryFormatter.Deserialize(fileStream);
    }

    public void Save(int fileNo = 0)
    {
        string filePath = directoryPath + "/saveFile" + fileNo;

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var binaryFormatter = new BinaryFormatter();
        var fileStream = File.OpenWrite(filePath);

        binaryFormatter.Serialize(fileStream, saveFile);
    }

    public void DeleteSaveFile(int fileNo = 0)
    {
        string filePath = directoryPath + "/saveFile" + fileNo;

        if (!File.Exists(filePath))
            return;

        File.Delete(filePath);
    }

    public void DeleteAllSaveFiles()
    {
        if (!Directory.Exists(directoryPath))
            return;

        foreach (string file in Directory.GetFiles(directoryPath))
            File.Delete(file);
    }

    public int GetSaveFileCount()
    {
        if (!Directory.Exists(directoryPath))
            return 0;

        return Directory.GetFiles(directoryPath).Length;
    }
}
