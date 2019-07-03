using Foundation;
using ObjCRuntime;

namespace TestLib
{
	[BaseType(typeof(NSObject))]
    [Model, Protocol]
	interface AdjustCommandDelegate {
		[Export("executeCommand:methodName:parameters:")]
		void ExecuteCommand(string className, string methodName, NSDictionary parameters);

		[Export("executeCommand:methodName:jsonParameters:")]
		void ExecuteCommand(string className, string methodName, string jsonParameters);

		[Export("executeCommandRawJson:")]
		void ExecuteCommandRawJson(string json);
	}

	[BaseType (typeof(NSObject))]
	interface ATLTestLibrary {
		[Static, Export("testLibraryWithBaseUrl:andControlUrl:andCommandDelegate:")]
		ATLTestLibrary TestLibraryWithBaseUrl(string baseUrl, string controlUrl, AdjustCommandDelegate commandDelegate);

		[Export("addTest:")]
		void AddTest(string testName);

		[Export("addTestDirectory:")]
		void AddTestDirectory(string testDirectory);

		[Export("startTestSession:")]
		void StartTestSession(string clientSdk);

		[Export("addInfoToSend:value:")]
		void AddInfoToSend(string key, string value);

		[Export("sendInfoToServer:")]
		void SendInfoToServer(string basePath);      
	}
}
