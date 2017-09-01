using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax
{
    public class TrailingVolume
    {
        [JsonProperty("product_id")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType ProductType { get; set; }

        [JsonProperty("exchange_volume")]
        public decimal ExchangeVolume { get; set; }

        [JsonProperty("volume")]
        public decimal Volume { get; set; }

        [JsonProperty("recorded_at")]
        public DateTime RecordedAt { get; set; }
    }
}
