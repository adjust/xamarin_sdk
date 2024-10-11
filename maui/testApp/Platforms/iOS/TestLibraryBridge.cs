using System;
using adjustSdk;
using Foundation;

public partial class TestLibraryBridge {
    //private const string baseIp = "192.168.86.165";
    //private const string baseIp = "192.168.1.4";
    private const string baseIp = "127.0.0.1";

    private testApp.iOsBinding.ATLTestLibrary testLibrary { get; init; }

    public TestLibraryBridge() {
        overwriteUrl = $"http://{baseIp}:8080";
        controlUrl = $"ws://{baseIp}:1987";

        testLibrary = testApp.iOsBinding.ATLTestLibrary.TestLibraryWithBaseUrl(
            overwriteUrl, controlUrl, new CommandDelegate(this));
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
        if (infoToSend is null) { return; }
        foreach (KeyValuePair<string, string> keyValuePair in infoToSend) {
            addInfoToSend(keyValuePair.Key, keyValuePair.Value);
        }
    }
    private partial void sendInfoToServer(string? extraPath) {
        testLibrary.SendInfoToServer(extraPath);
    }

    private void trackAppStoreSubscription(Dictionary<string, List<string>> parameters) {
        if (! (firstStringValue(parameters, "revenue") is string price
            && firstStringValue(parameters, "currency") is string currency
            && firstStringValue(parameters, "transactionId") is string transactionId
        )) { return; }

        AdjustAppStoreSubscription adjustAppStoreSubscription = new (price, currency, transactionId);

        if (firstStringValue(parameters, "transactionDate") is string transactionDate) {
            adjustAppStoreSubscription.TransactionDate = transactionDate;
        }

        if (firstStringValue(parameters, "salesRegion") is string salesRegion) {
            adjustAppStoreSubscription.SalesRegion = salesRegion;
        }

        iterateTwoPairList(listValues(parameters, "callbackParams"),
            adjustAppStoreSubscription.AddCallbackParameter);

        iterateTwoPairList(listValues(parameters, "partnerParams"),
            adjustAppStoreSubscription.AddPartnerParameter);

        Adjust.TrackAppStoreSubscription(adjustAppStoreSubscription);
    }

    private void verifyAppStorePurchase(Dictionary<string, List<string>> parameters) {
        if (! (firstStringValue(parameters, "productId") is string productId
            && firstStringValue(parameters, "transactionId") is string transactionId))
        {
            return;
        }

        string? localBasePath = currentExtraPath;
        Adjust.VerifyAppStorePurchase(new (transactionId, productId),
            verificationResultCallback(localBasePath));
    }
    private static string? jsonResponseConvert(NSDictionary? jsonResponse) {
        if (jsonResponse is null) { return null; }

        return NSJsonSerialization.Serialize(jsonResponse, 0, out NSError nsError) switch {
            NSData jsonResponseData when jsonResponseData.Length > 0 =>
                new NSString(jsonResponseData, NSStringEncoding.UTF8).ToString(),
            _ => null
        };
    }
}

internal class CommandDelegate(TestLibraryBridge testLibraryBridge) :
    testApp.iOsBinding.AdjustCommandDelegate
{
/*
    public override void ExecuteCommand(string className, string methodName, NSDictionary parameters) {
    }
*/
    public override void ExecuteCommand(string className, string methodName, string jsonParameters) {
        testLibraryBridge.executeCommon(className, methodName, jsonParameters);
    }
/*
    public override void ExecuteCommandRawJson(string json) {

    }
*/
}