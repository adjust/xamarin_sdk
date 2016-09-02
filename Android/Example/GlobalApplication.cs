using System;
using Android.App;
using Android.Runtime;

using Com.Adjust.Sdk;

namespace Example
{
	[Application(AllowBackup = true)]
	public class GlobalApplication : Application, IOnAttributionChangedListener, IOnSessionTrackingFailedListener,
	IOnSessionTrackingSucceededListener, IOnEventTrackingFailedListener, IOnEventTrackingSucceededListener, IOnDeeplinkResponseListener
	{
		public GlobalApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();

			// Configure Adjust.
			const String appToken = "{YourAppToken}";
			const String environment = AdjustConfig.EnvironmentSandbox;
			AdjustConfig config = new AdjustConfig(this, appToken, environment);

			// Change the log level.
			config.SetLogLevel(LogLevel.Verbose);

			// Enable event buffering.
			// config.SetEventBufferingEnabled((Java.Lang.Boolean)true);

			// Enable background tracking.
			// config.SetSendInBackground(true);

			// Set default tracker.
			// config.SetDefaultTracker("{YourDefaultTracker}");

			// Set attribution callback.
			config.SetOnAttributionChangedListener(this);

			// Set session callbacks.
			config.SetOnSessionTrackingFailedListener(this);
			config.SetOnSessionTrackingSucceededListener(this);

			// Set event callbacks.
			config.SetOnEventTrackingFailedListener(this);
			config.SetOnEventTrackingSucceededListener(this);

			// Set deferred deeplink callback.
			config.SetOnDeeplinkResponseListener(this);

			Adjust.OnCreate(config);

			// Put the SDK in offline mode.
			// Adjust.SetOfflineMode(true);

			// Disable the SDK.
			// Adjust.Enabled = false;
		}

		public void OnAttributionChanged(AdjustAttribution attribution)
		{
			Console.WriteLine("Attribution changed! New attribution: {0}", attribution.ToString());
		}

		public void OnFinishedSessionTrackingFailed(AdjustSessionFailure sessionFailure)
		{
			Console.WriteLine("Session tracking failed! " + sessionFailure.ToString());
		}

		public void OnFinishedSessionTrackingSucceeded(AdjustSessionSuccess sessionSuccess)
		{
			Console.WriteLine("Session tracking succeeded! " + sessionSuccess.ToString());
		}

		public void OnFinishedEventTrackingFailed(AdjustEventFailure eventFailure)
		{
			Console.WriteLine("Event tracking failed! " + eventFailure.ToString());
		}

		public void OnFinishedEventTrackingSucceeded(AdjustEventSuccess eventSuccess)
		{
			Console.WriteLine("Event tracking succeeded! " + eventSuccess.ToString());
		}

		public bool LaunchReceivedDeeplink(Android.Net.Uri deeplink)
		{
			Console.WriteLine("Deferred deeplink arrived! URL = " + deeplink.ToString());
			return true;
		}
	}
}
