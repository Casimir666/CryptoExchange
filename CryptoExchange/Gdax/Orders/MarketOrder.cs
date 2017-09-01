using Newtonsoft.Json;

namespace CryptoExchange.Gdax.Orders
{
    /// <summary>
    /// One of size or funds is required
    /// </summary>
    public class MarketOrder : BaseOrder
    {
        /// <summary>
        /// Desired amount in BTC
        /// </summary>
        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Size { get; set; }

        /// <summary>
        /// Desired amount of quote currency to use
        /// </summary>
        [JsonProperty("funds", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Funds { get; set; }

        public override OrderType Type => OrderType.Market;
    }
}
