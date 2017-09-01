using System;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax
{
    public class ProductTicker
    {
        [JsonProperty("trade_id")]
        public int TradeId { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("size")]
        public decimal Size { get; set; }

        [JsonProperty("bid")]
        public decimal Bid { get; set; }

        [JsonProperty("ask")]
        public decimal Ask { get; set; }

        [JsonProperty("volume")]
        public decimal Volume { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        public override string ToString()
        {
            return $"{Time} : price={Price}, volume={Volume}, size={Size}, bid={Bid} -> ask={Ask}  {(Bid > Ask ? "Down" : "Up")}";
        }
    }
}
