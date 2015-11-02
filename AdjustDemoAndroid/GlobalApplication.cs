using System;

using Android.App;
using Android.Runtime;

using Com.Adjust.Sdk;

namespace AdjustDemoAndroid
{
    [Application (AllowBackup = true)]
    public class GlobalApplication : Application, IOnAttributionChangedListener
    {
        public GlobalApplication (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
        {
        }

        public override void OnCreate ()
        {
            base.OnCreate ();

            // Configure Adjust.
            const String appToken = "{YourAppToken}";
            const String environment = AdjustConfig.EnvironmentSandbox;
            AdjustConfig config = new AdjustConfig (this, appToken, environment);

            // Change the log level.
            config.SetLogLevel (LogLevel.Verbose);

            // Enable event buffering.
            // config.SetEventBufferingEnabled((Java.Lang.Boolean)true);

            // Set default tracker.
            // config.SetDefaultTracker("{YourDefaultTracker}");

            // Set attribution delegate.
            config.SetOnAttributionChangedListener (this);

            Adjust.OnCreate (config);

            // Put the SDK in offline mode.
            // Adjust.SetOfflineMode(true);

            // Disable the SDK.
            // Adjust.Enabled = false;
        }

        public void OnAttributionChanged (AdjustAttribution attribution)
        {
            Console.WriteLine ("Attribution changed! New attribution: {0}", attribution.ToString ());
        }
    }
}
