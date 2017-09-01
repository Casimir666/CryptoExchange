using CryptoExchange.Common;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax
{
    public class SepaDepositInformation
    {
        [JsonProperty("iban")]
        public string Iban { get; set; }

        [JsonProperty("swift")]
        public string Swift { get; set; }

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
