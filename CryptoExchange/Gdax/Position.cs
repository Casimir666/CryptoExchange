using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax
{
    public class AccountDetail
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("hold")]
        public decimal Hold { get; set; }

        [JsonProperty("funded_amount")]
        public decimal FundedAmount { get; set; }

        [JsonProperty("default_amount")]
        public decimal DefaultAmount { get; set; }
    }

    public class Position
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProfileStatus Status { get; set; }

        [JsonProperty("accounts")]
        public Dictionary<Common.Currency, AccountDetail> Accounts { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("profile_id")]
        public Guid ProfileId { get; set; }
    }
}
