using ObjCRuntime;

namespace adjustSdk.iOSBinding {
	[Native]
	public enum ADJLogLevel : ulong
	{
		Verbose = 1,
		Debug = 2,
		Info = 3,
		Warn = 4,
		Error = 5,
		Assert = 6,
		Suppress = 7
	}
}
