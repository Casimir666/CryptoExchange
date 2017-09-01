using System;
using CryptoExchange.Common;

namespace CryptoExchange
{
    public static class Api
    {
        internal static IGdaxLog Log { get; private set; }


        public static void SetLogger(IGdaxLog logger)
        {
            Log = logger;
        }

        public static IGdaxClient CreateGdaxClient(string apiKey, string apiSecret, string apiPassphrase, ClientMode mode, string dataPath = null)
        {
            switch (mode)
            {
                case ClientMode.Production:
                case ClientMode.Sandbox:
                    return new Gdax.Client(apiKey, apiSecret, apiPassphrase, mode);
                case ClientMode.Simulated:
                    return new Gdax.SimulatedClient(dataPath);
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static ICoinbaseClient CreateCoinbaseClient(string apiKey, string apiSecret, ClientMode mode)
        {
            switch (mode)
            {
                case ClientMode.Production:
                case ClientMode.Sandbox:
                    return new Coinbase.Client(apiKey, apiSecret, mode);
                //case ClientMode.Sandbox:
                //    break;
                //case ClientMode.Simulated:
                //    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static long FromSatoshi(this decimal value)
        {
            return (long)(value * 100000000);
        }

        public static decimal ToSatoshi(this long value)
        {
            return value / 100000000M;
        }

        static Api()
        {
            Log = new NullLogger();
        }
    }
}
