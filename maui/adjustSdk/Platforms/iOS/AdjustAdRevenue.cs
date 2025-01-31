namespace adjustSdk;

public partial class AdjustAdRevenue {

    internal adjustSdk.iOSBinding.ADJAdRevenue toNative() {
        adjustSdk.iOSBinding.ADJAdRevenue nativeAdRevenue = new (Source);

        if (Revenue is double revenueValue && Currency is not null) {
            nativeAdRevenue.SetRevenue(revenueValue, Currency);
        }

        AdjustUtil.iterateTwoPairList(innerCallbackParameters,
            nativeAdRevenue.AddCallbackParameter);

        AdjustUtil.iterateTwoPairList(innerPartnerParameters,
            nativeAdRevenue.AddPartnerParameter);

        if (AdImpressionsCount is int adImpressionsCountValue) {
            nativeAdRevenue.SetAdImpressionsCount(adImpressionsCountValue);
        }

        if (AdRevenueNetwork is not null) {
            nativeAdRevenue.SetAdRevenueNetwork(AdRevenueNetwork);
        }

        if (AdRevenueUnit is not null) {
            nativeAdRevenue.SetAdRevenueUnit(AdRevenueUnit);
        }
        if (AdRevenuePlacement is not null) {
            nativeAdRevenue.SetAdRevenuePlacement(AdRevenuePlacement);
        }

        return nativeAdRevenue;
    }
}
