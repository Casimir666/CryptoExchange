using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax.Book
{
    public abstract class BookBase : ITimeStamped
    {

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public abstract BookType Type { get; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }

        [JsonProperty("product_id")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType ProductType { get; set; }

        protected BookBase()
        {
        }

        internal virtual void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Type);
            writer.Write(Time.Ticks);
            writer.Write(Sequence);
            writer.Write((byte)ProductType);
        }

        internal BookBase(BinaryReader reader)
        {
            Time = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
            Sequence = reader.ReadInt64();
            ProductType = (ProductType)reader.ReadByte();
        }

        protected string ReadString(BinaryReader reader)
        {
            var value = reader.ReadString();
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}