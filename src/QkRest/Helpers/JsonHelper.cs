using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace QkRest.Helpers
{
    /// <summary>
    /// JSON helper methods.
    /// </summary>
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// Serialize object to JSON.
        /// </summary>
        public static string SerializeObject<TObject>(TObject obj)
        {
            return JsonConvert.SerializeObject(obj, serializerSettings);
        }

        /// <summary>
        /// Deserialize JSON string to object.
        /// </summary>
        public static TObject DeserializeObject<TObject>(string jsonString)
        {
            return JsonConvert.DeserializeObject<TObject>(jsonString, serializerSettings);
        }
    }
}
