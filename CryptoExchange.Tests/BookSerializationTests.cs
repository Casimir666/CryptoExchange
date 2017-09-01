using System;
using System.IO;
using System.Linq;
using CryptoExchange.Gdax;
using CryptoExchange.Gdax.Book;
using FluentAssertions;
using NUnit.Framework;

namespace CryptoExchange.Tests
{
    [TestFixture]
    public class BookSerializationTests
    {
        private Random _random = new Random();

        [Test]
        public void BookChangeSerialize()
        {
            var saved = (BookChange)CreateBook(BookType.Change);

            using (var memory = new MemoryStream())
            {
                using (var serial = GdaxFile.OpenWrite(memory, true))
                {
                    serial.Serialize(saved);
                }

                memory.Seek(0, SeekOrigin.Begin);
                using (var serial = GdaxFile.OpenRead(memory, true))
                { 
                    var read = serial.Deserialize() as BookChange;

                    AssertBase(saved, read);
                    Assert.AreEqual(saved.Price, read.Price);
                    Assert.AreEqual(saved.OldSize, read.OldSize);
                    Assert.AreEqual(saved.NewSize, read.NewSize);
                }
            }
        }

        [Test]
        public void BookDoneSerialize()
        {
            var saved = (BookDone)CreateBook(BookType.Done);

            using (var memory = new MemoryStream())
            {
                using (var serial = GdaxFile.OpenWrite(memory, true))
                {
                    serial.Serialize(saved);
                }

                memory.Seek(0, SeekOrigin.Begin);
                using (var serial = GdaxFile.OpenRead(memory, true))
                {
                    var read = serial.Deserialize() as BookDone;

                    AssertBase(saved, read);
                    Assert.AreEqual(saved.Price, read.Price);
                    Assert.AreEqual(saved.Reason, read.Reason);
                    Assert.AreEqual(saved.RemainingSize, read.RemainingSize);
                }
            }
        }

        [Test]
        public void BookMatchSerialize()
        {
            var saved = (BookMatch)CreateBook(BookType.Match);

            using (var memory = new MemoryStream())
            {
                using (var serial = GdaxFile.OpenWrite(memory, true))
                {
                    serial.Serialize(saved);
                }

                memory.Seek(0, SeekOrigin.Begin);
                using (var serial = GdaxFile.OpenRead(memory, true))
                {
                    var read = serial.Deserialize() as BookMatch;

                    AssertBase(saved, read);
                    Assert.AreEqual(saved.Price, read.Price);
                    Assert.AreEqual(saved.TradeId, read.TradeId);
                    Assert.AreEqual(saved.MakerOrderId, read.MakerOrderId);
                    Assert.AreEqual(saved.TakerOrderId, read.TakerOrderId);
                    Assert.AreEqual(saved.Size, read.Size);
                }
            }
        }

        [Test]
        public void BookOpenSerialize()
        {
            var saved = (BookOpen) CreateBook(BookType.Open);

            using (var memory = new MemoryStream())
            {
                using (var serial = GdaxFile.OpenWrite(memory, true))
                {
                    serial.Serialize(saved);
                }

                memory.Seek(0, SeekOrigin.Begin);
                using (var serial = GdaxFile.OpenRead(memory, true))
                {
                    var read = serial.Deserialize() as BookOpen;

                    AssertBase(saved, read);
                    Assert.AreEqual(saved.Price, read.Price);
                    Assert.AreEqual(saved.RemainingSize, read.RemainingSize);
                }
            }
        }

        [Test]
        public void BookReceiveSerialize()
        {
            var saved = (BookReceive) CreateBook(BookType.Received);

            using (var memory = new MemoryStream())
            {
                using (var serial = GdaxFile.OpenWrite(memory, true))
                {
                    serial.Serialize(saved);
                }

                memory.Seek(0, SeekOrigin.Begin);
                using (var serial = GdaxFile.OpenRead(memory, true))
                {
                    var read = serial.Deserialize() as BookReceive;

                    AssertBase(saved, read);
                    Assert.AreEqual(saved.Price, read.Price);
                    Assert.AreEqual(saved.OrderType, read.OrderType);
                    Assert.AreEqual(saved.Size, read.Size);
                }
            }
        }

        [TestCase(typeof(BookChange))]
        [TestCase(typeof(BookDone))]
        [TestCase(typeof(BookMatch))]
        [TestCase(typeof(BookReceive))]
        public void SerializeEmptyObject(Type objectType)
        {
            var saved = (BookBase)Activator.CreateInstance(objectType);
            using (var memory = new MemoryStream())
            {
                using (var serial = GdaxFile.OpenWrite(memory, true))
                {
                    serial.Serialize(saved);
                }

                memory.Seek(0, SeekOrigin.Begin);
                using (var serial = GdaxFile.OpenRead(memory, true))
                {
                    var read = serial.Deserialize();
                    AssertBase(saved, read as BookBase);
                }
            }
        }

