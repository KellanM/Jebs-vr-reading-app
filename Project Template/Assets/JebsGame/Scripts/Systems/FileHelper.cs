using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JesbReadingGame.Helpers
{
    public static class FileHelpers
    {
        public static void WriteJson<T>(string fileName, T item)
        {
            string path = Application.persistentDataPath + "/" + fileName;

            string str = JsonUtility.ToJson(item);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(str);
                }
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif

        }

        public static T ReadJson<T>(string fileName)
        {
            string path = Application.persistentDataPath + "/" + fileName;

            string str = "";

            StreamReader reader = new StreamReader(path);
            str = reader.ReadToEnd();
            reader.Close();

            T item = JsonUtility.FromJson<T>(str);
            return item;
        }
    }
}
