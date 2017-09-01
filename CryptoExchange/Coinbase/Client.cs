using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Common;
using RestSharp.Portable;

namespace CryptoExchange.Coinbase
{

    class Client : BaseClient, ICoinbaseClient
    {
        public Client(string apiKey, string apiSecret, ClientMode mode,  IGdaxLog logger = null, bool useTimeApi = false)
            : base(apiKey, apiSecret, null, mode == ClientMode.Production ? Constants.LiveApiUrl : Constants.TestApiUrl, mode == ClientMode.Production ? Constants.LiveCheckoutUrl : Constants.TestCheckoutUrl, useTimeApi)
        {            
        }


        protected override IAuthenticator GetAuthenticator()
        {
            return new ApiAuthenticator(ApiKey, ApiSecret, ApiPassphrase, UseTimeApi, false, JsonSettings);
        }

        #region Added functions

        public virtual async Task<User> GetUser()
        {
            var response = await SendGetRequest<User>(Constants.RestUrl.User);
            return response.Data;
        }

        public virtual async Task<List<Account>> GetAccounts()
        {
            var response = await SendGetRequest<List<Account>>(Constants.RestUrl.Accounts);
            return response.Data;
        }

        public virtual async Task<Account> GetAccountDetails(Account account)
        {
            var response = await SendGetRequest<Account>(account.ResourcePath);
            return response.Data;
        }

        public virtual async Task<List<Order>> GetOrders()
        {
            var response = await SendGetRequest<List<Order>>(Constants.RestUrl.Orders);
            return response.Data;
        }

        public virtual async Task<List<Addresses>> GetAddresses(Account account)
        {
            var response = await SendGetRequest<List<Addresses>>(string.Format(Constants.RestUrl.Addresses, account.Id));
            return response.Data;
        }

        public virtual async Task<ExchangeRate> GetExchangeRates(string currency = "BTC")
        {
            var response = await SendGetRequest<ExchangeRate>(Constants.RestUrl.ExchangeRate, new KeyValuePair<string, string>("currency", currency));
            return response.Data;
        }

        #endregion

        public virtual async Task<Prices> GetRates(string from, string to)
        {
            var sell = await SendGetRequest<Balance>(string.Format(Constants.RestUrl.SellPrice, from, to));
            var buy = await SendGetRequest<Balance>(string.Format(Constants.RestUrl.BuyPrice, from, to));
            var spot = await SendGetRequest<Balance>(string.Format(Constants.RestUrl.SpotPrice, from, to));
            return new Prices
            {
                From = from,
                To = to,
                Sell = sell.Data,
                Buy = buy.Data,
                Spot = spot.Data
            };
        }

        public virtual async Task<List<OrderRequest>> GetBuysList(Account account)
        {
            return (await SendGetRequest<List<OrderRequest>>(string.Format(Constants.RestUrl.Buys, account.Id))).Data;
        }

        public virtual async Task<List<PaymentMethod>> GetPaymentMethod()
        {
            var res = await SendGetRequest<List<PaymentMethod>>(Constants.RestUrl.PaymentMethod);
            return res.Data;
        }

        public virtual async Task<OrderRequest> CreateAmountBuyOrder(Account account, PaymentMethod paymentMethod, decimal amount, string currency)
        {
            var orderParams =
            new
            {
                amount = amount.ToString(),
                payment_method = paymentMethod.Id,
                currency,
                commit = "false",
                quote = "true"
            };
/*            var parameters = new []
            {
                new KeyValuePair<string, string>("amount",   amount.ToString()),
                //new KeyValuePair<string, string>("total",   ""),
                new KeyValuePair<string, string>("currency", currency),
                //new KeyValuePair<string, string>("payment_method",   ""),
                //new KeyValuePair<string, string>("agree_btc_amount_varies",   ""),
                new KeyValuePair<string, string>("commit", "false"),
                new KeyValuePair<string, string>("quote", "true")
            };*/
            var order = await SendRequest<OrderRequest>(string.Format(Constants.RestUrl.Buys, account.Id), orderParams);

            return order.Data;
        }

        public virtual async Task<List<OrderRequest>> GetSellsList(Account account)
        {
            return (await SendGetRequest<List<OrderRequest>>(string.Format(Constants.RestUrl.ListSells, account.Id))).Data;
        }


        private async Task<CoinbaseResponse> SendRequest(string endpoint, object body, Method httpMethod = Method.POST)
        {
            var client = CreateClient();

            var req = CreateRequest(endpoint, httpMethod)
                .AddJsonBody(body);

            var resp = await client.Execute<CoinbaseResponse>(req);

            return resp.Data;
        }

        private async Task<CoinbaseResponse<TResponse>> SendRequest<TResponse>(string endpoint, object body, Method httpMethod = Method.POST)
        {
            var client = CreateClient();

            var req = CreateRequest(endpoint, httpMethod)
                .AddJsonBody(body);

            var resp = await client.Execute<CoinbaseResponse<TResponse>>(req);

            return resp.Data;
        }

        private async Task<CoinbaseResponse<TResponse>> SendGetRequest<TResponse>(string endpoint, params KeyValuePair<string, string>[] queryParams)
        {
            var client = CreateClient();
            var req = CreateRequest(endpoint, Method.GET, queryParams);
            var resp = await client.Execute<CoinbaseResponse<TResponse>>(req);

            return resp.Data;
        }

        private async Task<CoinbaseResponse> CreateCheckout(CheckoutRequest checkout)
        {
            return await SendRequest("checkouts", checkout);
        }

        /// <summary>
        /// Get the API server time.
        /// </summary>
        private async Task<Time> GetTime()
        {
            var resp = await SendRequest<Time>("time", null, Method.GET);
            return resp.Data;
        }

    }
}
