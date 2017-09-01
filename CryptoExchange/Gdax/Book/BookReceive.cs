using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax.Book
{
    /// <summary>
    /// A valid order has been received and is now active. This message is emitted for every single valid order as soon as the matching engine receives 
    /// it whether it fills immediately or not.
    /// </summary>
    public class BookReceive : BookOrder
    {
        public override BookType Type => BookType.Received;

        [JsonProperty("order_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType OrderType { get; set; }

        [JsonProperty("size")]
        public decimal Size { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("client_oid")]
        public Guid? ClientOid { get; set; }

        public BookReceive()
        {            
        }

        internal override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write((byte)OrderType);
            writer.Write(Size);
            writer.Write(Price);
        }


        internal BookReceive(BinaryReader reader)
            : base(reader)
        {
            OrderType = (OrderType) reader.ReadByte();
            Size = reader.ReadDecimal();
            Price = reader.ReadDecimal();
        }

        public override string ToString()
        {
            return $"{Time} : Type={Type}, Side={Side}, Price={Price:F2}, Size={Size}, Order={OrderType}, Id={OrderId}";
        }

    }
}