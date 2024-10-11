namespace adjustSdk;

public partial class AdjustThirdPartySharing  {
    internal Com.Adjust.Sdk.AdjustThirdPartySharing toNative() {
        // public unsafe AdjustThirdPartySharing (global::Java.Lang.Boolean? isEnabled) : base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        Java.Lang.Boolean? nativeIsEnabled = IsEnabled switch {
            true => Java.Lang.Boolean.True,
            false => Java.Lang.Boolean.False,
            _ => null,
        };

        var nativeAdjustThirdPartySharing = new Com.Adjust.Sdk.AdjustThirdPartySharing(nativeIsEnabled);

        if (GranularOptions is not null) {
            foreach (var element in GranularOptions) {
                nativeAdjustThirdPartySharing.AddGranularOption(
                    element.PartnerName, element.Key, element.StringValue);
            }
        }

        if (PartnerSharingSettings is not null) {
            foreach (var element in PartnerSharingSettings) {
                nativeAdjustThirdPartySharing.AddPartnerSharingSetting(
                    element.PartnerName, element.Key, element.BoolValue);
            }
        }

        return nativeAdjustThirdPartySharing;
    }
}