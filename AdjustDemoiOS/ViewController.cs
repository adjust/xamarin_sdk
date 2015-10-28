using System;

using UIKit;

using AdjustBindingsiOS;

namespace AdjustDemoiOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void BtnTrackSimpleEvent_TouchUpInside (UIButton sender)
        {
            ADJEvent adjustEvent = new ADJEvent ("{YourEventToken}");

            Adjust.TrackEvent (adjustEvent);
        }

        partial void BtnTrackRevenueEvent_TouchUpInside (UIButton sender)
        {
            ADJEvent adjustEvent = new ADJEvent ("{YourEventToken}");

            adjustEvent.SetRevenue (0.01, "EUR");

            Adjust.TrackEvent (adjustEvent);
        }

        partial void BtnTrackCallbackEvent_TouchUpInside (UIButton sender)
        {
            ADJEvent adjustEvent = new ADJEvent ("{YourEventToken}");

            adjustEvent.AddCallbackParameter ("a", "b");
            adjustEvent.AddCallbackParameter ("key", "value");
            adjustEvent.AddCallbackParameter ("a", "c");

            Adjust.TrackEvent (adjustEvent);
        }

        partial void BtnTrackPartnerEvent_TouchUpInside (UIButton sender)
        {
            ADJEvent adjustEvent = new ADJEvent ("{YourEventToken}");

            adjustEvent.AddPartnerParameter ("x", "y");
            adjustEvent.AddPartnerParameter ("foo", "bar");
            adjustEvent.AddPartnerParameter ("x", "z");

            Adjust.TrackEvent (adjustEvent);
        }

        partial void BtnEnableOfflineMode_TouchUpInside (UIButton sender)
        {
            Adjust.SetOfflineMode (true);
        }

        partial void BtnDisableOfflineMode_TouchUpInside (UIButton sender)
        {
            Adjust.SetOfflineMode (false);
        }

        partial void BtnEnableSDK_TouchUpInside (UIButton sender)
        {
            Adjust.SetEnabled (true);
        }

        partial void BtnDisableSDK_TouchUpInside (UIButton sender)
        {
            Adjust.SetEnabled (false);
        }

        partial void BtnIsSDKEnabled_TouchUpInside (UIButton sender)
        {
            String message = Adjust.IsEnabled ? "SDK is ENABLED" : "SDK is DISABLED";

            UIAlertView alert = new UIAlertView () { 
                Title = "Is SDK enabled?", Message = message
            };

            alert.AddButton("OK");
            alert.Show ();
        }
    }
}
