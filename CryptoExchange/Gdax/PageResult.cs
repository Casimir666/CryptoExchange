namespace CryptoExchange.Gdax
{
    public class PageResult<T>
    {
        /// <summary>
        /// Data result for a page
        /// </summary>
        public T Result { get; }

        /// <summary>
        /// Page index to get newer information
        /// </summary>
        public long? IndexBefore { get; }

        /// <summary>
        /// Page index to get older information
        /// </summary>
        public long? IndexAfter { get; }

        internal PageResult(T result, long? indexBefore, long? indexAfter)
        {
            Result = result;
            IndexBefore = indexBefore;
            IndexAfter = indexAfter;
        }
    }
}
