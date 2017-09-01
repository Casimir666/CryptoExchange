using System;
using CryptoExchange;
using NLog;

namespace CoinbaseSample
{
    class GdaxApiLogger : IGdaxLog
    {
        protected Logger Log = LogManager.GetCurrentClassLogger();

        public void Info(string message)
        {
            Log.Info(message);
        }

        public void Warning(string message)
        {
            Log.Warn(message);
        }

        public void Error(Exception ex, string message)
        {
            Log.Error(ex, message);
        }

        public void Debug(string message)
        {
            Log.Debug(message);
        }
    }
}
