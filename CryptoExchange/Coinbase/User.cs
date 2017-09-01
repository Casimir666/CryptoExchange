using System;
using CryptoExchange.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Coinbase
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public object Username { get; set; }

        [JsonProperty("profile_location")]
        public object ProfileLocation { get; set; }

        [JsonProperty("profile_bio")]
        public object ProfileBio { get; set; }

        [JsonProperty("profile_url")]
        public object ProfileUrl { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("resource_path")]
        public string ResourcePath { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }

        [JsonProperty("native_currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency NativeCurrency { get; set; }

        [JsonProperty("bitcoin_unit")]
        public string BitcoinUnit { get; set; }

        [JsonProperty("state")]
        public object State { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("sends_disabled")]
        public bool SendsDisabled { get; set; }
    }
}
