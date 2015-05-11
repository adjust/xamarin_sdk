using Android.OS;
using Android.Net;
using Android.App;
using Android.Widget;
using Android.Content;

using Com.Adjust.Sdk;

namespace AdjustDemoAndroid
{
	[Activity (Label = "MainActivity", MainLauncher = true, Icon = "@drawable/icon")]
	[IntentFilter (new[]{Intent.ActionView}, Categories = new[]{Intent.CategoryDefault, Intent.CategoryBrowsable})]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource.
			SetContentView (Resource.Layout.Main);

			Intent intent = this.Intent;
			var data = intent.Data;
			Adjust.AppWillOpenUrl(data);

			// Get our button from the layout resource,
			// and attach an event to it.
			Button btnEventSimple = FindViewById<Button> (Resource.Id.btnEventSimple);
			Button btnEventRevenue = FindViewById<Button> (Resource.Id.btnEventRevenue);
			Button btnEventCallback = FindViewById<Button> (Resource.Id.btnEventCallback);
			Button btnEventPartner = FindViewById<Button> (Resource.Id.btnEventPartner);
			
			btnEventSimple.Click += delegate {
                AdjustEvent eventClick = new AdjustEvent("{YourEventToken}");

				Adjust.TrackEvent(eventClick);
			};

			btnEventRevenue.Click += delegate {
                AdjustEvent eventRevenue = new AdjustEvent("{YourEventToken}");

				// Add revenue 1 cent of an euro
				eventRevenue.SetRevenue(0.01, "EUR");

				Adjust.TrackEvent(eventRevenue);
			};

			btnEventCallback.Click += delegate {
                AdjustEvent eventCallback = new AdjustEvent("{YourEventToken}");

				// Add callback parameters to this parameter.
				eventCallback.AddCallbackParameter("key", "value");

				Adjust.TrackEvent(eventCallback);
			};

			btnEventPartner.Click += delegate {
                AdjustEvent eventPartner = new AdjustEvent("{YourEventToken}");

				// Add partner parameters to this parameter.
				eventPartner.AddPartnerParameter("foo", "bar");

				Adjust.TrackEvent(eventPartner);
			};
		}

		protected override void OnResume()
		{
			base.OnResume ();

			Adjust.OnResume ();
		}

		protected override void OnPause()
		{
			base.OnPause ();

			Adjust.OnPause ();
		}
	}
}
