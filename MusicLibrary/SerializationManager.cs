using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MusicLibrary.Model
{
    public static class SerializationManager
    {
        #region Binary

        public static void BinarySave(object obj, string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, obj);
            }
        }

        public static T BinaryLoad<T>(string filename)
        {
            T obj = default(T);
            using (FileStream file = new FileStream(filename, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = (T)formatter.Deserialize(file);
            }

            return obj;
        }

        #endregion Binary

        #region XML

        public static void XmlSave(object obj, string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create))
            {
                XmlSerializer formatter = new XmlSerializer(obj.GetType());
                formatter.Serialize(file, obj);
            }
        }

        public static T XmlLoad<T>(string filename)
        {
            T obj = default(T);
            using (FileStream file = new FileStream(filename, FileMode.Open))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                obj = (T)formatter.Deserialize(file);
            }

            return obj;
        }

        #endregion XML

        #region Json
        public static void JsonSave(object obj, string filename)
        {
            using (StreamWriter file = new StreamWriter(filename))
            {
                file.Write(JsonConvert.SerializeObject(obj, Formatting.Indented));
            }
        }

        public static T JsonLoad<T>(string filename)
        {
            T obj = default(T);
            using (StreamReader file = new StreamReader(filename))
            {
                obj = JsonConvert.DeserializeObject<T>(file.ReadToEnd());
            }

            return obj;
        }
        #endregion
    }
}