namespace adjustSdk;

public partial record AdjustDeeplink {
    internal Com.Adjust.Sdk.AdjustDeeplink toNative() {
        return new(Android.Net.Uri.Parse(Deeplink));
    }
}