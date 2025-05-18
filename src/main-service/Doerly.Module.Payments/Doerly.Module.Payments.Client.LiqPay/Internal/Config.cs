namespace Doerly.Module.Payments.Client.LiqPay;

internal class Config
{
    private const string LiqPayApiUrl = "https://www.liqpay.ua/api";


    /// <summary>
    /// Default language for LiqPay API requests
    /// <para>uk - Ukrainian is default the language</para>
    /// </summary>
    internal const string DefaultLanguage = "uk";

    internal const string DefaultLiqPayApiVersion = "3";
    
    internal static Uri CheckoutUrl => new($"{LiqPayApiUrl}/{DefaultLiqPayApiVersion}/checkout");
}
