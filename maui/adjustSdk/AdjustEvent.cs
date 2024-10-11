namespace adjustSdk;

public partial class AdjustEvent {
    public string EventToken { get; private set; }
    public double? Revenue { get; private set; }
    public string? Currency { get; private set; }
    public string? CallbackId { get; set; }
    public string? DeduplicationId { get; set; }
    public string? ProductId { get; set; }
    internal List<string>? innerCallbackParameters = null;
    internal List<string>? innerPartnerParameters = null;


    public AdjustEvent(string eventToken)
    {
        EventToken = eventToken;
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