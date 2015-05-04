// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AdjustDemoiOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonEventCallback { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonEventPartner { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonEventRevenue { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonEventSimple { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ButtonEventCallback != null) {
				ButtonEventCallback.Dispose ();
				ButtonEventCallback = null;
			}
			if (ButtonEventPartner != null) {
				ButtonEventPartner.Dispose ();
				ButtonEventPartner = null;
			}
			if (ButtonEventRevenue != null) {
				ButtonEventRevenue.Dispose ();
				ButtonEventRevenue = null;
			}
			if (ButtonEventSimple != null) {
				ButtonEventSimple.Dispose ();
				ButtonEventSimple = null;
			}
		}
	}
}
