using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax
{
    public class Trade
    {
        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("trade_id")]
        public int TradeId { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("size")]
        public decimal Size { get; set; }

        [JsonProperty("side")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SideType Side { get; set; }

        public override string ToString()
        {
            return $"{Time} : Side={Side}, Price={Price}, Size={Size}";
        }
    }
}
