using System.IO;
using Newtonsoft.Json;
using UnityEngine;

    public class JsonManager<T>
    {
        public void SaveJson(string name, T obj)
        {
             File.WriteAllText(Application.persistentDataPath + "/" + name + ".json", JsonConvert.SerializeObject(obj));
        }

        public T LoadJson(string name) 
        {
            if(File.Exists(Application.persistentDataPath + "/" + name + ".json"))
            {
                return JsonConvert.DeserializeObject<T>(
                    File.ReadAllText(Application.persistentDataPath + "/" + name + ".json"));
            }
           
            return default;
        }
   }
