using System;

using UIKit;
using Foundation;

using AdjustBindingsiOS;

namespace Example
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		private AdjustDelegateXamarin adjustDelegate = null;

		private class AdjustDelegateXamarin : AdjustDelegate
		{
			public override void AdjustAttributionChanged(ADJAttribution attribution)
			{
				Console.WriteLine("adjust: Attribution changed! New attribution: " + attribution.ToString());
			}

			public override void AdjustSessionTrackingFailed(ADJSessionFailure sessionFailureResponseData)
			{
				Console.WriteLine("adjust: Session tracking failed! Info: " + sessionFailureResponseData.ToString());
			}

			public override void AdjustSessionTrackingSucceeded(ADJSessionSuccess sessionSuccessResponseData)
			{
				Console.WriteLine("adjust: Session tracking succeeded! Info: " + sessionSuccessResponseData.ToString());
			}

			public override void AdjustEventTrackingFailed(ADJEventFailure eventFailureResponseData)
			{
				Console.WriteLine("adjust: Event tracking failed! Info: " + eventFailureResponseData.ToString());
			}

			public override void AdjustEventTrackingSucceeded(ADJEventSuccess eventSuccessResponseData)
			{
				Console.WriteLine("adjust: Event tracking succeeded! Info: " + eventSuccessResponseData.ToString());
			}

			public override bool AdjustDeeplinkResponse(NSUrl deeplink)
			{
				Console.WriteLine("adjust: Deferred deep link received! URL = " + deeplink.ToString());

				return true;
			}
		}

		public override UIWindow Window
		{
			get;
			set;
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			// Configure AdjustDelegate object.
			adjustDelegate = new AdjustDelegateXamarin();

			// Configure adjust.
			String yourAppToken = "{YourAppToken}";
			String environment = AdjustConfig.EnvironmentSandbox;

			var config = ADJConfig.ConfigWithAppToken(yourAppToken, environment);

			// Change the log level.
			config.LogLevel = ADJLogLevel.Verbose;

			// Enable event buffering.
			// config.EventBufferingEnabled = true;

			// Enable background tracking.
			// config.SendInBackground = true;

			// Set default tracker.
			// config.DefaultTracker = "{TrackerToken}";

			// Set an attribution delegate.
			config.Delegate = adjustDelegate;

			Adjust.AppDidLaunch(config);

			// Put the SDK in offline mode.
			// Adjust.SetOfflineMode(true);

			// Disable the SDK.
			// Adjust.SetEnabled(false);

			return true;
		}

		public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			Adjust.AppWillOpenUrl(url);

			return true;
		}

		public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
		{
			if (userActivity.ActivityType == NSUserActivityType.BrowsingWeb)
			{
				Adjust.AppWillOpenUrl(userActivity.WebPageUrl);
			}

			return true;
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
	}
}

