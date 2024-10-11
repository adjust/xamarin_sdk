namespace adjustSdk;

public partial class AdjustEvent {

    public string? TransactionId { get; set; }

    internal adjustSdk.iOSBinding.ADJEvent toNative() {
        adjustSdk.iOSBinding.ADJEvent nativeEvent = new (EventToken);

        if (Revenue is double revenueValue && Currency is not null) {
            nativeEvent.SetRevenue(revenueValue, Currency);
        }

        AdjustUtil.iterateTwoPairList(innerCallbackParameters,
            nativeEvent.AddCallbackParameter);

        AdjustUtil.iterateTwoPairList(innerPartnerParameters,
            nativeEvent.AddPartnerParameter);

        if (DeduplicationId is not null) {
            nativeEvent.SetDeduplicationId(DeduplicationId);
        }

        if (CallbackId is not null) {
            nativeEvent.SetCallbackId(CallbackId);
        }

        if (ProductId is not null) {
            nativeEvent.SetProductId(ProductId);
        }

        if (TransactionId is not null) {
            nativeEvent.SetTransactionId(TransactionId);
        }

        return nativeEvent;
    }
}