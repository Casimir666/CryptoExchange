using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CryptoExchange.Common;

namespace CryptoExchange.Gdax
{
    public class CoinbaseAccount
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency Currency { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AccountType Type { get; set; }

        [JsonProperty("primary")]
        public bool Primary { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("wire_deposit_information")]
        public WireDepositInformation WireDepositInformation { get; set; }

        [JsonProperty("sepa_deposit_information")]
        public SepaDepositInformation SepaDepositInformation { get; set; }

        public override string ToString()
        {
            return $"{Name} : balance {Balance} {Currency}";
        }
    }
}
