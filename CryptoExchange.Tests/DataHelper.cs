using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CoinbaseApi.Tests
{
    static class DataHelper
    {

        public static T LoadData<T>(string resourceName)
        {
            var assembly = typeof(DataHelper).Assembly;
            var fullname = assembly.GetManifestResourceNames().FirstOrDefault(f => f.Contains(resourceName));
            using (var stream = typeof(DataHelper).Assembly.GetManifestResourceStream(fullname))
            using (var reader = new StreamReader(stream))
            {
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
        }
    }
}
