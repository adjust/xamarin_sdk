using Foundation;
using UIKit;
using TestLib;
using AdjustBindingsiOS;

namespace TestApp
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        private static readonly string IpAddress = "192.168.86.80";
        public static readonly string TAG = "TestApp";
        public static readonly string BaseUrl = "http://" + IpAddress + ":8080";
        public static readonly string GdprUrl = "http://" + IpAddress + ":8080";
        public static readonly string SubscriptionUrl = "http://" + IpAddress + ":8080";
        public static readonly string ControlUrl = "ws://" + IpAddress + ":1987";

        private AdjustCommandDelegate _commandDelegate = new CommandListener();
        
        private static ATLTestLibrary _testLibrary;
        public static ATLTestLibrary TestLibrary
        {
            get { return _testLibrary; }
        }
        
        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            _testLibrary = ATLTestLibrary.TestLibraryWithBaseUrl(BaseUrl, ControlUrl, _commandDelegate);
            // _testLibrary.AddTestDirectory("current/event");
            // _testLibrary.AddTest("Test_ThirdPartySharing_after_install");
            _testLibrary.StartTestSession(Adjust.SdkVersion);
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

