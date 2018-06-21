using System;
using System.Collections.Generic;
using AdjustBindingsiOS;
using TestLib;

namespace TestApp
{
    public class AdjustCommandExecutor
    {
		private string TAG = AppDelegate.TAG;

		private Dictionary<int, ADJConfig> _savedConfigs = new Dictionary<int, ADJConfig>();
		private Dictionary<int, ADJEvent> _savedEvents = new Dictionary<int, ADJEvent>();

		private ATLTestLibrary _testLibrary;
        internal string BasePath;
        internal string GdprPath;
        internal Command Command;
              
		public void ExecuteCommand(Command command)
        {
            Command = command;
            //try
            //{
            //    Console.WriteLine(" \t>>> EXECUTING METHOD: {0}.{1} <<<", command.ClassName, command.MethodName);

            //    switch (command.MethodName)
            //    {
            //        case "testOptions": TestOptions(); break;
            //        case "config": Config(); break;
            //        case "start": Start(); break;
            //        case "event": Event(); break;
            //        case "trackEvent": TrackEvent(); break;
            //        case "resume": Resume(); break;
            //        case "pause": Pause(); break;
            //        case "setEnabled": SetEnabled(); break;
            //        case "setReferrer": SetReferrer(); break;
            //        case "setOfflineMode": SetOfflineMode(); break;
            //        case "sendFirstPackages": SendFirstPackages(); break;
            //        case "addSessionCallbackParameter": AddSessionCallbackParameter(); break;
            //        case "addSessionPartnerParameter": AddSessionPartnerParameter(); break;
            //        case "removeSessionCallbackParameter": RemoveSessionCallbackParameter(); break;
            //        case "removeSessionPartnerParameter": RemoveSessionPartnerParameter(); break;
            //        case "resetSessionCallbackParameters": ResetSessionCallbackParameters(); break;
            //        case "resetSessionPartnerParameters": ResetSessionPartnerParameters(); break;
            //        case "setPushToken": SetPushToken(); break;
            //        case "openDeeplink": OpenDeeplink(); break;
            //        case "sendReferrer": SendReferrer(); break;
            //        case "gdprForgetMe": GdprForgetMe(); break;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(TAG + ": {0} ---- {1}", "executeCommand: failed to parse command. Check commands' syntax", ex.ToString());
            //}
        }
    }
}
