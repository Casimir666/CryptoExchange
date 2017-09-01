using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CryptoExchange.Common;
using CryptoExchange.Gdax.Book;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax
{
    public class Candle : ITimeStamped, ICloneable
    {
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime Time { get; set; }

        public decimal Low { get; set; }

        public decimal High { get; set; }

        public decimal Open { get; set; }

        public decimal Close { get; set; }

        public decimal Volume { get; set; }

        public decimal AvrPrice => Math.Round ((Open + Close) / 2, 2);

        [JsonConstructor]
        public Candle(DateTime time, decimal low, decimal high, decimal open, decimal close, decimal volume)
        {
            Time = time;
            Low = low;
            High = high;
            Open = open;
            Close = close;
            Volume = volume;
        }

        public Candle(long timeEpoch, decimal low, decimal high, decimal open, decimal close, decimal volume)
        {
            Time = EpochConverter.DateTimeFromUnixTimestampSeconds(timeEpoch);
            Low = low;
            High = high;
            Open = open;
            Close = close;
            Volume = volume;
        }

        public override string ToString()
        {
            return $"{Time} : L={Low}, H={High}, Avr={AvrPrice}";
        }

        public static List<Candle> Parse(string content)
        {
            var result = new List<Candle>();
            var startIndex = 2;
            var endIndex = 0;
            var last = false;
            do
            {
                endIndex = content.IndexOf("],[", startIndex);
                if (endIndex == -1)
                {
                    endIndex = content.Length - 2;
                    last = true;

                    if (startIndex > endIndex) break;
                }
                var value = content.Substring(startIndex, endIndex - startIndex);
                var fields = value.Split(',');
                if (fields.Length == 6)
                    result.Add(new Candle(long.Parse(fields[0]),
                                          ParseDecimal(fields[1]),
                                          ParseDecimal(fields[2]),
                                          ParseDecimal(fields[3]),
                                          ParseDecimal(fields[4]),
                                          ParseDecimal(fields[5])));

                startIndex = endIndex + 3;
            } while (!last);

            return result.OrderBy(c => c.Time).ToList();
        }

        private static decimal ParseDecimal(string value)
        {
            decimal result;

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return result;

            return (decimal) double.Parse(value, CultureInfo.InvariantCulture);
        }

        public override bool Equals(object obj)
        {
            var candle2 = obj as Candle;

            return candle2 != null &&
                   Time.Equals(candle2.Time) &&
                   Open.Equals(candle2.Open) &&
                   Close == candle2.Close &&
                   High == candle2.High &&
                   Low == candle2.Low &&
                   Math.Abs(Volume - candle2.Volume) < Constants.Epsilon;
        }

        public override int GetHashCode()
        {
            return Time.GetHashCode() ^
                   Open.GetHashCode() ^
                   Close.GetHashCode() ^
                   High.GetHashCode() ^
                   Low.GetHashCode() ^
                   Volume.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Use only for tests and simulation !!
        /// </summary>
        public BookMatch ToBookMatch(ProductType productType)
        {
            return new BookMatch
            {
                Time = Time,
                Price = AvrPrice,
                ProductType = productType
            };
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Time.Ticks);
            writer.Write(Low);
            writer.Write(High);
            writer.Write(Open);
            writer.Write(Close);
            writer.Write(Volume);
        }

        internal Candle(BinaryReader reader)
        {
            Time = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
            Low = reader.ReadDecimal();
            High = reader.ReadDecimal();
            Open = reader.ReadDecimal();
            Close = reader.ReadDecimal();
            Volume = reader.ReadDecimal();
        }
    }
}
