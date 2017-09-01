using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax
{
    public class OrderHistory
    {
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LeggerType Type { get; set; }

        [JsonProperty("details")]
        public OrderDetail Details { get; set; }

        public override string ToString()
        {
            return $"{Type} : Amount={Amount}, Balance={Balance}, Creation date={CreatedAt}";
        }
    }
}
