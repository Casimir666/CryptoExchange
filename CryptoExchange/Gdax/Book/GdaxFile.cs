using System;
using System.IO;
using System.Text;
using CryptoExchange.Common;

namespace CryptoExchange.Gdax.Book
{
    public class GdaxFile : IDisposable
    {
        private BinaryWriter _writer;
        private BinaryReader _reader;

        public static GdaxFile OpenRead(Stream stream, bool leaveOpen)
        {
            return new GdaxFile(stream, true, leaveOpen);
        }

        public static GdaxFile OpenRead(string fullpath)
        {
            return new GdaxFile(fullpath, true);
        }

        public static string GetFilename(DateTime date, int granularity)
        {
            switch (granularity)
            {
                case 60:
                    return $"GDAX Book order {date:yyyy-MM-dd}.gdax";
                case 3600:
                    return $"GDAX Book order {date:yyyy-MM}.gdax";
                case 86400:
                    return $"GDAX Book order {date:yyyy-MM}.gdax";
                default:
                    throw new ArgumentOutOfRangeException(nameof(granularity));
            }
        }

        public static string GetRelativePath(ProductType productType, long granularity)
        {
            var granularityFolder = string.Empty;
            switch (granularity)
            {
                case 60:
                    granularityFolder = "minute";
                    break;
                case 3600:
                    granularityFolder = "hour";
                    break;
                case 86400:
                    granularityFolder = "daily";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(granularity));
            }

            return Path.Combine(granularityFolder, productType.ToString().ToLower());
        }

        public static GdaxFile OpenWrite(Stream stream, bool leaveOpen)
        {
            return new GdaxFile(stream, false, leaveOpen);
        }

        public static GdaxFile OpenWrite(string fullpath)
        {
            return new GdaxFile(fullpath, false);
        }

        GdaxFile(Stream stream, bool read, bool leaveOpen)
        {
            Open(stream, read, leaveOpen);
        }

        GdaxFile(string fullpath, bool read)
        {
            var stream = read
                ? File.Open(fullpath, FileMode.Open, FileAccess.Read, FileShare.Read)
                : File.Open(fullpath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            Open(stream, read, false);
        }

        private void Open(Stream stream, bool read, bool leaveOpen)
        {
            if (read)
            {
                _reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen);
                CheckHeader(stream);
            }
            else
            {
                _writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen);
                if (stream.Length > 0)
                {
                    CheckHeader(stream);
                    stream.Seek(0, SeekOrigin.End);
                }
            }
        }

        private void CheckHeader(Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                var header = reader.ReadString();
                var version = reader.ReadByte();
                var startTime = reader.ReadInt64();

                if (header != Constants.GdaxFile.Header || version != Constants.GdaxFile.Version)
                    throw new FormatException("Invalid Gdax file format");

                StartDate = new DateTime(startTime);
            }
        }

        private void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Constants.GdaxFile.Header);
            writer.Write(Constants.GdaxFile.Version);
            writer.Write(StartDate?.Ticks ?? 0);
        }


        public void Dispose()
        {
            _writer?.Dispose();
            _reader?.Dispose();
        }

        public DateTime? StartDate { get; private set; }

        public void Serialize(BookBase bookItem)
        {
            if (_writer.BaseStream.Position == 0)
            {
                StartDate = bookItem.Time;
                WriteHeader(_writer);
            }

            bookItem.Serialize(_writer);
            _writer.Flush();
        }

        public void Serialize(Candle candle)
        {
            if (_writer.BaseStream.Position == 0)
            {
                StartDate = candle.Time;
                WriteHeader(_writer);
            }

            _writer.Write(Constants.GdaxFile.CandleType);
            candle.Serialize(_writer);
            _writer.Flush();
        }

        public void Serialize(ProductBook book, DateTime startDate)
        {
            if (_writer.BaseStream.Position == 0)
            {
                StartDate = startDate;
                WriteHeader(_writer);
            }

            _writer.Write(Constants.GdaxFile.OrderBookType);
            book.Serialize(_writer);
            _writer.Flush();
        }

        public object Deserialize()
        {
            switch (_reader.ReadByte())
            {
                case (byte)BookType.Received:
                    return new BookReceive(_reader);

                case (byte)BookType.Open:
                    return new BookOpen(_reader);

                case (byte)BookType.Done:
                    return new BookDone(_reader);

                case (byte)BookType.Match:
                    return new BookMatch(_reader);

                case (byte)BookType.Change:
                    return new BookChange(_reader);

                case (byte)BookType.Heartbeat:
                    return new BookHeartbeat(_reader);

                case Constants.GdaxFile.CandleType:
                    return new Candle(_reader);

                case Constants.GdaxFile.OrderBookType:
                    return new ProductBook(_reader);
            }

            throw new FormatException("Invalid Gdax file format");
        }


        public Candle DeserializeCandle()
        {
            if (_reader.ReadByte() != Constants.GdaxFile.CandleType)
                throw new FormatException("Invalid Gdax file format");

            return new Candle(_reader);
        }

        public ProductBook DeserializeProductBook()
        {
            if (_reader.ReadByte() != Constants.GdaxFile.OrderBookType)
                throw new FormatException("Invalid Gdax file format");

            return new ProductBook(_reader);
        }
    }
}
