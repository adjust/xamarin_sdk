namespace adjustSdk;

public record class AdjustPlayStorePurchase(
    string ProductId,
    string PurchaseToken)
{
    internal Com.Adjust.Sdk.AdjustPlayStorePurchase toNative() {
        return new (ProductId, PurchaseToken);
    }
}
