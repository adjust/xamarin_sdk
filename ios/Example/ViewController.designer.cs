// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Example
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnDisableOfflineMode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnDisableSdk { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnEnableOfflineMode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnEnableSdk { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnGetIds { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnIsSdkEnabled { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnTrackCallbackEvent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnTrackPartnerEvent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnTrackRevenueEvent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnTrackSimpleEvent { get; set; }

        [Action ("BtnDisableOfflineMode_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnDisableOfflineMode_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnDisableSdk_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnDisableSdk_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnEnableOfflineMode_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnEnableOfflineMode_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnEnableSdk_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnEnableSdk_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnGetIds_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnGetIds_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnIsSdkEnabled_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnIsSdkEnabled_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnTrackCallbackEvent_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnTrackCallbackEvent_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnTrackPartnerEvent_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnTrackPartnerEvent_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnTrackRevenueEvent_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnTrackRevenueEvent_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnTrackSimpleEvent_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnTrackSimpleEvent_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BtnDisableOfflineMode != null) {
                BtnDisableOfflineMode.Dispose ();
                BtnDisableOfflineMode = null;
            }

            if (BtnDisableSdk != null) {
                BtnDisableSdk.Dispose ();
                BtnDisableSdk = null;
            }

            if (BtnEnableOfflineMode != null) {
                BtnEnableOfflineMode.Dispose ();
                BtnEnableOfflineMode = null;
            }

            if (BtnEnableSdk != null) {
                BtnEnableSdk.Dispose ();
                BtnEnableSdk = null;
            }

            if (BtnGetIds != null) {
                BtnGetIds.Dispose ();
                BtnGetIds = null;
            }

            if (BtnIsSdkEnabled != null) {
                BtnIsSdkEnabled.Dispose ();
                BtnIsSdkEnabled = null;
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