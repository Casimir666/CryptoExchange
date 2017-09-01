using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax.Orders
{
    public class OrderResponse
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType Type { get; set; }

        [JsonProperty("side")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SideType Side { get; set; }

        [JsonProperty("product_id")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType ProductType { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("size")]
        public decimal Size { get; set; }

        [JsonProperty("stp")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StpFlag SelfTradePreventionFlag { get; set; }

        [JsonProperty("time_in_force")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TimeInForceType TimeInForce { get; set; }

        [JsonProperty("post_only")]
        public bool PostOnly { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("fill_fees")]
        public decimal FillFees { get; set; }

        [JsonProperty("filled_size")]
        public decimal FilledSize { get; set; }

        [JsonProperty("executed_value")]
        public decimal ExecutedValue { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStatus Status { get; set; }

        [JsonProperty("settled")]
        public bool Settled { get; set; }

        public override string ToString()
        {
            return $"{Side} {Size} {ProductType} at {Price}";
        }
    }
}
