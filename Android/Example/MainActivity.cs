using Android.OS;
using Android.App;
using Android.Widget;
using Android.Content;

using Com.Adjust.Sdk;

namespace Example
{
	[Activity(Label = "Example", MainLauncher = true)]
	[IntentFilter
	 (new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataScheme = "adjustExample")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource.
			SetContentView(Resource.Layout.Main);

			Intent intent = this.Intent;
			var data = intent.Data;
			Adjust.AppWillOpenUrl(data);

			// Get our button from the layout resource,
			// and attach an event to it.
			Button btnTrackSimpleEvent = FindViewById<Button>(Resource.Id.btnTrackSimpleEvent);
			Button btnTrackRevenueEvent = FindViewById<Button>(Resource.Id.btnTrackRevenueEvent);
			Button btnTrackCallbackEvent = FindViewById<Button>(Resource.Id.btnTrackCallbackEvent);
			Button btnTrackPartnerEvent = FindViewById<Button>(Resource.Id.btnTrackPartnerEvent);
			Button btnEnableOfflineMode = FindViewById<Button>(Resource.Id.btnEnableOfflineMode);
			Button btnDisableOfflineMode = FindViewById<Button>(Resource.Id.btnDisableOfflineMode);
			Button btnEnableSDK = FindViewById<Button>(Resource.Id.btnEnableSDK);
			Button btnDisableSDK = FindViewById<Button>(Resource.Id.btnDisableSDK);
			Button btnIsSDKEnabled = FindViewById<Button>(Resource.Id.btnIsSDKEnabled);

			btnTrackSimpleEvent.Click += delegate
			{
				AdjustEvent adjustEvent = new AdjustEvent("{YourEventToken}");
				Adjust.TrackEvent(adjustEvent);
			};

			btnTrackRevenueEvent.Click += delegate
			{
				AdjustEvent adjustEvent = new AdjustEvent("{YourEventToken}");

				// Add revenue 1 cent of an euro
				adjustEvent.SetRevenue(0.01, "EUR");

				Adjust.TrackEvent(adjustEvent);
			};

			btnTrackCallbackEvent.Click += delegate
			{
				AdjustEvent adjustEvent = new AdjustEvent("{YourEventToken}");

				// Add callback parameters to this parameter.
				adjustEvent.AddCallbackParameter("a", "b");
				adjustEvent.AddCallbackParameter("key", "value");
				adjustEvent.AddCallbackParameter("a", "c");

				Adjust.TrackEvent(adjustEvent);
			};

			btnTrackPartnerEvent.Click += delegate
			{
				AdjustEvent adjustEvent = new AdjustEvent("{YourEventToken}");

				// Add partner parameters to this parameter.
				adjustEvent.AddPartnerParameter("x", "y");
				adjustEvent.AddPartnerParameter("foo", "bar");
				adjustEvent.AddPartnerParameter("x", "z");

				Adjust.TrackEvent(adjustEvent);
			};

			btnEnableOfflineMode.Click += delegate
			{
				Adjust.SetOfflineMode(true);
			};

			btnDisableOfflineMode.Click += delegate
			{
				Adjust.SetOfflineMode(false);
			};

			btnEnableSDK.Click += delegate
			{
				Adjust.Enabled = true;
			};

			btnDisableSDK.Click += delegate
			{
				Adjust.Enabled = false;
			};

			btnIsSDKEnabled.Click += delegate
			{
				string message = Adjust.Enabled ? "SDK is ENABLED" : "SDK is DISABLED";
				Toast.MakeText(this, message, ToastLength.Short).Show();
			};
		}

		protected override void OnResume()
		{
			base.OnResume();
			Adjust.OnResume();
		}

		protected override void OnPause()
		{
			base.OnPause();
			Adjust.OnPause();
		}
	}
}


