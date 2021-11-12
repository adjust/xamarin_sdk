using System;
using System.Collections.Generic;
using AdjustBindingsiOS;
using Foundation;
using TestLib;

namespace TestApp
{
    public class AdjustCommandExecutor
    {
        private string TAG = "[AdjustCommandExecutor]";
        private Dictionary<int, ADJConfig> _savedConfigs = new Dictionary<int, ADJConfig>();
        private Dictionary<int, ADJEvent> _savedEvents = new Dictionary<int, ADJEvent>();
  
        internal string ExtraPath;
        internal Command Command;
   
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
            testOptions.BaseUrl = AppDelegate.BaseUrl;
            testOptions.GdprUrl = AppDelegate.GdprUrl;
            testOptions.SubscriptionUrl = AppDelegate.SubscriptionUrl;

            if (Command.ContainsParameter("basePath"))
            {
                ExtraPath = Command.GetFirstParameterValue("basePath");
            }

            if (Command.ContainsParameter("timerInterval"))
            {
                long timerInterval = long.Parse(Command.GetFirstParameterValue("timerInterval"));
                testOptions.TimerIntervalInMilliseconds = timerInterval;
            }

            if (Command.ContainsParameter("timerStart"))
            {
                long timerStart = long.Parse(Command.GetFirstParameterValue("timerStart"));
                testOptions.TimerStartInMilliseconds = timerStart;
            }

            if (Command.ContainsParameter("sessionInterval"))
            {
                long sessionInterval = long.Parse(Command.GetFirstParameterValue("sessionInterval"));
                testOptions.SessionIntervalInMilliseconds = sessionInterval;
            }

            if (Command.ContainsParameter("subsessionInterval"))
            {
                long subsessionInterval = long.Parse(Command.GetFirstParameterValue("subsessionInterval"));
                testOptions.SubsessionIntervalInMilliseconds = subsessionInterval;
            }

            if (Command.ContainsParameter("noBackoffWait"))
            {
                if (Command.GetFirstParameterValue("noBackoffWait") == "true")
                {
                    testOptions.NoBackoffWait = true;
                }
            }
   
            if (Command.ContainsParameter("iAdFrameworkEnabled"))
            {
                if (Command.GetFirstParameterValue("iAdFrameworkEnabled") == "true")
                {
                    testOptions.IAdFrameworkEnabled = true;
                }
            }

            if (Command.ContainsParameter("adServicesFrameworkEnabled"))
            {
                if (Command.GetFirstParameterValue("adServicesFrameworkEnabled") == "true")
                {
                    testOptions.AdServicesFrameworkEnabled = true;
                }
            }

            if (Command.ContainsParameter("teardown"))
            {
                IList<string> teardownOptions = Command.Parameters["teardown"];
                foreach (string teardownOption in teardownOptions)
                {
                    if (teardownOption == "resetSdk")
                    {
                        testOptions.Teardown = true;
                        testOptions.ExtraPath = ExtraPath;
                    }
                    if (teardownOption == "deleteState")
                    {
                        testOptions.DeleteState = true;
                    }
                    if (teardownOption == "resetTest")
                    {
                        _savedEvents = new Dictionary<int, ADJEvent>();
                        _savedConfigs = new Dictionary<int, ADJConfig>();
                        testOptions.TimerIntervalInMilliseconds = -1000;
                        testOptions.TimerStartInMilliseconds = -1000;
                        testOptions.SessionIntervalInMilliseconds = -1000;
                        testOptions.SubsessionIntervalInMilliseconds = -1000;
                    }
                    if (teardownOption == "sdk")
                    {
                        testOptions.Teardown = true;
                        // System.ArgumentNullException is thrown when trying to nullify these two vals.
                        // iOS ApiDefinition object doesn't allow null-ing of these fields.
                        // testOptions.BasePath = null;
                        // testOptions.GdprPath = null;
                    }
                    if (teardownOption == "test")
                    {
                        _savedEvents = null;
                        _savedConfigs = null;
                        ExtraPath = null;
                        testOptions.TimerIntervalInMilliseconds = -1000;
                        testOptions.TimerStartInMilliseconds = -1000;
                        testOptions.SessionIntervalInMilliseconds = -1000;
                        testOptions.SubsessionIntervalInMilliseconds = -1000;
                    }
                }
            }
            
            Adjust.SetTestOptions(testOptions);
        }

