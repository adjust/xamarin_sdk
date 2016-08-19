using System;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace AdjustSdk.Xamarin.iOS {
	[BaseType(typeof(NSObject))]
	public interface Adjust {
		[Static, Export("appDidLaunch:")]
		void AppDidLaunch(ADJConfig adjustConfig);

		[Static, Export("trackEvent:")]
		void TrackEvent(ADJEvent @event);

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

		[Static, Export("setDeviceToken:")]
		void SetDeviceToken(NSData deviceToken);

		[Static, Export("setOfflineMode:")]
		void SetOfflineMode(bool enabled);

		[Static, Export("convertUniversalLink:scheme:")]
		NSUrl ConvertUniversalLink(NSUrl url, string scheme);

		[Static, Export("idfa")]
		string Idfa { get; }

		[Static, Export("sendAdWordsRequest")]
		void SendAdWordsRequest();
	}

	[BaseType(typeof(NSObject))]
	public interface ADJConfig : INSCopying {
		[Export("appToken")]
		string AppToken { get; }

		[Export("environment")]
		string Environment { get; }

		[Export("sdkPrefix")]
		string SdkPrefix { get; set; }

		[Export("defaultTracker")]
		string DefaultTracker { get; set; }

		[Export("logLevel", ArgumentSemantic.Assign)]
		ADJLogLevel LogLevel { get; set; }

		[Export("eventBufferingEnabled")]
		bool EventBufferingEnabled { get; set; }

		[Export("sendInBackground")]
		bool SendInBackground { get; set; }

		[Export("isValid")]
		bool IsValid { get; }

		[Static, Export("configWithAppToken:environment:")]
		ADJConfig ConfigWithAppToken(string appToken, string environment);
	}

	[BaseType(typeof(NSObject))]
	public interface ADJEvent : INSCopying {
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

		[Export("currency")]
		string Currency { get; }

		[Export("receipt", ArgumentSemantic.Copy)]
		NSData Receipt { get; }

		[Export("emptyReceipt")]
		bool EmptyReceipt { get; }

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

		[Export("setReceipt:transactionId:")]
		void SetReceipt(NSData receipt, string transactionId);

		[Export("isValid")]
		bool IsValid { get; }
	}

	[BaseType(typeof(NSObject))]
	public interface ADJAttribution : INSCoding, INSCopying {
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

		[Static, Export("dataWithJsonDict:")]
		ADJAttribution DataWithJsonDict(NSDictionary jsonDict);

		[Export("dictionary")]
		NSDictionary Dictionary { get; }
	}

	[BaseType(typeof(NSObject))]
	public interface ADJSessionSuccess : INSCopying {
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
	public interface ADJSessionFailure : INSCopying {
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
	public interface ADJEventSuccess {
		[Export("message")]
		string Message { get; set; }

		[Export("timeStamp")]
		string TimeStamp { get; set; }

		[Export("adid")]
		string Adid { get; set; }

		[Export("eventToken")]
		string EventToken { get; set; }

		[Export("jsonResponse", ArgumentSemantic.Retain)]
		NSDictionary JsonResponse { get; set; }
	}

	[BaseType(typeof(NSObject))]
	public interface ADJEventFailure {
		[Export("message")]
		string Message { get; set; }

		[Export("timeStamp")]
		string TimeStamp { get; set; }

		[Export("adid")]
		string Adid { get; set; }

		[Export("eventToken")]
		string EventToken { get; set; }

		[Export("willRetry")]
		bool WillRetry { get; set; }

		[Export("jsonResponse", ArgumentSemantic.Retain)]
		NSDictionary JsonResponse { get; set; }
	}

	[BaseType(typeof(NSObject))]
	[Protocol, Model]
	public interface AdjustDelegate {
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
	}

	[Static]
	partial interface Constants
	{
		// extern NSString *const ADJEnvironmentSandbox;
		[Field("ADJEnvironmentSandbox", "__Internal")]
		NSString ADJEnvironmentSandbox { get; }

		// extern NSString *const ADJEnvironmentProduction;
		[Field("ADJEnvironmentProduction", "__Internal")]
		NSString ADJEnvironmentProduction { get; }
	}
}
