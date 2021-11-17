using System;
using System.Collections.Generic;
using Android.Content;
using Com.Adjust.Sdk;
using Com.Adjust.Test;
using Com.Adjust.Test_options;
using Org.Json;
using Uri = Android.Net.Uri;

namespace TestApp
{
    public class AdjustCommandExecutor : Java.Lang.Object,
    IOnAttributionChangedListener, 
    IOnSessionTrackingFailedListener,
    IOnSessionTrackingSucceededListener,
    IOnEventTrackingFailedListener,
    IOnEventTrackingSucceededListener,
    IOnDeeplinkResponseListener
    {
        private string TAG = "[AdjustCommandExecutor]";
        private Context _context;
        private TestLibrary _testLibrary;
        private Dictionary<int, AdjustConfig> _savedConfigs = new Dictionary<int, AdjustConfig>();
        private Dictionary<int, AdjustEvent> _savedEvents = new Dictionary<int, AdjustEvent>();

        internal string BasePath;
        internal string GdprPath;
        internal string SubscriptionPath;
        internal Command Command;

        public AdjustCommandExecutor(Context context)
        {
            _context = context;
        }

        public void SetTestLibrary(TestLibrary testLibrary)
        {
            _testLibrary = testLibrary;
        }

        public void ExecuteCommand(Command command)
        {
            Command = command;
            try
            {
                Console.WriteLine(TAG + ": Executing method: {0}.{1}", command.ClassName, command.MethodName);
                switch (command.MethodName)
                {
                    case "testOptions": TestOptions(); break;
                    case "config": Config(); break;
                    case "start": Start(); break;
                    case "event": Event(); break;
                    case "trackEvent": TrackEvent(); break;
                    case "resume": Resume(); break;
                    case "pause": Pause(); break;
                    case "setEnabled": SetEnabled(); break;
                    case "setReferrer": SetReferrer(); break;
                    case "setOfflineMode": SetOfflineMode(); break;
                    case "sendFirstPackages": SendFirstPackages(); break;
                    case "addSessionCallbackParameter": AddSessionCallbackParameter(); break;
                    case "addSessionPartnerParameter": AddSessionPartnerParameter(); break;
                    case "removeSessionCallbackParameter": RemoveSessionCallbackParameter(); break;
                    case "removeSessionPartnerParameter": RemoveSessionPartnerParameter(); break;
                    case "resetSessionCallbackParameters": ResetSessionCallbackParameters(); break;
                    case "resetSessionPartnerParameters": ResetSessionPartnerParameters(); break;
                    case "setPushToken": SetPushToken(); break;
                    case "openDeeplink": OpenDeeplink(); break;
                    case "sendReferrer": SendReferrer(); break;
                    case "gdprForgetMe": GdprForgetMe(); break;
                    case "trackAdRevenue" : TrackAdRevenue(); break;
                    case "disableThirdPartySharing": DisableThirdPartySharing(); break;
                    case "trackSubscription": TrackSubscription(); break;
                    case "thirdPartySharing": TrackThirdPartySharing(); break;
                    case "measurementConsent": TrackMeasurementConsent(); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(TAG + ": {0} ---- {1}", "executeCommand: failed to parse command. Check commands' syntax", ex.ToString());
            }
        }

        private void TestOptions()
        {
            AdjustTestOptions testOptions = new AdjustTestOptions();
            testOptions.BaseUrl = MainActivity.BaseUrl;
            testOptions.GdprUrl = MainActivity.GdprUrl;
            testOptions.SubscriptionUrl = MainActivity.SubscriptionUrl;

            if (Command.ContainsParameter("basePath"))
            {
                BasePath = Command.GetFirstParameterValue("basePath");
                GdprPath = Command.GetFirstParameterValue("basePath");
                SubscriptionPath = Command.GetFirstParameterValue("basePath");
            }

            if (Command.ContainsParameter("timerInterval"))
            {
                long timerInterval = long.Parse(Command.GetFirstParameterValue("timerInterval"));
                testOptions.TimerIntervalInMilliseconds = new Java.Lang.Long(timerInterval);
            }

            if (Command.ContainsParameter("timerStart"))
            {
                long timerStart = long.Parse(Command.GetFirstParameterValue("timerStart"));
                testOptions.TimerStartInMilliseconds = new Java.Lang.Long(timerStart);
            }

            if (Command.ContainsParameter("sessionInterval"))
            {
                long sessionInterval = long.Parse(Command.GetFirstParameterValue("sessionInterval"));
                testOptions.SessionIntervalInMilliseconds = new Java.Lang.Long(sessionInterval);
            }

            if (Command.ContainsParameter("subsessionInterval"))
            {
                long subsessionInterval = long.Parse(Command.GetFirstParameterValue("subsessionInterval"));
                testOptions.SubsessionIntervalInMilliseconds = new Java.Lang.Long(subsessionInterval);
            }

            if (Command.ContainsParameter("tryInstallReferrer"))
            {
                String tryInstallReferrerString = Command.GetFirstParameterValue("tryInstallReferrer");
                bool tryInstallReferrer;
                if (bool.TryParse(tryInstallReferrerString, out tryInstallReferrer))
                {
                    testOptions.TryInstallReferrer = new Java.Lang.Boolean(tryInstallReferrer);
                }
            }

            if (Command.ContainsParameter("noBackoffWait"))
            {
                if (Command.GetFirstParameterValue("noBackoffWait") == "true")
                {
                    testOptions.NoBackoffWait = new Java.Lang.Boolean(true);
                }
            }

            bool useTestConnectionOptions = false;
            if (Command.ContainsParameter("teardown"))
            {
                IList<string> teardownOptions = Command.Parameters["teardown"];
                foreach (string teardownOption in teardownOptions)
                {
                    if (teardownOption == "resetSdk")
                    {
                        testOptions.Teardown = new Java.Lang.Boolean(true);
                        testOptions.BasePath = BasePath;
                        testOptions.GdprPath = GdprPath;
                        testOptions.SubscriptionPath = SubscriptionPath;
                        useTestConnectionOptions = true;
                        testOptions.TryInstallReferrer = new Java.Lang.Boolean(false);
                    }
                    if (teardownOption == "deleteState")
                    {
                        testOptions.Context = _context;
                    }
                    if (teardownOption == "resetTest")
                    {
                        _savedEvents = new Dictionary<int, AdjustEvent>();
                        _savedConfigs = new Dictionary<int, AdjustConfig>();
                        testOptions.TimerIntervalInMilliseconds = new Java.Lang.Long(-1);
                        testOptions.TimerStartInMilliseconds = new Java.Lang.Long(-1);
                        testOptions.SessionIntervalInMilliseconds = new Java.Lang.Long(-1);
                        testOptions.SubsessionIntervalInMilliseconds = new Java.Lang.Long(-1);
                    }
                    if (teardownOption == "sdk")
                    {
                        testOptions.Teardown = new Java.Lang.Boolean(true);
                        testOptions.BasePath = null;
                        testOptions.GdprPath = null;
                        testOptions.SubscriptionPath = null;
                        useTestConnectionOptions = false;
                    }
                    if (teardownOption == "test")
                    {
                        _savedEvents = null;
                        _savedConfigs = null;
                        testOptions.TimerIntervalInMilliseconds = new Java.Lang.Long(-1);
                        testOptions.TimerStartInMilliseconds = new Java.Lang.Long(-1);
                        testOptions.SessionIntervalInMilliseconds = new Java.Lang.Long(-1);
                        testOptions.SubsessionIntervalInMilliseconds = new Java.Lang.Long(-1);
                    }
                }
            }

            Adjust.SetTestOptions(testOptions);
            if (useTestConnectionOptions == true)
            {
                TestConnectionOptions.SetTestConnectionOptions();
            }
        }

        private void Config()
        {
            var configNumber = 0;
            if (Command.ContainsParameter("configName"))
            {
                var configName = Command.GetFirstParameterValue("configName");
                configNumber = int.Parse(configName.Substring(configName.Length - 1));
            }

            AdjustConfig adjustConfig;
            LogLevel logLevel = null;
            if (Command.ContainsParameter("logLevel"))
            {
                var logLevelString = Command.GetFirstParameterValue("logLevel");
                switch (logLevelString)
                {
                    case "verbose":
                        logLevel = LogLevel.Verbose;
                        break;
                    case "debug":
                        logLevel = LogLevel.Debug;
                        break;
                    case "info":
                        logLevel = LogLevel.Info;
                        break;
                    case "warn":
                        logLevel = LogLevel.Warn;
                        break;
                    case "error":
                        logLevel = LogLevel.Error;
                        break;
                    case "assert":
                        logLevel = LogLevel.Assert;
                        break;
                    case "suppress":
                        logLevel = LogLevel.Supress;
                        break;
                }

                Console.WriteLine(TAG + ": TestApp LogLevel = {0}", logLevel);
            }

            if (_savedConfigs.ContainsKey(configNumber))
            {
                adjustConfig = _savedConfigs[configNumber];
            }
            else
            {
                var environment = Command.GetFirstParameterValue("environment");
                var appToken = Command.GetFirstParameterValue("appToken");
                adjustConfig = new AdjustConfig(_context, appToken, environment);

                if (logLevel != null)
                {
                    adjustConfig.SetLogLevel(logLevel);
                }
                else
                {
                    adjustConfig.SetLogLevel(LogLevel.Verbose);
                }

                _savedConfigs.Add(configNumber, adjustConfig);
            }

            if (Command.ContainsParameter("sdkPrefix"))
            {
                adjustConfig.SetSdkPrefix(Command.GetFirstParameterValue("sdkPrefix"));
            }

            if (Command.ContainsParameter("defaultTracker"))
            {
                adjustConfig.SetDefaultTracker(Command.GetFirstParameterValue("defaultTracker"));
            }

            if (Command.ContainsParameter("externalDeviceId"))
            {
                adjustConfig.SetExternalDeviceId(Command.GetFirstParameterValue("externalDeviceId"));
            }

            if (Command.ContainsParameter("delayStart"))
            {
                var delayStartStr = Command.GetFirstParameterValue("delayStart");
                var delayStart = double.Parse(delayStartStr);
                Console.WriteLine(TAG + ": Delay start set to: " + delayStart);
                adjustConfig.SetDelayStart(delayStart);
            }

            if (Command.ContainsParameter("appSecret"))
            {
                var appSecretList = Command.Parameters["appSecret"];
                Console.WriteLine(TAG + ": Received AppSecret array: " + string.Join(",", appSecretList));

                if (!string.IsNullOrEmpty(appSecretList[0]) && appSecretList.Count == 5)
                {
                    long secretId, info1, info2, info3, info4;
                    long.TryParse(appSecretList[0], out secretId);
                    long.TryParse(appSecretList[1], out info1);
                    long.TryParse(appSecretList[2], out info2);
                    long.TryParse(appSecretList[3], out info3);
                    long.TryParse(appSecretList[4], out info4);

                    adjustConfig.SetAppSecret(secretId, info1, info2, info3, info4);
                }
                else
                {
                    Console.WriteLine(TAG + ": App secret list does not contain 5 elements! Skip setting app secret.");
                }
            }

            if (Command.ContainsParameter("deviceKnown"))
            {
                var deviceKnownS = Command.GetFirstParameterValue("deviceKnown");
                var deviceKnown = deviceKnownS.ToLower() == "true";
                adjustConfig.SetDeviceKnown(deviceKnown);
            }

            if (Command.ContainsParameter("eventBufferingEnabled"))
            {
                var eventBufferingEnabledS = Command.GetFirstParameterValue("eventBufferingEnabled");
                var eventBufferingEnabled = new Java.Lang.Boolean(eventBufferingEnabledS.ToLower() == "true");
                adjustConfig.SetEventBufferingEnabled(eventBufferingEnabled);
            }

            if (Command.ContainsParameter("sendInBackground"))
            {
                var sendInBackgroundS = Command.GetFirstParameterValue("sendInBackground");
                var sendInBackground = sendInBackgroundS.ToLower() == "true";
                adjustConfig.SetSendInBackground(sendInBackground);
            }

            if (Command.ContainsParameter("userAgent"))
            {
                var userAgent = Command.GetFirstParameterValue("userAgent");
                adjustConfig.SetUserAgent(userAgent);
            }

            if (Command.ContainsParameter("deferredDeeplinkCallback"))
            {
                adjustConfig.SetOnDeeplinkResponseListener(this);
            }

            if (Command.ContainsParameter("attributionCallbackSendAll"))
            {
                adjustConfig.SetOnAttributionChangedListener(this);
            }

            if (Command.ContainsParameter("sessionCallbackSendSuccess"))
            {
                adjustConfig.SetOnSessionTrackingSucceededListener(this);
            }

            if (Command.ContainsParameter("sessionCallbackSendFailure"))
            {
                adjustConfig.SetOnSessionTrackingFailedListener(this);
            }

            if (Command.ContainsParameter("eventCallbackSendSuccess"))
            {
                adjustConfig.SetOnEventTrackingSucceededListener(this);
            }

            if (Command.ContainsParameter("eventCallbackSendFailure"))
            {
                adjustConfig.SetOnEventTrackingFailedListener(this);
            }
        }

        public void OnAttributionChanged(AdjustAttribution attribution)
        {
            Console.WriteLine(TAG + ": AttributionChanged, attribution = " + attribution);

            _testLibrary.AddInfoToSend("trackerToken", attribution.TrackerToken);
            _testLibrary.AddInfoToSend("trackerName", attribution.TrackerName);
            _testLibrary.AddInfoToSend("network", attribution.Network);
            _testLibrary.AddInfoToSend("campaign", attribution.Campaign);
            _testLibrary.AddInfoToSend("adgroup", attribution.Adgroup);
            _testLibrary.AddInfoToSend("creative", attribution.Creative);
            _testLibrary.AddInfoToSend("clickLabel", attribution.ClickLabel);
            _testLibrary.AddInfoToSend("adid", attribution.Adid);
            _testLibrary.AddInfoToSend("costType", attribution.CostType);
            _testLibrary.AddInfoToSend("costAmount", attribution.CostAmount == null ? null : attribution.CostAmount.ToString());
            _testLibrary.AddInfoToSend("costCurrency", attribution.CostCurrency);
            _testLibrary.SendInfoToServer(BasePath);
        }

        public void OnFinishedSessionTrackingFailed(AdjustSessionFailure sessionFailureResponseData)
        {
            Console.WriteLine(TAG + ": SesssionTrackingFailed, sessionFailureResponseData = " + sessionFailureResponseData);

            _testLibrary.AddInfoToSend("message", sessionFailureResponseData.Message);
            _testLibrary.AddInfoToSend("timestamp", sessionFailureResponseData.Timestamp);
            _testLibrary.AddInfoToSend("adid", sessionFailureResponseData.Adid);
            _testLibrary.AddInfoToSend("willRetry", sessionFailureResponseData.WillRetry.ToString().ToLower());
            if (sessionFailureResponseData.JsonResponse != null)
                _testLibrary.AddInfoToSend("jsonResponse", sessionFailureResponseData.JsonResponse.ToString());
            _testLibrary.SendInfoToServer(BasePath);
        }

        public void OnFinishedSessionTrackingSucceeded(AdjustSessionSuccess sessionSuccessResponseData)
        {
            Console.WriteLine(TAG + ": SesssionTrackingSucceeded, sessionSuccessResponseData = " + sessionSuccessResponseData);

            _testLibrary.AddInfoToSend("message", sessionSuccessResponseData.Message);
            _testLibrary.AddInfoToSend("timestamp", sessionSuccessResponseData.Timestamp);
            _testLibrary.AddInfoToSend("adid", sessionSuccessResponseData.Adid);
            if (sessionSuccessResponseData.JsonResponse != null)
            {
                _testLibrary.AddInfoToSend("jsonResponse", sessionSuccessResponseData.JsonResponse.ToString());
            }
            _testLibrary.SendInfoToServer(BasePath);
        }

        public void OnFinishedEventTrackingFailed(AdjustEventFailure eventFailureResponseData)
        {
            Console.WriteLine(TAG + ": EventTrackingFailed, eventFailureResponseData = " + eventFailureResponseData);

            _testLibrary.AddInfoToSend("message", eventFailureResponseData.Message);
            _testLibrary.AddInfoToSend("timestamp", eventFailureResponseData.Timestamp);
            _testLibrary.AddInfoToSend("adid", eventFailureResponseData.Adid);
            _testLibrary.AddInfoToSend("eventToken", eventFailureResponseData.EventToken);
            _testLibrary.AddInfoToSend("callbackId", eventFailureResponseData.CallbackId);
            _testLibrary.AddInfoToSend("willRetry", eventFailureResponseData.WillRetry.ToString().ToLower());
            if (eventFailureResponseData.JsonResponse != null)
            {
                _testLibrary.AddInfoToSend("jsonResponse", eventFailureResponseData.JsonResponse.ToString());
            }
            _testLibrary.SendInfoToServer(BasePath);
        }

        public void OnFinishedEventTrackingSucceeded(AdjustEventSuccess eventSuccessResponseData)
        {
            Console.WriteLine(TAG + ": EventTrackingSucceeded, eventSuccessResponseData = " + eventSuccessResponseData);

            _testLibrary.AddInfoToSend("message", eventSuccessResponseData.Message);
            _testLibrary.AddInfoToSend("timestamp", eventSuccessResponseData.Timestamp);
            _testLibrary.AddInfoToSend("adid", eventSuccessResponseData.Adid);
            _testLibrary.AddInfoToSend("eventToken", eventSuccessResponseData.EventToken);
            _testLibrary.AddInfoToSend("callbackId", eventSuccessResponseData.CallbackId);
            if (eventSuccessResponseData.JsonResponse != null)
            {
                _testLibrary.AddInfoToSend("jsonResponse", eventSuccessResponseData.JsonResponse.ToString());
            }
            _testLibrary.SendInfoToServer(BasePath);
        }

        public bool LaunchReceivedDeeplink(Android.Net.Uri deeplink)
        {
            if (deeplink == null)
            {
                Console.WriteLine(TAG + ": DeeplinkResponse, uri = null");
                return false;
            }

            Console.WriteLine(TAG + ": DeeplinkResponse, uri = " + deeplink.ToString());
            return deeplink.ToString().StartsWith("adjusttest", StringComparison.CurrentCulture);
        }

        private void Start()
        {
            Config();

            var configNumber = 0;
            if (Command.ContainsParameter("configName"))
            {
                var configName = Command.GetFirstParameterValue("configName");
                configNumber = int.Parse(configName.Substring(configName.Length - 1));
            }

            var adjustConfig = _savedConfigs[configNumber];
            Adjust.OnCreate(adjustConfig);
            _savedConfigs.Remove(0);
        }

        private void Event()
        {
            var eventNumber = 0;
            if (Command.ContainsParameter("eventName"))
            {
                var eventName = Command.GetFirstParameterValue("eventName");
                eventNumber = int.Parse(eventName.Substring(eventName.Length - 1));
            }

            AdjustEvent adjustEvent = null;
            if (_savedEvents.ContainsKey(eventNumber))
            {
                adjustEvent = _savedEvents[eventNumber];
            }
            else
            {
                var eventToken = Command.GetFirstParameterValue("eventToken");
                adjustEvent = new AdjustEvent(eventToken);
                _savedEvents.Add(eventNumber, adjustEvent);
            }

            if (Command.ContainsParameter("revenue"))
            {
                var revenueParams = Command.Parameters["revenue"];
                var currency = revenueParams[0];
                var revenue = double.Parse(revenueParams[1]);
                adjustEvent.SetRevenue(revenue, currency);
            }

            if (Command.ContainsParameter("callbackParams"))
            {
                var callbackParams = Command.Parameters["callbackParams"];
                for (var i = 0; i < callbackParams.Count; i = i + 2)
                {
                    var key = callbackParams[i];
                    var value = callbackParams[i + 1];
                    adjustEvent.AddCallbackParameter(key, value);
                }
            }

            if (Command.ContainsParameter("partnerParams"))
            {
                var partnerParams = Command.Parameters["partnerParams"];
                for (var i = 0; i < partnerParams.Count; i = i + 2)
                {
                    var key = partnerParams[i];
                    var value = partnerParams[i + 1];
                    adjustEvent.AddPartnerParameter(key, value);
                }
            }

            if (Command.ContainsParameter("orderId"))
            {
                var purchaseId = Command.GetFirstParameterValue("orderId");
                adjustEvent.SetOrderId(purchaseId);
            }

            if (Command.ContainsParameter("callbackId"))
            {
                var callbackId = Command.GetFirstParameterValue("callbackId");
                adjustEvent.SetCallbackId(callbackId);
            }
        }

        private void TrackEvent()
        {
            Event();

            var eventNumber = 0;
            if (Command.ContainsParameter("eventName"))
            {
                var eventName = Command.GetFirstParameterValue("eventName");
                eventNumber = int.Parse(eventName.Substring(eventName.Length - 1));
            }

            var adjustEvent = _savedEvents[eventNumber];
            Adjust.TrackEvent(adjustEvent);

            _savedEvents.Remove(0);
        }

        private void SetReferrer()
        {
            String referrer = Command.GetFirstParameterValue("referrer");
            Adjust.SetReferrer(referrer, _context);
        }

        private void Pause()
        {
            Adjust.OnPause();
        }

        private void Resume()
        {
            Adjust.OnResume();
        }

        private void SetEnabled()
        {
            var enabled = bool.Parse(Command.GetFirstParameterValue("enabled"));
            Adjust.Enabled = enabled;
        }

        private void SetOfflineMode()
        {
            var enabled = bool.Parse(Command.GetFirstParameterValue("enabled"));
            Adjust.SetOfflineMode(enabled);
        }

        private void SendFirstPackages()
        {
            Adjust.SendFirstPackages();
        }

        private void GdprForgetMe()
        {
            Adjust.GdprForgetMe(_context);
        }

        private void AddSessionCallbackParameter()
        {
            if (!Command.ContainsParameter("KeyValue")) 
            {
                return;
            }

            var keyValuePairs = Command.Parameters["KeyValue"];
            for (var i = 0; i < keyValuePairs.Count; i = i + 2)
            {
                var key = keyValuePairs[i];
                var value = keyValuePairs[i + 1];
                Adjust.AddSessionCallbackParameter(key, value);
            }
        }

        private void AddSessionPartnerParameter()
        {
            if (!Command.ContainsParameter("KeyValue"))
            {
                return;
            }

            var keyValuePairs = Command.Parameters["KeyValue"];
            for (var i = 0; i < keyValuePairs.Count; i = i + 2)
            {
                var key = keyValuePairs[i];
                var value = keyValuePairs[i + 1];
                Adjust.AddSessionPartnerParameter(key, value);
            }
        }

        private void RemoveSessionCallbackParameter()
        {
            if (!Command.ContainsParameter("key"))
            {
                return;
            }

            var keys = Command.Parameters["key"];
            for (var i = 0; i < keys.Count; i = i + 1)
            {
                var key = keys[i];
                Adjust.RemoveSessionCallbackParameter(key);
            }
        }

        private void RemoveSessionPartnerParameter()
        {
            if (!Command.ContainsParameter("key"))
            {
                return;
            }

            var keys = Command.Parameters["key"];
            for (var i = 0; i < keys.Count; i = i + 1)
            {
                var key = keys[i];
                Adjust.RemoveSessionPartnerParameter(key);
            }
        }

        private void ResetSessionCallbackParameters()
        {
            Adjust.ResetSessionCallbackParameters();
        }

        private void ResetSessionPartnerParameters()
        {
            Adjust.ResetSessionPartnerParameters();
        }

        private void SetPushToken()
        {
            var token = Command.GetFirstParameterValue("pushToken");
            Adjust.SetPushToken(token, _context);
        }

        private void OpenDeeplink()
        {
            var deeplink = Command.GetFirstParameterValue("deeplink");
            Adjust.AppWillOpenUrl(Uri.Parse(deeplink), _context);
        }

        private void SendReferrer()
        {
            String referrer = Command.GetFirstParameterValue("referrer");
            Adjust.SetReferrer(referrer, _context);
        }

        private void TrackAdRevenue()
        {
            var source = Command.GetFirstParameterValue("adRevenueSource");
            var payload = Command.GetFirstParameterValue("adRevenueJsonString");
            JSONObject jsonPayload = new JSONObject(payload);
            Adjust.TrackAdRevenue(source, jsonPayload);
        }

        private void DisableThirdPartySharing()
        {
            Adjust.DisableThirdPartySharing(_context);
        }

        private void TrackSubscription()
        {
            var price = Command.GetFirstParameterValue("revenue");
            var currency = Command.GetFirstParameterValue("currency");
            var sku = Command.GetFirstParameterValue("productId");
            var signature = Command.GetFirstParameterValue("receipt");
            var purchaseToken = Command.GetFirstParameterValue("purchaseToken");
            var orderId = Command.GetFirstParameterValue("transactionId");
            var purchaseTime = Command.GetFirstParameterValue("transactionDate");

            AdjustPlayStoreSubscription subscription = new AdjustPlayStoreSubscription(
                long.Parse(price),
                currency,
                sku,
                orderId,
                signature,
                purchaseToken);
            subscription.SetPurchaseTime(long.Parse(purchaseTime));

            if (Command.ContainsParameter("callbackParams"))
            {
                var callbackParams = Command.Parameters["callbackParams"];
                for (var i = 0; i < callbackParams.Count; i = i + 2)
                {
                    var key = callbackParams[i];
                    var value = callbackParams[i + 1];
                    subscription.AddCallbackParameter(key, value);
                }
            }

            if (Command.ContainsParameter("partnerParams"))
            {
                var partnerParams = Command.Parameters["partnerParams"];
                for (var i = 0; i < partnerParams.Count; i = i + 2)
                {
                    var key = partnerParams[i];
                    var value = partnerParams[i + 1];
                    subscription.AddPartnerParameter(key, value);
                }
            }

            Adjust.TrackPlayStoreSubscription(subscription);
        }

        private void TrackThirdPartySharing()
        {
            var isEnabledS = Command.GetFirstParameterValue("isEnabled");
            AdjustThirdPartySharing thirdPartySharing = new AdjustThirdPartySharing(
                isEnabledS == null ? null : Java.Lang.Boolean.ValueOf(isEnabledS));

            if (Command.ContainsParameter("granularOptions"))
            {
                var granularOptions = Command.Parameters["granularOptions"];
                for (var i = 0; i < granularOptions.Count; i = i + 3)
                {
                    var partnerName = granularOptions[i];
                    var key = granularOptions[i + 1];
                    var value = granularOptions[i + 2];
                    thirdPartySharing.AddGranularOption(partnerName, key, value);
                }
            }

            Adjust.TrackThirdPartySharing(thirdPartySharing);
        }

        private void TrackMeasurementConsent()
        {
            var measurementConsent = bool.Parse(Command.GetFirstParameterValue("isEnabled"));
            Adjust.TrackMeasurementConsent(measurementConsent);
        }
    }
}
