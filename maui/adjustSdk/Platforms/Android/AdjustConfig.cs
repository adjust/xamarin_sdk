namespace adjustSdk;

public partial class AdjustConfig {
    public bool? IsPlayStoreKidsComplianceEnabled { get; set; }
    public bool? IsPreinstallTrackingEnabled { get; set; }
    public string? PreinstallFilePath { get; set; }
    public string? FbAppId { get; set; }

    internal Com.Adjust.Sdk.AdjustConfig toNative() {
        string? nativeEnvironment = Environment switch {
            AdjustEnvironment.Sandbox => Com.Adjust.Sdk.AdjustConfig.EnvironmentSandbox,
            AdjustEnvironment.Production => Com.Adjust.Sdk.AdjustConfig.EnvironmentProduction,
            _ => null
        };

        Com.Adjust.Sdk.AdjustConfig nativeAdjustConfig =
            AllowSuppressLogLevel switch {
                bool allowSuppressLogLevel => new Com.Adjust.Sdk.AdjustConfig(
                    Adjust.AppContext, AppToken, nativeEnvironment, allowSuppressLogLevel),
                _ => new Com.Adjust.Sdk.AdjustConfig(
                    Adjust.AppContext, AppToken, nativeEnvironment)
            };

        Com.Adjust.Sdk.LogLevel? nativeLogLevel =
            LogLevel switch {
                AdjustLogLevel.VERBOSE => Com.Adjust.Sdk.LogLevel.Verbose,
                AdjustLogLevel.DEBUG => Com.Adjust.Sdk.LogLevel.Debug,
                AdjustLogLevel.INFO => Com.Adjust.Sdk.LogLevel.Info,
                AdjustLogLevel.WARN => Com.Adjust.Sdk.LogLevel.Warn,
                AdjustLogLevel.ERROR => Com.Adjust.Sdk.LogLevel.Error,
                AdjustLogLevel.ASSERT => Com.Adjust.Sdk.LogLevel.Assert,
                AdjustLogLevel.SUPPRESS => Com.Adjust.Sdk.LogLevel.Supress,
                //AdjustLogLevel.SUPPRESS => Com.Adjust.Sdk.LogLevel.Suppress,
                _ => null
            };
        if (nativeLogLevel is Com.Adjust.Sdk.LogLevel nativeLogLevelValue) {
            nativeAdjustConfig.SetLogLevel(nativeLogLevelValue);
        }

        nativeAdjustConfig.SdkPrefix = AdjustConfig.SdkPrefix;

        if (IsDeviceIdsReadingOnceEnabled is true) {
            nativeAdjustConfig.EnableDeviceIdsReadingOnce();
        }

        if (IsCoppaComplianceEnabled is true) {
            nativeAdjustConfig.EnableCoppaCompliance();
        }

        if (IsSendingInBackgroundEnabled is true) {
            nativeAdjustConfig.EnableSendingInBackground();
        }

        if (IsCostDataInAttributionEnabled is true) {
            nativeAdjustConfig.EnableCostDataInAttribution();
        }

        if (DefaultTracker is string defaultTrackerValue) {
            nativeAdjustConfig.DefaultTracker = defaultTrackerValue;
        }

        if (ExternalDeviceId is string externalDeviceIdValue) {
            nativeAdjustConfig.ExternalDeviceId = externalDeviceIdValue;
        }

        if (EventDeduplicationIdsMaxSize is int eventDeduplicationIdsMaxSizeValue) {
            nativeAdjustConfig.EventDeduplicationIdsMaxSize =
                Java.Lang.Integer.ValueOf(eventDeduplicationIdsMaxSizeValue);
        }

        if (UrlStrategyDomains is List<string> urlStrategyDomainsValue) {
            nativeAdjustConfig.SetUrlStrategy(
                urlStrategyDomainsValue,
                ShouldUseSubdomains?? false,
                IsDataResidency?? false);
        }

        if (AttributionChangedDelegate is Action<AdjustAttribution> attributionChangedDelegateValue) {
            nativeAdjustConfig.OnAttributionChangedListener =
                new OnAttributionChangedListenerAdapter(attributionChangedDelegateValue);
        }

        if (EventSuccessDelegate is Action<AdjustEventSuccess> eventSuccessDelegateValue) {
            nativeAdjustConfig.OnEventTrackingSucceededListener =
                new OnEventTrackingSucceededListenerAdapter(eventSuccessDelegateValue);
        }

        if (EventFailureDelegate is Action<AdjustEventFailure> eventFailureDelegateValue) {
            nativeAdjustConfig.OnEventTrackingFailedListener =
                new OnEventTrackingFailedListenerAdapter(eventFailureDelegateValue);
        }

        if (SessionSuccessDelegate is Action<AdjustSessionSuccess> sessionSuccessDelegateValue) {
            nativeAdjustConfig.OnSessionTrackingSucceededListener =
                new OnSessionTrackingSucceededListenerAdapter(sessionSuccessDelegateValue);
        }

        if (SessionFailureDelegate is Action<AdjustSessionFailure> sessionFailureDelegateValue) {
            nativeAdjustConfig.OnSessionTrackingFailedListener =
                new OnSessionTrackingFailedListenerAdapter(sessionFailureDelegateValue);
        }

        if (DeferredDeeplinkDelegate is Func<string, bool> deferredDeeplinkDelegateValue) {
            nativeAdjustConfig.SetOnDeferredDeeplinkResponseListener(
                new OnDeferredDeeplinkResponseListenerAdapter(deferredDeeplinkDelegateValue));
        }

        if (IsPlayStoreKidsComplianceEnabled is true) {
            nativeAdjustConfig.EnablePlayStoreKidsCompliance();
        }

        if (IsPreinstallTrackingEnabled is true) {
            nativeAdjustConfig.EnablePreinstallTracking();
        }

        if (PreinstallFilePath is string preinstallFilePathValue) {
            nativeAdjustConfig.PreinstallFilePath = preinstallFilePathValue;
        }

        if (FbAppId is string fbAppIdValue) {
            nativeAdjustConfig.FbAppId = fbAppIdValue;
        }

        return nativeAdjustConfig;
    }
}

