using CryptoExchange.Common;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax
{
    public class WireDepositInformation
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("routing_number")]
        public string RoutingNumber { get; set; }

        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        [JsonProperty("bank_address")]
        public string BankAddress { get; set; }

        [JsonProperty("bank_country")]
        public Country BankCountry { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("account_address")]
        public string AccountAddress { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}
