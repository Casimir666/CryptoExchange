using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Coinbase;

namespace CryptoExchange
{
    public interface ICoinbaseClient
    {
        Task<User> GetUser();
        Task<List<Account>> GetAccounts();
        Task<Account> GetAccountDetails(Account account);
        Task<List<Order>> GetOrders();
        Task<List<Addresses>> GetAddresses(Account account);
        Task<ExchangeRate> GetExchangeRates(string currency = "BTC");
        Task<Prices> GetRates(string from, string to);
        Task<List<OrderRequest>> GetBuysList(Account account);
        Task<List<PaymentMethod>> GetPaymentMethod();
        Task<List<OrderRequest>> GetSellsList(Account account);
    }
}