        private void Config()
        {
            var configNumber = 0;
            if (Command.ContainsParameter("configName"))
            {
                var configName = Command.GetFirstParameterValue("configName");
                configNumber = int.Parse(configName.Substring(configName.Length - 1));
            }

            ADJConfig adjustConfig;
            ADJLogLevel logLevel = ADJLogLevel.Verbose;
            if (Command.ContainsParameter("logLevel"))
            {
                var logLevelString = Command.GetFirstParameterValue("logLevel");
                switch (logLevelString)
                {
                    case "verbose":
                        logLevel = ADJLogLevel.Verbose;
                        break;
                    case "debug":
                        logLevel = ADJLogLevel.Debug;
                        break;
                    case "info":
                        logLevel = ADJLogLevel.Info;
                        break;
                    case "warn":
                        logLevel = ADJLogLevel.Warn;
                        break;
                    case "error":
                        logLevel = ADJLogLevel.Error;
                        break;
                    case "assert":
                        logLevel = ADJLogLevel.Assert;
                        break;
                    case "suppress":
                        logLevel = ADJLogLevel.Suppress;
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
                
                adjustConfig = ADJConfig.ConfigWithAppToken(appToken, environment);
                adjustConfig.LogLevel = logLevel;
                            
                _savedConfigs.Add(configNumber, adjustConfig);
            }

            if (Command.ContainsParameter("sdkPrefix"))
            {
                adjustConfig.SdkPrefix = Command.GetFirstParameterValue("sdkPrefix");
            }

            if (Command.ContainsParameter("defaultTracker"))
            {
                adjustConfig.DefaultTracker = Command.GetFirstParameterValue("defaultTracker");
            }

            if (Command.ContainsParameter("externalDeviceId"))
            {
                adjustConfig.ExternalDeviceId = Command.GetFirstParameterValue("externalDeviceId");
            }

            if (Command.ContainsParameter("delayStart"))
            {
                var delayStartStr = Command.GetFirstParameterValue("delayStart");
                var delayStart = double.Parse(delayStartStr);
                Console.WriteLine(TAG + ": Delay start set to: " + delayStart);
                adjustConfig.DelayStart = delayStart;
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
                    Console.WriteLine("App secret list does not contain 5 elements! Skip setting app secret.");
                }
            }

            if (Command.ContainsParameter("deviceKnown"))
            {
                var deviceKnownS = Command.GetFirstParameterValue("deviceKnown");
                var deviceKnown = deviceKnownS.ToLower() == "true";
                adjustConfig.isDeviceKnown = deviceKnown;
            }

            if (Command.ContainsParameter("eventBufferingEnabled"))
            {
                var eventBufferingEnabledS = Command.GetFirstParameterValue("eventBufferingEnabled");
                var eventBufferingEnabled = eventBufferingEnabledS.ToLower() == "true";
                adjustConfig.EventBufferingEnabled = eventBufferingEnabled;
            }

            if (Command.ContainsParameter("sendInBackground"))
            {
                var sendInBackgroundS = Command.GetFirstParameterValue("sendInBackground");
                var sendInBackground = sendInBackgroundS.ToLower() == "true";
                adjustConfig.SendInBackground = sendInBackground;
            }

            if (Command.ContainsParameter("allowIdfaReading"))
            {
                var allowIdfaReadingS = Command.GetFirstParameterValue("allowIdfaReading");
                var allowIdfaReading = allowIdfaReadingS.ToLower() == "true";
                adjustConfig.AllowIdfaReading = allowIdfaReading;
            }

            if (Command.ContainsParameter("allowiAdInfoReading"))
            {
                var allowiAdInfoReadingS = Command.GetFirstParameterValue("allowiAdInfoReading");
                var allowiAdInfoReading = allowiAdInfoReadingS.ToLower() == "true";
                adjustConfig.AllowiAdInfoReading = allowiAdInfoReading;
            }

            if (Command.ContainsParameter("allowAdServicesInfoReading"))
            {
                var allowAdServicesInfoReadingS = Command.GetFirstParameterValue("allowAdServicesInfoReading");
                var allowAdServicesInfoReading = allowAdServicesInfoReadingS.ToLower() == "true";
                adjustConfig.AllowAdServicesInfoReading = allowAdServicesInfoReading;
            }

            if (Command.ContainsParameter("allowSkAdNetworkHandling"))
            {
                var allowSkAdNetworkHandlingS = Command.GetFirstParameterValue("allowSkAdNetworkHandling");
                var allowSkAdNetworkHandling = allowSkAdNetworkHandlingS.ToLower() == "true";
                if (allowSkAdNetworkHandling == false)
                {
                    adjustConfig.DeactivateSKAdNetworkHandling();
                }
            }

            if (Command.ContainsParameter("userAgent"))
            {
                var userAgent = Command.GetFirstParameterValue("userAgent");
                adjustConfig.UserAgent = userAgent;
            }

            AdjustDelegateXamarinOptions delegateOptions = new AdjustDelegateXamarinOptions();
            if (Command.ContainsParameter("deferredDeeplinkCallback"))
            {
                Console.WriteLine(TAG + ": AdjustDelegate - deferredDeeplinkCallback detected!");
                delegateOptions.SetDeeplinkResponseDelegate = true;
            }

            if (Command.ContainsParameter("attributionCallbackSendAll"))
            {
                Console.WriteLine(TAG + ": AdjustDelegate - attributionCallbackSendAll detected!");
                delegateOptions.SetAttributionChangedDelegate = true;
            }

            if (Command.ContainsParameter("sessionCallbackSendSuccess"))
            {
                Console.WriteLine(TAG + ": AdjustDelegate - sessionCallbackSendSuccess detected!");
                delegateOptions.SetSessionTrackingSuccessDelegate = true;
            }

            if (Command.ContainsParameter("sessionCallbackSendFailure"))
            {
                Console.WriteLine(TAG + ": AdjustDelegate - sessionCallbackSendFailure detected!");
                delegateOptions.SetSessionTrackingFailedDelegate = true;
            }

            if (Command.ContainsParameter("eventCallbackSendSuccess"))
            {
                Console.WriteLine(TAG + ": AdjustDelegate - eventCallbackSendSuccess detected!");
                delegateOptions.SetEventTrackingSuccessDelegate = true;
            }

            if (Command.ContainsParameter("eventCallbackSendFailure"))
            {
                Console.WriteLine(TAG + ": AdjustDelegate - eventCallbackSendFailure detected!");
                delegateOptions.SetEventTrackingFailedDelegate = true;
            }
   
            adjustConfig.Delegate = new AdjustDelegateXamarin(ExtraPath, delegateOptions);
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
            Adjust.AppDidLaunch(adjustConfig);
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

            ADJEvent adjustEvent = null;
            if (_savedEvents.ContainsKey(eventNumber))
            {
                adjustEvent = _savedEvents[eventNumber];
            }
            else
            {
                var eventToken = Command.GetFirstParameterValue("eventToken");
                adjustEvent = ADJEvent.EventWithEventToken(eventToken);
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
                adjustEvent.SetTransactionId(purchaseId);
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

        private void Pause()
        {
            Adjust.TrackSubsessionEnd();
        }

        private void Resume()
        {
            Adjust.TrackSubsessionStart();
        }

        private void SetEnabled()
        {
            var enabled = bool.Parse(Command.GetFirstParameterValue("enabled"));
            Adjust.SetEnabled(enabled);
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
            Adjust.GdprForgetMe();
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
            Adjust.SetPushToken(token);
        }

        private void OpenDeeplink()
        {
            var deeplink = Command.GetFirstParameterValue("deeplink");
            Adjust.AppWillOpenUrl(new NSUrl(deeplink));
        }

        private void TrackAdRevenue()
        {
            var source = Command.GetFirstParameterValue("adRevenueSource");
            var payload = Command.GetFirstParameterValue("adRevenueJsonString");
            NSData dataPayload = NSData.FromString(payload);
            Adjust.TrackAdRevenue(source, dataPayload);
        }

        private void DisableThirdPartySharing()
        {
            Adjust.DisableThirdPartySharing();
        }

        private void TrackSubscription()
        {
            var price = Command.GetFirstParameterValue("revenue");
            var currency = Command.GetFirstParameterValue("currency");
            var transactionId = Command.GetFirstParameterValue("transactionId");
            var receipt = Command.GetFirstParameterValue("receipt");
            var transactionDate = Command.GetFirstParameterValue("transactionDate");
            var salesRegion = Command.GetFirstParameterValue("salesRegion");

            ADJSubscription subscription = new ADJSubscription(new NSDecimalNumber(price), currency, transactionId, NSData.FromString(receipt));
            subscription.SetTransactionDate(NSDate.FromTimeIntervalSince1970(Convert.ToDouble(transactionDate)));
            subscription.SetSalesRegion(salesRegion);

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

            Adjust.TrackSubscription(subscription);
        }

        private void TrackThirdPartySharing()
        {
            var isEnabledS = Command.GetFirstParameterValue("isEnabled");
            ADJThirdPartySharing thirdPartySharing;
            if (isEnabledS != null)
            {
                thirdPartySharing = new ADJThirdPartySharing(NSNumber.FromBoolean(bool.Parse(isEnabledS)));
            }
            else
            {
                thirdPartySharing = new ADJThirdPartySharing(null);
            }

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

        private class AdjustDelegateXamarinOptions 
        {
            public bool SetAttributionChangedDelegate { get; set; } = false;
            public bool SetSessionTrackingFailedDelegate { get; set; } = false;
            public bool SetSessionTrackingSuccessDelegate { get; set; } = false;         
            public bool SetEventTrackingFailedDelegate { get; set; } = false;
            public bool SetEventTrackingSuccessDelegate { get; set; } = false;         
            public bool SetDeeplinkResponseDelegate { get; set; } = false;         
        }

        private class AdjustDelegateXamarin : AdjustDelegate
        {
            private string TAG = "[AdjustDelegateXamarin]";
            private string _currentBasePath;
            private AdjustDelegateXamarinOptions _delegateOptions;
            private ATLTestLibrary _testLibrary = AppDelegate.TestLibrary;

            public AdjustDelegateXamarin(string currentBasePath, AdjustDelegateXamarinOptions delegateOptions)
            {
                _currentBasePath = currentBasePath;
                _delegateOptions = delegateOptions;
            }

            public override void AdjustAttributionChanged(ADJAttribution attribution)
            {
                if (!_delegateOptions.SetAttributionChangedDelegate)
                {
                    return;
                }
                
                Console.WriteLine(TAG + ": AttributionChanged, attribution = " + attribution);
                AddInfoToSendSafe("trackerToken", attribution.TrackerToken);
                AddInfoToSendSafe("trackerName", attribution.TrackerName);
                AddInfoToSendSafe("network", attribution.Network);
                AddInfoToSendSafe("campaign", attribution.Campaign);
                AddInfoToSendSafe("adgroup", attribution.Adgroup);
                AddInfoToSendSafe("creative", attribution.Creative);
                AddInfoToSendSafe("clickLabel", attribution.ClickLabel);
                AddInfoToSendSafe("adid", attribution.Adid);
                AddInfoToSendSafe("costType", attribution.CostType);
                AddInfoToSendSafe("costAmount", attribution.CostAmount != null ? attribution.CostAmount.StringValue : null);
                AddInfoToSendSafe("costCurrency", attribution.CostCurrency);
                _testLibrary.SendInfoToServer(_currentBasePath);
            }

            public override void AdjustSessionTrackingFailed(ADJSessionFailure sessionFailureResponseData)
            {
                if (!_delegateOptions.SetSessionTrackingFailedDelegate)
                {
                    return;
                }
                
                Console.WriteLine(TAG + ": SesssionTrackingFailed, sessionFailureResponseData = " + sessionFailureResponseData);
                AddInfoToSendSafe("message", sessionFailureResponseData.Message);
                AddInfoToSendSafe("timestamp", sessionFailureResponseData.TimeStamp);
                AddInfoToSendSafe("adid", sessionFailureResponseData.Adid);
                AddInfoToSendSafe("willRetry", sessionFailureResponseData.WillRetry.ToString().ToLower());
                if (sessionFailureResponseData.JsonResponse != null)
                {
                    NSError error = new NSError();
                    NSData dataJsonResponse = NSJsonSerialization.Serialize(sessionFailureResponseData.JsonResponse, 0, out error);
                    NSString stringJsonResponse = new NSString(dataJsonResponse, NSStringEncoding.UTF8);
                    AddInfoToSendSafe("jsonResponse", stringJsonResponse.ToString());
                }
                _testLibrary.SendInfoToServer(_currentBasePath);
            }

            public override void AdjustSessionTrackingSucceeded(ADJSessionSuccess sessionSuccessResponseData)
            {
                if (!_delegateOptions.SetSessionTrackingSuccessDelegate)
                {
                    return;
                }
                
                Console.WriteLine(TAG + ": SesssionTrackingSucceeded, sessionSuccessResponseData = " + sessionSuccessResponseData);
                AddInfoToSendSafe("message", sessionSuccessResponseData.Message);
                AddInfoToSendSafe("timestamp", sessionSuccessResponseData.TimeStamp);
                AddInfoToSendSafe("adid", sessionSuccessResponseData.Adid);
                if (sessionSuccessResponseData.JsonResponse != null)
                {
                    NSError error = new NSError();
                    NSData dataJsonResponse = NSJsonSerialization.Serialize(sessionSuccessResponseData.JsonResponse, 0, out error);
                    NSString stringJsonResponse = new NSString(dataJsonResponse, NSStringEncoding.UTF8);
                    AddInfoToSendSafe("jsonResponse", stringJsonResponse.ToString());
                }
                _testLibrary.SendInfoToServer(_currentBasePath);
            }

            public override void AdjustEventTrackingFailed(ADJEventFailure eventFailureResponseData)
            {
                if (!_delegateOptions.SetEventTrackingFailedDelegate)
                {
                    return;
                }
                
                Console.WriteLine(TAG + ": EventTrackingFailed, eventFailureResponseData = " + eventFailureResponseData);
                AddInfoToSendSafe("message", eventFailureResponseData.Message);
                AddInfoToSendSafe("timestamp", eventFailureResponseData.TimeStamp);
                AddInfoToSendSafe("adid", eventFailureResponseData.Adid);
                AddInfoToSendSafe("eventToken", eventFailureResponseData.EventToken);
                AddInfoToSendSafe("callbackId", eventFailureResponseData.CallbackId);
                AddInfoToSendSafe("willRetry", eventFailureResponseData.WillRetry.ToString().ToLower());
                if (eventFailureResponseData.JsonResponse != null)
                {
                    NSError error = new NSError();
                    NSData dataJsonResponse = NSJsonSerialization.Serialize(eventFailureResponseData.JsonResponse, 0, out error);
                    NSString stringJsonResponse = new NSString(dataJsonResponse, NSStringEncoding.UTF8);
                    AddInfoToSendSafe("jsonResponse", stringJsonResponse.ToString());
                }
                _testLibrary.SendInfoToServer(_currentBasePath);
            }

            public override void AdjustEventTrackingSucceeded(ADJEventSuccess eventSuccessResponseData)
            {
                if (!_delegateOptions.SetEventTrackingSuccessDelegate)
                {
                    return;
                }
                
                Console.WriteLine(TAG + ": EventTrackingSucceeded, eventSuccessResponseData = " + eventSuccessResponseData);
                AddInfoToSendSafe("message", eventSuccessResponseData.Message);
                AddInfoToSendSafe("timestamp", eventSuccessResponseData.TimeStamp);
                AddInfoToSendSafe("adid", eventSuccessResponseData.Adid);
                AddInfoToSendSafe("eventToken", eventSuccessResponseData.EventToken);
                AddInfoToSendSafe("callbackId", eventSuccessResponseData.CallbackId);
                if (eventSuccessResponseData.JsonResponse != null)
                {
                    NSError error = new NSError();
                    NSData dataJsonResponse = NSJsonSerialization.Serialize(eventSuccessResponseData.JsonResponse, 0, out error);
                    NSString stringJsonResponse = new NSString(dataJsonResponse, NSStringEncoding.UTF8);
                    AddInfoToSendSafe("jsonResponse", stringJsonResponse.ToString());
                }
                _testLibrary.SendInfoToServer(_currentBasePath);
            }
            
            public override bool AdjustDeeplinkResponse(NSUrl deeplink)
            {
                if (!_delegateOptions.SetDeeplinkResponseDelegate)
                {
                    return false;
                }
                if (deeplink == null)
                {
                    Console.WriteLine(TAG + ": DeeplinkResponse, uri = null");
                    return false;
                }

                Console.WriteLine(TAG + ": DeeplinkResponse, uri = " + deeplink.ToString());
                return deeplink.ToString().StartsWith("adjusttest", StringComparison.CurrentCulture);
            }

            private void AddInfoToSendSafe(string key, string value)
            {
                if (value == null)
                {
                    return;
                }
                
                _testLibrary.AddInfoToSend(key, value);
            }
        }
    }
}
