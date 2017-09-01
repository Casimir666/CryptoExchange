using Newtonsoft.Json;

namespace CryptoExchange.Gdax
{
    class ErrorResponse
    {
        [JsonProperty("message")]
        public string Cause { get; set; }
    }
}
