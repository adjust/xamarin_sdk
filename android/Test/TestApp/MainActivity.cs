using Android.App;
using Android.OS;
using Com.Adjust.Testlibrary;

namespace TestApp
{
    [Activity(Label = "TestApp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
		public static readonly string BaseUrl = "https://10.0.2.2:8443";
		public static readonly string GdprUrl = "https://10.0.2.2:8443";      
		private TestLibrary _testLibrary;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

			CommandListener commandListener = new CommandListener(this);
			_testLibrary = new TestLibrary(BaseUrl, commandListener);
            // _testLibrary.DoNotExitAfterEnd();
            // _testLibrary.AddTestDirectory("current/gdpr");
            // _testLibrary.AddTest("current/gdpr/Test_GdprForgetMe_after_install_kill_before_install");
			
            commandListener.SetTestLibrary(_testLibrary);
            _testLibrary.StartTestSession("xamarin4.14.0@android4.14.0");
        }
    }
}

