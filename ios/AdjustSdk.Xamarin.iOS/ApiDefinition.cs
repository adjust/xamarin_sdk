using System;
using Foundation;
using ObjCRuntime;

namespace AdjustBindingsiOS
{
    public delegate void RequestTrackingAuthorizationCompletionHandler(nuint status);

    [BaseType(typeof(NSObject))]
    public interface Adjust
    {
        [Static, Export("appDidLaunch:")]
        void AppDidLaunch(ADJConfig adjustConfig);

        [Static, Export("trackEvent:")]
        void TrackEvent(ADJEvent @event);

        [Static, Export("trackSubscription:")]
        void TrackSubscription(ADJSubscription subscription);

        [Static, Export("requestTrackingAuthorizationWithCompletionHandler:")]
        void RequestTrackingAuthorization(RequestTrackingAuthorizationCompletionHandler completion);

        [Static, Export("trackSubsessionStart")]
        void TrackSubsessionStart();

        [Static, Export("trackSubsessionEnd")]
        void TrackSubsessionEnd();

        [Static, Export("setEnabled:")]
        void SetEnabled(bool enabled);

        [Static, Export("isEnabled")]
        bool IsEnabled { get; }

        [Static, Export("appWillOpenUrl:")]
        void AppWillOpenUrl(NSUrl url);

        [Obsolete("Starting from SDK 4.14.0 setDeviceToken: is deprecated. Use setPushToken: method instead.")]
        [Static, Export("setDeviceToken:")]
        void SetDeviceToken(NSData deviceToken);

        [Static, Export("setPushToken:")]
        void SetPushToken(string pushToken);

        [Static, Export("setOfflineMode:")]
        void SetOfflineMode(bool enabled);

        [Static, Export("convertUniversalLink:scheme:")]
        NSUrl ConvertUniversalLink(NSUrl url, string scheme);

        [Static, Export("idfa")]
        string Idfa { get; }

        [Static, Export("sdkVersion")]
        string SdkVersion { get; }

        [Static, Export("adid")]
        string Adid { get; }

        [Static, Export("attribution")]
        ADJAttribution Attribution { get; }

        [Static, Export("sendFirstPackages")]
        void SendFirstPackages();

        [Static, Export("trackAdRevenue:payload:")]
        void TrackAdRevenue(string source, NSData payload);

        [Static, Export("gdprForgetMe")]
        void GdprForgetMe();

        [Static, Export("disableThirdPartySharing")]
        void DisableThirdPartySharing();

        [Static, Export("addSessionCallbackParameter:value:")]
        void AddSessionCallbackParameter(string key, string value);

        [Static, Export("addSessionPartnerParameter:value:")]
        void AddSessionPartnerParameter(string key, string value);

        [Static, Export("removeSessionCallbackParameter:")]
        void RemoveSessionCallbackParameter(string key);

        [Static, Export("removeSessionPartnerParameter:")]
        void RemoveSessionPartnerParameter(string key);

        [Static, Export("resetSessionCallbackParameters")]
        void ResetSessionCallbackParameters();

        [Static, Export("resetSessionPartnerParameters")]
        void ResetSessionPartnerParameters();

        [Static, Export("trackThirdPartySharing:")]
        void TrackThirdPartySharing(ADJThirdPartySharing thirdPartySharing);

        [Static, Export("trackMeasurementConsent:")]
        void TrackMeasurementConsent(bool measurementConsent);

        [Static, Export("appTrackingAuthorizationStatus")]
        int AppTrackingAuthorizationStatus { get; }

        [Static, Export("updateConversionValue:")]
        void UpdateConversionValue(int conversionValue);

        [Static, Export("setTestOptions:")]
        void SetTestOptions(AdjustTestOptions testOptions);
    }

    [BaseType(typeof(NSObject))]
    public interface ADJConfig : INSCopying
    {
        [Export("appToken")]
        string AppToken { get; }

        [Export("environment")]
        string Environment { get; }

        [Export("sdkPrefix")]
        string SdkPrefix { get; set; }

