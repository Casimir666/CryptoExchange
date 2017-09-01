using System.Runtime.Serialization;

namespace CryptoExchange.Gdax
{
    public enum BookType
    {
        [EnumMember(Value = "received")]
        Received,

        [EnumMember(Value = "open")]
        Open,

        [EnumMember(Value = "done")]
        Done,

        [EnumMember(Value = "match")]
        Match,

        [EnumMember(Value = "change")]
        Change,

        [EnumMember(Value = "heartbeat")]
        Heartbeat,

        [EnumMember(Value = "margin_profile_update")]
        MarginProfileUpdate
    }

    public enum SideType
    {
        [EnumMember(Value = "sell")]
        Sell,

        [EnumMember(Value = "buy")]
        Buy
    }

    public enum OrderType
    {
        [EnumMember(Value = "market")]
        Market,

        [EnumMember(Value ="limit")]
        Limit,

        [EnumMember(Value = "stop")]
        Stop
    }

    public enum OrderStatus
    {
        [EnumMember(Value = "open")]
        Open,

        [EnumMember(Value = "pending")]
        Pending,

        [EnumMember(Value = "active")]
        Active,

        [EnumMember(Value = "done")]
        Done,

        [EnumMember(Value = "rejected")]
        Rejected
    }

    public enum OrderReason
    {
        [EnumMember(Value = "canceled")]
        Canceled,

        [EnumMember(Value = "filled")]
        Filled
    }

    // ReSharper disable InconsistentNaming
    public enum StpFlag
    {
        /// <summary>
        /// Decrease and Cancel 
        /// </summary>
        [EnumMember(Value = "dc")]
        DC,

        /// <summary>
        /// Cancel oldest
        /// </summary>
        [EnumMember(Value = "co")]
        CO,

        /// <summary>
        /// Cancel newest
        /// </summary>
        [EnumMember(Value = "cn")]
        CN,

        /// <summary>
        /// Cancel both
        /// </summary>
        [EnumMember(Value = "cb")]
        CB
    }
    // ReSharper restore InconsistentNaming


    // ReSharper disable InconsistentNaming
    public enum TimeInForceType
    {
        /// <summary>
        /// Good till canceled 
        /// </summary>
        [EnumMember(Value = "gtc")]
        GTC,

        /// <summary>
        /// Good till time 
        /// </summary>
        [EnumMember(Value = "gtt")]
        GTT,

        /// <summary>
        /// Immediate or cancel 
        /// </summary>
        [EnumMember(Value = "ioc")]
        IOC,

        /// <summary>
        /// Fill or kill 
        /// </summary>
        [EnumMember(Value = "fok")]
        FOK
    }
    // ReSharper restore InconsistentNaming

    public enum CancelType
    {
        [EnumMember(Value = "min")]
        Min,

        [EnumMember(Value = "hour")]
        Hour,

        [EnumMember(Value = "day")]
        Day
    }

    public enum LeggerType
    {
        [EnumMember(Value = "transfer")]
        Transfer,

        [EnumMember(Value = "match")]
        Match,

        [EnumMember(Value = "fee")]
        Fee,

        [EnumMember(Value = "rebate")]
        Rebate
    }

    public enum AccountType
    {
        [EnumMember(Value = "fiat")]
        Fiat,

        [EnumMember(Value = "wallet")]
        Wallet,

        [EnumMember(Value = "vault")]
        Vault
    }

    // ReSharper disable InconsistentNaming
    public enum ProductType
    {
        [EnumMember(Value = "BTC-USD")]
        BTC_USD,

        [EnumMember(Value = "BTC-GBP")]
        BTC_GBP,

        [EnumMember(Value = "BTC-EUR")]
        BTC_EUR,

        [EnumMember(Value = "ETH-BTC")]
        ETH_BTC,

        [EnumMember(Value= "ETH-EUR")]
        ETH_EUR,

        [EnumMember(Value = "ETH-USD")]
        ETH_USD,

        [EnumMember(Value = "LTC-BTC")]
        LTC_BTC,

        [EnumMember(Value = "LTC-EUR")]
        LTC_EUR,

        [EnumMember(Value = "LTC-USD")]
        LTC_USD
    }
    // ReSharper restore InconsistentNaming

    public enum PaymentType
    {
        /// <summary>
        /// Regular US bank account
        /// </summary>
        [EnumMember(Value = "ach_bank_account")]
        AchBankAccount,

        /// <summary>
        /// European SEPA bank account
        /// </summary>
        [EnumMember(Value = "sepa_bank_account")]
        SepaBankAccount,

        /// <summary>
        /// iDeal bank account (Europe)
        /// </summary>
        [EnumMember(Value = "ideal_bank_account")]
        IdealBankAccount,

        /// <summary>
        /// Fiat nominated Coinbase account
        /// </summary>
        [EnumMember(Value = "fiat_account")]
        FiatAccount,

        /// <summary>
        /// Bank wire (US only)
        /// </summary>
        [EnumMember(Value = "bank_wire")]
        BankWire,

        /// <summary>
        /// Credit card (can’t be used for buying/selling)
        /// </summary>
        [EnumMember(Value = "credit_card")]
        CreditCard,

        /// <summary>
        /// Secure3D verified payment card
        /// </summary>
        [EnumMember(Value = "secure3d_card")]
        Secure3DCard,

        /// <summary>
        /// Canadian EFT bank account
        /// </summary>
        [EnumMember(Value = "eft_bank_account")]
        EftBankAccount,

        /// <summary>
        /// Interac Online for Canadian bank accounts
        /// </summary>
        [EnumMember(Value = "interac")]
        Interac
    }

    public enum HoldType
    {
        /// <summary>
        /// Holds related to a withdraw
        /// </summary>
        [EnumMember(Value = "transfer")]
        Transfer,

        /// <summary>
        /// Holds related to open orders 
        /// </summary>
        [EnumMember(Value = "order")]
        Order
    }

    public enum LiquidityType
    {
        [EnumMember(Value = "M")]
        Maker,

        [EnumMember(Value = "T")]
        Taker
    }

    public enum ProfileStatus
    {
        [EnumMember(Value = "active")]
        Active,

        [EnumMember(Value = "pending")]
        Pending,

        [EnumMember(Value = "locked")]
        Locked,
    }

    public enum FundingStatus
    {
        [EnumMember(Value = "outstanding")]
        Outstanding,

        [EnumMember(Value = "settled")]
        Settled,

        [EnumMember(Value = "rejected")]
        Rejected
    }

    public enum ReportType
    {
        [EnumMember(Value = "fills")]
        Fills,

        [EnumMember(Value = "account")]
        Account
    }

    public enum ReportFormat
    {
        [EnumMember(Value = "pdf")]
        Pdf,

        [EnumMember(Value = "csv")]
        Csv
    }

    public enum ReportStatus
    {
        [EnumMember(Value = "pending")]
        Pending,

        [EnumMember(Value = "creating")]
        Creating,

        [EnumMember(Value = "ready")]
        Ready
    }

    public enum BookLevel
    {
        /// <summary>
        /// Only the best bid and ask
        /// </summary>
        Best = 1,

        /// <summary>
        /// Top 50 bids and asks (aggregated)
        /// </summary>
        Top50 = 2,

        /// <summary>
        /// Full order book (non aggregated)
        /// </summary>
        Full = 3
    }
}
