using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CryptoExchange.Gdax
{
    class ProductOfferComparer : IComparer<ProductOffer>
    {
        private readonly ListSortDirection _sortDirection;

        public ProductOfferComparer(ListSortDirection sortDirection)
        {
            _sortDirection = sortDirection;
        }

        public int Compare(ProductOffer x, ProductOffer y)
        {
            switch (_sortDirection)
            {
                case ListSortDirection.Ascending:
                    if (ReferenceEquals(x, null))
                        return -1;
                    int ascResult = x.Price.CompareTo(y?.Price);
                    if (ascResult == 0)
                        ascResult = x.OrderId.CompareTo(y?.OrderId);
                    return ascResult;

                case ListSortDirection.Descending:
                    if (ReferenceEquals(y, null))
                        return -1;
                    int descResult = y.Price.CompareTo(x?.Price);
                    if (descResult == 0)
                        descResult = y.OrderId.CompareTo(x?.OrderId);
                    return descResult;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}