        [Export("defaultTracker")]
        string DefaultTracker { get; set; }

        [Export("externalDeviceId")]
        string ExternalDeviceId { get; set; }

        [Export("logLevel", ArgumentSemantic.Assign)]
        ADJLogLevel LogLevel { get; set; }

        [Export("eventBufferingEnabled")]
        bool EventBufferingEnabled { get; set; }

        [Export("needsCost")]
        bool NeedsCost { get; set; }

        [Export("sendInBackground")]
        bool SendInBackground { get; set; }

        [Export("allowIdfaReading")]
        bool AllowIdfaReading { get; set; }

        [Export("allowiAdInfoReading")]
        bool AllowiAdInfoReading { get; set; }

        [Export("allowAdServicesInfoReading")]
        bool AllowAdServicesInfoReading { get; set; }

        [Export("delayStart")]
        double DelayStart { get; set; }

        [Export("userAgent")]
        string UserAgent { get; set; }

        [Export("isDeviceKnown")]
        bool isDeviceKnown { get; set; }

        [Export("setAppSecret:info1:info2:info3:info4:")]
        void SetAppSecret(long secretId, long info1, long info2, long info3, long info4);

        [Export("isSKAdNetworkHandlingActive")]
        bool IsSKAdNetworkHandlingActive { get; }

        [Export("deactivateSKAdNetworkHandling")]
        void DeactivateSKAdNetworkHandling();

        [Export("urlStrategy")]
        string UrlStrategy { get; set; }

        // [Abstract]
        [NullAllowed, Export("delegate", ArgumentSemantic.Assign)]
        AdjustDelegate Delegate { get; set; }

        [Static, Export("configWithAppToken:environment:")]
        ADJConfig ConfigWithAppToken(string appToken, string environment);

        [Static, Export("configWithAppToken:environment:allowSuppressLogLevel:")]
        ADJConfig ConfigWithAppToken(string appToken, string environment, bool allowSuppressLogLevel);

        [Obsolete("This method is deprecated. Please use this method: ConfigWithAppToken(string appToken, string environment)")]
        [Export("initWithAppToken:environment:")]
        IntPtr Constructor(string appToken, string environment);

        [Export("isValid")]
        bool IsValid { get; }
    }

    [BaseType(typeof(NSObject))]
    public interface ADJEvent : INSCopying
    {
        [Export("eventToken")]
        string EventToken { get; }

        [Export("revenue", ArgumentSemantic.Copy)]
        NSNumber Revenue { get; }

        [Export("callbackParameters")]
        NSDictionary CallbackParameters { get; }

        [Export("partnerParameters")]
        NSDictionary PartnerParameters { get; }

        [Export("transactionId")]
        string TransactionId { get; }

        [Export("callbackId")]
        string CallbackId { get; }

        [Export("currency")]
        string Currency { get; }

        [Export("receipt", ArgumentSemantic.Copy)]
        NSData Receipt { get; }

        [Export("emptyReceipt")]
        bool EmptyReceipt { get; }

        [Obsolete("This method is deprecated. Please use this method: EventWithEventToken(string eventToken)")]
        [Export("initWithEventToken:")]
        IntPtr Constructor(string eventToken);

        [Static, Export("eventWithEventToken:")]
        ADJEvent EventWithEventToken(string eventToken);

        [Export("addCallbackParameter:value:")]
        void AddCallbackParameter(string key, string value);

        [Export("addPartnerParameter:value:")]
        void AddPartnerParameter(string key, string value);

        [Export("setRevenue:currency:")]
        void SetRevenue(double amount, string currency);

        [Export("setTransactionId:")]
        void SetTransactionId(string transactionId);

        [Export("setCallbackId:")]
        void SetCallbackId(string callbackId);

        [Obsolete("This method is deprecated. Please use Xamarin purchase SDK instead. For more information, please contact support@adjust.com")]
        [Export("setReceipt:transactionId:")]
        void SetReceipt(NSData receipt, string transactionId);

        [Export("isValid")]
        bool IsValid { get; }
    }

