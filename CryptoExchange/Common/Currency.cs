using System.Runtime.Serialization;

namespace CryptoExchange.Common
{
    // ReSharper disable InconsistentNaming
    public enum Currency
    {
        [EnumMember(Value = "CAD")]
        CAD,

        [EnumMember(Value = "BTC")]
        BTC,

        [EnumMember(Value = "ETH")]
        ETH,

        [EnumMember(Value = "EUR")]
        EUR,

        [EnumMember(Value = "GBP")]
        GBP,

        [EnumMember(Value = "LTC")]
        LTC,

        [EnumMember(Value = "USD")]
        USD
    }
    // ReSharper restore InconsistentNaming

}