public partial class AdjustAttribution {
    public string? FbInstallReferrer { get; private set; }

    internal static AdjustAttribution? fromNative(
        Com.Adjust.Sdk.AdjustAttribution? nativeAdjustAttribution)
    {
        if (nativeAdjustAttribution is null) { return null; }

        return new()
        {
            TrackerToken = nativeAdjustAttribution.TrackerToken,
            TrackerName = nativeAdjustAttribution.TrackerName,
            Network = nativeAdjustAttribution.Network,
            Campaign = nativeAdjustAttribution.Campaign,
            Adgroup = nativeAdjustAttribution.Adgroup,
            Creative = nativeAdjustAttribution.Creative,
            ClickLabel = nativeAdjustAttribution.ClickLabel,
            CostType = nativeAdjustAttribution.CostType,
            CostAmount = nativeAdjustAttribution.CostAmount?.DoubleValue(),
            CostCurrency = nativeAdjustAttribution.CostCurrency,
            FbInstallReferrer = nativeAdjustAttribution.FbInstallReferrer
        };
    }
}
internal class OnAttributionChangedListenerAdapter(Action<AdjustAttribution> AttributionChangedDelegate)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnAttributionChangedListener
{
    public void OnAttributionChanged(Com.Adjust.Sdk.AdjustAttribution? nativeAdjustAttribution)
    {
        if (AdjustAttribution.fromNative(nativeAdjustAttribution)
        is AdjustAttribution adjustAttribution)
        {
            AttributionChangedDelegate(adjustAttribution);
        }
    }
}

public partial class AdjustEventSuccess {
    public Org.Json.JSONObject? JsonResponse { get; private set; }

    internal static AdjustEventSuccess? fromNativeAdjustEventSuccess(
        Com.Adjust.Sdk.AdjustEventSuccess? nativeAdjustEventSuccess)
    {
        if (nativeAdjustEventSuccess is null) { return null; }

        return new()
        {
            Adid = nativeAdjustEventSuccess.Adid,
            Message = nativeAdjustEventSuccess.Message,
            Timestamp = nativeAdjustEventSuccess.Timestamp,
            EventToken = nativeAdjustEventSuccess.EventToken,
            CallbackId = nativeAdjustEventSuccess.CallbackId,
            JsonResponse = nativeAdjustEventSuccess.JsonResponse
        };
    }
}
internal class OnEventTrackingSucceededListenerAdapter(Action<AdjustEventSuccess> EventSuccessDelegate)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnEventTrackingSucceededListener
{
    public void OnEventTrackingSucceeded (Com.Adjust.Sdk.AdjustEventSuccess? nativeAdjustEventSuccess)
    {
        if (AdjustEventSuccess.fromNativeAdjustEventSuccess(nativeAdjustEventSuccess)
        is AdjustEventSuccess adjustEventSuccess)
        {
            EventSuccessDelegate(adjustEventSuccess);
        }
    }
}

public partial class AdjustEventFailure {
    public Org.Json.JSONObject? JsonResponse { get; private set; }

