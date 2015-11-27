using System;

namespace AdjustDemo.Shared 
{
	public class AdjustDemoSharedInfo 
	{
		public static string Environment 
		{
			get 
			{ 
				string env = "none";
				#if NETFX_CORE
                env = "Windows";
				#elif SILVERLIGHT
                env = "Windows Phone";
				#elif __ANDROID__
                env = "Android";
				#elif __IOS__
                env = "IOS";
				#endif
                return env;	
			}
		}
	}
}

