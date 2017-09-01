using System;
using CryptoExchange.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax
{
    public class Funding
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("order_id")]
        public Guid OrderId { get; set; }

        [JsonProperty("")]
        public Guid ProfileId { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FundingStatus Status { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency currency { get; set; }

        [JsonProperty("repaid_amount")]
        public decimal RepaidAmount { get; set; }

        [JsonProperty("default_amount")]
        public decimal DefaultAmount { get; set; }

        [JsonProperty("repaid_default")]
        public bool RepaidDefault { get; set; }
    }
}
