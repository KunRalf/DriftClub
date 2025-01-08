using System.IO;
using Infrastructure.SaveLoad;
using Newtonsoft.Json;
using UnityEngine;

    public class JsonManager : ISaveLoad
    {
        public T Load<T>(string name)
        {
            if(File.Exists(Application.persistentDataPath + "/" + name + ".json"))
            {
                return JsonConvert.DeserializeObject<T>(
                    File.ReadAllText(Application.persistentDataPath + "/" + name + ".json"));
            }
            
            return default;
        }

        public void Save<T>(string name, T obj)
        {
            File.WriteAllText(Application.persistentDataPath + "/" + name + ".json", JsonConvert.SerializeObject(obj));
        }
    }
