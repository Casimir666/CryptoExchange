using System;

namespace CryptoExchange.Gdax
{
    public interface ITimeStamped
    {
        DateTime Time { get; }
    }
}
