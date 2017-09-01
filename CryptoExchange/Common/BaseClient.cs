using System;
using System.Collections.Generic;
using CryptoExchange.Coinbase;
using CryptoExchange.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace CryptoExchange.Common
{
    abstract class BaseClient
    {
        protected string ApiKey;
        protected string ApiSecret;
        protected string ApiUrl;
        protected string ApiPassphrase;
        protected string ApiCheckoutUrl;
        protected bool UseTimeApi;

        public JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        protected BaseClient(string apiKey, string apiSecret, string apiPassphrase, string apiUrl, string checkoutUrl, bool useTimeApi)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
            ApiPassphrase = apiPassphrase;
            if (string.IsNullOrWhiteSpace(ApiKey) || string.IsNullOrWhiteSpace(ApiSecret))
            {
                throw new ArgumentException("The API key / secret must not be empty. A valid API key and API secret should be used in the CryptoExchange constructor or an appSettings configuration element with <add key='CoinbaseApiKey' value='my_api_key' /> and <add key='CoinbaseApiSecret' value='my_api_secret' /> should exist.", nameof(apiKey));
            }

            ApiUrl = !string.IsNullOrWhiteSpace(apiUrl) ? apiUrl : Constants.LiveApiUrl;
            ApiCheckoutUrl = !string.IsNullOrWhiteSpace(checkoutUrl) ? checkoutUrl : Constants.LiveCheckoutUrl;

            UseTimeApi = useTimeApi;
        }

        protected abstract IAuthenticator GetAuthenticator();

        protected virtual RestClient CreateClient()
        {
            var client = new RestClient(ApiUrl)
            {
                Authenticator = GetAuthenticator()
            };

            client.ContentHandlers["application/json"] = new JsonNetDeseralizer(JsonSettings);
            return client;
        }

        protected virtual IRestRequest CreateRequest(string action, Method method, params KeyValuePair<string, string>[] queryParams)
        {
            var request = new RestRequest(action, method)
            {                 
                Serializer = new JsonNetSerializer(JsonSettings)
            };
            request.AddOrUpdateHeader("Accept", "application/json, application/xml, text/json, text/x-json, text/javascript, text/xml, application/json");

            if (queryParams != null)
            {

                foreach (var kvp in queryParams)
                {
                    request.AddQueryParameter(kvp.Key, kvp.Value);
                }
            }

            return request;
        }

        /// <summary>
        /// Get the final checkout redirect URL from a CoinbaseResponse. The response
        /// from CreateCheckout() call should be used.
        /// </summary>
        /// <param name="checkoutResponse">The response from calling CreateCheckout()</param>
        /// <returns>The redirect URL for the customer checking out</returns>
        public virtual string GetCheckoutUrl(CoinbaseResponse checkoutResponse)
        {
            var id = checkoutResponse.Data["id"]?.ToString();
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("The checkout response must have an ID field. None was found.", nameof(checkoutResponse));

            return ApiCheckoutUrl.Replace("{code}", id);
        }

        /// <summary>
        /// Gets a notification object from JSON.
        /// </summary>
        /// <param name="json">Received from Coinbase in the HTTP callback</param>
        /// <returns></returns>
        public virtual Notification GetNotification(string json)
        {
            return JsonConvert.DeserializeObject<Notification>(json, JsonSettings);
        }
    }
}
