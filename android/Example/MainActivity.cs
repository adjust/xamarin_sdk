using Android.OS;
using Android.App;
using Android.Widget;
using Android.Content;

using Com.Adjust.Sdk;
using System;

namespace Example
{
    [Activity(Label = "Example", MainLauncher = true)]
    [IntentFilter
     (new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "adjust-example")]
    public class MainActivity : Activity, IOnDeviceIdsRead
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Main);

            Intent intent = this.Intent;
            var data = intent.Data;
            Adjust.AppWillOpenUrl(data, this);

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
            Button btnGetIds = FindViewById<Button>(Resource.Id.btnGetIds);

            btnTrackSimpleEvent.Click += delegate
            {
                AdjustEvent adjustEvent = new AdjustEvent("g3mfiw");
                Adjust.TrackEvent(adjustEvent);
            };

            btnTrackRevenueEvent.Click += delegate
            {
                AdjustEvent adjustEvent = new AdjustEvent("a4fd35");

                // Add revenue 1 cent of an euro.
                adjustEvent.SetRevenue(0.01, "EUR");
                adjustEvent.SetOrderId("dummy_id");

                Adjust.TrackEvent(adjustEvent);
            };

            btnTrackCallbackEvent.Click += delegate
            {
                AdjustEvent adjustEvent = new AdjustEvent("34vgg9");

                // Add callback parameters to this parameter.
                adjustEvent.AddCallbackParameter("a", "b");
                adjustEvent.AddCallbackParameter("key", "value");
                adjustEvent.AddCallbackParameter("a", "c");

                Adjust.TrackEvent(adjustEvent);
            };

            btnTrackPartnerEvent.Click += delegate
            {
                AdjustEvent adjustEvent = new AdjustEvent("w788qs");

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

            btnGetIds.Click += delegate
            {
                Adjust.GetGoogleAdId(this, this);
                Console.WriteLine("Amazon Ad Id: " + Adjust.GetAmazonAdId(this));
                Console.WriteLine("Adid: " + Adjust.Adid);

                if (Adjust.Attribution != null)
                {
                    Console.WriteLine("Attribution Traker Token: " + Adjust.Attribution.TrackerToken);
                    Console.WriteLine("Attribution Traker Name: " + Adjust.Attribution.TrackerName);
                    Console.WriteLine("Attribution Network: " + Adjust.Attribution.Network);
                    Console.WriteLine("Attribution Campaign: " + Adjust.Attribution.Campaign);
                    Console.WriteLine("Attribution AdGroup: " + Adjust.Attribution.Adgroup);
                    Console.WriteLine("Attribution Creative: " + Adjust.Attribution.Creative);
                    Console.WriteLine("Attribution Click Label: " + Adjust.Attribution.ClickLabel);
                    Console.WriteLine("Attribution Adid: " + Adjust.Attribution.Adid);
                }
            };
        }

        public void OnGoogleAdIdRead(string googleAdId)
        {
            Console.WriteLine("Google Ad Id: " + googleAdId);
        }

        protected override void OnResume()
        {
            base.OnResume();

            // In case you are supporting API level lower than 14.
            // Adjust.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();

            // In case you are supporting API level lower than 14.
            // Adjust.OnPause();
        }
    }
}


