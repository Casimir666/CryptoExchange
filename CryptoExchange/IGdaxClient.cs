using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Common;
using CryptoExchange.Gdax;
using CryptoExchange.Gdax.Book;
using CryptoExchange.Gdax.Orders;

namespace CryptoExchange
{
    public interface IGdaxClient
    {
        #region Account

        /// <summary>
        /// Get a list of trading accounts
        /// </summary>
        Task<List<Account>> GetAccounts();

        /// <summary>
        /// Information for a single account.
        /// </summary>
        Task<Account> GetAccount(Guid accountId);

        /// <summary>
        /// List account activity. Account activity either increases or decreases your account balance. Items
        /// are paginated and sorted latest first.
        /// </summary>
        Task<PageResult<List<OrderHistory>>> GetAccountHistory(Guid accountId, long? before = null, long? after = null, int? limit = null);

        /// <summary>
        /// Holds are placed on an account for any active orders or pending withdraw requests. As an order is filled, the
        /// hold amount is updated. If an order is canceled, any remaining hold is removed. For a withdraw, once it is 
        /// completed, the hold is removed.
        /// </summary>
        Task<PageResult<List<Hold>>> GetHolds(Guid accountId, long? before = null, long? after = null, int? limit = null);

        /// <summary>
        /// This request will return your 30-day trailing volume for all products. This is a cached value that’s 
        /// calculated every day at midnight UTC.
        /// </summary>
        Task<List<TrailingVolume>> GetTrailingVolume();

        /// <summary>
        /// Get a list of your coinbase accounts.
        /// </summary>
        Task<List<CoinbaseAccount>> GetCoinbaseAccount();

        #endregion

        #region Products

        /// <summary>
        /// Get a list of available currency pairs for trading
        /// </summary>
        Task<List<Product>> GetProducts();

        /// <summary>
        /// Get a list of open orders for a product. The amount of detail shown can be customized with the level parameter
        /// </summary>
        Task<ProductBook> GetProductBook(ProductType productType, BookLevel level = BookLevel.Best);

        /// <summary>
        /// Snapshot information about the last trade (tick), best bid/ask and 24h volume.
        /// </summary>
        Task<ProductTicker> GetProductTicker(ProductType productType);

        /// <summary>
        /// List the latest trades for a product.
        /// </summary>
        Task<PageResult<List<Trade>>> GetProductTrades(ProductType productType, long? before = null, long? after = null, int? limit = null);

        /// <summary>
        /// Get 24 hr stats for the product. volume is in base currency units. open, high, low are in quote currency units
        /// </summary>
        Task<ProductStat> GetProductStats(ProductType productType);

        /// <summary>
        /// Historic rates for a product. Rates are returned in grouped buckets based on requested granularity.
        /// </summary>
        Task<List<Candle>> GetProductCandle(ProductType productType, DateTime start, DateTime end, long granularity);

        #endregion

        #region Payments

        /// <summary>
        /// Get a list of your payment methods.
        /// </summary>
        /// <returns></returns>
        Task<List<PaymentMethod>> GetPaymentMethods();

        /// <summary>
        /// Deposit funds from a payment method
        /// </summary>
        Task<PaymentResponse> Payment(decimal amount, Currency currency, Guid paymentMethodId);

        /// <summary>
        /// Move currency from Coinbase to Gdax
        /// </summary>
        Task<PaymentResponse> CoinbasePayment(decimal amount, Currency currency, Guid accountId);

        /// <summary>
        /// Move currency from Gdax to Coinbase
        /// </summary>
        Task<PaymentResponse> CoinbaseWithdrawal(decimal amount, Currency currency, Guid accountId, string twoFactorCode = null);

        /// <summary>
        /// Withdraws funds to a crypto address
        /// </summary>
        Task<PaymentResponse> CryptoWithdrawal(decimal amount, Currency currency, string cryptoAddress);

        #endregion

        #region Order management

        /// <summary>
        /// Place a new market order
        /// </summary>
        Task<OrderResponse> PutMarketOrder(SideType side, ProductType productType, decimal amount, Guid? clientId = null);

        /// <summary>
        /// Place a new stop order
        /// </summary>
        Task<OrderResponse> PutStopOrder(SideType side, ProductType productType, decimal price, decimal amount, Guid? clientId = null);

        /// <summary>
        /// Place a new limit order
        /// </summary>
        Task<OrderResponse> PutLimitOrder(SideType side, ProductType productType, decimal price, decimal size, Guid? clientId = null, bool? postOnly = null);

        /// <summary>
        /// List your current open orders. Only open or un-settled orders are returned. As soon as an order is 
        /// no longer open and settled, it will no longer appear in the default request.
        /// </summary>
        Task<List<OrderResponse>> GetOrders(ProductType? productType = null, OrderStatus? status = null);

        /// <summary>
        /// Get a single order by order id.
        /// </summary>
        Task<OrderResponse> GetOrder(Guid orderId);

        /// <summary>
        /// Get a list of recent fills.
        /// </summary>
        Task<List<Fill>> GetFills(Guid? orderId = null, ProductType? productType = null);

        /// <summary>
        /// Cancel a previously placed order
        /// </summary>
        Task CancelOrder(Guid orderId);

        /// <summary>
        /// With best effort, cancel all open orders. The response is a list of ids of the canceled orders.
        /// </summary>
        /// <returns></returns>
        Task<List<Guid>> CancelAllOrders();

        #endregion

        #region Margin transfer

        /// <summary>
        /// Every order placed with a margin profile that draws funding will create a funding record.
        /// </summary>
        /// <returns></returns>
        Task<List<Funding>> GetFundings();

        /// <summary>
        /// Transfer funds between your standard/default profile and a margin profile. A deposit will transfer funds 
        /// from the default profile into the margin profile. A withdraw will transfer funds from the margin profile 
        /// to the default profile. Withdraws will fail if they would set your margin ratio below the initial margin 
        /// ratio requirement
        /// </summary>
        Task<Position> GetPosition();

        #endregion

        /// <summary>
        /// Start real-time market data feed notifications
        /// </summary>
        Task RegisterLiveFeeds(IEnumerable<ProductType> productTypes, Func<BookBase,Task> eventsCallback, Action<string, Exception> errorCallback = null);

        /// <summary>
        /// Stop real-time market data feed notifications
        /// </summary>
        Task UnregisterLiveFeeds();


        /// <summary>
        /// Get the API server time.
        /// </summary>
        Task<DateTime> GetTime();

        /// <summary>
        /// List known currencies.
        /// </summary>
        Task<List<CurrencyDetail>> GetCurrencies();

        /// <summary>
        /// Reports provide batches of historic information about your account in various human and machine readable forms
        /// </summary>
        Task<ReportInfo> CreateReport(ReportFormat format, DateTime start, DateTime end, ProductType productType, Guid? accountId = null, string email=null);

        /// <summary>
        /// Once a report request has been accepted for processing, the status is available by polling the report resource endpoint.
        /// </summary>
        Task<ReportInfo> GetReport(Guid id);

        ClientMode Mode { get; }
    }
}
