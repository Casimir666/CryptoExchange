using System;
using System.IO;
using CryptoExchange.Common;

namespace CryptoExchange.Gdax
{
    public class ProductOffer
    {
        public ProductOffer()
        {            
        }

        public ProductOffer(BinaryReader reader)
        {
            Price = reader.ReadDecimal();
            Size = reader.ReadDecimal();
            OrderId = reader.ReadGuid();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Price);
            writer.Write(Size);
            writer.Write(OrderId);
        }

        public decimal Price { get; set; }

        public decimal Size { get; set; }

        public Guid OrderId { get; set; }

        public override string ToString()
        {
            return $"Price={Price}, Size={Size}";
        }
    }
}