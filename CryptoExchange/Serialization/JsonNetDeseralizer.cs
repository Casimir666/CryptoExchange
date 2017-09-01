using Newtonsoft.Json;
using RestSharp.Portable;

namespace CryptoExchange.Serialization
{
    public class JsonNetDeseralizer : IDeserializer
    {
        private readonly JsonSerializerSettings settings;

        public JsonNetDeseralizer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public T Deserialize<T>( IRestResponse response )
        {
            return response.IsSuccess ? JsonConvert.DeserializeObject<T>( response.Content, settings ) : default(T);
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
    }
}