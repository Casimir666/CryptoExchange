using System;
using CryptoExchange.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax
{
    class PaymentRequest
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency Currency { get; set; }

        [JsonProperty("payment_method_id")]
        public Guid? PaymentMethodId { get; set; }

        [JsonProperty("coinbase_account_id")]
        public Guid? CoinbaseId { get; set; }

        [JsonProperty("crypto_address")]
        public string CryptoAddress { get; set; }

        [JsonProperty("two_factor_code")]
        public string TwoFactorCode { get; set; }
    }

}
