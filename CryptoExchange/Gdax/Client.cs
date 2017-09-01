using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Common;
using CryptoExchange.Gdax.Book;
using CryptoExchange.Gdax.Orders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace CryptoExchange.Gdax
{
    class Client : BaseClient, IGdaxClient
    {
        private readonly ClientMode _clientMode;
        private Func<BookBase, Task> _eventsCallback;
        private Action<string, Exception> _errorCallback;
        private List<string> _listenProductTypes;

        private Timer _watchdogTimer;
        CancellationTokenSource _wsCancellation;
        private bool _stopListening;

        class BookTypeReader
        {
            [JsonProperty("type")]
            [JsonConverter(typeof(StringEnumConverter))]
            public BookType Type { get; set; }

        }


        internal Client(string apiKey, string apiSecret, string apiPassphrase, ClientMode clientMode, bool useTimeApi = false)
            : base(apiKey, apiSecret, apiPassphrase, clientMode == ClientMode.Production ? Constants.Url.ProductionRest : Constants.Url.SandboxRest, null, useTimeApi)
        {
            _clientMode = clientMode;
        }


        protected override IAuthenticator GetAuthenticator()
        {
            return new ApiAuthenticator(ApiKey, ApiSecret, ApiPassphrase, UseTimeApi, true, JsonSettings);
        }

        public async Task<List<Account>> GetAccounts()
        {
            return await SendGetRequest<List<Account>>(Constants.RestUrl.Accounts, Method.GET);
        }

        public async Task<Account> GetAccount(Guid accountId)
        {
            return await SendGetRequest<Account>(string.Format(Constants.RestUrl.Account, accountId), Method.GET);
        }

        public async Task<PageResult<List<OrderHistory>>> GetAccountHistory(Guid accountId, long? before = null, long? after = null, int? limit = null)
        {
            return await SendGetRequestPaged<List<OrderHistory>>(string.Format(Constants.RestUrl.AccountHistory, accountId), Method.GET, before, after, limit);
        }

        public async Task<PageResult<List<Hold>>> GetHolds(Guid accountId, long? before = null, long? after = null, int? limit = null)
        {
            return await SendGetRequestPaged<List<Hold>>(string.Format(Constants.RestUrl.AccountHolds, accountId), Method.GET, before, after, limit);
        }

        public async Task<List<TrailingVolume>> GetTrailingVolume()
        {
            return await SendGetRequest<List<TrailingVolume>>(Constants.RestUrl.TrailingVolume, Method.GET);
        }

        public async Task<List<CoinbaseAccount>> GetCoinbaseAccount()
        {
            return await SendGetRequest<List<CoinbaseAccount>>(Constants.RestUrl.CoinbaseAccount, Method.GET);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await SendGetRequest<List<Product>>(Constants.RestUrl.Products, Method.GET);
        }

        public async Task<ProductBook> GetProductBook(ProductType productType, BookLevel level)
        {
            var parameters = new []{ new KeyValuePair<string,string>("level", ((int)level).ToString())};

            return await SendGetRequest<ProductBook>(string.Format(Constants.RestUrl.ProductBook, productType.EnumToString()), Method.GET, parameters);
        }

        public async Task<ProductTicker> GetProductTicker(ProductType productType)
        {
            return await SendGetRequest<ProductTicker>(string.Format(Constants.RestUrl.ProductTicker, productType.EnumToString()), Method.GET);
        }

        /// <summary>
        /// Historic rates for a product. Rates are returned in grouped buckets based on requested granularity.
        /// </summary>
        /// <param name="productType"></param>
        /// <param name="start">Start time in ISO 8601</param>
        /// <param name="end">End time in ISO 8601</param>
        /// <param name="granularity">Desired timeslice in seconds</param>
        /// <returns></returns>
        public async Task<List<Candle>> GetProductCandle(ProductType productType, DateTime start, DateTime end, long granularity)
        {
            var parameters = new[]
            {
                new KeyValuePair<string, string>("start", start.ToString("o")),
                new KeyValuePair<string, string>("end", end.ToString("o")),
                new KeyValuePair<string, string>("granularity", granularity.ToString())
            };

            var client = new RestClient(ApiUrl)
            {
                Authenticator = GetAuthenticator()
            };

            var req = CreateRequest(string.Format(Constants.RestUrl.ProductCandle, productType.EnumToString()), Method.GET, parameters);

            var resp = await Execute(client, req);

            return Candle.Parse(resp.Content);
        }


        public async Task<PageResult<List<Trade>>> GetProductTrades(ProductType productType, long? before, long? after, int? limit)
        {
            return await SendGetRequestPaged<List<Trade>>(string.Format(Constants.RestUrl.ProductTrades, productType.EnumToString()), Method.GET, before, after, limit);            
        }

        public async Task<ProductStat> GetProductStats(ProductType productType)
        {
            return await SendGetRequest<ProductStat>(string.Format(Constants.RestUrl.ProductStats, productType.EnumToString()), Method.GET);
        }

        public async Task<List<PaymentMethod>> GetPaymentMethods()
        {
            return await SendGetRequest<List<PaymentMethod>>(Constants.RestUrl.PaymentMethods, Method.GET);
        }

        public async Task<PaymentResponse> Payment(decimal amount, Currency currency, Guid paymentMethodId)
        {
            var request = new PaymentRequest
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodId = paymentMethodId
            };
            return await SendPostRequest<PaymentResponse>(Constants.RestUrl.Payment,request);
        }

        public async Task<PaymentResponse> Withdrawal(decimal amount, Currency currency, Guid paymentMethodId)
        {
            var request = new PaymentRequest
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodId = paymentMethodId
            };
            return await SendPostRequest<PaymentResponse>(Constants.RestUrl.Withdrawal, request);
        }

        public async Task<PaymentResponse> CoinbasePayment(decimal amount, Currency currency, Guid accountId)
        {
            var request = new PaymentRequest
            {
                Amount = amount,
                Currency = currency,
                CoinbaseId = accountId
            };

            return await SendPostRequest<PaymentResponse>(Constants.RestUrl.CoinbasePayment, request);
        }

        public async Task<PaymentResponse> CoinbaseWithdrawal(decimal amount, Currency currency, Guid accountId, string twoFactorCode)
        {
            var request = new PaymentRequest
            {
                Amount = amount,
                Currency = currency,
                CoinbaseId = accountId,
                TwoFactorCode = twoFactorCode
            };

            return await SendPostRequest<PaymentResponse>(Constants.RestUrl.CoinbaseWithdrawal, request);
        }

        public async Task<PaymentResponse> CryptoWithdrawal(decimal amount, Currency currency, string cryptoAddress)
        {
            var request = new PaymentRequest
            {
                Amount = amount,
                Currency = currency,
                CryptoAddress = cryptoAddress
            };

            return await SendPostRequest<PaymentResponse>(Constants.RestUrl.CryptoWithdrawal, request);
        }


        #region Websocket

        public async Task RegisterLiveFeeds(IEnumerable<ProductType> productTypes, Func<BookBase, Task> eventsCallback, Action<string, Exception> errorCallback = null)
        {
            _listenProductTypes = productTypes.Select(p => p.EnumToString()).ToList();
            _stopListening = false;
            _eventsCallback = eventsCallback;
            _errorCallback = errorCallback;
            _watchdogTimer = new Timer(WebsockerWatchdog, null, Constants.WebsocketTimer, Constants.WebsocketTimer);
            await Task.Factory.StartNew(ConnectToWebsocket);
        }

        public Task UnregisterLiveFeeds()
        {
            _stopListening = true;
            _wsCancellation.Cancel();

            return Task.CompletedTask;
        }


        private void WebsockerWatchdog(object state)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
                return;
