namespace adjustSdk;

public record class AdjustPlayStoreSubscription(
    long Price,
    string Currency,
    string ProductId,
    string OrderId,
    string Signature,
    string PurchaseToken)
{
    public long? PurchaseTime { get; set; }
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
   internal Com.Adjust.Sdk.AdjustPlayStoreSubscription toNative() {
        Com.Adjust.Sdk.AdjustPlayStoreSubscription nativeAdjustPlayStoreSubscription = new (
            Price,
            Currency,
            ProductId,
            OrderId,
            Signature,
            PurchaseToken);

        if (PurchaseTime is long purchaseTimeValue) {
            nativeAdjustPlayStoreSubscription.SetPurchaseTime(purchaseTimeValue);
            //nativeAdjustPlayStoreSubscription.PurchaseTime = purchaseTimeValue;
        }

        AdjustUtil.iterateTwoPairList(innerCallbackParameters,
            nativeAdjustPlayStoreSubscription.AddCallbackParameter);

        AdjustUtil.iterateTwoPairList(innerPartnerParameters,
            nativeAdjustPlayStoreSubscription.AddPartnerParameter);

        return nativeAdjustPlayStoreSubscription;
    }
}