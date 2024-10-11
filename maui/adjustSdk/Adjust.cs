using System.Diagnostics;

namespace adjustSdk;

public partial class Adjust {
    public static partial void InitSdk(AdjustConfig adjustConfig);

    public static partial void TrackEvent(AdjustEvent adjustEvent);

    public static partial void Enable();
    public static partial void Disable();

    public static partial void IsEnabled(Action<bool> callback);

    public static partial void SwitchToOfflineMode();
    public static partial void SwitchBackToOnlineMode();

    public static partial void SetPushToken(string pushToken);

    public static partial void GdprForgetMe();

    public static partial void ProcessDeeplink(AdjustDeeplink deeplink);

    public static partial void AddGlobalPartnerParameter(string key, string value);
    public static partial void AddGlobalCallbackParameter(string key, string value);

    public static partial void RemoveGlobalPartnerParameter(string key);
    public static partial void RemoveGlobalCallbackParameter(string key);

    public static partial void RemoveGlobalPartnerParameters();
    public static partial void RemoveGlobalCallbackParameters();

    public static partial void TrackAdRevenue(AdjustAdRevenue adRevenue);

    public static partial void TrackThirdPartySharing(AdjustThirdPartySharing thirdPartySharing);

    public static partial void TrackMeasurementConsent(bool measurementConsent);

    public static partial void GetAdid(Action<string> callback);

    public static partial void GetAttribution(Action<AdjustAttribution> callback);

    public static partial void GetSdkVersion(Action<string> callback);

    public static partial void GetLastDeeplink(Action<string?> callback);

    public static partial void ProcessAndResolveDeeplink(AdjustDeeplink deeplink, Action<string> callback);

    public static partial void setTestOptions(Dictionary<string, object> testOptions);

    public static partial void Resume();
    public static partial void Pause();

    #if ANDROID
    public static partial void TrackPlayStoreSubscription(AdjustPlayStoreSubscription subscription);

    public static partial void GetGoogleAdId(Action<string> callback);

    public static partial void GetAmazonAdId(Action<string> callback);

    public static partial void VerifyPlayStorePurchase(
        AdjustPlayStorePurchase purchase,
        Action<AdjustPurchaseVerificationResult> verificationResultCallback);

    public static partial void VerifyAndTrackPlayStorePurchase(
        AdjustEvent adjustEvent,
        Action<AdjustPurchaseVerificationResult> verificationResultCallback);

    #elif IOS
    public static partial void RequestAppTrackingAuthorization(Action<int> callback);

    public static partial void TrackAppStoreSubscription(AdjustAppStoreSubscription subscription);

    public static partial void UpdateSkanConversionValue(
        int conversionValue,
        string coarseValue,
        bool lockWindow,
        Action<string> callback);

    public static partial int GetAppTrackingAuthorizationStatus();

    public static partial void GetIdfa(Action<string> callback);

    public static partial void GetIdfv(Action<string> callback);

    public static partial void VerifyAppStorePurchase(
        AdjustAppStorePurchase purchase,
        Action<AdjustPurchaseVerificationResult> callback);

    public static partial void VerifyAndTrackAppStorePurchase(
        AdjustEvent adjustEvent,
        Action<AdjustPurchaseVerificationResult> callback);

    #endif
}
