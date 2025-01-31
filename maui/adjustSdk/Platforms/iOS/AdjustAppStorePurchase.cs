namespace adjustSdk;

public record class AdjustAppStorePurchase(string TransactionId, string ProductId)
{
    internal adjustSdk.iOSBinding.ADJAppStorePurchase toNative() {
        return new (TransactionId, ProductId);
    }
}
