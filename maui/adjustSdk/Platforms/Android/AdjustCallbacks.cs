namespace adjustSdk;

internal class OnAdidReadListenerAdapter(Action<string> AdidCallback)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnAdidReadListener
{
    public void OnAdidRead (string? adid) {
        if (adid is null) { return; }

        AdidCallback.Invoke(adid);
    }
}

internal class OnAttributionReadListenerAdapter(Action<AdjustAttribution> attributionCallback)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnAttributionReadListener
{
    public void OnAttributionRead (Com.Adjust.Sdk.AdjustAttribution? nativeAdjustAttribution) {
        var adjustAttribution = AdjustAttribution.fromNative(nativeAdjustAttribution);

        if (adjustAttribution is null) { return; }

        attributionCallback.Invoke(adjustAttribution);
    }
}

internal class OnSdkVersionReadListenerAdapter(Action<string> SdkVersionCallback)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnSdkVersionReadListener
{
    public void OnSdkVersionRead (string? sdkVersion) {
        if (sdkVersion is null) { return; }

        SdkVersionCallback($"{AdjustConfig.SdkPrefix}@{sdkVersion}");
    }
}

internal class OnLastDeeplinkReadListenerAdapter(Action<string?> LastDeeplinkReadCallback)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnLastDeeplinkReadListener
{
    public void OnLastDeeplinkRead (Android.Net.Uri? nativeDeeplink) {
        LastDeeplinkReadCallback(nativeDeeplink?.ToString());
    }
}

internal class OnDeeplinkResolvedListenerAdapter(Action<string> DeeplinkResolvedCallback)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnDeeplinkResolvedListener
{
    public void OnDeeplinkResolved (string? nativeDeeplink) {
        if (nativeDeeplink is null) { return; }

        var deeplink = nativeDeeplink.ToString();
        if (deeplink is null) { return; }

        DeeplinkResolvedCallback.Invoke(deeplink);
    }
}

internal class OnGoogleAdIdReadListenerAdapter(Action<string> GoogleAdIdReadCallback)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnGoogleAdIdReadListener
{
    public void OnGoogleAdIdRead (string? nativeGoogleAdId) {
        if (nativeGoogleAdId is null) { return; }

        GoogleAdIdReadCallback.Invoke(nativeGoogleAdId);
    }
}

internal class OnAmazonAdIdReadListenerAdapter(Action<string> AmazonAdIdReadCallback)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnAmazonAdIdReadListener
{
    public void OnAmazonAdIdRead (string? nativeAmazonAdId) {
        if (nativeAmazonAdId is null) { return; }

        AmazonAdIdReadCallback.Invoke(nativeAmazonAdId);
    }
}

internal class OnPurchaseVerificationFinishedListenerAdapter(
    Action<AdjustPurchaseVerificationResult> AdjustPurchaseVerificationCallback)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnPurchaseVerificationFinishedListener
{
    public void OnVerificationFinished (
        Com.Adjust.Sdk.AdjustPurchaseVerificationResult? nativeAdjustPurchaseVerificationResult)
    {
        var result =
            AdjustPurchaseVerificationResult.fromNative(nativeAdjustPurchaseVerificationResult);

        if (result is null) { return; }

        AdjustPurchaseVerificationCallback.Invoke(result);
    }
}
