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
		UIButton BtnDisableOfflineMode { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BtnDisableSDK { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BtnEnableOfflineMode { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BtnEnableSDK { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BtnIsSDKEnabled { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BtnTrackCallbackEvent { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BtnTrackPartnerEvent { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BtnTrackRevenueEvent { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BtnTrackSimpleEvent { get; set; }

		[Action ("BtnDisableOfflineMode_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnDisableOfflineMode_TouchUpInside (UIButton sender);

		[Action ("BtnDisableSDK_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnDisableSDK_TouchUpInside (UIButton sender);

		[Action ("BtnEnableOfflineMode_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnEnableOfflineMode_TouchUpInside (UIButton sender);

		[Action ("BtnEnableSDK_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnEnableSDK_TouchUpInside (UIButton sender);

		[Action ("BtnIsSDKEnabled_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnIsSDKEnabled_TouchUpInside (UIButton sender);

		[Action ("BtnTrackCallbackEvent_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnTrackCallbackEvent_TouchUpInside (UIButton sender);

		[Action ("BtnTrackPartnerEvent_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnTrackPartnerEvent_TouchUpInside (UIButton sender);

		[Action ("BtnTrackRevenueEvent_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnTrackRevenueEvent_TouchUpInside (UIButton sender);

		[Action ("BtnTrackSimpleEvent_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnTrackSimpleEvent_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (BtnDisableOfflineMode != null) {
				BtnDisableOfflineMode.Dispose ();
				BtnDisableOfflineMode = null;
			}
			if (BtnDisableSDK != null) {
				BtnDisableSDK.Dispose ();
				BtnDisableSDK = null;
			}
			if (BtnEnableOfflineMode != null) {
				BtnEnableOfflineMode.Dispose ();
				BtnEnableOfflineMode = null;
			}
			if (BtnEnableSDK != null) {
				BtnEnableSDK.Dispose ();
				BtnEnableSDK = null;
			}
			if (BtnIsSDKEnabled != null) {
				BtnIsSDKEnabled.Dispose ();
				BtnIsSDKEnabled = null;
			}
			if (BtnTrackCallbackEvent != null) {
				BtnTrackCallbackEvent.Dispose ();
				BtnTrackCallbackEvent = null;
			}
			if (BtnTrackPartnerEvent != null) {
				BtnTrackPartnerEvent.Dispose ();
				BtnTrackPartnerEvent = null;
			}
			if (BtnTrackRevenueEvent != null) {
				BtnTrackRevenueEvent.Dispose ();
				BtnTrackRevenueEvent = null;
			}
			if (BtnTrackSimpleEvent != null) {
				BtnTrackSimpleEvent.Dispose ();
				BtnTrackSimpleEvent = null;
			}
		}
	}
}
