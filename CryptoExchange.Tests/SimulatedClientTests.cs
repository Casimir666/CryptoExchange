using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CryptoExchange;
using CryptoExchange.Gdax;
using FluentAssertions;
using NUnit.Framework;

namespace CoinbaseApi.Tests
{
    /*
    [TestFixture]
    public class SimulatedClientTests
    {

        [TestCase("SimulatedClient_GetCandles1", "2017-01-01T00:00:00.0000000Z", "2017-01-01T12:00:00.0000000Z", 120)]
        [TestCase("SimulatedClient_GetCandles2", "2017-01-01T12:00:00.0000000Z", "2017-01-02T00:00:00.0000000Z", 120)]
        [TestCase("SimulatedClient_GetCandles3", "2017-05-13T12:00:00.0000000Z", "2017-05-13T13:00:00.0000000Z", 120*3)]
        public async Task GetProductCandles(string resourceName, string startString, string endString, long granularity)
        {
            var expected = DataHelper.LoadData<List<Candle>>(resourceName);
            var start = DateTime.Parse(startString).ToUniversalTime();
            var end = DateTime.Parse(endString).ToUniversalTime();
            var path = Path.GetFullPath(TestContext.CurrentContext.TestDirectory + @"..\..\..\..\Data\gdax\gdax");
            var client = Api.CreateGdaxClient("", "", "", ClientMode.Simulated, path);

            var actual = await client.GetProductCandle(ProductType.BTC_EUR, start, end, granularity);

            actual.ShouldBeEquivalentTo<IList<Candle>>(expected, EquivalencyHelper.CandleOptions);
        }
    }
    */
}
