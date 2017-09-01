using CryptoExchange.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoExchange.Coinbase
{

    public class ExchangeRate
    {
        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency Currency { get; set; }

        [JsonProperty("rates")]
        public Rates Rates { get; set; }
    }

    // ReSharper disable InconsistentNaming
    public class Rates
    {

        [JsonProperty("AED")]
        public decimal AED { get; set; }

        [JsonProperty("AFN")]
        public decimal AFN { get; set; }

        [JsonProperty("ALL")]
        public decimal ALL { get; set; }

        [JsonProperty("AMD")]
        public decimal AMD { get; set; }

        [JsonProperty("ANG")]
        public decimal ANG { get; set; }

        [JsonProperty("AOA")]
        public decimal AOA { get; set; }

        [JsonProperty("ARS")]
        public decimal ARS { get; set; }

        [JsonProperty("AUD")]
        public decimal AUD { get; set; }

        [JsonProperty("AWG")]
        public decimal AWG { get; set; }

        [JsonProperty("AZN")]
        public decimal AZN { get; set; }

        [JsonProperty("BAM")]
        public decimal BAM { get; set; }

        [JsonProperty("BBD")]
        public decimal BBD { get; set; }

        [JsonProperty("BDT")]
        public decimal BDT { get; set; }

        [JsonProperty("BGN")]
        public decimal BGN { get; set; }

        [JsonProperty("BHD")]
        public decimal BHD { get; set; }

        [JsonProperty("BIF")]
        public decimal BIF { get; set; }

        [JsonProperty("BMD")]
        public decimal BMD { get; set; }

        [JsonProperty("BND")]
        public decimal BND { get; set; }

        [JsonProperty("BOB")]
        public decimal BOB { get; set; }

        [JsonProperty("BRL")]
        public decimal BRL { get; set; }

        [JsonProperty("BSD")]
        public decimal BSD { get; set; }

        [JsonProperty("BTC")]
        public decimal BTC { get; set; }

        [JsonProperty("BTN")]
        public decimal BTN { get; set; }

        [JsonProperty("BWP")]
        public decimal BWP { get; set; }

        [JsonProperty("BYN")]
        public decimal BYN { get; set; }

        [JsonProperty("BYR")]
        public decimal BYR { get; set; }

        [JsonProperty("BZD")]
        public decimal BZD { get; set; }

        [JsonProperty("CAD")]
        public decimal CAD { get; set; }

        [JsonProperty("CDF")]
        public decimal CDF { get; set; }

        [JsonProperty("CHF")]
        public decimal CHF { get; set; }

        [JsonProperty("CLF")]
        public decimal CLF { get; set; }

        [JsonProperty("CLP")]
        public decimal CLP { get; set; }

        [JsonProperty("CNY")]
        public decimal CNY { get; set; }

        [JsonProperty("COP")]
        public decimal COP { get; set; }

        [JsonProperty("CRC")]
        public decimal CRC { get; set; }

        [JsonProperty("CUC")]
        public decimal CUC { get; set; }

        [JsonProperty("CVE")]
        public decimal CVE { get; set; }

        [JsonProperty("CZK")]
        public decimal CZK { get; set; }

        [JsonProperty("DJF")]
        public decimal DJF { get; set; }

        [JsonProperty("DKK")]
        public decimal DKK { get; set; }

        [JsonProperty("DOP")]
        public decimal DOP { get; set; }

        [JsonProperty("DZD")]
        public decimal DZD { get; set; }

        [JsonProperty("EEK")]
        public decimal EEK { get; set; }

        [JsonProperty("EGP")]
        public decimal EGP { get; set; }

        [JsonProperty("ERN")]
        public decimal ERN { get; set; }

        [JsonProperty("ETB")]
        public decimal ETB { get; set; }

        [JsonProperty("ETH")]
        public decimal ETH { get; set; }

        [JsonProperty("EUR")]
        public decimal EUR { get; set; }

        [JsonProperty("FJD")]
        public decimal FJD { get; set; }

        [JsonProperty("FKP")]
        public decimal FKP { get; set; }

        [JsonProperty("GBP")]
        public decimal GBP { get; set; }

        [JsonProperty("GEL")]
        public decimal GEL { get; set; }

        [JsonProperty("GGP")]
        public decimal GGP { get; set; }

        [JsonProperty("GHS")]
        public decimal GHS { get; set; }

        [JsonProperty("GIP")]
        public decimal GIP { get; set; }

        [JsonProperty("GMD")]
        public decimal GMD { get; set; }

        [JsonProperty("GNF")]
        public decimal GNF { get; set; }

        [JsonProperty("GTQ")]
        public decimal GTQ { get; set; }

        [JsonProperty("GYD")]
        public decimal GYD { get; set; }

        [JsonProperty("HKD")]
        public decimal HKD { get; set; }

        [JsonProperty("HNL")]
        public decimal HNL { get; set; }

        [JsonProperty("HRK")]
        public decimal HRK { get; set; }

        [JsonProperty("HTG")]
        public decimal HTG { get; set; }

        [JsonProperty("HUF")]
        public decimal HUF { get; set; }

        [JsonProperty("IDR")]
        public decimal IDR { get; set; }

        [JsonProperty("ILS")]
        public decimal ILS { get; set; }

        [JsonProperty("IMP")]
        public decimal IMP { get; set; }

        [JsonProperty("INR")]
        public decimal INR { get; set; }

        [JsonProperty("IQD")]
        public decimal IQD { get; set; }

        [JsonProperty("ISK")]
        public decimal ISK { get; set; }

        [JsonProperty("JEP")]
        public decimal JEP { get; set; }

        [JsonProperty("JMD")]
        public decimal JMD { get; set; }

        [JsonProperty("JOD")]
        public decimal JOD { get; set; }

        [JsonProperty("JPY")]
        public decimal JPY { get; set; }

        [JsonProperty("KES")]
        public decimal KES { get; set; }

        [JsonProperty("KGS")]
        public decimal KGS { get; set; }

        [JsonProperty("KHR")]
        public decimal KHR { get; set; }

        [JsonProperty("KMF")]
        public decimal KMF { get; set; }

        [JsonProperty("KRW")]
        public decimal KRW { get; set; }

        [JsonProperty("KWD")]
        public decimal KWD { get; set; }

        [JsonProperty("KYD")]
        public decimal KYD { get; set; }

        [JsonProperty("KZT")]
        public decimal KZT { get; set; }

        [JsonProperty("LAK")]
        public decimal LAK { get; set; }

        [JsonProperty("LBP")]
        public decimal LBP { get; set; }

        [JsonProperty("LKR")]
        public decimal LKR { get; set; }

        [JsonProperty("LRD")]
        public decimal LRD { get; set; }

        [JsonProperty("LSL")]
        public decimal LSL { get; set; }

        [JsonProperty("LTL")]
        public decimal LTL { get; set; }

        [JsonProperty("LVL")]
        public decimal LVL { get; set; }

        [JsonProperty("LYD")]
        public decimal LYD { get; set; }

        [JsonProperty("MAD")]
        public decimal MAD { get; set; }

        [JsonProperty("MDL")]
        public decimal MDL { get; set; }

        [JsonProperty("MGA")]
        public decimal MGA { get; set; }

        [JsonProperty("MKD")]
        public decimal MKD { get; set; }

        [JsonProperty("MMK")]
        public decimal MMK { get; set; }

        [JsonProperty("MNT")]
        public decimal MNT { get; set; }

        [JsonProperty("MOP")]
        public decimal MOP { get; set; }

        [JsonProperty("MRO")]
        public decimal MRO { get; set; }

        [JsonProperty("MTL")]
        public decimal MTL { get; set; }

        [JsonProperty("MUR")]
        public decimal MUR { get; set; }

        [JsonProperty("MVR")]
        public decimal MVR { get; set; }

        [JsonProperty("MWK")]
        public decimal MWK { get; set; }

        [JsonProperty("MXN")]
        public decimal MXN { get; set; }

        [JsonProperty("MYR")]
        public decimal MYR { get; set; }

        [JsonProperty("MZN")]
        public decimal MZN { get; set; }

        [JsonProperty("NAD")]
        public decimal NAD { get; set; }

        [JsonProperty("NGN")]
        public decimal NGN { get; set; }

        [JsonProperty("NIO")]
        public decimal NIO { get; set; }

        [JsonProperty("NOK")]
        public decimal NOK { get; set; }

        [JsonProperty("NPR")]
        public decimal NPR { get; set; }

        [JsonProperty("NZD")]
        public decimal NZD { get; set; }

        [JsonProperty("OMR")]
        public decimal OMR { get; set; }

        [JsonProperty("PAB")]
        public decimal PAB { get; set; }

        [JsonProperty("PEN")]
        public decimal PEN { get; set; }

        [JsonProperty("PGK")]
        public decimal PGK { get; set; }

        [JsonProperty("PHP")]
        public decimal PHP { get; set; }

        [JsonProperty("PKR")]
        public decimal PKR { get; set; }

        [JsonProperty("PLN")]
        public decimal PLN { get; set; }

        [JsonProperty("PYG")]
        public decimal PYG { get; set; }

        [JsonProperty("QAR")]
        public decimal QAR { get; set; }

        [JsonProperty("RON")]
        public decimal RON { get; set; }

        [JsonProperty("RSD")]
        public decimal RSD { get; set; }

        [JsonProperty("RUB")]
        public decimal RUB { get; set; }

        [JsonProperty("RWF")]
        public decimal RWF { get; set; }

        [JsonProperty("SAR")]
        public decimal SAR { get; set; }

        [JsonProperty("SBD")]
        public decimal SBD { get; set; }

        [JsonProperty("SCR")]
        public decimal SCR { get; set; }

        [JsonProperty("SEK")]
        public decimal SEK { get; set; }

        [JsonProperty("SGD")]
        public decimal SGD { get; set; }

        [JsonProperty("SHP")]
        public decimal SHP { get; set; }

        [JsonProperty("SLL")]
        public decimal SLL { get; set; }

        [JsonProperty("SOS")]
        public decimal SOS { get; set; }

        [JsonProperty("SRD")]
        public decimal SRD { get; set; }

        [JsonProperty("SSP")]
        public decimal SSP { get; set; }

        [JsonProperty("STD")]
        public decimal STD { get; set; }

        [JsonProperty("SVC")]
        public decimal SVC { get; set; }

        [JsonProperty("SZL")]
        public decimal SZL { get; set; }

        [JsonProperty("THB")]
        public decimal THB { get; set; }

        [JsonProperty("TJS")]
        public decimal TJS { get; set; }

        [JsonProperty("TMT")]
        public decimal TMT { get; set; }

        [JsonProperty("TND")]
        public decimal TND { get; set; }

        [JsonProperty("TOP")]
        public decimal TOP { get; set; }

        [JsonProperty("TRY")]
        public decimal TRY { get; set; }

        [JsonProperty("TTD")]
        public decimal TTD { get; set; }

        [JsonProperty("TWD")]
        public decimal TWD { get; set; }

        [JsonProperty("TZS")]
        public decimal TZS { get; set; }

        [JsonProperty("UAH")]
        public decimal UAH { get; set; }

        [JsonProperty("UGX")]
        public decimal UGX { get; set; }

        [JsonProperty("USD")]
        public decimal USD { get; set; }

        [JsonProperty("UYU")]
        public decimal UYU { get; set; }

        [JsonProperty("UZS")]
        public decimal UZS { get; set; }

        [JsonProperty("VEF")]
        public decimal VEF { get; set; }

        [JsonProperty("VND")]
        public decimal VND { get; set; }

        [JsonProperty("VUV")]
        public decimal VUV { get; set; }

        [JsonProperty("WST")]
        public decimal WST { get; set; }

        [JsonProperty("XAF")]
        public decimal XAF { get; set; }

        [JsonProperty("XAG")]
        public decimal XAG { get; set; }

        [JsonProperty("XAU")]
        public decimal XAU { get; set; }

        [JsonProperty("XCD")]
        public decimal XCD { get; set; }

        [JsonProperty("XDR")]
        public decimal XDR { get; set; }

        [JsonProperty("XOF")]
        public decimal XOF { get; set; }

        [JsonProperty("XPD")]
        public decimal XPD { get; set; }

        [JsonProperty("XPF")]
        public decimal XPF { get; set; }

        [JsonProperty("XPT")]
        public decimal XPT { get; set; }

        [JsonProperty("YER")]
        public decimal YER { get; set; }

        [JsonProperty("ZAR")]
        public decimal ZAR { get; set; }

        [JsonProperty("ZMK")]
        public decimal ZMK { get; set; }

        [JsonProperty("ZMW")]
        public decimal ZMW { get; set; }

        [JsonProperty("ZWL")]
        public decimal ZWL { get; set; }
    }
    // ReSharper restore InconsistentNaming
}

