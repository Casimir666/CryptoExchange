using System;
using System.Collections.Generic;
using CryptoExchange.Common;
using CryptoExchange.Gdax;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Coinbase
{
    public class Requirement
    {

        [JsonProperty("type")]
        public string Type { get; set; }
    }


    public class LimitDetail
    {

        [JsonProperty("period_in_days")]
        public int PeriodInDays { get; set; }

        [JsonProperty("total")]
        public Balance Total { get; set; }

        [JsonProperty("remaining")]
        public Balance Remaining { get; set; }

        [JsonProperty("next_requirement")]
        public Requirement NextRequirement { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public class Limits
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("buy")]
        public List<LimitDetail> Buy { get; set; }

        [JsonProperty("sell")]
        public List<LimitDetail> Sell { get; set; }
    }

    public class PaymentMethod
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentType Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency Currency { get; set; }

        [JsonProperty("primary_buy")]
        public bool PrimaryBuy { get; set; }

        [JsonProperty("primary_sell")]
        public bool PrimarySell { get; set; }

        [JsonProperty("allow_buy")]
        public bool AllowBuy { get; set; }

        [JsonProperty("allow_sell")]
        public bool AllowSell { get; set; }

        [JsonProperty("allow_deposit")]
        public bool AllowDeposit { get; set; }

        [JsonProperty("allow_withdraw")]
        public bool AllowWithdraw { get; set; }

        [JsonProperty("instant_buy")]
        public bool InstantBuy { get; set; }

        [JsonProperty("instant_sell")]
        public bool InstantSell { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("resource_path")]
        public string ResourcePath { get; set; }

        [JsonProperty("limits")]
        public Limits Limits { get; set; }

        [JsonProperty("fiat_account")]
        public FiatAccount FiatAccount { get; set; }
    }
}
