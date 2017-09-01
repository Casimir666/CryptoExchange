﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CryptoExchange.Common;

namespace CryptoExchange.Gdax
{
    public class Account
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency Currency { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("available")]
        public decimal Available { get; set; }

        [JsonProperty("hold")]
        public decimal Hold { get; set; }

        [JsonProperty("profile_id")]
        public Guid ProfileId { get; set; }

        [JsonProperty("margin_enabled")]
        public bool? MarginEnabled { get; set; }

        [JsonProperty("funded_amount")]
        public decimal? FundedAmount { get; set; }

        [JsonProperty("default_amount")]
        public decimal? DefaultAmount { get; set; }

        public override string ToString()
        {
            return $"{Currency} : balance={Balance:F2}, available={Available:F2}, hold={Hold:F2}";
        }
    }
}
