using System;
using System.IO;
using CryptoExchange.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax.Book
{
    public abstract class BookOrder : BookBase
    {
        [JsonProperty("order_id")]
        public Guid OrderId { get; set; }

        [JsonProperty("side")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SideType Side { get; set; }

        protected BookOrder()
        {            
        }

        internal override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(OrderId);
            writer.Write((byte)Side);
        }

        internal BookOrder(BinaryReader reader)
            : base(reader)
        {
            OrderId = reader.ReadGuid();
            Side = (SideType)reader.ReadByte();
        }

        public override string ToString()
        {
            return $"{Time} : Type={Type}, Side={Side}";
        }

    }
}
