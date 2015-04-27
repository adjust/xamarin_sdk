using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Adjust.Sdk;

namespace AdjustDemoAndroid
{
	[Application (AllowBackup = true)]
	public class GlobalApplication : Application, IOnAttributionChangedListener
	{
		public GlobalApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate ();

			// Configure Adjust.
			const String appToken = "rb4g27fje5ej";
			const String environment = AdjustConfig.EnvironmentSandbox;
			AdjustConfig config = new AdjustConfig(this, appToken, environment);

			// Change the log level.
			config.SetLogLevel(LogLevel.Verbose);

			// Enable event buffering.
			// config.SetEventBufferingEnabled((Java.Lang.Boolean)true);

			// Set default tracker.
			// config.SetDefaultTracker("{YourDefaultTracker}");

			// Set attribution delegate.
			config.SetOnAttributionChangedListener(this);

			Adjust.OnCreate (config);

			// Register onResume and onPause events of all activities
			// for applications with minimum support of Android v4 or greater.
			// TODO: This is not currenly not supported in Xamarin Android sample app.
			// registerActivityLifecycleCallbacks(new AdjustLifecycleCallbacks());

			// Put the SDK in offline mode.
			// Adjust.SetOfflineMode(true);

			// Disable the SDK.
			// Adjust.Enabled = false;
		}

		public void OnAttributionChanged (AdjustAttribution attribution)
		{
			Console.WriteLine ("Attribution changed!");
			Console.WriteLine ("New attribution: {0}", attribution.ToString ());
		}
	}
}
