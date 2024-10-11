

using adjustSdk;

public partial class TestLibraryBridge {
    private const string baseIp = "10.0.2.2";

    private Com.Adjust.Test.TestLibrary testLibrary { get; init;}

    public TestLibraryBridge() {
        overwriteUrl = $"https://{baseIp}:8443";
        controlUrl = $"ws://{baseIp}:1987";

        testLibrary = new Com.Adjust.Test.TestLibrary(
            overwriteUrl, controlUrl, Platform.AppContext,
            new CommandJsonListener(this));
    }

    public partial void start() {
        Adjust.GetSdkVersion(testLibrary.StartTestSession);
    }

    public partial void addTest(string testName) {
        testLibrary.AddTest(testName);
    }
    public partial void addTestDirectory(string testDirectory) {
        testLibrary.AddTestDirectory(testDirectory);
    }

    private partial void addInfoToSend(string key, string value) {
        testLibrary.AddInfoToSend(key, value);
    }

    private partial void setInfoToServer(IDictionary<string, string>? infoToSend) {
        testLibrary.SetInfoToSend(infoToSend);
    }

    private partial void sendInfoToServer(string? extraPath) {
        testLibrary.SendInfoToServer(extraPath);
    }

    private void trackPlayStoreSubscription(Dictionary<string, List<string>> parameters) {
        if (! (firstLongValue(parameters, "revenue") is long price
            && firstStringValue(parameters, "currency") is string currency
            && firstStringValue(parameters, "productId") is string productId
            && firstStringValue(parameters, "receipt") is string signature
            && firstStringValue(parameters, "purchaseToken") is string purchaseToken
            && firstStringValue(parameters, "transactionId") is string orderId
        )) { return; }

        AdjustPlayStoreSubscription adjustPlayStoreSubscription = new (
            price, currency, productId, orderId, signature, purchaseToken);

        if (firstLongValue(parameters, "transactionDate") is long purchaseTime) {
            adjustPlayStoreSubscription.PurchaseTime = purchaseTime;
        }

        iterateTwoPairList(listValues(parameters, "callbackParams"),
            adjustPlayStoreSubscription.AddCallbackParameter);

        iterateTwoPairList(listValues(parameters, "partnerParams"),
            adjustPlayStoreSubscription.AddPartnerParameter);

        Adjust.TrackPlayStoreSubscription(adjustPlayStoreSubscription);
    }

    private void verifyPlayStorePurchase(Dictionary<string, List<string>> parameters) {
        if (! (firstStringValue(parameters, "productId") is string productId
            && firstStringValue(parameters, "purchaseToken") is string purchaseToken))
        {
            return;
        }

        string? localBasePath = currentExtraPath;
        Adjust.VerifyPlayStorePurchase(new (productId, purchaseToken),
            verificationResultCallback(localBasePath));
    }

    private static string? jsonResponseConvert(Org.Json.JSONObject? jsonResponse) {
        return jsonResponse?.ToString();
    }
}

internal class CommandJsonListener(TestLibraryBridge testLibraryBridge) :
    Java.Lang.Object, Com.Adjust.Test.ICommandJsonListener
{
    public void ExecuteCommand(string? className, string? methodName, string? jsonParameters)
    {
        if (className is null || methodName is null || jsonParameters is null) {
            return;
        }

        testLibraryBridge.executeCommon(className, methodName, jsonParameters);
    }
}