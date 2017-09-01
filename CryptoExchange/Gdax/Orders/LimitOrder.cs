using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax.Orders
{
    public class LimitOrder : BaseOrder
    {
        /// <summary>
        /// Price per bitcoin
        /// </summary>
        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Amount of BTC to buy or sell
        /// </summary>
        [JsonProperty("size")]
        public decimal Size { get; set; }

        [JsonProperty("time_in_force", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public TimeInForceType? TimeInForce { get; set; }

        [JsonProperty("cancel_after", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public CancelType? CancelAfter { get; set; }

        [JsonProperty("post_only", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PostOnly { get; set; }


        public override OrderType Type => OrderType.Limit;
    }
}
