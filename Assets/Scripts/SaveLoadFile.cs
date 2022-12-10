using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveLoadFile
{
    public const string CONFIGFILE = "/config.cfg";
    public const string SAVEFILE = "/save.sav";

    public static void Save(System.Object savebileObject, string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + fileName, FileMode.Create);

        bf.Serialize(stream, savebileObject);
        stream.Close();
    }

    public static System.Object Load(string fileName)
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + fileName, FileMode.Open);

            System.Object data = bf.Deserialize(stream);

            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
