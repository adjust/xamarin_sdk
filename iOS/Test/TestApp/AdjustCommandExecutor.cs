using System;
using System.Collections.Generic;
using AdjustBindingsiOS;
using Foundation;
using TestLib;

namespace TestApp
{
    public class AdjustCommandExecutor
    {
		private string TAG = AppDelegate.TAG;

		private Dictionary<int, ADJConfig> _savedConfigs = new Dictionary<int, ADJConfig>();
		private Dictionary<int, ADJEvent> _savedEvents = new Dictionary<int, ADJEvent>();
  
        internal string BasePath;
        internal string GdprPath;

		private AdjustDelegateXamarin _adjustDelegate = null;

        internal Command Command;
              
		public void ExecuteCommand(Command command)
        {
            Command = command;
            try
            {
                Console.WriteLine(" \t>>> EXECUTING METHOD: {0}.{1} <<<", command.ClassName, command.MethodName);

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
            if (Command.ContainsParameter("basePath"))
            {
                BasePath = Command.GetFirstParameterValue("basePath");
                GdprPath = Command.GetFirstParameterValue("basePath");
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

            if (Command.ContainsParameter("teardown"))
            {
                IList<string> teardownOptions = Command.Parameters["teardown"];
                foreach (string teardownOption in teardownOptions)
                {
                    if (teardownOption == "resetSdk")
                    {
						testOptions.Teardown = true;
                        testOptions.BasePath = BasePath;
                        testOptions.GdprPath = GdprPath;
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
                        testOptions.BasePath = null;
                        testOptions.GdprPath = null;
                    }
                    if (teardownOption == "test")
                    {
                        _savedEvents = null;
                        _savedConfigs = null;
						_adjustDelegate = null;
						BasePath = null;
						GdprPath = null;
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

                Console.WriteLine("TestApp LogLevel = {0}", logLevel);
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
				adjustConfig.SdkPrefix = Command.GetFirstParameterValue("sdkPrefix");

			if (Command.ContainsParameter("defaultTracker"))
				adjustConfig.DefaultTracker = Command.GetFirstParameterValue("defaultTracker");

            if (Command.ContainsParameter("delayStart"))
            {
                var delayStartStr = Command.GetFirstParameterValue("delayStart");
                var delayStart = double.Parse(delayStartStr);
                Console.WriteLine("delay start set to: " + delayStart);
				adjustConfig.DelayStart = delayStart;
            }

            if (Command.ContainsParameter("appSecret"))
            {
                var appSecretList = Command.Parameters["appSecret"];
                Console.WriteLine("Received AppSecret array: " + string.Join(",", appSecretList));

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
                    Console.WriteLine("App secret list does not contain 5 elements! Skip setting app secret.");
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

            if (Command.ContainsParameter("userAgent"))
            {
                var userAgent = Command.GetFirstParameterValue("userAgent");
				adjustConfig.UserAgent = userAgent;
            }

			if (Command.ContainsParameter("deferredDeeplinkCallback"))
            {
                
            }

            if (Command.ContainsParameter("attributionCallbackSendAll"))
            {
                
            }

            if (Command.ContainsParameter("sessionCallbackSendSuccess"))
            {
                
            }

            if (Command.ContainsParameter("sessionCallbackSendFailure"))
            {
                
            }

            if (Command.ContainsParameter("eventCallbackSendSuccess"))
            {
                
            }

            if (Command.ContainsParameter("eventCallbackSendFailure"))
            {
                
            }

			// TODO: implement callbacks later as in iOS test app
			_adjustDelegate = new AdjustDelegateXamarin();
			adjustConfig.Delegate = _adjustDelegate;
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

			// TrackSubsessionStart has to be called explicitly like this, because, unlike in other non-natives (e.g. Unity SDK),
			// TrackSubsessionStart is not called automatically in Xamarin, and the tests fail otherwise (because of the native-filter
			// in the SDK Test Server)
			//Adjust.TrackSubsessionStart();

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
            if (!Command.ContainsParameter("KeyValue")) return;

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
            if (!Command.ContainsParameter("KeyValue")) return;

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
            if (!Command.ContainsParameter("key")) return;

            var keys = Command.Parameters["key"];
            for (var i = 0; i < keys.Count; i = i + 1)
            {
                var key = keys[i];
                Adjust.RemoveSessionCallbackParameter(key);
            }
        }

        private void RemoveSessionPartnerParameter()
        {
            if (!Command.ContainsParameter("key")) return;

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

		private class AdjustDelegateXamarin : AdjustDelegate
        {
            public override void AdjustAttributionChanged(ADJAttribution attribution)
            {
                Console.WriteLine("adjust: Attribution changed! New attribution: " + attribution.ToString());
            }

            public override void AdjustSessionTrackingFailed(ADJSessionFailure sessionFailureResponseData)
            {
                Console.WriteLine("adjust: Session tracking failed! Info: " + sessionFailureResponseData.ToString());
            }

            public override void AdjustSessionTrackingSucceeded(ADJSessionSuccess sessionSuccessResponseData)
            {
                Console.WriteLine("adjust: Session tracking succeeded! Info: " + sessionSuccessResponseData.ToString());
            }

            public override void AdjustEventTrackingFailed(ADJEventFailure eventFailureResponseData)
            {
                Console.WriteLine("adjust: Event tracking failed! Info: " + eventFailureResponseData.ToString());
            }

            public override void AdjustEventTrackingSucceeded(ADJEventSuccess eventSuccessResponseData)
            {
                Console.WriteLine("adjust: Event tracking succeeded! Info: " + eventSuccessResponseData.ToString());
            }

            public override bool AdjustDeeplinkResponse(NSUrl deeplink)
            {
                Console.WriteLine("adjust: Deferred deep link received! URL = " + deeplink.ToString());

                return true;
            }
        }
    }
}
