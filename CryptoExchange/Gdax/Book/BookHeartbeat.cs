using System.IO;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax.Book
{
    public class BookHeartbeat : BookBase
    {
        public override BookType Type => BookType.Heartbeat;

        [JsonProperty("last_trade_id")]
        public int LastTradeId { get; set; }

        public BookHeartbeat()
        {            
        }

        internal override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(LastTradeId);
        }

        internal BookHeartbeat(BinaryReader reader)
            : base(reader)
        {
            LastTradeId = reader.ReadInt32();
        }

    }
}
