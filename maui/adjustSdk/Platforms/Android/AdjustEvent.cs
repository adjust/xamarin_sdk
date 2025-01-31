namespace adjustSdk;

public partial class AdjustEvent {

    public string? PurchaseToken { get; set; }

    internal Com.Adjust.Sdk.AdjustEvent toNative() {
        Com.Adjust.Sdk.AdjustEvent nativeEvent = new Com.Adjust.Sdk.AdjustEvent(EventToken);

        if (Revenue is double revenueValue) {
            nativeEvent.SetRevenue(revenueValue, Currency);
        }

        AdjustUtil.iterateTwoPairList(innerCallbackParameters,
            nativeEvent.AddCallbackParameter);

        AdjustUtil.iterateTwoPairList(innerPartnerParameters,
            nativeEvent.AddPartnerParameter);

        if (DeduplicationId is not null) {
            nativeEvent.DeduplicationId = DeduplicationId;
        }

        if (CallbackId is not null) {
            nativeEvent.CallbackId = CallbackId;
        }

        if (ProductId is not null) {
            nativeEvent.ProductId = ProductId;
        }

        if (PurchaseToken is not null) {
            nativeEvent.PurchaseToken = PurchaseToken;
        }

        return nativeEvent;
    }
}