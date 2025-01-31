namespace adjustSdk;

public partial class AdjustAdRevenue {
    public string Source { get; private set; }
    public double? Revenue { get; private set; }
    public string? Currency { get; private set; }
    public int? AdImpressionsCount { get; set; }
    public string? AdRevenueNetwork { get; set; }
    public string? AdRevenueUnit { get; set; }
    public string? AdRevenuePlacement { get; set; }
    internal List<string>? innerCallbackParameters = null;
    internal List<string>? innerPartnerParameters = null;


    public AdjustAdRevenue(string source)
    {
        Source = source;
    }

    public void SetRevenue(double amount, string currency)
    {
        Revenue = amount;
        Currency = currency;
    }

    public void AddCallbackParameter(string key, string value)
    {
        innerCallbackParameters ??= [];
        innerCallbackParameters.Add(key);
        innerCallbackParameters.Add(value);
    }

    public void AddPartnerParameter(string key, string value)
    {
        innerPartnerParameters ??= [];
        innerPartnerParameters.Add(key);
        innerPartnerParameters.Add(value);
    }
}