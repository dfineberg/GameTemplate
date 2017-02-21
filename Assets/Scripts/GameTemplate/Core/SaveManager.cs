using System;
using System.IO;
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

        SaveFile = ObjectSerialiser.LoadObjectAt<SaveFile>(filePath);
    }

    public void Save(int fileNo = 0)
    {
        if (!Directory.Exists(_directoryPath))
            Directory.CreateDirectory(_directoryPath);

        var filePath = _directoryPath + "/saveFile" + fileNo;

        ObjectSerialiser.SaveObjectAt(SaveFile, filePath);
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
        return Directory.Exists(_directoryPath) ? Directory.GetFiles(_directoryPath).Length : 0;
    }
}