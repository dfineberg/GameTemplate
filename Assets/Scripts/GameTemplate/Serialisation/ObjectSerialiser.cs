using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ObjectSerialiser {

    public static void SaveObjectAt(Object serializeObject, string filePath)
    {
        var binaryFormatter = new BinaryFormatter();
        var fileStream = File.OpenWrite(filePath);

        binaryFormatter.Serialize(fileStream, serializeObject);
    }

    public static T LoadObjectAt<T>(string filePath)
    {
        var binaryFormatter = new BinaryFormatter();
        var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);

        return (T) binaryFormatter.Deserialize(fileStream);
    }
}
