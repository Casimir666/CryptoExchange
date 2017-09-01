using System;
using System.IO;
using System.Threading.Tasks;
using CryptoExchange;
using CryptoExchange.Gdax;
using CryptoExchange.Gdax.Book;

namespace CoinbaseSample
{
    class WebStreamDumper
    {
        private readonly IGdaxClient _gdaxClient;
        private readonly ProductType _productType;
        private readonly string _rootPath;

        private ProductBook _productBook;

        private string _filename;
        private GdaxFile _gdaxFile;
        private DateTime _fileDate;

        public WebStreamDumper(IGdaxClient gdaxClient, ProductType productType, string rootPath)
        {
            _gdaxClient = gdaxClient;
            _productType = productType;
            _rootPath = rootPath;
        }

        public async Task Run()
        {
            await _gdaxClient.RegisterLiveFeeds(new[] {_productType}, DoReceiveBook, DoReceiveError);

            await Task.Factory.StartNew(() =>
            {
                while (Console.ReadKey().Key != ConsoleKey.Q) ;
            });
        }

        public async Task Stop()
        {
            await _gdaxClient.UnregisterLiveFeeds();
            Program.Log.Info($"Stop dumping events to {_filename}");
            ClosePreviousFile();
        }

        private void DoReceiveError(string message, Exception exception)
        {
            Program.Log.Error(message);
        }

        private async Task DoReceiveBook(BookBase bookBase)
        {
            if (_gdaxFile == null || DateTime.Today != _fileDate)
                OpenFile();

            if (_productBook == null)
            {
                _productBook = await _gdaxClient.GetProductBook(_productType, BookLevel.Full);
                _gdaxFile.Serialize(_productBook, DateTime.UtcNow);
                Program.Log.Info("Product book loaded.");
            }

            if (_productBook.Update(bookBase))
            {
                _gdaxFile.Serialize(bookBase);
            }
            else
            {
                _productBook = null;
                Program.Log.Error("Some event are missing, book order will be fully reloaded.");
            }
        }


        private void OpenFile()
        {
            ClosePreviousFile();

            _fileDate = DateTime.Today;

            _filename = Path.Combine(_rootPath, GdaxFile.GetFilename(_fileDate, 60));
            _gdaxFile = GdaxFile.OpenWrite(_filename);
            Program.Log.Info($"File '{_filename}' is open");

            if (_productBook != null)
            {
                _gdaxFile.Serialize(_productBook, DateTime.UtcNow);
                Program.Log.Info("Current product book added to file");
            }
        }

        private void ClosePreviousFile()
        {
            if (_gdaxFile != null)
            {
                _gdaxFile?.Dispose();
                _gdaxFile = null;
                Program.Log.Info($"File '{_filename}' is closed");
            }
        }
    }
}
