using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AdjustBindingsAndroid
{
	[BroadcastReceiver (Exported = true)]
	[IntentFilter (new[]{"com.android.vending.INSTALL_REFERRER"})]
	public class AdjustReferrerReceiver : BroadcastReceiver
	{
		private readonly Com.Adjust.Sdk.AdjustReferrerReceiver broadcastReceiver = new Com.Adjust.Sdk.AdjustReferrerReceiver();

		public override void OnReceive (Context context, Intent intent)
		{
			broadcastReceiver.OnReceive (context, intent);
		}
	}
}

