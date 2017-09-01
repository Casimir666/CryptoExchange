using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Common
{
    public class Balance
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency Currency { get; set; }

        public override string ToString()
        {
            return $"{Currency}: {Amount}";
        }

        public Balance()
        {            
        }

        public Balance(Currency currency, decimal amount)
        {
            Currency = currency;
            Amount = amount;
        }
    }
}
