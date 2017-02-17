using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public struct SaveFile
{
    public bool TestBool;
    public int TestInt;
    public string TestString;
}

public class SaveManager : MonoBehaviour
{
    public SaveFile SaveFile;

    public bool UseDebugSaveFile;

    public int LoadedSaveFileNo { get; private set; }

    private string _directoryPath;

    private void Awake()
    {
        LoadedSaveFileNo = -1;

        _directoryPath = Application.persistentDataPath + "/saveFiles";
    }

    public void LoadSaveFile(int fileNo = 0)
    {
        LoadedSaveFileNo = fileNo;
        var filePath = _directoryPath + "/saveFile" + fileNo;

        if (UseDebugSaveFile)
            return;

        if (!File.Exists(filePath))
        {
            SaveFile = new SaveFile();
            return;
        }

        var binaryFormatter = new BinaryFormatter();
        var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);

        SaveFile = (SaveFile) binaryFormatter.Deserialize(fileStream);
    }

    public void Save(int fileNo = 0)
    {
        var filePath = _directoryPath + "/saveFile" + fileNo;

        if (!Directory.Exists(_directoryPath))
            Directory.CreateDirectory(_directoryPath);

        var binaryFormatter = new BinaryFormatter();
        var fileStream = File.OpenWrite(filePath);

        binaryFormatter.Serialize(fileStream, SaveFile);
    }

    public void DeleteSaveFile(int fileNo = 0)
    {
        var filePath = _directoryPath + "/saveFile" + fileNo;

        if (!File.Exists(filePath))
            return;

        File.Delete(filePath);
    }

    public void DeleteAllSaveFiles()
    {
        if (!Directory.Exists(_directoryPath))
            return;

        foreach (var file in Directory.GetFiles(_directoryPath))
            File.Delete(file);
    }

    public int GetSaveFileCount()
    {
        if (!Directory.Exists(_directoryPath))
            return 0;

        return Directory.GetFiles(_directoryPath).Length;
    }
}