    internal static AdjustEventFailure? fromNativeAdjustEventFailure(
        Com.Adjust.Sdk.AdjustEventFailure? nativeAdjustEventFailure)
    {
        if (nativeAdjustEventFailure is null) { return null; }

        return new()
        {
            Adid = nativeAdjustEventFailure.Adid,
            Message = nativeAdjustEventFailure.Message,
            Timestamp = nativeAdjustEventFailure.Timestamp,
            EventToken = nativeAdjustEventFailure.EventToken,
            CallbackId = nativeAdjustEventFailure.CallbackId,
            WillRetry = nativeAdjustEventFailure.WillRetry,
            JsonResponse = nativeAdjustEventFailure.JsonResponse
        };
    }
}
internal class OnEventTrackingFailedListenerAdapter(Action<AdjustEventFailure> EventFailureDelegate)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnEventTrackingFailedListener
{
    public void OnEventTrackingFailed(Com.Adjust.Sdk.AdjustEventFailure? nativeAdjustEventFailure) {
        if (AdjustEventFailure.fromNativeAdjustEventFailure(nativeAdjustEventFailure)
        is AdjustEventFailure adjustEventFailure)
        {
            EventFailureDelegate(adjustEventFailure);
        }
    }
}

public partial class AdjustSessionSuccess {
    public Org.Json.JSONObject? JsonResponse { get; private set; }

    internal static AdjustSessionSuccess? fromNativeAdjustSessionSuccess(
        Com.Adjust.Sdk.AdjustSessionSuccess? nativeAdjustSessionSuccess)
    {
        if (nativeAdjustSessionSuccess is null) { return null; }

        return new()
        {
            Adid = nativeAdjustSessionSuccess.Adid,
            Message = nativeAdjustSessionSuccess.Message,
            Timestamp = nativeAdjustSessionSuccess.Timestamp,
            JsonResponse = nativeAdjustSessionSuccess.JsonResponse
        };
    }
}
internal class OnSessionTrackingSucceededListenerAdapter(Action<AdjustSessionSuccess> SessionSuccessDelegate)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnSessionTrackingSucceededListener
{
    public void OnSessionTrackingSucceeded (Com.Adjust.Sdk.AdjustSessionSuccess? nativeAdjustSessionSuccess)
    {
        if (AdjustSessionSuccess.fromNativeAdjustSessionSuccess(nativeAdjustSessionSuccess)
        is AdjustSessionSuccess adjustSessionSuccess)
        {
            SessionSuccessDelegate(adjustSessionSuccess);
        }
    }
}

public partial class AdjustSessionFailure {
    public Org.Json.JSONObject? JsonResponse { get; private set; }

    internal static AdjustSessionFailure? fromNativeAdjustSessionFailure(
        Com.Adjust.Sdk.AdjustSessionFailure? nativeAdjustSessionFailure)
    {
        if (nativeAdjustSessionFailure is null) { return null; }

        return new()
        {
            Adid = nativeAdjustSessionFailure.Adid,
            Message = nativeAdjustSessionFailure.Message,
            Timestamp = nativeAdjustSessionFailure.Timestamp,
            WillRetry = nativeAdjustSessionFailure.WillRetry,
            JsonResponse = nativeAdjustSessionFailure.JsonResponse
        };
    }
}
internal class OnSessionTrackingFailedListenerAdapter(Action<AdjustSessionFailure> SessionFailureDelegate)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnSessionTrackingFailedListener
{
    public void OnSessionTrackingFailed(Com.Adjust.Sdk.AdjustSessionFailure? nativeAdjustSessionFailure)
    {
        if (AdjustSessionFailure.fromNativeAdjustSessionFailure(nativeAdjustSessionFailure)
        is AdjustSessionFailure adjustSessionFailure)
        {
            SessionFailureDelegate(adjustSessionFailure);
        }
    }
}

internal class OnDeferredDeeplinkResponseListenerAdapter(Func<string, bool> DeferredDeeplinkDelegate)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnDeferredDeeplinkResponseListener
{
    public bool LaunchReceivedDeeplink (Android.Net.Uri? nativeDeeplink) {
        if (nativeDeeplink?.ToString() is string deeplink) {
            return DeferredDeeplinkDelegate(deeplink);
        }

        return false;
    }
}

internal class OnIsEnabledListenerAdapter(Action<bool> IsEnabledCallback)
    : Java.Lang.Object, Com.Adjust.Sdk.IOnIsEnabledListener
{
    public void OnIsEnabledRead (bool isEnabled) {
        IsEnabledCallback.Invoke(isEnabled);
    }
}