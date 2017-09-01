namespace CryptoExchange.Gdax
{
    public static class Constants
    {
        public const decimal Epsilon = 0.00000000000001M;

        public const int WebsocketTimer = 4000;

        public static class GdaxFile
        {
            public const string Header = "GDAX Dump";
            public const byte Version = 1;

            public const byte CandleType = 0xFF;
            public const byte OrderBookType = 0xFE;
        }

        public static class Url
        {
            public const string ProductionRest = "https://api.gdax.com";
            public const string ProductionWebsocket = "wss://ws-feed.gdax.com";

            public const string SandboxRest = "https://api-public.sandbox.gdax.com";
            public const string SandboxWebsocket = "wss://ws-feed-public.sandbox.gdax.com";
        }

        public static class RestUrl
        {
            // Account
            public const string Accounts = "/accounts";
            public const string Account = Accounts + "/{0}";                    // AccountId
            public const string AccountHistory = Account + "/ledger";
            public const string AccountHolds = Account + "/holds";
            public const string CoinbaseAccount = "/coinbase-accounts";
            public const string TrailingVolume = "/users/self/trailing-volume";
            public const string Time = "/time";
            public const string Currencies = "/currencies";

            // Products
            public const string Products = "/products";
            public const string Product = Products + "/{0}";
            public const string ProductBook = Product + "/book";
            public const string ProductTicker = Product + "/ticker";
            public const string ProductCandle = Product + "/candles";
            public const string ProductTrades = Product + "/trades";
            public const string ProductStats = Product + "/stats";

            // Orders
            public const string Orders = "/orders";
            public const string Order = Orders + "/{0}";                        // OrderId
            public const string Fills = "/fills";
            public const string Funding = "/funding";
            public const string FundingRepay = Funding + "/repay";              // TODO GET
            public const string MarginTransfer = "/profiles/margin-transfer";   // TODO POST;
            public const string Position = "/position";
            public const string ClosePosition = Position + "/close";            // TODO POST


            // Deposit
            public const string Payment = "/deposits/payment-method";
            public const string CoinbasePayment = "/deposits/coinbase-account";

            // Withdrawals
            public const string Withdrawal = "/withdrawals/payment-method";
            public const string CoinbaseWithdrawal = "/withdrawals/coinbase";
            public const string CryptoWithdrawal = "/withdrawals/crypto";


            //Payment Methods
            public const string PaymentMethods = "/payment-methods";

            // Reports
            public const string Reports = "/reports";
            public const string Report = Reports + "/{0}";
        }
    }
}
