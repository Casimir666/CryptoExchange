using System;
using System.IO;
using System.Threading.Tasks;
using CryptoExchange;
using CryptoExchange.Gdax;
using NLog;
using NLog.Config;
using NLog.Targets;


namespace CoinbaseSample
{
    class Program
    {
        public static Logger Log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var mode = ClientMode.Production;

            ConfigureNLog();

            Credentials.LoadGdax(mode, out var apiKey, out var apiSecret, out var passphrase);
            var gdaxClient = Api.CreateGdaxClient(apiKey, apiSecret, passphrase, mode, @"..\..\..\..\..\Samples");

            Credentials.LoadCoinbase(out apiKey, out apiSecret);
            var coinbaseClient = Api.CreateCoinbaseClient(apiKey, apiSecret, mode);

            Task.Factory.StartNew(() => DoTestApi(gdaxClient, coinbaseClient));

            while (Console.ReadKey().Key != ConsoleKey.Q);
        }

        private static WebStreamDumper _dumper;
        static async Task DoTestApi(IGdaxClient gdax, ICoinbaseClient coinbase)
        {
            var demo = new GdaxDemo(gdax, coinbase);
            _dumper = new WebStreamDumper(gdax, ProductType.BTC_EUR, Path.GetFullPath("Dump"));

            await demo.Init();
            await demo.ShowBalance();
            await _dumper.Run();
        }

        private static void ConfigureNLog()
        {
            var config = new LoggingConfiguration();


            var fileTarget =
                new FileTarget
                {
                    FileName = Path.GetFullPath("CoinbaseSample.log")
                };

            config.AddTarget("logfile", fileTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));


            var consoleTarget =
                new ConsoleTarget
                {
                    Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss}  ${message}"
                };
            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));

            LogManager.Configuration = config;

            LogManager.GetCurrentClassLogger().Info("Log initialized");
            Api.SetLogger(new GdaxApiLogger());
        }

    }
}
