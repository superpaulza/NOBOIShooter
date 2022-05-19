using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace withLuckAndWisdomProject
{
    public class FileManager
    {
        //set file path
        private static string saveToDir = Environment.CurrentDirectory;

        public static object ReadFromObj(string path)
        {
            object obj;
            string pathToPref = Path.Combine(saveToDir, path);
            if (File.Exists(pathToPref))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(pathToPref, FileMode.Open, FileAccess.Read, FileShare.Read);
                obj = formatter.Deserialize(stream);
                stream.Close();
            }
            else
            {
                return null;
            }

            return obj;
        }

        public static void WriteToObj(string path, object obj)
        {
            string pathToPref = Path.Combine(saveToDir, path);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(pathToPref, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, obj);
            stream.Close();
        }
    }
}
