using CryptoExchange.Common;
using Newtonsoft.Json;

namespace CryptoExchange.Coinbase
{
    public class OrderRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("payment_method")]
        public FiatAccount PaymentMethod { get; set; }

        [JsonProperty("transaction")]
        public Transaction Transaction { get; set; }

        [JsonProperty("amount")]
        public Balance Amount { get; set; }

        [JsonProperty("total")]
        public Balance Total { get; set; }

        [JsonProperty("subtotal")]
        public Balance Subtotal { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("resource_path")]
        public string ResourcePath { get; set; }

        [JsonProperty("committed")]
        public bool Committed { get; set; }

        [JsonProperty("instant")]
        public bool Instant { get; set; }

        [JsonProperty("fee")]
        public Balance Fee { get; set; }

        [JsonProperty("payout_at")]
        public string PayoutAt { get; set; }
    }
}
