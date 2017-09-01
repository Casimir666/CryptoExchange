using System.IO;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax.Book
{
    public class BookChange : BookOrder
    {
        public override BookType Type => BookType.Change;

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("old_size")]
        public decimal OldSize { get; set; }

        [JsonProperty("new_size")]
        public decimal NewSize { get; set; }


        public BookChange()
        {            
        }

        internal override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Price);
            writer.Write(OldSize);
            writer.Write(NewSize);
        }

        internal BookChange(BinaryReader reader)
            : base(reader)
        {
            Price = reader.ReadDecimal();
            OldSize = reader.ReadDecimal();
            NewSize = reader.ReadDecimal();
        }

        public override string ToString()
        {
            return $"{Time} : Type={Type}, Side={Side}, Price={Price:F2}, Old Size={OldSize}, New Size={NewSize}, Id={OrderId}";
        }
    }
}
