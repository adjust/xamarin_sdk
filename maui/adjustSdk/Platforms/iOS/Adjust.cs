using System.Diagnostics;

namespace adjustSdk;

using System;
using Foundation;

// All the code in this file is only included on iOS.
public partial class Adjust {
    #region Platform common

    public static partial void InitSdk(AdjustConfig adjustConfig) {
        adjustSdk.iOSBinding.Adjust.InitSdk(adjustConfig.toNative());
    }

    public static partial void TrackEvent(AdjustEvent adjustEvent) {
        adjustSdk.iOSBinding.Adjust.TrackEvent(adjustEvent.toNative());
    }

    public static partial void Enable() {
        adjustSdk.iOSBinding.Adjust.Enable();
    }
    public static partial void Disable(){
        adjustSdk.iOSBinding.Adjust.Disable();
    }

    public static partial void IsEnabled(Action<bool> callback) {
        adjustSdk.iOSBinding.Adjust.IsEnabledWithCompletionHandler((bool isEnabled) => {
            callback(isEnabled);
        });
    }

    public static partial void SwitchToOfflineMode() {
        adjustSdk.iOSBinding.Adjust.SwitchToOfflineMode();
    }
    public static partial void SwitchBackToOnlineMode() {
        adjustSdk.iOSBinding.Adjust.SwitchBackToOnlineMode();
    }

    public static partial void SetPushToken(string pushToken) {
        adjustSdk.iOSBinding.Adjust.SetPushTokenAsString(pushToken);
    }

    public static partial void GdprForgetMe() {
        adjustSdk.iOSBinding.Adjust.GdprForgetMe();
    }

    public static partial void ProcessDeeplink(AdjustDeeplink deeplink) {
        adjustSdk.iOSBinding.Adjust.ProcessDeeplink(
            new adjustSdk.iOSBinding.ADJDeeplink(new NSUrl(deeplink.Deeplink)));
    }

    public static partial void AddGlobalPartnerParameter(string key, string value) {
        adjustSdk.iOSBinding.Adjust.AddGlobalPartnerParameter(value, key);
    }
    public static partial void AddGlobalCallbackParameter(string key, string value) {
        adjustSdk.iOSBinding.Adjust.AddGlobalCallbackParameter(value, key);
    }

    public static partial void RemoveGlobalPartnerParameter(string key) {
        adjustSdk.iOSBinding.Adjust.RemoveGlobalPartnerParameterForKey(key);
    }
    public static partial void RemoveGlobalCallbackParameter(string key) {
        adjustSdk.iOSBinding.Adjust.RemoveGlobalCallbackParameterForKey(key);
    }

    public static partial void RemoveGlobalPartnerParameters() {
        adjustSdk.iOSBinding.Adjust.RemoveGlobalPartnerParameters();
    }
    public static partial void RemoveGlobalCallbackParameters() {
        adjustSdk.iOSBinding.Adjust.RemoveGlobalCallbackParameters();
    }

    public static partial void TrackAdRevenue(AdjustAdRevenue adRevenue) {
        adjustSdk.iOSBinding.Adjust.TrackAdRevenue(adRevenue.toNative());
    }

    public static partial void TrackThirdPartySharing(AdjustThirdPartySharing thirdPartySharing) {
        adjustSdk.iOSBinding.Adjust.TrackThirdPartySharing(thirdPartySharing.toNative());
    }

    public static partial void TrackMeasurementConsent(bool measurementConsent) {
        adjustSdk.iOSBinding.Adjust.TrackMeasurementConsent(measurementConsent);
    }

    public static partial void GetAdid(Action<string> callback) {
        adjustSdk.iOSBinding.Adjust.AdidWithCompletionHandler((string? adid) => {
            if (adid is not null) {
                callback(adid);
            }
        });
    }

    public static partial void GetAttribution(Action<AdjustAttribution> callback) {
        adjustSdk.iOSBinding.Adjust.AttributionWithCompletionHandler(
            (adjustSdk.iOSBinding.ADJAttribution? attribution) => {
            AdjustAttribution? adjustAttribution = AdjustAttribution.fromNative(attribution);

            if (adjustAttribution is null) { return; }

            callback(adjustAttribution);
        });
    }