    [BaseType(typeof(NSObject))]
    public interface ADJAttribution : INSCoding, INSCopying
    {
        [Export("trackerToken")]
        string TrackerToken { get; set; }

        [Export("trackerName")]
        string TrackerName { get; set; }

        [Export("network")]
        string Network { get; set; }

        [Export("campaign")]
        string Campaign { get; set; }

        [Export("adgroup")]
        string Adgroup { get; set; }

        [Export("creative")]
        string Creative { get; set; }

        [Export("clickLabel")]
        string ClickLabel { get; set; }

        [Export("adid")]
        string Adid { get; set; }

        [Export("costType")]
        string CostType { get; set; }

        [Export("costAmount")]
        NSNumber CostAmount { get; set; }

        [Export("costCurrency")]
        string CostCurrency { get; set; }

        [Static, Export("dataWithJsonDict:")]
        ADJAttribution DataWithJsonDict(NSDictionary jsonDict);

        [Export("dictionary")]
        NSDictionary Dictionary { get; }
    }

    [BaseType(typeof(NSObject))]
    public interface ADJSubscription : INSCopying
    {
        [Export("price")]
        NSDecimalNumber Price { get; }

        [Export("currency")]
        string Currency { get; }

        [Export("transactionId")]
        string TransactionId { get; }

        [Export("receipt")]
        NSData Receipt { get; }

        [Export("billingStore")]
        string BillingStore { get; }

        [Export("transactionDate")]
        NSDate TransactionDate { get; }

        [Export("salesRegion")]
        string SalesRegion { get; }

        [Export("callbackParameters")]
        NSDictionary CallbackParameters { get; }

        [Export("partnerParameters")]
        NSDictionary PartnerParameters { get; }

        [Export("initWithPrice:currency:transactionId:andReceipt:")]
        IntPtr Constructor(NSDecimalNumber price, string currency, string transactionId, NSData receipt);

        [Export("setTransactionDate:")]
        void SetTransactionDate(NSDate transactionDate);

        [Export("setSalesRegion:")]
        void SetSalesRegion(string salesRegion);

        [Export("addCallbackParameter:value:")]
        void AddCallbackParameter(string key, string value);

        [Export("addPartnerParameter:value:")]
        void AddPartnerParameter(string key, string value);
    }

    [BaseType(typeof(NSObject))]
    public interface ADJThirdPartySharing : INSCopying
    {
        [Export("enabled")]
        NSNumber Enabled { get; }

        [Export("granularOptions", ArgumentSemantic.Copy)]
        NSMutableDictionary granularOptions { get; }

        [Export("addGranularOption:key:value:")]
        void AddGranularOption(string partnerName, string key, string value);

        [Export("initWithIsEnabledNumberBool:")]
        IntPtr Constructor([NullAllowed] NSNumber isEnabledNumberBool);
    }

    [BaseType(typeof(NSObject))]
    public interface ADJSessionSuccess : INSCopying
    {
        [Export("message")]
        string Message { get; set; }

        [Export("timeStamp")]
        string TimeStamp { get; set; }

        [Export("adid")]
        string Adid { get; set; }

        [Export("jsonResponse", ArgumentSemantic.Retain)]
        NSDictionary JsonResponse { get; set; }
    }

    [BaseType(typeof(NSObject))]
    public interface ADJSessionFailure : INSCopying
    {
        [Export("message")]
        string Message { get; set; }

        [Export("timeStamp")]
        string TimeStamp { get; set; }

        [Export("adid")]
        string Adid { get; set; }

        [Export("willRetry")]
        bool WillRetry { get; set; }

        [Export("jsonResponse", ArgumentSemantic.Retain)]
        NSDictionary JsonResponse { get; set; }
    }

    [BaseType(typeof(NSObject))]
    public interface ADJEventSuccess
    {
        [Export("message")]
        string Message { get; set; }

        [Export("timeStamp")]
        string TimeStamp { get; set; }

        [Export("adid")]
        string Adid { get; set; }

        [Export("eventToken")]
        string EventToken { get; set; }

