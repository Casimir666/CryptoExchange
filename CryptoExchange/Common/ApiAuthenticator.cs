using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS_UWP
    using Windows.Security.Cryptography.Core;
#else
#endif
using CryptoExchange.Coinbase;
using CryptoExchange.Serialization;
using Newtonsoft.Json;
using RestSharp.Portable;

namespace CryptoExchange.Common
{

    class ApiAuthenticator : IAuthenticator
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _passphrase;
        private readonly bool _useTimeApi;
        private readonly bool _useBase64;
        private readonly JsonSerializerSettings _jsonSettings;

        public ApiAuthenticator(string apiKey, string apiSecret, bool useTimeApi, bool useBase64, JsonSerializerSettings jsonSettings)
            : this(apiKey, apiSecret, null, useTimeApi, useBase64, jsonSettings)
        {
            
        }

        public ApiAuthenticator(string apiKey, string apiSecret, string passphrase, bool useTimeApi, bool useBase64, JsonSerializerSettings jsonSettings)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _passphrase = passphrase;
            _useTimeApi = useTimeApi;
            _useBase64 = useBase64;
            _jsonSettings = jsonSettings;
        }


        public async Task Authenticate(IRestClient client, IRestRequest request)
        {
            var uri = client.BuildUri(request);
            var path = uri.AbsolutePath;
			
            if( path.EndsWith("/time") && path.Length <= 8 )
            {
                request.AddHeader("CB-VERSION", Constants.ApiVersionDate);
                return;
            }
            string timestamp;
            if( _useTimeApi )
            {
                var timeReq = new RestRequest("/time", Method.GET)
                    {
                        Serializer = new JsonNetSerializer(_jsonSettings)
                    };

                var timeResp = await client.Execute<CoinbaseResponse<Time>>(timeReq);
                timestamp = timeResp.Data.Data.Epoch.ToString();
            }
            else
            {
                timestamp = EpochConverter.GetCurrentUnixTimestampSeconds().ToString(CultureInfo.InvariantCulture);
            }

            var method = request.Method.ToString().ToUpper();

            var body = string.Empty;
            if( request.Method != Method.GET )
            {
                var param = request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);
                if (param != null && param.Value?.ToString() != "null" &&
                    !string.IsNullOrWhiteSpace(param?.Value?.ToString()))
                {
                    body = Encoding.UTF8.GetString ((byte[])param.Value);
                }
            }
            else
            {
                path = uri.PathAndQuery;
            }


            var hmacSig = GenerateSignature(timestamp, method, path, body, _apiSecret);

            // For GDAX api only
            if (!string.IsNullOrEmpty(_passphrase))
                request.AddHeader("CB-ACCESS-PASSPHRASE", _passphrase);

            request.AddHeader("CB-ACCESS-KEY", _apiKey)
                .AddHeader("CB-ACCESS-SIGN", hmacSig)
                .AddHeader("CB-ACCESS-TIMESTAMP", timestamp)
                .AddHeader("CB-VERSION", Constants.ApiVersionDate);
        }

        public string GenerateSignature(string timestamp, string method, string url, string body, string appSecret)
        {
            return GetHMACInHex(appSecret, timestamp + method + url + body);
        }

        internal string GetHMACInHex(string key, string data)
        {
            return _useBase64
                ? data.GetBase64Hmac(key)
                : data.GetHexHmac(key);
        }


        public bool CanPreAuthenticate(IRestClient client, IRestRequest request, ICredentials credentials)
        {
            return true;
        }

        public bool CanPreAuthenticate(IHttpClient client, IHttpRequestMessage request, ICredentials credentials)
        {
            return false;
        }

        public bool CanHandleChallenge(IHttpClient client, IHttpRequestMessage request, ICredentials credentials,
            IHttpResponseMessage response)
        {
            return false;
        }

        public Task PreAuthenticate(IRestClient client, IRestRequest request, ICredentials credentials)
        {
            return Authenticate(client, request);
        }

        public Task PreAuthenticate(IHttpClient client, IHttpRequestMessage request, ICredentials credentials)
        {
            throw new NotImplementedException();
        }

        public Task HandleChallenge(IHttpClient client, IHttpRequestMessage request, ICredentials credentials,
            IHttpResponseMessage response)
        {
            throw new NotImplementedException();
        }
    }
}