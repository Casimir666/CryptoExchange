using CryptoExchange;
using Microsoft.Win32;

namespace CoinbaseApi.Tests
{
    public static class Credentials
    {
        public static void LoadGdax(ClientMode mode, out string apiKey, out string apiSecret, out string passphrase)
        {
            using (var registry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var key = registry.OpenSubKey(@"SOFTWARE\GDAX", false))
            {
                if (mode == ClientMode.Sandbox)
                {
                    apiKey = (string)key.GetValue("GdaxSandboxApiKey");
                    apiSecret = (string)key.GetValue("GdaxSandboxApiSecret");
                    passphrase = (string)key.GetValue("GdaxSandboxPassphrase");
                }
                else
                {
                    apiKey = (string)key.GetValue("ApiKey");
                    apiSecret = (string)key.GetValue("ApiSecret");
                    passphrase = (string)key.GetValue("Passphrase");
                }
            }
        }


        public static void LoadCoinbase(out string apiKey, out string apiSecret)
        {
            using (var registry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var key = registry.OpenSubKey(@"SOFTWARE\GDAX", false))
            {
                apiKey = (string)key.GetValue("CoinbaseApiKey");
                apiSecret = (string)key.GetValue("CoinbaseApiSecret");
            }
        }
    }
}
