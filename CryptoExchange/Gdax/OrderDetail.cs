using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax
{
    public class OrderDetail
    {

        [JsonProperty("order_id")]
        public Guid OrderId { get; set; }

        [JsonProperty("trade_id")]
        public string TradeId { get; set; }

        [JsonProperty("product_id")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType ProductType { get; set; }

        [JsonProperty("transfer_id")]
        public string TransferId { get; set; }

        [JsonProperty("transfer_type")]
        public string TransferType { get; set; }
    }
}