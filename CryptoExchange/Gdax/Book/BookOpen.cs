using System.IO;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax.Book
{
    /// <summary>
    /// The order is now open on the order book. This message will only be sent for orders which are not fully filled immediately. remaining_size will 
    /// indicate how much of the order is unfilled and going on the book
    /// </summary>
    public class BookOpen : BookOrder
    {
        public override BookType Type => BookType.Open;

        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Indicate how much of the order is unfilled and going on the book
        /// </summary>
        [JsonProperty("remaining_size")]
        public decimal RemainingSize { get; set; }

        public BookOpen()
        {            
        }


        internal override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Price);
            writer.Write(RemainingSize);
        }


        internal BookOpen(BinaryReader reader)
            : base(reader)
        {
            Price = reader.ReadDecimal();
            RemainingSize = reader.ReadDecimal();
        }

        public override string ToString()
        {
            return $"{Time} : Type={Type}, Side={Side}, Price={Price:F2}, Size={RemainingSize}, Id={OrderId}";
        }
    }
}