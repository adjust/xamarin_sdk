using System.Text.Json;
using adjustSdk;

public partial class TestLibraryBridge {
    private readonly Dictionary<int, AdjustConfig> savedConfigs = new();
    private readonly Dictionary<int, AdjustEvent> savedEvents = new();
    private string? currentExtraPath;

    private string overwriteUrl { get ; init; }
    private string controlUrl { get ; init; }

    public partial void start();
    public partial void addTest(string testName);
    public partial void addTestDirectory(string testDirectory);
    private partial void addInfoToSend(string key, string value);
    private partial void setInfoToServer(IDictionary<string, string>? infoToSend);
    private partial void sendInfoToServer(string? extraPath);

#region Commands
    internal void executeCommon(string className, string methodName, string jsonParameters) {
        var parameters = 
            JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonParameters);

        if (parameters is null) { return; }

        switch (methodName) {
            case "testOptions": testOptions(parameters); break;
            case "config": configNative(parameters); break;
            case "start": start(parameters); break;
            case "event": eventNative(parameters); break;
            case "trackEvent": trackEvent(parameters); break;
            case "resume": resume(parameters); break;
            case "pause": pause(parameters); break;
            case "setEnabled": setEnabled(parameters); break;
            case "setOfflineMode": setOfflineMode(parameters); break;
            case "addGlobalCallbackParameter": addGlobalCallbackParameter(parameters); break;
            case "addGlobalPartnerParameter": addGlobalPartnerParameter(parameters); break;
            case "removeGlobalCallbackParameter": removeGlobalCallbackParameter(parameters); break;
            case "removeGlobalPartnerParameter": removeGlobalPartnerParameter(parameters); break;
            case "removeGlobalCallbackParameters": removeGlobalCallbackParameters(parameters); break;
            case "removeGlobalPartnerParameters": removeGlobalPartnerParameters(parameters); break;
            case "setPushToken": setPushToken(parameters); break;
            case "openDeeplink": openDeepLink(parameters); break;
            case "gdprForgetMe": gdprForgetMe(parameters); break;
            case "trackSubscription": trackSubscription(parameters); break;
            case "thirdPartySharing": thirdPartySharing(parameters); break;
            case "measurementConsent": measurementConsent(parameters); break;
            case "trackAdRevenue": trackAdRevenue(parameters); break;
            case "getLastDeeplink": getLastDeeplink(parameters); break;
            case "verifyPurchase": verifyPurchase(parameters); break;
            case "processDeeplink": processDeeplink(parameters); break;
            case "attributionGetter": attributionGetter(parameters); break;
            case "verifyTrack": verifyTrack(parameters); break;
        }
    }

    private void testOptions(Dictionary<string, List<string>> parameters) {
        Dictionary<string, object> testOptions = new() {
            {"baseUrl", overwriteUrl},
            {"gdprUrl", overwriteUrl},
            {"subscriptionUrl", overwriteUrl},
            {"purchaseVerificationUrl", overwriteUrl},
            {"testUrlOverwrite", overwriteUrl},
        };

        if (firstStringValue(parameters, "basePath") is string basePath) {
            currentExtraPath = basePath;
        }

        if (firstLongValue(parameters, "timerInterval") is long timerInterval) {
            testOptions.Add("timerIntervalInMilliseconds", timerInterval);
        }

        if (firstLongValue(parameters, "timerStart") is long timerStart) {
            testOptions.Add("timerStartInMilliseconds", timerStart);
        }

        if (firstLongValue(parameters, "sessionInterval") is long sessionInterval) {
            testOptions.Add("sessionIntervalInMilliseconds", sessionInterval);
        }

        if (firstLongValue(parameters, "subsessionInterval") is long subsessionInterval) {
            testOptions.Add("subsessionIntervalInMilliseconds", subsessionInterval);
        }

        if (firstBoolValue(parameters, "noBackoffWait") is bool noBackoffWait) {
            testOptions.Add("noBackoffWait", noBackoffWait);
        }

#if IOS
        // AdServices.framework will not be used in test app by default
        testOptions.Add("adServicesFrameworkEnabled",
            firstBoolValue(parameters, "adServicesFrameworkEnabled") ?? false);

        if (firstIntValue(parameters, "attStatus") is int attStatus) {
            testOptions.Add("attStatusInt", attStatus);
        }

        if (firstStringValue(parameters, "idfa") is string idfa) {
            testOptions.Add("idfa", idfa);
        }
#endif

        if (firstBoolValue(parameters, "doNotIgnoreSystemLifecycleBootstrap") is true) {
            testOptions.Add("ignoreSystemLifecycleBootstrap", false);
        }

#if ANDROID
        bool useTestConnectionOptions = false;
#endif

        if (listValues(parameters, "teardown") is List<string> teardownOptions) {
            foreach (string teardownOption in teardownOptions) {
                if (teardownOption == "resetSdk") {
                    testOptions.Add("teardown", true);
                    if (currentExtraPath is not null) {
                        testOptions.Add("extraPath", currentExtraPath);
                    }
#if ANDROID
                    useTestConnectionOptions = true;
#endif
                } else if (teardownOption == "deleteState") {
                    testOptions.Add("deleteState", true);
                } else if (teardownOption == "resetTest") {
                    savedConfigs.Clear();
                    savedEvents.Clear();
                    testOptions.Add("timerIntervalInMilliseconds", -1L);
                    testOptions.Add("timerStartInMilliseconds", -1L);
                    testOptions.Add("sessionIntervalInMilliseconds", -1L);
                    testOptions.Add("subsessionIntervalInMilliseconds", -1L);
                } else if (teardownOption == "sdk") {
                    testOptions.Add("teardown", "true");
                    testOptions.Remove("extraPath");
                } else if (teardownOption == "test") {
                    savedConfigs.Clear();
                    savedEvents.Clear();
                    testOptions.Add("timerIntervalInMilliseconds", -1L);
                    testOptions.Add("timerStartInMilliseconds", -1L);
                    testOptions.Add("sessionIntervalInMilliseconds", -1L);
                    testOptions.Add("subsessionIntervalInMilliseconds", -1L);
                }
            }
        }

        Adjust.setTestOptions(testOptions);

#if ANDROID
        if (useTestConnectionOptions) {
            Com.Adjust.Test_options.TestConnectionOptions.SetTestConnectionOptions();
        }
#endif
    }

    private AdjustConfig? configNative(Dictionary<string, List<string>> parameters) {
        if (! Int32.TryParse(firstStringValue(parameters, "configName"), out int configNumber)) {
            configNumber = 0;
        }

        if (! savedConfigs.TryGetValue(configNumber, out AdjustConfig? adjustConfig)) {
            string? appToken = firstStringValue(parameters, "appToken");
            AdjustEnvironment? environment =
                firstStringValue(parameters, "environment") switch {
                    "sandbox" => AdjustEnvironment.Sandbox,
                    "production"  => AdjustEnvironment.Production,
                    _ => null,
                };

            if (!(appToken is string appTokenValid
                && environment is AdjustEnvironment environmentValid))
            {
                return null;
            }

            adjustConfig = new (appTokenValid, environmentValid);
            savedConfigs.Add(configNumber, adjustConfig);
        }

        AdjustLogLevel? adjustLogLevel = firstStringValue(parameters, "appToken") switch {
            "verbose" => AdjustLogLevel.VERBOSE,
            "debug" => AdjustLogLevel.DEBUG,
            "info" => AdjustLogLevel.INFO,
            "warn" => AdjustLogLevel.WARN,
            "error" => AdjustLogLevel.ERROR,
            "assert" => AdjustLogLevel.ASSERT,
            "suppress" => AdjustLogLevel.SUPPRESS,
            _ => null };
        if (adjustLogLevel is not null) {
            adjustConfig.LogLevel = adjustLogLevel;
        }

        // sdk prefix not tested from non-natives

        if (firstStringValue(parameters, "defaultTracker") is string defaultTracker) {
            adjustConfig.DefaultTracker = defaultTracker;
        }

        if (firstBoolValue(parameters, "needsCost") is true) {
            adjustConfig.IsCostDataInAttributionEnabled = true;
        }

        if (firstBoolValue(parameters, "sendInBackground") is true) {
            adjustConfig.IsSendingInBackgroundEnabled = true;
        }

        if (firstIntValue(parameters, "eventDeduplicationIdsMaxSize") 
            is int eventDeduplicationIdsMaxSize) 
        {
            adjustConfig.EventDeduplicationIdsMaxSize = eventDeduplicationIdsMaxSize;
        }

        if (firstStringValue(parameters, "externalDeviceId") is string externalDeviceId) {
            adjustConfig.ExternalDeviceId = externalDeviceId;
        }

        if (firstBoolValue(parameters, "coppaCompliant") is true) {
            adjustConfig.IsCoppaComplianceEnabled = true;
        }

#if ANDROID
        if (firstBoolValue(parameters, "playStoreKids") is true) {
            adjustConfig.IsPlayStoreKidsComplianceEnabled = true;
        }
        /* not being tested:
            IsPreinstallTrackingEnabled
            PreinstallFilePath
            FbAppId
        */
#elif IOS

        if (firstBoolValue(parameters, "allowIdfaReading") is false) {
            adjustConfig.IsIdfaReadingEnabled = false;
        }

        if (firstBoolValue(parameters, "allowAdServicesInfoReading") is false) {
            adjustConfig.IsAdServicesEnabled = false;
        }

        if (firstBoolValue(parameters, "allowSkAdNetworkHandling") is false) {
            adjustConfig.IsSkanAttributionEnabled = false;
        }

        if (firstIntValue(parameters, "attConsentWaitingSeconds") is int attConsentWaitingSeconds) {
            adjustConfig.AttConsentWaitingInterval = attConsentWaitingSeconds;
        }

        if (parameters.ContainsKey("skanCallback")) {
            string? localBasePath = currentExtraPath;

            adjustConfig.SkanUpdatedDelegate = (Dictionary<string, string> data) => {
                setInfoToServer(data);
                sendInfoToServer(localBasePath);
            };
        }
#endif
        if (parameters.ContainsKey("attributionCallbackSendAll")) {
            adjustConfig.AttributionChangedDelegate = attributionCallback(currentExtraPath);
        }

        if (parameters.ContainsKey("sessionCallbackSendSuccess")) {
            string? localBasePath = currentExtraPath;
            adjustConfig.SessionSuccessDelegate = (AdjustSessionSuccess adjustSessionSuccess) => {
                Dictionary<string, string> infoToSend = new();

                if (adjustSessionSuccess.Message is not null) {
                    infoToSend.Add("message", adjustSessionSuccess.Message); }
                if (adjustSessionSuccess.Timestamp is not null) {
                    infoToSend.Add("timestamp", adjustSessionSuccess.Timestamp); }
                if (adjustSessionSuccess.Adid is not null) {
                    infoToSend.Add("adid", adjustSessionSuccess.Adid); }
                if (jsonResponseConvert(adjustSessionSuccess.JsonResponse) is string jsonResponse) {
                    infoToSend.Add("jsonResponse", jsonResponse);
                }
                setInfoToServer(infoToSend);
                sendInfoToServer(localBasePath);
            };
        }

        if (parameters.ContainsKey("sessionCallbackSendFailure")) {
            string? localBasePath = currentExtraPath;
            adjustConfig.SessionFailureDelegate = (AdjustSessionFailure adjustSessionFailure) => {
                Dictionary<string, string> infoToSend = new();

                if (adjustSessionFailure.Message is not null) {
                    infoToSend.Add("message", adjustSessionFailure.Message); }
                if (adjustSessionFailure.Timestamp is not null) {
                    infoToSend.Add("timestamp", adjustSessionFailure.Timestamp); }
                if (adjustSessionFailure.Adid is not null) {
                    infoToSend.Add("adid", adjustSessionFailure.Adid); }
                infoToSend.Add("willRetry", adjustSessionFailure.WillRetry.ToString().ToLowerInvariant());
                if (jsonResponseConvert(adjustSessionFailure.JsonResponse) is string jsonResponse) {
                    infoToSend.Add("jsonResponse", jsonResponse);
                }
                setInfoToServer(infoToSend);
                sendInfoToServer(localBasePath);
            };
        }

        if (parameters.ContainsKey("eventCallbackSendSuccess")) {
            string? localBasePath = currentExtraPath;
            adjustConfig.EventSuccessDelegate = (AdjustEventSuccess adjustEventSuccess) => {
                Dictionary<string, string> infoToSend = new();

                if (adjustEventSuccess.Message is not null) {
                    infoToSend.Add("message", adjustEventSuccess.Message); }
                if (adjustEventSuccess.Timestamp is not null) {
                    infoToSend.Add("timestamp", adjustEventSuccess.Timestamp); }
                if (adjustEventSuccess.Adid is not null) {
                    infoToSend.Add("adid", adjustEventSuccess.Adid); }
                if (adjustEventSuccess.EventToken is not null) {
                    infoToSend.Add("eventToken", adjustEventSuccess.EventToken); }
                if (adjustEventSuccess.CallbackId is not null) {
                    infoToSend.Add("callbackId", adjustEventSuccess.CallbackId); }
                if (jsonResponseConvert(adjustEventSuccess.JsonResponse) is string jsonResponse) {
                    infoToSend.Add("jsonResponse", jsonResponse);
                }
                setInfoToServer(infoToSend);
                sendInfoToServer(localBasePath);
            };
        }

        if (parameters.ContainsKey("eventCallbackSendFailure")) {
            string? localBasePath = currentExtraPath;
            adjustConfig.EventFailureDelegate = (AdjustEventFailure adjustEventFailure) => {
                Dictionary<string, string> infoToSend = new();

                if (adjustEventFailure.Message is not null) {
                    infoToSend.Add("message", adjustEventFailure.Message); }
                if (adjustEventFailure.Timestamp is not null) {
                    infoToSend.Add("timestamp", adjustEventFailure.Timestamp); }
                if (adjustEventFailure.Adid is not null) {
                    infoToSend.Add("adid", adjustEventFailure.Adid); }
                if (adjustEventFailure.EventToken is not null) {
                    infoToSend.Add("eventToken", adjustEventFailure.EventToken); }
                if (adjustEventFailure.CallbackId is not null) {
                    infoToSend.Add("callbackId", adjustEventFailure.CallbackId); }
                infoToSend.Add("willRetry", adjustEventFailure.WillRetry.ToString().ToLowerInvariant());
                if (jsonResponseConvert(adjustEventFailure.JsonResponse) is string jsonResponse) {
                    infoToSend.Add("jsonResponse", jsonResponse);
                }
                setInfoToServer(infoToSend);
                sendInfoToServer(localBasePath);
            };
        }

        if (parameters.ContainsKey("deferredDeeplinkCallback")) {
            string? localBasePath = currentExtraPath;
            bool launchDeferredDeeplink = 
                firstBoolValue(parameters, "deferredDeeplinkCallback") is true;
            adjustConfig.DeferredDeeplinkDelegate  = (string deeplink) => {
                addInfoToSend("deeplink", deeplink);

                sendInfoToServer(localBasePath);

                return launchDeferredDeeplink;
            };
        }

        return adjustConfig;
    }

    private void start(Dictionary<string, List<string>> parameters) {
        if (configNative(parameters) is not AdjustConfig adjustConfig) { return; }

        Adjust.InitSdk(adjustConfig);

        savedConfigs.Remove(firstIntValue(parameters, "configName") ?? 0);
    }

    private AdjustEvent eventNative(Dictionary<string, List<string>> parameters) {
        if (! Int32.TryParse(firstStringValue(parameters, "eventName"), out int eventNumber)) {
            eventNumber = 0;
        }

        if (! savedEvents.TryGetValue(eventNumber, out AdjustEvent? adjustEvent)) {
            string eventToken = firstStringValue(parameters, "eventToken") ?? "";

            adjustEvent = new (eventToken);

            savedEvents.Add(eventNumber, adjustEvent);
        }

        if (revenueCurrencyValues(parameters) is (string currency, double amount)) {
            adjustEvent.SetRevenue(amount, currency);
        }

        iterateTwoPairList(listValues(parameters, "callbackParams"),
            adjustEvent.AddCallbackParameter);

        iterateTwoPairList(listValues(parameters, "partnerParams"),
            adjustEvent.AddPartnerParameter);

        if (firstStringValue(parameters, "callbackId") is string callbackId) {
            adjustEvent.CallbackId = callbackId;
        }

        if (firstStringValue(parameters, "productId") is string productId) {
            adjustEvent.ProductId = productId;
        }

        if (firstStringValue(parameters, "deduplicationId") is string deduplicationId) {
            adjustEvent.DeduplicationId = deduplicationId;
        }

#if ANDROID
        if (firstStringValue(parameters, "purchaseToken") is string purchaseToken) {
            adjustEvent.PurchaseToken = purchaseToken;
        }
#elif IOS
        if (firstStringValue(parameters, "transactionId") is string transactionId) {
            adjustEvent.TransactionId = transactionId;
        }
#endif

        return adjustEvent;
    }

    private void trackEvent(Dictionary<string, List<string>> parameters) {
        Adjust.TrackEvent(eventNative(parameters));

        savedEvents.Remove(firstIntValue(parameters, "eventName") ?? 0);
    }

    private void resume(Dictionary<string, List<string>> parameters) {
        Adjust.Resume();
    }
    private void pause(Dictionary<string, List<string>> parameters) {
        Adjust.Pause();
    }

    private void setEnabled(Dictionary<string, List<string>> parameters) {
        if (firstBoolValue(parameters, "enabled") is true) {
            Adjust.Enable();
        } else {
            Adjust.Disable();
        }
    }
    private void setOfflineMode(Dictionary<string, List<string>> parameters) {
        if (firstBoolValue(parameters, "enabled") is true) {
            Adjust.SwitchToOfflineMode();
        } else {
            Adjust.SwitchBackToOnlineMode();
        }
    }

    private void addGlobalCallbackParameter(Dictionary<string, List<string>> parameters) {
        iterateTwoPairList(listValues(parameters, "KeyValue"),
            Adjust.AddGlobalCallbackParameter);
    }
    private void addGlobalPartnerParameter(Dictionary<string, List<string>> parameters) {
        iterateTwoPairList(listValues(parameters, "KeyValue"),
            Adjust.AddGlobalPartnerParameter);
    }
    private void removeGlobalCallbackParameter(Dictionary<string, List<string>> parameters) {
        if (listValues(parameters, "key") is not List<string> keys) { return; }

        foreach (var key in keys) {
            Adjust.RemoveGlobalCallbackParameter(key);
        }
    }
    private void removeGlobalPartnerParameter(Dictionary<string, List<string>> parameters) {
        if (listValues(parameters, "key") is not List<string> keys) { return; }

        foreach (var key in keys) {
            Adjust.RemoveGlobalPartnerParameter(key);
        }
    }
    private void removeGlobalCallbackParameters(Dictionary<string, List<string>> parameters) {
        Adjust.RemoveGlobalCallbackParameters();
    }

    private void removeGlobalPartnerParameters(Dictionary<string, List<string>> parameters) {
        Adjust.RemoveGlobalPartnerParameters();
    }

    private void setPushToken(Dictionary<string, List<string>> parameters) {
        if (firstStringValue(parameters, "pushToken") is string pushToken) {
            // TODO try to change test app proj to allow null
            Adjust.SetPushToken(pushToken);
        }
    }

    private void openDeepLink(Dictionary<string, List<string>> parameters) {
        if (firstStringValue(parameters, "deeplink") is string deeplink) {
            Adjust.ProcessDeeplink(new AdjustDeeplink(deeplink));
        }
    }

    private void gdprForgetMe(Dictionary<string, List<string>> parameters) {
        Adjust.GdprForgetMe();
    }

    private void trackSubscription(Dictionary<string, List<string>> parameters) {
#if ANDROID
        trackPlayStoreSubscription(parameters);
#elif IOS
        trackAppStoreSubscription(parameters);
#endif
    }

    private void thirdPartySharing(Dictionary<string, List<string>> parameters) {
        AdjustThirdPartySharing adjustThirdPartySharing =
            new(firstBoolValue(parameters, "isEnabled"));

        iterateThreePairList(listValues(parameters, "granularOptions"),
            adjustThirdPartySharing.AddGranularOption);

        iterateThreePairList(listValues(parameters, "partnerSharingSettings"),
            (string partnerName, string key, string boolStrValue) =>
                adjustThirdPartySharing.AddPartnerSharingSettings(
                    partnerName, key, boolStrValue == "true"));

        Adjust.TrackThirdPartySharing(adjustThirdPartySharing);
    }

    private void measurementConsent(Dictionary<string, List<string>> parameters) {
        Adjust.TrackMeasurementConsent(firstBoolValue(parameters, "isEnabled") is true);
    }

    private void trackAdRevenue(Dictionary<string, List<string>> parameters) {
        if (firstStringValue(parameters, "adRevenueSource") is not string adRevenueSource) {
            return;
        }

        AdjustAdRevenue adRevenue = new(adRevenueSource);

        if (revenueCurrencyValues(parameters) is (string currency, double amount)) {
            adRevenue.SetRevenue(amount, currency);
        }

        if (firstIntValue(parameters, "adImpressionsCount") is int adImpressionsCount) {
            adRevenue.AdImpressionsCount = adImpressionsCount;
        }

        if (firstStringValue(parameters, "adRevenueUnit") is string adRevenueUnit) {
            adRevenue.AdRevenueUnit = adRevenueUnit;
        }

        if (firstStringValue(parameters, "adRevenuePlacement") is string adRevenuePlacement) {
            adRevenue.AdRevenuePlacement = adRevenuePlacement;
        }

        if (firstStringValue(parameters, "adRevenueNetwork") is string adRevenueNetwork) {
            adRevenue.AdRevenueNetwork = adRevenueNetwork;
        }

        iterateTwoPairList(listValues(parameters, "callbackParams"),
            adRevenue.AddCallbackParameter);

        iterateTwoPairList(listValues(parameters, "partnerParams"),
            adRevenue.AddPartnerParameter);

        Adjust.TrackAdRevenue(adRevenue);
    }

    private void getLastDeeplink(Dictionary<string, List<string>> parameters) {
        string? localBasePath = currentExtraPath;
        Adjust.GetLastDeeplink((string? lastDeeplink) => {
            addInfoToSend("last_deeplink", lastDeeplink ?? "");

            sendInfoToServer(localBasePath);
        });
    }

    private void verifyPurchase(Dictionary<string, List<string>> parameters) {
#if ANDROID
        verifyPlayStorePurchase(parameters);
#elif IOS
        verifyAppStorePurchase(parameters);
#endif
    }

    private void processDeeplink(Dictionary<string, List<string>> parameters) {
        if (firstStringValue(parameters, "deeplink") is not string deeplink) { return; }

        string? localBasePath = currentExtraPath;
        Adjust.ProcessAndResolveDeeplink(new AdjustDeeplink(deeplink), (string resolvedLink) => {
            addInfoToSend("resolved_link", resolvedLink);

            sendInfoToServer(localBasePath);
        });
    }

    private void attributionGetter(Dictionary<string, List<string>> parameters) {
        Adjust.GetAttribution(attributionCallback(currentExtraPath));
    }

    private void verifyTrack(Dictionary<string, List<string>> parameters) {
        AdjustEvent adjustEvent = eventNative(parameters);

        string? localBasePath = currentExtraPath;
#if ANDROID
        Adjust.VerifyAndTrackPlayStorePurchase(adjustEvent, verificationResultCallback(localBasePath));
#elif IOS
        Adjust.VerifyAndTrackAppStorePurchase(adjustEvent, verificationResultCallback(localBasePath));
#endif
    }
    #endregion

    private Action<AdjustPurchaseVerificationResult> verificationResultCallback(
        string? localBasePath) => (AdjustPurchaseVerificationResult result) => {
            addInfoToSend("verification_status", result.VerificationStatus ?? "");
            addInfoToSend("code", Convert.ToString(result.Code));
            addInfoToSend("message", result.Message ?? "");

            sendInfoToServer(localBasePath);
        };

    private Action<AdjustAttribution> attributionCallback(string? localBasePath) =>
        (AdjustAttribution attribution) => {
            Dictionary<string, string> infoToSend = new();

            if (attribution.TrackerToken is not null) {
                infoToSend.Add("tracker_token", attribution.TrackerToken); }
            if (attribution.TrackerName is not null) {
                infoToSend.Add("tracker_name", attribution.TrackerName); }
            if (attribution.Network is not null) {
                infoToSend.Add("network", attribution.Network); }
            if (attribution.Campaign is not null) {
                infoToSend.Add("campaign", attribution.Campaign); }
            if (attribution.Adgroup is not null) {
                infoToSend.Add("adgroup", attribution.Adgroup); }
            if (attribution.Creative is not null) {
                infoToSend.Add("creative", attribution.Creative); }
            if (attribution.ClickLabel is not null) {
                infoToSend.Add("click_label", attribution.ClickLabel); }
            if (attribution.CostType is not null) {
                infoToSend.Add("cost_type", attribution.CostType); }
            if (attribution.CostAmount is double costAmountValue) {
                infoToSend.Add("cost_amount", costAmountValue.ToString(
                    System.Globalization.CultureInfo.InvariantCulture)); }
            if (attribution.CostCurrency is not null) {
                infoToSend.Add("cost_currency", attribution.CostCurrency); }
    #if ANDROID
            if (attribution.FbInstallReferrer is not null) {
                infoToSend.Add("fb_install_referrer", attribution.FbInstallReferrer);
            }
    #endif

            setInfoToServer(infoToSend);
            sendInfoToServer(localBasePath);
        };

    private static (string, double)? revenueCurrencyValues(
        Dictionary<string, List<string>> parameters,
        string key = "revenue")
    {
        if (firstStringValue(parameters, key) is string currency
            && double.TryParse(
                listValues(parameters, key)?.ElementAt(1),
                System.Globalization.CultureInfo.InvariantCulture,
                out double amount))
        {
            return (currency, amount);
        } else {
            return null;
        }
    }
    private static List<string>? listValues(
        Dictionary<string, List<string>> parameters,
        string key)
    {
        parameters.TryGetValue(key, out List<string>? listValue);
        return listValue;
    }

    private static string? firstStringValue(
        Dictionary<string, List<string>> parameters,
        string key)
    {
        return listValues(parameters, key)?.FirstOrDefault();
    }

    private static bool? firstBoolValue(
        Dictionary<string, List<string>> parameters,
        string key)
    {
        return firstStringValue(parameters, key) switch {
            "true" => true,
            "false" => false,
            _ => null };
    }

    private static int? firstIntValue(
        Dictionary<string, List<string>> parameters,
        string key)
    {
        return Int32.TryParse(firstStringValue(parameters, key), out int result) ?
            result : null;
    }

    private static long? firstLongValue(
        Dictionary<string, List<string>> parameters,
        string key)
    {
        return Int64.TryParse(firstStringValue(parameters, key), out long result) ?
            result : null;
    }

    private static void iterateTwoPairList(
        List<string>? twoPairList, Action<string, string> twoPairApply)
    {
        if (twoPairList is null) { return; }

        for (int i = 0; i + 1 < twoPairList.Count; i = i + 2) {
            if (twoPairList[i] is string key && twoPairList[i + 1] is string value) {
                twoPairApply(key, value);
            }
        }
    }

    private static void iterateThreePairList(
        List<string>? threePairList, Action<string, string, string> threePairApply)
    {
        if (threePairList is null) { return; }

        for (int i = 0; i + 2 < threePairList.Count; i = i + 3) {
            if (threePairList[i] is string key
                && threePairList[i + 1] is string value
                && threePairList[i + 2] is string option)
            {
                threePairApply(key, value, option);
            }
        }
    }
}