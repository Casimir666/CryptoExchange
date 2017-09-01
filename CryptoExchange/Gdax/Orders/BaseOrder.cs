using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax.Orders
{
    // TODO : margin parameters ??

    public abstract class BaseOrder
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public abstract OrderType Type { get; }

        [JsonProperty("side")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SideType Side { get; set; }

        [JsonProperty("product_id")]
        public string ProductType { get; set; }

        [JsonProperty("client_oid", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ClientGuid { get; set; }

        [JsonProperty("stp", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public StpFlag? SelfTradePreventionFlag { get; set; }
    }
}
