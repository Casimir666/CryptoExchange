using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Common;
using CryptoExchange.Gdax.Book;
using CryptoExchange.Gdax.Orders;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax
{
    class SimulatedClient : IGdaxClient, IGdaxSimulator
    {
        private const int MinGranularity = 60;
        private readonly string _dataPath;
        private Func<BookBase, Task> _callback;
        private IEnumerator<BookBase> _bookEnumerator;
        private bool _isRunning;
        private readonly Queue<BookDone> _waitingOrder;
        private List<Account> _accounts;

        public SimulatedClient(string dataPath)
        {
            _dataPath = dataPath;
            _isRunning = true;
            _waitingOrder = new Queue<BookDone>();
        }

        public async Task<List<Account>> GetAccounts()
        {
            _accounts = _accounts ?? await ReadJsonFile<List<Account>>("accounts.json");
            return _accounts;
        }

        public async Task<Account> GetAccount(Guid accountId)
        {
            var accounts = await ReadJsonFile<List<Account>>("accounts.json");
            return accounts.FirstOrDefault(a => a.Id == accountId);
        }

        public async Task<PageResult<List<OrderHistory>>> GetAccountHistory(Guid accountId, long? before = null, long? after = null, int? limit = null)
        {
            return new PageResult<List<OrderHistory>>(await ReadJsonFile<List<OrderHistory>>($"history_{accountId}.json"), null, null);
        }

        public Task<PageResult<List<Hold>>> GetHolds(Guid accountId, long? before = null, long? after = null, int? limit = null)
        {
            return Task.FromResult(new PageResult<List<Hold>>(new List<Hold>(), null, null));
        }

        public async Task<List<TrailingVolume>> GetTrailingVolume()
        {
            return await ReadJsonFile<List<TrailingVolume>>("trailing.json");
        }

        public async Task<List<CoinbaseAccount>> GetCoinbaseAccount()
        {
            return await ReadJsonFile<List<CoinbaseAccount>>("coinbaseaccount.json");
        }

        public async Task<List<Product>> GetProducts()
        {
            return await ReadJsonFile<List<Product>>("products.json");
        }

        public Task<ProductBook> GetProductBook(ProductType productType, BookLevel level)
        {
            return Task.FromResult<ProductBook>(null);
        }

        public async Task<ProductTicker> GetProductTicker(ProductType productType)
        {
            return await ReadJsonFile<ProductTicker>("ticker.json");
        }

        public Task<PageResult<List<Trade>>> GetProductTrades(ProductType productType, long? before, long? after, int? limit)
        {
            return Task.FromResult(new PageResult<List<Trade>>(new List<Trade>(), null, null));
        }

        public Task<ProductStat> GetProductStats(ProductType productType)
        {
            return Task.FromResult(new ProductStat());
        }

        public Task<List<Candle>> GetProductCandle(ProductType productType, DateTime start, DateTime end, long granularity)
        {
            var result = new List<Candle>();
            var currentTime = start;
            var bookGranularity = (granularity == 3600 || granularity == 86400) ? (int)granularity : MinGranularity;
            Candle current = null;

            // Aggregate candles from data with a 120s resolution
            foreach (var c in GetBooks<Candle>(productType, start, bookGranularity).TakeWhile(c => c.Time < end).Where(c => c.Time >= start))
            {
                if (current == null)
                {
                    current = c;
                }
                else if (c.Time < currentTime + TimeSpan.FromSeconds(granularity))
                {
                    current.Close = c.Close;
                    current.High = Math.Max(current.High, c.High);
                    current.Low = Math.Min(current.Low, c.Low);
                    current.Volume += c.Volume;
                }

                if (c.Time >= currentTime + TimeSpan.FromSeconds(granularity))
                {
                    if (FillMissingData)
                    {
                        var repeat = (result.LastOrDefault() ?? current);
                        while (currentTime + TimeSpan.FromSeconds(granularity) < current.Time)
                        {
                            var fake = (Candle) repeat.Clone();
                            currentTime = currentTime + TimeSpan.FromSeconds(granularity);
                            fake.Time = currentTime;
                            result.Add(fake);
                        }
                    }

                    result.Add(current);
                    currentTime = currentTime + TimeSpan.FromSeconds(granularity);
                    current = c;
                }
            }

            return Task.FromResult(result);
        }

        public Task<List<PaymentMethod>> GetPaymentMethods()
        {
            return Task.FromResult(new List<PaymentMethod>());
        }

        public Task<PaymentResponse> Payment(decimal amount, Currency currency, Guid paymentMethodId)
        {
            return Task.FromResult(new PaymentResponse());
        }

        public Task<PaymentResponse> CoinbasePayment(decimal amount, Currency currency, Guid accountId)
        {
            return Task.FromResult(new PaymentResponse());
        }

        public Task<PaymentResponse> CoinbaseWithdrawal(decimal amount, Currency currency, Guid accountId, string twoFactorCode)
        {
            return Task.FromResult(new PaymentResponse());
        }

        public Task<PaymentResponse> CryptoWithdrawal(decimal amount, Currency currency, string cryptoAddress)
        {
            return Task.FromResult(new PaymentResponse());
        }

        public async Task RegisterLiveFeeds(IEnumerable<ProductType> productTypes, Func<BookBase,Task> callback, Action<string, Exception> errorCallback = null)
        {
            _callback = callback;
            await Task.Factory.StartNew(StartNotifications);
        }

        public Task UnregisterLiveFeeds()
        {
            return Task.CompletedTask;
        }

        public Task<OrderResponse> PutMarketOrder(SideType side, ProductType productType, decimal amount, Guid? clientId)
        {
            return Task.FromResult<OrderResponse>(null);
        }

        public Task<OrderResponse> PutStopOrder(SideType side, ProductType productType, decimal price, decimal amount, Guid? clientId)
        {
            return Task.FromResult<OrderResponse>(null);
        }

        public Task<OrderResponse> PutLimitOrder(SideType side, ProductType productType, decimal price, decimal size, Guid? clientId, bool? postOnly)
        {
            var done = new BookDone
            {
                ProductType = productType,
                OrderId = Guid.NewGuid(),
                Price = price,
                Side = side,
                RemainingSize = size,
                Reason = OrderReason.Filled
            };
            var reponse = new OrderResponse
            {
                CreatedAt = DateTime.UtcNow,
                Price = price,
                Size = size,
                ProductType = productType,
                Id = done.OrderId,
                Side = side
            };
            _waitingOrder.Enqueue(done);
            return Task.FromResult(reponse);
        }

        public Task<List<OrderResponse>> GetOrders(ProductType? productType = null, OrderStatus? status = null)
        {
            return Task.FromResult(new List<OrderResponse>());
        }

        public Task<List<Fill>> GetFills(Guid? orderId = null, ProductType? productType = null)
        {
            return Task.FromResult(new List<Fill>());
        }

        public Task<Position> GetPosition()
        {
            return Task.FromResult<Position>(null);
        }

        public Task<OrderResponse> GetOrder(Guid orderId)
        {
            return Task.FromResult<OrderResponse>(null);
        }

        public Task CancelOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Guid>> CancelAllOrders()
        {
            return Task.FromResult(new List<Guid>());
        }

        public Task<DateTime> GetTime()
        {
            return Task.FromResult(DateTime.UtcNow);
        }

        public Task<List<CurrencyDetail>> GetCurrencies()
        {
            return Task.FromResult(new List<CurrencyDetail>());
        }

        public Task<ReportInfo> CreateReport(ReportFormat format, DateTime start, DateTime end, ProductType productType, Guid? accountId = null,
            string email = null)
        {
            return Task.FromResult(new ReportInfo());
        }

        public Task<ReportInfo> GetReport(Guid id)
        {
            return Task.FromResult(new ReportInfo());
        }

        public ClientMode Mode => ClientMode.Simulated;

        private Task<T> ReadJsonFile<T>(string filename)
        {
            using (var reader = File.OpenText(Path.Combine(_dataPath, filename)))
            {
                return Task.FromResult(JsonConvert.DeserializeObject<T>(reader.ReadToEnd()));
            }
        }

        private async Task StartNotifications()
        {
            while (_bookEnumerator.Current != null)
            {
                //await Task.Delay(20);
                if (!_isRunning) continue;

                if (_waitingOrder.Count > 0)
                {
                    var done = _waitingOrder.Dequeue();
                    var btcAccount = _accounts.First(a => a.Currency == Currency.BTC);
                    var eurAccount = _accounts.First(a => a.Currency == Currency.EUR);
                    if (done.Side == SideType.Sell)
                    {
                        btcAccount.Available = btcAccount.Available - done.RemainingSize;
                        eurAccount.Available = eurAccount.Available + done.RemainingSize * done.Price;
                    }
                    else
                    {
                        btcAccount.Available = btcAccount.Available + done.RemainingSize;
                        eurAccount.Available = eurAccount.Available - done.RemainingSize * done.Price;
                    }
                    await _callback(done);
                }

                await _callback(_bookEnumerator.Current);
                _bookEnumerator.MoveNext();
                UtcNow = _bookEnumerator.Current.Time;
            }
        }

        private IEnumerable<T> GetBooks<T>(ProductType productType, DateTime time, int granularity = MinGranularity) where T : class
        {
            bool dataExists;
            do
            {
                var filename = Path.Combine(_dataPath, GdaxFile.GetRelativePath(productType, granularity), GdaxFile.GetFilename(time, granularity));
                dataExists = File.Exists(filename);
                if (dataExists)
                {
                    using (var serialiser = GdaxFile.OpenRead(filename))
                    {
                        var eof = false;
                        do
                        {
                            T book = null;
                            try
                            {
                                var obj = serialiser.Deserialize();
                                
                                // Return fake BookMatch for candles (for simulation from candle dump)
                                book = obj is T
                                    ? obj as T
                                    : ((Candle) obj).ToBookMatch(ProductType.BTC_EUR) as T;
                            }
                            catch (EndOfStreamException)
                            {
                                eof = true;
                            }
                            if (book != null) // && book.Time >= time)
                                yield return book;
                        } while (!eof);

                        time += TimeSpan.FromDays(1);
                    }
                }
            } while (dataExists);
        }


        public void SetReplayDate(ProductType productType, DateTime date)
        {
            UtcNow = date;
            _bookEnumerator = GetBooks<BookBase>(productType, date).GetEnumerator();
            _bookEnumerator.MoveNext();
        }

        public void PlayPause()
        {
            _isRunning = !_isRunning;
        }

        public DateTime UtcNow { get; private set; }
        public bool FillMissingData { get; set; }

        public Task<List<Funding>> GetFundings()
        {
            return Task.FromResult(new List<Funding>());
        }
    }
}
