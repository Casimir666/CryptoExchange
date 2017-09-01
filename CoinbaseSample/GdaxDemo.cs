using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange;
using CryptoExchange.Common;
using CryptoExchange.Gdax;

namespace CoinbaseSample
{
    class GdaxDemo
    {

        private readonly IGdaxClient _gdaxClient;
        private readonly ICoinbaseClient _coinbaseClient;
        private Product _product;

        public IGdaxClient Client => _gdaxClient;

        public GdaxDemo(IGdaxClient gdaxClient, ICoinbaseClient coinbaseClient)
        {
            _gdaxClient = gdaxClient;
            _coinbaseClient = coinbaseClient;
        }

        public async Task Init()
        {
            var products = await _gdaxClient.GetProducts();
            _product = products.FirstOrDefault(p => p.Type == ProductType.BTC_EUR);
        }

        #region Show account history

        public async Task ShowAccountHistory()
        {
            foreach (var account in await _gdaxClient.GetAccounts())
            {
                var histo = (await _gdaxClient.GetAccountHistory(account.Id)).Result;
                if (histo.Count > 0)
                {
                    Console.WriteLine($"Ledger for {account.Currency}");
                    foreach (var h in histo)
                    {
                        Console.WriteLine($"{h.Type}; {h.CreatedAt}; {h.Amount}; {h.Balance}; {h.Details.OrderId}");
                    }
                    Console.WriteLine();
                }
            }
        }

        #endregion

        #region Show order history

        public async Task ShowOrdersHistory()
        {
            var orders = new List<Guid>();
            foreach (var account in await _gdaxClient.GetAccounts())
            {
                var histo = (await _gdaxClient.GetAccountHistory(account.Id)).Result;
                if (histo.Count > 0)
                {
                    foreach (var h in histo)
                    {
                        if (h.Details.OrderId != Guid.Empty && !orders.Contains(h.Details.OrderId))
                            orders.Insert(0, h.Details.OrderId);
                    }
                }
            }

            Program.Log.Info("Created at; Side; Price; Type; Executed value; Size; Filled size; Fill fees; Status");
            foreach (var o in orders)
            {
                var r = await _gdaxClient.GetOrder(o);
                Program.Log.Info($"{r.CreatedAt}; {r.Side}; {r.Price}; {r.Type}; {r.ExecutedValue}; {r.Size}; {r.FilledSize}; {r.FillFees}; {r.Status} ");
            }
        }

        #endregion


        public async Task ShowBalance()
        {
            var balanceList = new Dictionary<Currency, Balance>();
            foreach (var account in await _gdaxClient.GetCoinbaseAccount())
            {
                if (!balanceList.ContainsKey(account.Currency))
                    balanceList.Add(account.Currency, new Balance {Currency = account.Currency});

                balanceList[account.Currency].Amount += account.Balance;
            }

            foreach (var account in await _gdaxClient.GetAccounts())
            {
                if (!balanceList.ContainsKey(account.Currency))
                    balanceList.Add(account.Currency, new Balance { Currency = account.Currency });
                balanceList[account.Currency].Amount += account.Balance;
            }

            var total = 0m;
            foreach (var balance in balanceList)
            {
                switch (balance.Value.Currency)
                {
                    case Currency.BTC:
                        if (balance.Value.Amount > 0)
                        {
                            var btcTicker = await _gdaxClient.GetProductTicker(ProductType.BTC_EUR);
                            total += balance.Value.Amount * btcTicker.Price;
                            Console.WriteLine($"  - {btcTicker.Price:F2} BTC");
                        }
                        break;
                    case Currency.ETH:
                        if (balance.Value.Amount > 0)
                        {
                            var ethTicker = await _gdaxClient.GetProductTicker(ProductType.ETH_EUR);
                            total += balance.Value.Amount * ethTicker.Price;
                            Console.WriteLine($"  - {ethTicker.Price:F2} ETH");
                        }
                        break;
                    case Currency.EUR:
                        total += balance.Value.Amount;
                        Console.WriteLine($"  - {balance.Value.Amount:F2} EUR");
                        break;
                    case Currency.LTC:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            Console.WriteLine($"Total amount : {total:F2} EUR");
        }
    }
}
