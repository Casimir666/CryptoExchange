using Newtonsoft.Json;

namespace CryptoExchange.Coinbase
{
    public class FiatAccount
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("resource_path")]
        public string ResourcePath { get; set; }
    }
}
