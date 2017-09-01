using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange;
using CryptoExchange.Gdax;
using FluentAssertions;
using NUnit.Framework;

namespace CoinbaseApi.Tests
{

    [TestFixture]
    public class GdaxClientTests
    {
        const ClientMode Mode = ClientMode.Production;

        [Test]
        public async Task GetAllAccounts()
        {
            var client = CreateClient();
            var accounts = await client.GetAccounts();

            accounts.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetAccountHistoryAndHolds()
        {
            var client = CreateClient();
            foreach (var targetAccount in await client.GetAccounts())
            {
                var account = await client.GetAccount(targetAccount.Id);

                account.ShouldBeEquivalentTo(targetAccount);

                var history = await client.GetAccountHistory(targetAccount.Id);
                history.Result.Should().NotBeNull();

                //var holds = await client.GetHolds(targetAccount.Id);
                //holds.Result.Should().NotBeNull();
            }
        }


        [Test]
        public async Task GetTrailingVolume()
        {
            var client = CreateClient();
            var trailing = await client.GetTrailingVolume();
            trailing.Should().NotBeNullOrEmpty();
        }


        [Test]
        public async Task GetCoinbaseAccount()
        {
            var client = CreateClient();
            var account = await client.GetCoinbaseAccount();

            account.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetProducts()
        {
            var targetProducts = DataHelper.LoadData<List<Product>>("Products.json");
            var client = CreateClient();
            var products = await client.GetProducts();

            products.ShouldBeEquivalentTo(targetProducts);
        }

        [Test]
        public async Task GetProductBook()
        {
            var client = CreateClient();
            var book = await client.GetProductBook(ProductType.BTC_EUR);

            book.Should().NotBeNull();
        }

        [Test]
        public async Task GetProductTicker()
        {
            var client = CreateClient();
            var ticker = await client.GetProductTicker(ProductType.BTC_EUR);

            ticker.Should().NotBeNull();
        }

        [Test]
        public async Task GetProductTrades()
        {
            var client = CreateClient();
            var trades = await client.GetProductTrades(ProductType.BTC_EUR);

            trades.Result.Should().NotBeNullOrEmpty();
            trades.Result.Count.Should().Be((int)(trades.IndexBefore.Value - trades.IndexAfter.Value + 1));

            var before = await client.GetProductTrades(ProductType.BTC_EUR, trades.IndexAfter, limit:10);
            before.Result.Count.Should().Be(10);
        }

        [Test]
        public async Task GetProductCandle()
        {
            var client = CreateClient();
            var candles = await client.GetProductCandle(ProductType.BTC_EUR, new DateTime(2017, 1, 1), new DateTime(2017, 1, 2), 240);
            var refCandles = DataHelper.LoadData<List<Candle>>("Candles.json");

            candles.ShouldBeEquivalentTo<IList<Candle>>(refCandles, EquivalencyHelper.CandleOptions);
        }

        [Test]
        public async Task GetOrders()
        {
            var client = CreateClient();
            var orders = await client.GetOrders(ProductType.BTC_EUR, OrderStatus.Done);

            orders.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetOrderAndFill()
        {
            var client = CreateClient();
            var testOrder = (await client.GetOrders(ProductType.BTC_EUR, OrderStatus.Done)).FirstOrDefault();

            var order = await client.GetOrder(testOrder.Id);
            var fills = await client.GetFills(testOrder.Id);
            order.Should().NotBeNull();
            fills.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetFills()
        {
            var client = CreateClient();
            var fills = await client.GetFills();

            fills.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetPosition()
        {
            var client = CreateClient();
            var position = await client.GetPosition();

            position.Should().NotBeNull();
        }

        [Test()]
        public async Task InvalidCredentials()
        {
            string apiKey, apiSecret, passphrase;
            Credentials.LoadGdax(Mode, out apiKey, out apiSecret, out passphrase);
            var client = Api.CreateGdaxClient("bad key", apiSecret, passphrase, Mode);

            Func<Task> act = async () => await client.GetAccounts();
            act.ShouldThrow<AggregateException>().WithInnerException<ApiException>();
        }

        [Test]
        public async Task GetCurrencies()
        {            
            var client = CreateClient();
            var currencies = await client.GetCurrencies();

            currencies.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetPaymentMethods()
        {
            var client = CreateClient();
            var payments = await client.GetPaymentMethods();

            payments.Should().NotBeNullOrEmpty();
        }

        private IGdaxClient CreateClient()
        {
            string apiKey, apiSecret, passphrase;
            Credentials.LoadGdax(Mode, out apiKey, out apiSecret, out passphrase);
            return Api.CreateGdaxClient(apiKey, apiSecret, passphrase, Mode);
        }
    }
}
