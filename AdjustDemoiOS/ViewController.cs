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

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			ButtonEventSimple.TouchUpInside += (object sender, EventArgs e) => {
				ADJEvent adjustEvent = new ADJEvent("uqg17r");

				Adjust.TrackEvent(adjustEvent);
			};

			ButtonEventRevenue.TouchUpInside += (object sender, EventArgs e) => {
				ADJEvent adjustEvent = new ADJEvent("71iltz");

				adjustEvent.SetRevenue(0.01, "EUR");

				Adjust.TrackEvent(adjustEvent);
			};

			ButtonEventCallback.TouchUpInside += (object sender, EventArgs e) => {
				ADJEvent adjustEvent = new ADJEvent("1ziip1");

				adjustEvent.AddCallbackParameter("key", "value");

				Adjust.TrackEvent(adjustEvent);
			};

			ButtonEventPartner.TouchUpInside += (object sender, EventArgs e) => {
				ADJEvent adjustEvent = new ADJEvent("9s4lqn");

				adjustEvent.AddPartnerParameter("foo", "bar");

				Adjust.TrackEvent(adjustEvent);
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

