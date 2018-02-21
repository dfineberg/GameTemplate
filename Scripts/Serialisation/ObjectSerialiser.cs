using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameTemplate
{
    public static class ObjectSerialiser {

        public static void SaveObjectAt(object serializeObject, string filePath)
        {
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.OpenWrite(filePath);

            binaryFormatter.Serialize(fileStream, serializeObject);

            fileStream.Close();
        }

        public static T LoadObjectAt<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return default(T);
            
            var binaryFormatter = new BinaryFormatter();
            var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);

            var obj = (T)binaryFormatter.Deserialize(fileStream);

            fileStream.Close();

            return obj;
        }
    }
}
