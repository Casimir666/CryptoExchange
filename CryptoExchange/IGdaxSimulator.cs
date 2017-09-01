using System;
using CryptoExchange.Gdax;

namespace CryptoExchange
{
    public interface IGdaxSimulator
    {
        void SetReplayDate(ProductType productType, DateTime date);

        void PlayPause();

        DateTime UtcNow { get; }

        bool FillMissingData { get; set; }
    }
}
