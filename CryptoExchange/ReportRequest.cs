using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoExchange.Gdax;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange
{
    class ReportRequest
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("product_id")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType ProductType { get; set; }

        [JsonProperty("account_id")]
        public Guid? AccountId { get; set; }

        [JsonProperty("format")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReportFormat Format { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
