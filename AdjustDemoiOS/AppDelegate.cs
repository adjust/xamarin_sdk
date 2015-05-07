using UIKit;
using System;
using Foundation;
using ObjCRuntime;

using AdjustBindingsiOS;

namespace AdjustDemoiOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		private AdjustDelegateXamarin adjustDelegate = null;

		// class-level declarations
		public class AdjustDelegateXamarin : AdjustDelegate
		{
			public override void AdjustAttributionChanged (ADJAttribution attribution)
			{
				Console.WriteLine ("Attribution changed!");
				Console.WriteLine ("New attribution: {0}", attribution.ToString ());
			}
		}

		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method

			// Code to start the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
			#endif

			// Configure AdjustDelegate object.
			adjustDelegate = new AdjustDelegateXamarin();

			// Configure adjust.
			String yourAppToken = "rb4g27fje5ej";
			String environment = ADJConfig.ADJEnvironmentSandbox;
			ADJConfig adjustConfig = new ADJConfig (yourAppToken, environment);

			// Change the log level.
			adjustConfig.LogLevel = ADJLogLevel.Verbose;

			// Enable event buffering.
			// adjustConfig.EventBufferingEnabled = true;

			// Disable MAC MD5 tracking.
			// adjustConfig.MacMd5TrackingEnabled = false;

			// Set default tracker.
			// adjustConfig.DefaultTracker = "{TrackerToken}";

			// Set an attribution delegate.
			adjustConfig.Delegate = adjustDelegate;

			Adjust.AppDidLaunch (adjustConfig);

			// Put the SDK in offline mode.
			// Adjust.SetOfflineMode(true);

			// Disable the SDK.
			// Adjust.SetEnabled(false);

			return true;
		}

		public override bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			Adjust.AppWillOpenUrl (url);

			return true;
		}

		public override void OnResignActivation (UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground (UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground (UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated (UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate (UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
	}
}