        [Export("callbackId")]
        string CallbackId { get; set; }

        [Export("jsonResponse", ArgumentSemantic.Retain)]
        NSDictionary JsonResponse { get; set; }
    }

    [BaseType(typeof(NSObject))]
    public interface ADJEventFailure
    {
        [Export("message")]
        string Message { get; set; }

        [Export("timeStamp")]
        string TimeStamp { get; set; }

        [Export("adid")]
        string Adid { get; set; }

        [Export("eventToken")]
        string EventToken { get; set; }

        [Export("callbackId")]
        string CallbackId { get; set; }

        [Export("willRetry")]
        bool WillRetry { get; set; }

        [Export("jsonResponse", ArgumentSemantic.Retain)]
        NSDictionary JsonResponse { get; set; }
    }

    [BaseType(typeof(NSObject))]
    public interface AdjustTestOptions
    {
        [Export("baseUrl")]
        string BaseUrl { get; set; }

        [Export("gdprUrl")]
        string GdprUrl { get; set; }

        [Export("subscriptionUrl")]
        string SubscriptionUrl { get; set; }

        [Export("extraPath")]
        string ExtraPath { get; set; }

        [Export("timerIntervalInMilliseconds")]
        NSNumber TimerIntervalInMilliseconds { get; set; }

        [Export("timerStartInMilliseconds")]
        NSNumber TimerStartInMilliseconds { get; set; }

        [Export("sessionIntervalInMilliseconds")]
        NSNumber SessionIntervalInMilliseconds { get; set; }

        [Export("subsessionIntervalInMilliseconds")]
        NSNumber SubsessionIntervalInMilliseconds { get; set; }

        [Export("teardown")]
        bool Teardown { get; set; }

        [Export("deleteState")]
        bool DeleteState { get; set; }

        [Export("noBackoffWait")]
        bool NoBackoffWait { get; set; }

        [Export("iAdFrameworkEnabled")]
        bool IAdFrameworkEnabled { get; set; }

        [Export("adServicesFrameworkEnabled")]
        bool AdServicesFrameworkEnabled { get; set; }
    }

    [BaseType(typeof(NSObject))]
    [Protocol, Model]
    public interface AdjustDelegate
    {
        [Export("adjustAttributionChanged:")]
        void AdjustAttributionChanged(ADJAttribution attribution);

        [Export("adjustEventTrackingSucceeded:")]
        void AdjustEventTrackingSucceeded(ADJEventSuccess eventSuccessResponseData);

        [Export("adjustEventTrackingFailed:")]
        void AdjustEventTrackingFailed(ADJEventFailure eventFailureResponseData);

        [Export("adjustSessionTrackingSucceeded:")]
        void AdjustSessionTrackingSucceeded(ADJSessionSuccess sessionSuccessResponseData);

        [Export("adjustSessionTrackingFailed:")]
        void AdjustSessionTrackingFailed(ADJSessionFailure sessionFailureResponseData);

        [Export("adjustDeeplinkResponse:")]
        bool AdjustDeeplinkResponse(NSUrl deeplink);

        [Export("adjustConversionValueUpdated:")]
        void AdjustConversionValueUpdated(NSNumber conversionValue);
    }

    [Static]
    public partial interface AdjustConfig
    {
        [Field("ADJEnvironmentSandbox", "__Internal")]
        NSString EnvironmentSandbox { get; }

        [Field("ADJEnvironmentProduction", "__Internal")]
        NSString EnvironmentProduction { get; }

        [Field("ADJUrlStrategyIndia", "__Internal")]
        NSString UrlStrategyIndia { get; }

        [Field("ADJUrlStrategyChina", "__Internal")]
        NSString UrlStrategyChina { get; }

        [Field("ADJDataResidencyEU", "__Internal")]
        NSString DataResidencyEU { get; }

        [Field("ADJDataResidencyTR", "__Internal")]
        NSString DataResidencyTR { get; }

        [Field("ADJDataResidencyUS", "__Internal")]
        NSString DataResidencyUS { get; }
    }
}
