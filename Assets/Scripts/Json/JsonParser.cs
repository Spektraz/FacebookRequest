using Newtonsoft.Json;
using RequestApiLibrary.Services;
using UnityEngine;

namespace StarSoccerSlim.Json
{
    public class JsonParser : IJsonService
    {
        public string Serialize<T>(T jsonData)
        {
            // return JsonConvert.SerializeObject(jsonData);
            Debug.Log($"jsonData: {jsonData}");
           
            return JsonConvert.SerializeObject(jsonData, 
                Newtonsoft.Json.Formatting.None, 
                new JsonSerializerSettings { 
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        public T Deserialize<T>(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}