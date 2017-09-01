using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax
{
    public class Hold
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("account_id")]
        public Guid AccountId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public HoldType Type { get; set; }

        [JsonProperty("ref")]
        public Guid Ref { get; set; }
    }
}
