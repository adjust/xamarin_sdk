using Android.App;
using Android.Content;

namespace AdjustBindingsAndroid
{
    [BroadcastReceiver(Exported = true, Name = "com.adjust.binding.AdjustReferrerReceiver", Permission = "android.permission.INSTALL_PACKAGES")]
    [IntentFilter(new[] { "com.android.vending.INSTALL_REFERRER" })]
    public class AdjustReferrerReceiver : BroadcastReceiver
    {
        private readonly Com.Adjust.Sdk.AdjustReferrerReceiver broadcastReceiver = new Com.Adjust.Sdk.AdjustReferrerReceiver();

        public override void OnReceive(Context context, Intent intent)
        {
            broadcastReceiver.OnReceive(context, intent);
        }
    }
}
