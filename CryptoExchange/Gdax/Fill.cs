using System;
using CryptoExchange.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax
{
    public class Fill
    {
        [JsonProperty("trade_id")]
        public int TradeId { get; set; }

        [JsonProperty("product_id")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType ProductType { get; set; }

        [JsonProperty("order_id")]
        public Guid OrderId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("profile_id")]
        public Guid ProfileId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("liquidity")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LiquidityType Liquidity { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("size")]
        public decimal Size { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("side")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SideType Side { get; set; }

        [JsonProperty("settled")]
        public bool Settled { get; set; }
    }
}
