using Foundation;

namespace adjustSdk;

public record class AdjustAppStoreSubscription(string Price, string Currency, string TransactionId) {
    public string? TransactionDate { get; set; }
    public string? SalesRegion { get; set; }
    internal List<string>? innerCallbackParameters = null;
    internal List<string>? innerPartnerParameters = null;

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
    internal adjustSdk.iOSBinding.ADJAppStoreSubscription toNative() {
        adjustSdk.iOSBinding.ADJAppStoreSubscription nativeAppStoreSubscription =
            new (new NSDecimalNumber(Price), Currency, TransactionId);

        if (TransactionDate is not null) {
            // following Unity implementation
            nativeAppStoreSubscription.SetTransactionDate(
                NSDate.FromTimeIntervalSince1970(
                    new NSNumberFormatter().NumberFromString(TransactionDate).DoubleValue / 1000));
        }

        if (SalesRegion is not null) {
            nativeAppStoreSubscription.SetSalesRegion(SalesRegion);
        }

        AdjustUtil.iterateTwoPairList(innerCallbackParameters,
            nativeAppStoreSubscription.AddCallbackParameter);

        AdjustUtil.iterateTwoPairList(innerPartnerParameters,
            nativeAppStoreSubscription.AddPartnerParameter);

        return nativeAppStoreSubscription;
    }
}