using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

namespace CryptoExchange.Gdax
{
    /// <summary>
    /// Custom Json converter for request /products/book. Json for level 3 is :
    /// 
    /// {
    ///     "sequence": 1951509167,
    ///     "bids": [
    ///         [ "2524.22", "0.30975668", "4fefa7ce-6d67-429d-a10f-27f9dad04c44" ],
    ///         ...
    /// </summary>
    class ProductOfferSerializer : JsonConverter
    {
        private readonly ListSortDirection _sortDirection;

        public ProductOfferSerializer(ListSortDirection sortDirection)
        {
            _sortDirection = sortDirection;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new SortedSet<ProductOffer>(new ProductOfferComparer(_sortDirection));
            foreach (var offer in serializer.Deserialize<List<string[]>>(reader))
            {
                var productOffer = new ProductOffer
                {
                    Price = decimal.Parse(offer[0], System.Globalization.CultureInfo.InvariantCulture),
                    Size = decimal.Parse(offer[1], System.Globalization.CultureInfo.InvariantCulture)
                };

                Guid orderId;
                if (Guid.TryParse(offer[2], out orderId))
                    productOffer.OrderId = orderId;

                result.Add(productOffer);
            }
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(List<ProductOffer>).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }
    }
}