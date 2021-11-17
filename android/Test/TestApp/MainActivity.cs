using Android.OS;
using Android.App;
using Android.Content;
using Com.Adjust.Sdk;
using Com.Adjust.Test;

namespace TestApp
{
    [Activity(Label = "TestApp", MainLauncher = true, Icon = "@mipmap/icon", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    [IntentFilter
     (new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "adjust-test")]
    public class MainActivity : Activity
    {
        private static readonly string IpAddress = "192.168.86.80";
        public static readonly string BaseUrl = "https://" + IpAddress + ":8443";
        public static readonly string GdprUrl = "https://" + IpAddress + ":8443";
        public static readonly string SubscriptionUrl = "https://" + IpAddress + ":8443";
        public static readonly string ControlUrl = "ws://" + IpAddress + ":1987";
        private TestLibrary _testLibrary;

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            var data = intent.Data;
            Adjust.AppWillOpenUrl(data, this);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            CommandListener commandListener = new CommandListener(this);
            _testLibrary = new TestLibrary(BaseUrl, ControlUrl, commandListener);
            // _testLibrary.DoNotExitAfterEnd();
            // _testLibrary.AddTestDirectory("current/gdpr");
            // _testLibrary.AddTest("current/gdpr/Test_GdprForgetMe_after_install_kill_before_install");
            
            commandListener.SetTestLibrary(_testLibrary);
            _testLibrary.StartTestSession(Adjust.SdkVersion);
        }
    }
}

