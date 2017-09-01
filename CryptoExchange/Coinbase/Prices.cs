using CryptoExchange.Common;

namespace CryptoExchange.Coinbase
{
    public class Prices
    {
        public string From { get; set; }

        public string To { get; set; }

        /// <summary>
        /// Get the total price to sell one bitcoin or ether.
        /// 
        /// Note that exchange rates fluctuates so the price is only correct for seconds at the time.This sell price includes standard 
        /// Coinbase fee (1%) but excludes any other fees including bank fees
        /// </summary>
        public Balance Sell { get; set; }

        /// <summary>
        /// Get the total price to buy one bitcoin or ether.
        /// 
        /// Note that exchange rates fluctuates so the price is only correct for seconds at the time.This buy price includes standard
        /// Coinbase fee (1%) but excludes any other fees including bank fees.
        /// </summary>
        public Balance Buy { get; set; }

        /// <summary>
        /// Get the current market price for bitcoin. This is usually somewhere in between the buy and sell price.
        /// </summary>
        public Balance Spot { get; set; }
    }
}
