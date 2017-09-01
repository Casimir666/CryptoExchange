using System.Collections.Generic;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax
{
    class WsAuth
    {
        [JsonProperty("type")]
        public string Type { get; set; }


        [JsonProperty("product_ids")]
        public List<string> ProductTypes { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("passphrase")]
        public string Passphrase { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}
