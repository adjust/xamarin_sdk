namespace adjustSdk;

public partial class AdjustAdRevenue {
    internal Com.Adjust.Sdk.AdjustAdRevenue toNative() {
        Com.Adjust.Sdk.AdjustAdRevenue nativeAdRevenue = new (Source);

        if (Revenue is double revenueValue) {
            nativeAdRevenue.SetRevenue(Java.Lang.Double.ValueOf(revenueValue), Currency);
        }

        AdjustUtil.iterateTwoPairList(innerCallbackParameters,
            nativeAdRevenue.AddCallbackParameter);

        AdjustUtil.iterateTwoPairList(innerPartnerParameters,
            nativeAdRevenue.AddPartnerParameter);

        if (AdImpressionsCount is int adImpressionsCountValue) {
            nativeAdRevenue.AdImpressionsCount = Java.Lang.Integer.ValueOf(adImpressionsCountValue);
        }

        if (AdRevenueNetwork is not null) {
            nativeAdRevenue.AdRevenueNetwork = AdRevenueNetwork;
        }

        if (AdRevenueUnit is not null) {
            nativeAdRevenue.AdRevenueUnit = AdRevenueUnit;
        }

        if (AdRevenuePlacement is not null) {
            nativeAdRevenue.AdRevenuePlacement = AdRevenuePlacement;
        }

        return nativeAdRevenue;
    }
}