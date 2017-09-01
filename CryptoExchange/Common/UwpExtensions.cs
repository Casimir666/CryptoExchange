using System;
using System.IO;
using System.Linq;
#if WINDOWS_UWP
    using Windows.Security.Cryptography;
    using Windows.Security.Cryptography.Core;
    using Windows.Storage.Streams;
#else
    using System.Text;
using System.Security.Cryptography;
#endif

namespace CryptoExchange.Common
{
    public static class UwpExtensions
    {

#if WINDOWS_UWP

        public static string GetHexHmac(this string input, string key)
        {
            var algorithm = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
            var keyBuffer = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            CryptographicKey hmacKey = algorithm.CreateKey(keyBuffer);

            IBuffer buffMsg1 = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
            var buffHash1 = CryptographicEngine.Sign(hmacKey, buffMsg1);

            return CryptographicBuffer.EncodeToHexString(buffHash1);
        }

        public static string GetBase64Hmac(this string input, string key)
        {
            var algorithm = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
            var keyBuffer = CryptographicBuffer.DecodeFromBase64String(key);
            CryptographicKey hmacKey = algorithm.CreateKey(keyBuffer);

            IBuffer buffMsg1 = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
            var buffHash1 = CryptographicEngine.Sign(hmacKey, buffMsg1);

            return CryptographicBuffer.EncodeToBase64String(buffHash1);
        }

#else

        public static string GetHexHmac(this string input, string key)
        {
            var hmacKey = Encoding.UTF8.GetBytes(key);

            var hex = new HMACSHA256(hmacKey).ComputeHash(Encoding.UTF8.GetBytes(input))
                .Aggregate(new StringBuilder(), (sb, b) => sb.AppendFormat("{0:x2}", b), sb => sb.ToString());

            return hex;
        }


        public static string GetBase64Hmac(this string input, string key)
        {
            var hmacKey = Convert.FromBase64String(key);

            var hex = new HMACSHA256(hmacKey).ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hex);
        }

#endif

    }
}
