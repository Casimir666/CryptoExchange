using System;

namespace CryptoExchange
{
    public interface IGdaxLog
    {
        void Info(string message);
        void Warning(string message);
        void Error(Exception ex, string message);
        void Debug(string message);
    }
}
