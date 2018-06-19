using Android.App;
using Android.Widget;
using Android.OS;
using Com.Adjust.Testlibrary;

namespace TestApp
{
    [Activity(Label = "TestApp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
		public static readonly string BaseUrl = "https://10.0.2.2:8443";
		public static readonly string GdprUrl = "https://10.0.2.2:8443";

		public static readonly string TAG = "TestApp";

		private TestLibrary _testLibrary;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

			CommandListener commandListener = new CommandListener(this);
			_testLibrary = new TestLibrary(BaseUrl, commandListener);
			commandListener.SetTestLibrary(_testLibrary);
			// _testLibrary.DoNotExitAfterEnd();

            StartTestSession();
        }

		private void StartTestSession()
        {
			// _testLibrary.AddTestDirectory("current/gdpr");
			// _testLibrary.AddTest("current/gdpr/Test_GdprForgetMe_after_install_kill_before_install");

			_testLibrary.StartTestSession("xamarin4.14.0@android4.14.0");
        }
    }
}

