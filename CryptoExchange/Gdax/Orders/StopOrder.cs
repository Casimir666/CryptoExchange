using Newtonsoft.Json;

namespace CryptoExchange.Gdax.Orders
{
    public class StopOrder : MarketOrder
    {
        /// <summary>
        /// Desired price at which the stop order triggers
        /// </summary>
        [JsonProperty("price")]
        public decimal Price { get; set; }

        public override OrderType Type => OrderType.Stop;
    }
}