    public static partial void GetSdkVersion(Action<string> callback) {
        adjustSdk.iOSBinding.Adjust.SdkVersionWithCompletionHandler((string? sdkVersion) => {
            if (sdkVersion is not null) {
                callback($"{AdjustConfig.SdkPrefix}@{sdkVersion}");
            }
        });
    }

    public static partial void GetLastDeeplink(Action<string?> callback) {
        adjustSdk.iOSBinding.Adjust.LastDeeplinkWithCompletionHandler((NSUrl? deeplink) => {
            callback(deeplink?.ToString());
        });
    }

    public static partial void ProcessAndResolveDeeplink(
        AdjustDeeplink deeplink, Action<string> callback)
    {
        adjustSdk.iOSBinding.Adjust.ProcessAndResolveDeeplink(
            new adjustSdk.iOSBinding.ADJDeeplink(new NSUrl(deeplink.Deeplink)),
            (string? resolvedLink) => {
                if (resolvedLink is not null) { callback(resolvedLink); }
            });
    }

    public static partial void setTestOptions(Dictionary<string, object> testOptions) {
        adjustSdk.iOSBinding.Adjust.SetTestOptions(NSDictionary.FromObjectsAndKeys(
            testOptions.Values.ToArray(),
            testOptions.Keys.ToArray()
        ));
    }

    public static partial void Resume() {
        adjustSdk.iOSBinding.Adjust.TrackSubsessionStart();
    }
    public static partial void Pause() {
        adjustSdk.iOSBinding.Adjust.TrackSubsessionEnd();
    }

    #endregion

    #region iOs specific
    public static partial void RequestAppTrackingAuthorization(Action<int> callback) {
        adjustSdk.iOSBinding.Adjust
            .RequestAppTrackingAuthorizationWithCompletionHandler(
                (nuint status) => callback((int) status));

    }

    public static partial void TrackAppStoreSubscription(AdjustAppStoreSubscription subscription) {
        adjustSdk.iOSBinding.Adjust.TrackAppStoreSubscription(subscription.toNative());
    }

    public static partial void UpdateSkanConversionValue(
        int conversionValue,
        string coarseValue,
        bool lockWindow,
        Action<string> callback)
    {
        adjustSdk.iOSBinding.Adjust.UpdateSkanConversionValue(
            (nint)conversionValue,
            coarseValue,
            NSNumber.FromBoolean(lockWindow),
            (NSError nsError) => callback(nsError.ToString())
        );
    }

    public static partial int GetAppTrackingAuthorizationStatus() {
        return adjustSdk.iOSBinding.Adjust.AppTrackingAuthorizationStatus;
    }

    public static partial void GetIdfa(Action<string> callback) {
        adjustSdk.iOSBinding.Adjust.IdfaWithCompletionHandler((string? idfa) => {
            if (idfa is not null) {
                callback(idfa);
            }
        });
    }

    public static partial void GetIdfv(Action<string> callback) {
        adjustSdk.iOSBinding.Adjust.IdfvWithCompletionHandler((string? idfv) => {
            if (idfv is not null) {
                callback(idfv);
            }
        });
    }

    public static partial void VerifyAppStorePurchase(
        AdjustAppStorePurchase purchase,
        Action<AdjustPurchaseVerificationResult> callback)
    {
        adjustSdk.iOSBinding.Adjust.VerifyAppStorePurchase(
            purchase.toNative(),
            (adjustSdk.iOSBinding.ADJPurchaseVerificationResult result) => {
                callback(new (result.VerificationStatus, result.Code, result.Message));
            }
        );
    }

    public static partial void VerifyAndTrackAppStorePurchase(
        AdjustEvent adjustEvent,
        Action<AdjustPurchaseVerificationResult> callback)
    {
        adjustSdk.iOSBinding.Adjust.VerifyAndTrackAppStorePurchase(
            adjustEvent.toNative(),
            (adjustSdk.iOSBinding.ADJPurchaseVerificationResult result) => {
                callback(new (result.VerificationStatus, result.Code, result.Message));
            }
        );
    }
    #endregion
}
