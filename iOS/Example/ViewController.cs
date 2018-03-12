using UIKit;
using System;

using AdjustBindingsiOS;

namespace Example
{
    public partial class ViewController : UIViewController
    {
        partial void BtnTrackSimpleEvent_TouchUpInside(UIButton sender)
        {
            var adjustEvent = ADJEvent.EventWithEventToken("g3mfiw");

            Adjust.TrackEvent(adjustEvent);
        }

        partial void BtnTrackRevenueEvent_TouchUpInside(UIButton sender)
        {
            var adjustEvent = ADJEvent.EventWithEventToken("a4fd35");

            adjustEvent.SetRevenue(0.01, "EUR");
            adjustEvent.SetTransactionId("dummy_id");

            Adjust.TrackEvent(adjustEvent);
        }

        partial void BtnTrackCallbackEvent_TouchUpInside(UIButton sender)
        {
            var adjustEvent = ADJEvent.EventWithEventToken("34vgg9");

            adjustEvent.AddCallbackParameter("a", "b");
            adjustEvent.AddCallbackParameter("key", "value");
            adjustEvent.AddCallbackParameter("a", "c");

            Adjust.TrackEvent(adjustEvent);
        }

        partial void BtnTrackPartnerEvent_TouchUpInside(UIButton sender)
        {
            var adjustEvent = ADJEvent.EventWithEventToken("w788qs");

            adjustEvent.AddPartnerParameter("x", "y");
            adjustEvent.AddPartnerParameter("foo", "bar");
            adjustEvent.AddPartnerParameter("x", "z");

            Adjust.TrackEvent(adjustEvent);
        }

        partial void BtnEnableOfflineMode_TouchUpInside(UIButton sender)
        {
            Adjust.SetOfflineMode(true);
        }

        partial void BtnDisableOfflineMode_TouchUpInside(UIButton sender)
        {
            Adjust.SetOfflineMode(false);
        }

        partial void BtnEnableSdk_TouchUpInside(UIButton sender)
        {
            Adjust.SetEnabled(true);
        }

        partial void BtnDisableSdk_TouchUpInside(UIButton sender)
        {
            Adjust.SetEnabled(false);
        }

        partial void BtnIsSdkEnabled_TouchUpInside(UIButton sender)
        {
            String message = Adjust.IsEnabled ? "SDK is ENABLED" : "SDK is DISABLED";

            UIAlertView alert = new UIAlertView()
            {
                Title = "Is SDK enabled?",
                Message = message
            };

            alert.AddButton("OK");
            alert.Show();
        }

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        partial void BtnGetIds_TouchUpInside(UIButton sender)
        {
            Console.WriteLine("Adid: " + Adjust.Adid);
            Console.WriteLine("IDFA: " + Adjust.Idfa);

            if (Adjust.Attribution != null) {
                Console.WriteLine("Attribution Traker Token: " + Adjust.Attribution.TrackerToken);
                Console.WriteLine("Attribution Traker Name: " + Adjust.Attribution.TrackerName);
                Console.WriteLine("Attribution Network: " + Adjust.Attribution.Network);
                Console.WriteLine("Attribution Campaign: " + Adjust.Attribution.Campaign);
                Console.WriteLine("Attribution AdGroup: " + Adjust.Attribution.Adgroup);
                Console.WriteLine("Attribution Creative: " + Adjust.Attribution.Creative);
                Console.WriteLine("Attribution Click Label: " + Adjust.Attribution.ClickLabel);
                Console.WriteLine("Attribution Adid: " + Adjust.Attribution.Adid);    
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
