using System;
using System.Collections.Generic;
using CryptoExchange.Common;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax
{

    public class LimitDetail
    {
        [JsonProperty("period_in_days")]
        public int PeriodInDays { get; set; }

        [JsonProperty("total")]
        public Balance Total { get; set; }

        [JsonProperty("remaining")]
        public Balance Remaining { get; set; }
    }

    public class Limits
    {

        [JsonProperty("buy")]
        public List<LimitDetail> Buy { get; set; }

        [JsonProperty("instant_buy")]
        public List<LimitDetail> InstantBuy { get; set; }

        [JsonProperty("sell")]
        public List<LimitDetail> Sell { get; set; }

        [JsonProperty("deposit")]
        public List<LimitDetail> Deposit { get; set; }
    }

    public class PaymentMethod
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

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

        [JsonProperty("limits")]
        public Limits Limits { get; set; }
    }
}
