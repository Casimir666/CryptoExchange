using System;
using CryptoExchange.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax
{
    public class Product
    {
        [JsonProperty("id")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType Type { get; set; }

        [JsonProperty("base_currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency BaseCurrency { get; set; }

        [JsonProperty("quote_currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency QuoteCurrency { get; set; }

        [JsonProperty("base_min_size")]
        public decimal BaseMinSize { get; set; }

        [JsonProperty("base_max_size")]
        public decimal BaseMaxSize { get; set; }

        [JsonProperty("quote_increment")]
        public decimal QuoteIncrement { get; set; }

        public override string ToString()
        {
            return $"{Type}";
        }

        /// <summary>
        /// Buy or sell order should be a multiple of QuoteIncrement
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>Size in bitcoin</returns>
        public decimal GetQuotedCurrency(decimal amount)
        {
            switch ((int)(QuoteIncrement*100000))
            {
                case 1000:
                    return Math.Round(amount, 2);
                case 100:
                    return Math.Round(amount, 3);
                case 10:
                    return Math.Round(amount, 4);
                case 1:
                    return Math.Round(amount, 5);
                default:
                    throw new ArgumentException(nameof(amount));
            }
        }

        public decimal GetQuotedSize(decimal size)
        {
            // For size round to Satoshi units (0.00000001 BTC)
            return Math.Round(size - 0.000000005m, 8);
        }

    }
}
