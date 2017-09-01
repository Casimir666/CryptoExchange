using CryptoExchange.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax
{
    public class CurrencyDetail
    {
        [JsonProperty("id")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("min_size")]
        public decimal MinSize { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Name}";
        }
    }
}
