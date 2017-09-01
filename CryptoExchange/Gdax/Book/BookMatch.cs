using System;
using System.IO;
using CryptoExchange.Common;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax.Book
{
    /// <summary>
    /// A trade occurred between two orders. The aggressor or taker order is the one executing immediately after being received 
    /// and the maker order is a resting order on the book. The side field indicates the maker order side. If the side is sell this 
    /// indicates the maker was a sell order and the match is considered an up-tick. A buy side match is a down-tick.
    /// </summary>
    public class BookMatch : BookOrder
    {
        public override BookType Type => BookType.Match;

        [JsonProperty("trade_id")]
        public int TradeId { get; set; }

        [JsonProperty("maker_order_id")]
        public Guid MakerOrderId { get; set; }

        [JsonProperty("taker_order_id")]
        public Guid TakerOrderId { get; set; }

        [JsonProperty("size")]
        public decimal Size { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }


        #region // If authenticated, and you were the taker, the message would also have the following fields:
        [JsonProperty("taker_user_id")]
        public string TakerUserId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("taker_profile_id")]
        public string TakerProfileId { get; set; }

        [JsonProperty("profile_id")]
        public string ProfileId { get; set; }

        #endregion


        public override string ToString()
        {
            return $"{Time} : Type={Type}, Side={Side}, Price={Price:F2}, Maker={MakerOrderId}, Taker={TakerOrderId}";
        }


        public BookMatch()
        {            
        }

        internal override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(TradeId);
            writer.Write(MakerOrderId);
            writer.Write(TakerOrderId);
            writer.Write(Size);
            writer.Write(Price);
        }

        internal BookMatch(BinaryReader reader)
            : base(reader)
        {
            TradeId = reader.ReadInt32();
            MakerOrderId = reader.ReadGuid();
            TakerOrderId = reader.ReadGuid();
            Size = reader.ReadDecimal();
            Price = reader.ReadDecimal();
        }
    }
}