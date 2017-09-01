using System;

namespace CryptoExchange.Common
{
    class NullLogger : IGdaxLog
    {
        public void Info(string message)
        {
        }

        public void Warning(string message)
        {
        }

        public void Error(Exception ex, string message)
        {
        }

        public void Debug(string message)
        {
        }
    }
}
