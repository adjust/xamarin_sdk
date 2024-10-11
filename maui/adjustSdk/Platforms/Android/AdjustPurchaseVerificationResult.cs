namespace adjustSdk;

public partial record AdjustPurchaseVerificationResult
{
    internal static AdjustPurchaseVerificationResult? fromNative(
        Com.Adjust.Sdk.AdjustPurchaseVerificationResult? nativeResult)
    {
        if (nativeResult is null) { return null; }

        return new(
            nativeResult.VerificationStatus,
            nativeResult.Code,
            nativeResult.Message);
    }
}