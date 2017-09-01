namespace CryptoExchange.Coinbase
{
    public static class Constants
    {
        public const string LiveApiUrl = "https://api.coinbase.com/";
        public const string TestApiUrl = "https://api.sandbox.coinbase.com/";
        public const string LiveCheckoutUrl = "https://coinbase.com/checkouts/{code}";
        public const string TestCheckoutUrl = "https://sandbox.coinbase.com/checkouts/{code}";
        public const string ApiVersionDate = "2016-08-10";

        private const string RestVersion = "v2/";

        public class RestUrl
        {
            public const string User = RestVersion + "user";
            public const string Accounts = RestVersion + "accounts";
            public const string Orders = RestVersion + "orders";
            public const string Addresses = RestVersion + "accounts/{0}/addresses";
            public const string ExchangeRate = RestVersion + "exchange-rates";
            public const string BuyPrice = RestVersion + "prices/{0}-{1}/buy";
            public const string SellPrice = RestVersion + "prices/{0}-{1}/sell";
            public const string SpotPrice = RestVersion + "prices/{0}-{1}/spot";

            public const string PaymentMethod = RestVersion + "payment-methods";
            public const string Buys = RestVersion + "accounts/{0}/buys";

            public const string ListSells = RestVersion + "accounts/{0}/sells";
        }

    }
}
