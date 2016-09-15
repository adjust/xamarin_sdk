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
			const String appToken = "rb4g27fje5ej";
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

			// Add session callback parameters.
			Adjust.AddSessionCallbackParameter("scp_foo", "scp_bar");
			Adjust.AddSessionCallbackParameter("scp_key", "scp_value");

			// Add session partner parameters.
			Adjust.AddSessionPartnerParameter("spp_a", "spp_b");
			Adjust.AddSessionPartnerParameter("spp_x", "spp_y");
			Adjust.AddSessionPartnerParameter("spp_x", "spp_z");

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
