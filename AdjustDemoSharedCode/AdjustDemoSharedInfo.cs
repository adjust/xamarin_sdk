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
				#else

				#if SILVERLIGHT
                env = "Windows Phone";
				#else

				#if __ANDROID__
                env = "Android";
				#else

                #if __IOS__
                env = "IOS";
				#endif
                return env;	
			}
		}
	}
}