#endif

            Api.Log.Error(null, $"Missing heartbeat, websocket will be restarted");

            try
            {
                _wsCancellation.Cancel();
            }
            catch (Exception ex)
            {
                Api.Log.Error(ex, ex.Message);
            }
        }



        private async Task ConnectToWebsocket()
        {
            var uri = _clientMode == ClientMode.Production ? new Uri(Constants.Url.ProductionWebsocket) : new Uri(Constants.Url.SandboxWebsocket);

            do
            {
                try
                {
                    var websocket = new ClientWebSocket();

                    Api.Log.Info($"Connecting to real time feeds at url {uri}");
                    _wsCancellation = new CancellationTokenSource();
                    await websocket.ConnectAsync(uri, _wsCancellation.Token);

                    Api.Log.Info("Websocket is connected, sending authentification");
                    await websocket.Send(_wsCancellation.Token, CreateAuthenticationMessage(_listenProductTypes));

                    // TODO : see Gdax bug...
                    //await websocket.Send(_wsCancellation.Token, CreateHeartbeat(true));

                    while (websocket.State == WebSocketState.Open && !_wsCancellation.Token.IsCancellationRequested)
                    {
                        var message = await websocket.Receive(_wsCancellation.Token);
                        if (websocket.State == WebSocketState.Open)
                            DoReceivedSocket(message);
                    }
                }
                catch (Exception ex)
                {
                    Api.Log.Error(ex, $"Connection close with real time feeds, {ex.Message}");
                    NotifyError(ex.Message, ex);
                }
            } while (!_stopListening);

            Api.Log.Info($"Connection close with real time feeds");
        }


        private void DoReceivedSocket(string json)
        {
            var result = (BookBase)null;

            try
            {
                var typeReader = JsonConvert.DeserializeObject<BookTypeReader>(json);

                switch (typeReader.Type)
                {
                    case BookType.Received:
                        result = JsonConvert.DeserializeObject<BookReceive>(json);
                        break;
                    case BookType.Open:
                        result = JsonConvert.DeserializeObject<BookOpen>(json);
                        break;
                    case BookType.Done:
                        result = JsonConvert.DeserializeObject<BookDone>(json);
                        break;
                    case BookType.Match:
                        result = JsonConvert.DeserializeObject<BookMatch>(json);
                        break;
                    case BookType.Change:
                        result = JsonConvert.DeserializeObject<BookChange>(json);
                        break;
                    case BookType.Heartbeat:
                        break;
                }
                _watchdogTimer.Change(Constants.WebsocketTimer, Constants.WebsocketTimer);
            }
            catch (Exception ex)
            {
                NotifyError(ex.Message, ex);
            }

            NotifyEvent(result, json);
        }

        //private void DoErrorSocket(object sender, ErrorEventArgs arg)
        //{
        //    DisconnectFromWebsocket();
        //    ConnectToWebsocket();
        //    NotifyError(arg.Message, arg.Exception);
        //}

        private void NotifyError(string message, Exception exception)
        {
            if (_errorCallback != null)
            {
                try
                {
                    _errorCallback(message, exception);
                }
                catch
                {
                    // Continue on error
                }
            }
        }

        private void NotifyEvent(BookBase result, string json)
        {
            if (result != null && _eventsCallback != null)
            {
                try
                {
                    _eventsCallback(result).Wait();
                }
                catch (Exception ex)
                {
                    Api.Log.Error(ex, $"Error sending message {json}, {ex.Message}");
                }
            }
        }



        private string CreateAuthenticationMessage(List<string> productTypes)
        {
            var timestamp = EpochConverter.GetCurrentUnixTimestampSeconds().ToString(CultureInfo.InvariantCulture);
            var authenticator = new ApiAuthenticator(ApiKey, ApiSecret, ApiPassphrase, UseTimeApi, true, JsonSettings);
            return JsonConvert.SerializeObject(new WsAuth
            {
                ProductTypes = productTypes,
                Key = ApiKey,
                Passphrase = ApiPassphrase,
                Signature = authenticator.GenerateSignature(timestamp, "GET", "/users/self", string.Empty, ApiSecret),
                Timestamp = timestamp,
                Type = "subscribe"
            });
        }

        private string CreateHeartbeat(bool isOn)
        {
            return JsonConvert.SerializeObject(new
            {
                on = isOn,
                type = "heartbeat"
            });
        }

        #endregion

        #region Orders

        public async Task<OrderResponse> PutMarketOrder(SideType side, ProductType productType, decimal amount, Guid? clientId)
        {
            var order = new MarketOrder
            {
                Side = side,
                ProductType = productType.EnumToString(),
                Size = amount,
                ClientGuid = clientId
            };

            return await SendPostRequest<OrderResponse>(Constants.RestUrl.Orders, order);
        }


        public async Task<OrderResponse> PutStopOrder(SideType side, ProductType productType, decimal price, decimal amount, Guid? clientId)
        {
            var order = new StopOrder
            {
                Side = side,
                ProductType = productType.EnumToString(),
                Price = price,
                Size = amount,
                ClientGuid = clientId
            };


            return await SendPostRequest<OrderResponse>(Constants.RestUrl.Orders, order);
        }

        public async Task<OrderResponse> PutLimitOrder(SideType side, ProductType productType, decimal price, decimal size, Guid? clientId, bool? postOnly)
        {
            var order = new LimitOrder
            {
                Side = side,
                Price = price,
                Size = size,
                ProductType = productType.EnumToString(),
                ClientGuid = clientId,
                PostOnly = postOnly
            };

            return await SendPostRequest<OrderResponse>(Constants.RestUrl.Orders, order);
        }

        public async Task<List<OrderResponse>> GetOrders(ProductType? productType = null, OrderStatus? status = null)
        {
            var queryParams = new List<KeyValuePair<string, string>>();
            if (productType.HasValue) queryParams.Add(new KeyValuePair<string, string>("product", productType.Value.EnumToString()));
            if (status.HasValue) queryParams.Add(new KeyValuePair<string, string>("status", status.Value.EnumToString()));

            return await SendGetRequest< List<OrderResponse>>(Constants.RestUrl.Orders, Method.GET, queryParams.ToArray());
        }

        public async Task<OrderResponse> GetOrder(Guid orderId)
        {
            return await SendGetRequest<OrderResponse>(string.Format(Constants.RestUrl.Order, orderId), Method.GET);
        }

        public async Task<List<Fill>> GetFills(Guid? orderId = null, ProductType? productType = null)
        {
            var queryParams = new List<KeyValuePair<string, string>>();
            if (productType.HasValue)
                queryParams.Add(new KeyValuePair<string, string>("product_id", productType.EnumToString()));
            if (orderId != null)
                queryParams.Add(new KeyValuePair<string, string>("order_id", orderId.ToString()));
            return await SendGetRequest<List<Fill>>(Constants.RestUrl.Fills, Method.GET, queryParams.ToArray());
        }

        public async Task<List<Funding>> GetFundings()
        {
            return await SendGetRequest<List<Funding>>(Constants.RestUrl.Funding, Method.GET);
        }

        public async Task<Position> GetPosition()
        {
            return await SendGetRequest<Position>(Constants.RestUrl.Position, Method.GET);
        }


        public async Task CancelOrder(Guid orderId)
        {
            try
            {
                await SendGetRequest(string.Format(Constants.RestUrl.Order, orderId), Method.DELETE);
            }
            catch (ApiException ex)
            {
                if (ex.ErrorCode != ApiError.OrderAlreadyDone)
                    throw;
            }
        }

        public async Task<List<Guid>> CancelAllOrders()
        {
            return await SendGetRequest<List<Guid>>(Constants.RestUrl.Orders, Method.DELETE);
        }

        public async Task<DateTime> GetTime()
        {
            var gdaxTime = await SendGetRequest<GdaxTime>(Constants.RestUrl.Time, Method.GET);
            return gdaxTime.Time;
        }

        public async Task<List<CurrencyDetail>> GetCurrencies()
        {
            return await SendGetRequest<List<CurrencyDetail>>(Constants.RestUrl.Currencies, Method.GET);
        }

        public async Task<ReportInfo> CreateReport(ReportFormat format, DateTime start, DateTime end, ProductType productType, Guid? accountId = null, string email = null)
        {
            var request = new ReportRequest
            {
                Type = accountId != null ? "account" : "fills",
                Format = format,
                StartDate = start,
                EndDate = end,
                ProductType = productType,
                AccountId = accountId,
                Email = email
            };

            return await SendPostRequest<ReportInfo>(Constants.RestUrl.Reports, request);
        }

        public async Task<ReportInfo> GetReport(Guid id)
        {
            return await SendGetRequest<ReportInfo>(string.Format(Constants.RestUrl.Report, id), Method.GET);
        }

        public ClientMode Mode => _clientMode;

        #endregion

        #region Send REST request

        private async Task<TResponse> SendPostRequest<TResponse>(string endpoint, object body)
        {
            var client = CreateClient();

            var req = CreateRequest(endpoint, Method.POST)
                .AddJsonBody(body)
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Accept", "*/*");

            var resp = await Execute<TResponse>(client, req);

            return resp.Data;
        }

        private async Task<TResponse> SendGetRequest<TResponse>(string endpoint, Method method, params KeyValuePair<string, string>[] queryParams)
        {
            var client = CreateClient();
            var req = CreateRequest(endpoint, method, queryParams);
            var resp = await Execute<TResponse>(client, req);

            return resp.Data;
        }

        private async Task<PageResult<TResponse>> SendGetRequestPaged<TResponse>(string endpoint, Method method, long? before, long? after, int? limit, params KeyValuePair<string, string>[] addQuery)
        {
            var queryParams = new List<KeyValuePair<string, string>>(addQuery);

            if (before.HasValue)
                queryParams.Add(new KeyValuePair<string, string>("before", before.Value.ToString()));
            if (after.HasValue)
                queryParams.Add(new KeyValuePair<string, string>("after", after.Value.ToString()));
            if (limit.HasValue)
                queryParams.Add(new KeyValuePair<string, string>("limit", limit.Value.ToString()));

            var client = CreateClient();
            var req = CreateRequest(endpoint, method, queryParams.ToArray());
            var resp = await Execute < TResponse > (client, req);

            return new PageResult<TResponse>(resp.Data, ReadHeader(resp, "cb-before"), ReadHeader(resp, "cb-after"));
        }

        private async Task SendGetRequest(string endpoint, Method method, params KeyValuePair<string, string>[] queryParams)
        {
            var client = CreateClient();
            var req = CreateRequest(endpoint, method, queryParams);

            await Execute(client, req);
        }

        private long? ReadHeader(IRestResponse resp, string name)
        {
            var value = resp.Headers.GetValue(name);
            return string.IsNullOrEmpty(value) ? (long?)null : long.Parse(value);
        }

        private async Task<IRestResponse<TResponse>> Execute<TResponse>(RestClient client, IRestRequest request)
        {
            client.IgnoreResponseStatusCode = true;
            var response = await client.Execute<TResponse>(request);
            if (response.IsSuccess)
                return response;

            var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
            if (error != null)
                throw new ApiException(error.Cause);

            throw new ApiException(response.StatusDescription);
        }

        private async Task<IRestResponse> Execute(RestClient client, IRestRequest request)
        {
            client.IgnoreResponseStatusCode = true;
            var response = await client.Execute(request);
            if (response.IsSuccess)
                return response;

            var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
            if (error != null)
                throw new ApiException(error.Cause);

            throw new ApiException(response.StatusDescription);
        }

        #endregion
    }
}
