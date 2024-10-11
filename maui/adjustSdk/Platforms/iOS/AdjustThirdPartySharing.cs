namespace adjustSdk;

public partial class AdjustThirdPartySharing  {
    internal adjustSdk.iOSBinding.ADJThirdPartySharing toNative() {
        adjustSdk.iOSBinding.ADJThirdPartySharing nativeAdjustThirdPartySharing = new(IsEnabled);
        
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