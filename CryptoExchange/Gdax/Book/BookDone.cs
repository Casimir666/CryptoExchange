using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Gdax.Book
{
    /// <summary>
    /// The order is no longer on the order book. Sent for all orders for which there was a received message. This message can result from an order
    /// being canceled or filled. There will be no more messages for this order_id after a done message. remaining_size indicates how much of the order went
    /// unfilled; this will be 0 for filled orders.
    /// 
    /// Market orders will not have a remaining_size or price field as they are never on the open order book at a given price.
    /// </summary>
    public class BookDone : BookOrder
    {
        public override BookType Type => BookType.Done;

        [JsonProperty("reason")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderReason Reason { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("remaining_size")]
        public decimal RemainingSize { get; set; }

        public BookDone()
        {            
        }

        internal override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write((byte)Reason);
            writer.Write(Price);
            writer.Write(RemainingSize);
        }

        internal BookDone(BinaryReader reader)
            : base(reader)
        {
            Reason = (OrderReason) reader.ReadByte();
            Price = reader.ReadDecimal();
            RemainingSize = reader.ReadDecimal();
        }

        public override string ToString()
        {
            return $"{Time} : Type={Type}, Side={Side}, Price={Price:F2}, Reason={Reason}, Id={OrderId}";
        }

    }
}