        [Test]
        public void SerializeCandle()
        {
            var saved = new Candle(new DateTime(2017, 5, 20, 0, 0, 0, DateTimeKind.Utc), 42, 84, 120.54M, 654.235M, 452.125M);
            using (var memory = new MemoryStream())
            {
                using (var serial = GdaxFile.OpenWrite(memory, true))
                {
                    serial.Serialize(saved);
                }

                memory.Seek(0, SeekOrigin.Begin);
                using (var serial = GdaxFile.OpenRead(memory, true))
                {
                    var read = serial.DeserializeCandle();
                    Assert.AreEqual(saved, read);
                }
            }
        }

        [Test]
        public void SerializeProductBook()
        {
            var product = new ProductBook(42);
            product.Buys.Add(new ProductOffer {OrderId = Guid.NewGuid(), Price = 4210.2m, Size = 0.004m});
            product.Sells.Add(new ProductOffer { OrderId = Guid.NewGuid(), Price = 3852.12m, Size = 2.56m });
            product.Sells.Add(new ProductOffer { OrderId = Guid.NewGuid(), Price = 3642.12m, Size = 7.12m });

            using (var memory = new MemoryStream())
            {
                using (var serial = GdaxFile.OpenWrite(memory, true))
                {
                    serial.Serialize(product, DateTime.UtcNow);
                }

                memory.Seek(0, SeekOrigin.Begin);
                using (var serial = GdaxFile.OpenRead(memory, true))
                {
                    var read = serial.DeserializeProductBook();

                    product.ShouldBeEquivalentTo(read);
                }
            }
        }

        private void AssertBase(BookBase saved, BookBase read)
        {
            Assert.IsNotNull(read);
            Assert.AreEqual(saved.Type, read.Type);
            Assert.AreEqual(saved.Time, read.Time);
            Assert.AreEqual(saved.Sequence, read.Sequence);
            Assert.AreEqual(saved.ProductType, read.ProductType);
            if (saved is BookOrder)
            {
                Assert.AreEqual(((BookOrder)saved).OrderId, ((BookOrder)read).OrderId);
                Assert.AreEqual(((BookOrder)saved).Side, ((BookOrder)read).Side);
            }
        }

        private BookBase CreateBook(BookType type)
        {
            switch(type)
            {
                case BookType.Received:
                    return new BookReceive
                    {
                        Time = DateTime.UtcNow,
                        Sequence = _random.Next(),
                        ProductType = ProductType.ETH_USD,
                        OrderId = Guid.NewGuid(),
                        Side = SideType.Buy,
                        Price = (decimal)_random.NextDouble(),
                        OrderType = OrderType.Stop,
                        Size = (decimal)_random.NextDouble()
                    };
                case BookType.Open:
                    return new BookOpen
                    {
                        Time = DateTime.UtcNow,
                        Sequence = _random.Next(),
                        ProductType = ProductType.LTC_USD,
                        OrderId = Guid.NewGuid(),
                        Side = SideType.Buy,
                        Price = (decimal)_random.NextDouble(),
                        RemainingSize = (decimal)_random.NextDouble()
                    };
                case BookType.Done:
                    return new BookDone
                    {
                        Time = DateTime.UtcNow,
                        Sequence = _random.Next(),
                        ProductType = ProductType.LTC_BTC,
                        OrderId = Guid.NewGuid(),
                        Side = SideType.Buy,
                        Price = (decimal)_random.NextDouble(),
                        Reason = OrderReason.Filled,
                        RemainingSize = (decimal)_random.NextDouble()
                    };
                case BookType.Match:
                    return new BookMatch
                    {
                        Time = DateTime.UtcNow,
                        Sequence = _random.Next(),
                        ProductType = ProductType.ETH_USD,
                        OrderId = Guid.NewGuid(),
                        Side = SideType.Buy,
                        Price = (decimal)_random.NextDouble(),
                        TradeId = _random.Next(),
                        MakerOrderId = Guid.NewGuid(),
                        TakerOrderId = Guid.NewGuid(),
                        Size = (decimal)_random.NextDouble(),
                    };
                case BookType.Change:
                    return new BookChange
                    {
                        Time = DateTime.UtcNow,
                        Sequence = _random.Next(),
                        ProductType = ProductType.BTC_EUR,
                        OrderId = Guid.NewGuid(),
                        Side = SideType.Buy,
                        Price = (decimal)_random.NextDouble(),
                        NewSize = _random.Next(),
                        OldSize = _random.Next()
                    };
            }
            return null;
        }
    }
}
