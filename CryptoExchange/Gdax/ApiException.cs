using System;

namespace CryptoExchange.Gdax
{
    public enum ApiError
    {
        Unknown,
        InvalidApiKey,

        /// <summary>
        /// User hasn’t authenticated necessary scope
        /// </summary>
        InvalidScope,

        Forbidden,

        RateLimitExceeded,

        /// <summary>
        /// This order is already cancelled or done
        /// </summary>
        OrderAlreadyDone
    }

    [Serializable]
    public class ApiException : Exception
    {
        public ApiException()
        {            
        }

        public ApiException(string message)
            : base (message)
        {            
        }

        public ApiException(string message, Exception innerException)
            : base(message, innerException)
        {            
        }

        public ApiError ErrorCode
        {
            get
            {
                if (Message == "Invalid API Key")
                    return ApiError.InvalidApiKey;

                if (Message == "Invalid scope")
                    return ApiError.InvalidScope;

                if (Message == "Forbidden")
                    return ApiError.Forbidden;

                if (Message == "Rate limit exceeded")
                    return ApiError.RateLimitExceeded;

                if (Message == "Order already done")
                    return ApiError.OrderAlreadyDone;
                return ApiError.Unknown;
            }
        }
    }
}
