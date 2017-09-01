using System.Collections.Generic;
using CryptoExchange.Gdax;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace CoinbaseApi.Tests
{
    static class EquivalencyHelper
    {
        public static EquivalencyAssertionOptions<IList<Candle>> CandleOptions(EquivalencyAssertionOptions<IList<Candle>> options)
        {
            return options
                .Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, Constants.Epsilon))
                .When(info => info.SelectedMemberPath.EndsWith("Volume"));
        }

    }
}
