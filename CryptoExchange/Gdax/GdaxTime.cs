using System;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax
{
    class GdaxTime
    {
        [JsonProperty("iso")]
        public DateTime Time { get; set; }

        [JsonProperty("epoch")]
        public decimal EpochTime { get; set; }
    }
}
