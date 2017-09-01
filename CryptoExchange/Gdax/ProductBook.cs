using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CryptoExchange.Gdax.Book;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax
{
    public class ProductBook
    {
        // Received orders cannot be put in order book immediatly, because it can be filled
        // immediatly (no OpenOrder event in this case)
        private readonly List<BookReceive> _orderReceived = new List<BookReceive>();

        [JsonProperty("sequence")]
        public long Sequence { get; private set; }

        /// <summary>
        /// List of buy orders (Bids)
        /// </summary>
        [JsonProperty("bids")]
        [JsonConverter(typeof(ProductOfferSerializer), ListSortDirection.Descending)]
        public SortedSet<ProductOffer> Buys { get; private set; }

        /// <summary>
        /// List of sells orders (Asks)
        /// </summary>
        [JsonProperty("asks")]
        [JsonConverter(typeof(ProductOfferSerializer), ListSortDirection.Ascending)]
        public SortedSet<ProductOffer> Sells { get; private set; }

        public ProductBook()
        {            
        }

        public ProductBook(BinaryReader reader)
        {
            Sequence = reader.ReadInt64();
            Sells = new SortedSet<ProductOffer>(new ProductOfferComparer(ListSortDirection.Ascending));
            Buys = new SortedSet<ProductOffer>(new ProductOfferComparer(ListSortDirection.Descending));

            var buyCount = reader.ReadInt32();
            for (int i = 0; i < buyCount; i++)
            {
                Buys.Add(new ProductOffer(reader));
            }

            var sellCount = reader.ReadInt32();
            for (int i = 0; i < sellCount; i++)
            {
                Sells.Add(new ProductOffer(reader));
            }
        }

        public ProductBook(long sequence)
        {
            Sequence = sequence;
            Sells = new SortedSet<ProductOffer>(new ProductOfferComparer(ListSortDirection.Ascending));
            Buys = new SortedSet<ProductOffer>(new ProductOfferComparer(ListSortDirection.Descending));
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Sequence);

            writer.Write(Buys.Count);
            foreach (var buy in Buys)
            {
                buy.Serialize(writer);
            }

            writer.Write(Sells.Count);
            foreach (var sell in Sells)
            {
                sell.Serialize(writer);
            }
        }


        public bool Update(BookBase book)
        {
            // Ignore oldest sequences
            if (book.Sequence <= Sequence)
                return true;

            var result = book.Sequence == Sequence + 1;
            if (!result)
            {
                Api.Log.Warning($"Missing book sequence, expected {Sequence + 1} but receive {book.Sequence}");
            }

            Sequence = book.Sequence;

            switch (book.Type)
            {
                case BookType.Received:
                    ReceiveOrder(book as BookReceive);
                    break;
                case BookType.Open:
                    OpenOrder(book as BookOpen);
                    break;
                case BookType.Done:
                    RemoveOrder(book as BookDone);
                    break;
                case BookType.Match:
                    // TODO : Update order remaining size ???
                    break;
                case BookType.Change:
                    UpdateOrder(book as BookChange);
                    break;
            }

            return result;
        }

        public decimal Spread
        {
            get
            {
                if (Sells.Count == 0 || Buys.Count == 0) return 0;
                return Sells.First().Price - Buys.First().Price;
            }
        }

        public ProductOffer BestBuy => Buys.FirstOrDefault();

        public ProductOffer BestSell => Sells.FirstOrDefault();

        private void ReceiveOrder(BookReceive receiveOrder)
        {
            _orderReceived.Add(receiveOrder);
        }

        private void OpenOrder(BookOpen openOrder)
        {
            var offer = new ProductOffer
            {
                OrderId = openOrder.OrderId,
                Price = openOrder.Price,
                Size = openOrder.RemainingSize
            };

            if (openOrder.Side == SideType.Buy)
                Buys.Add(offer);
            else
                Sells.Add(offer);

            if (!RemoveReceiveOrder(openOrder.OrderId))
                Api.Log.Warning($"No receive message for order id {openOrder.OrderId}");
        }

        private bool RemoveReceiveOrder(Guid orderId)
        {
            var order = _orderReceived.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                _orderReceived.Remove(order);
            }
            return order != null;
        }

        private void RemoveOrder(BookDone removeOrder)
        {
            if (removeOrder.Side == SideType.Buy)
            {
                var offer = Buys.FirstOrDefault(a => a.OrderId == removeOrder.OrderId);
                if (offer != null)
                    Buys.Remove(offer);
                else if (!RemoveReceiveOrder(removeOrder.OrderId))
                    Api.Log.Warning($"Order not found for {removeOrder.Reason} buy offer : {removeOrder.OrderId}");
            }
            else
            {
                var offer = Sells.FirstOrDefault(b => b.OrderId == removeOrder.OrderId);
                if (offer != null)
                    Sells.Remove(offer);
                else if (!RemoveReceiveOrder(removeOrder.OrderId))
                    Api.Log.Warning($"Order not found for {removeOrder.Reason} sell offer : {removeOrder.OrderId}");
            }
        }

        private void UpdateOrder(BookChange changeOrder)
        {
            var offer = changeOrder.Side == SideType.Buy
                ? Buys.FirstOrDefault(a => a.OrderId == changeOrder.OrderId)
                : Sells.FirstOrDefault(b => b.OrderId == changeOrder.OrderId);

            if (offer != null)
            {
                offer.Price = changeOrder.Price;
                offer.Size = changeOrder.NewSize;
            }
            else
            {
                Api.Log.Warning("Change receive for unknown order id");
                OpenOrder(new BookOpen { OrderId = changeOrder.OrderId, Side = changeOrder.Side, Price = changeOrder.Price, RemainingSize = changeOrder.NewSize});
            }
        }
    }
}
