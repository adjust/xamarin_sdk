using System;
using Foundation;
using ObjCRuntime;

namespace adjustSdk.iOSBinding {

	// The first step to creating a binding is to add your native framework ("MyLibrary.xcframework")
	// to the project.
	// Open your binding csproj and add a section like this
	// <ItemGroup>
	//   <NativeReference Include="MyLibrary.xcframework">
	//     <Kind>Framework</Kind>
	//     <Frameworks></Frameworks>
	//   </NativeReference>
	// </ItemGroup>
	//
	// Once you've added it, you will need to customize it for your specific library:
	//  - Change the Include to the correct path/name of your library
	//  - Change Kind to Static (.a) or Framework (.framework/.xcframework) based upon the library kind and extension.
	//    - Dynamic (.dylib) is a third option but rarely if ever valid, and only on macOS and Mac Catalyst
	//  - If your library depends on other frameworks, add them inside <Frameworks></Frameworks>
	// Example:
	// <NativeReference Include="libs\MyTestFramework.xcframework">
	//   <Kind>Framework</Kind>
	//   <Frameworks>CoreLocation ModelIO</Frameworks>
	// </NativeReference>
	// 
	// Once you've done that, you're ready to move on to binding the API...
	//
	// Here is where you'd define your API definition for the native Objective-C library.
	//
	// For example, to bind the following Objective-C class:
	//
	//     @interface Widget : NSObject {
	//     }
	//
	// The C# binding would look like this:
	//
	//     [BaseType (typeof (NSObject))]
	//     interface Widget {
	//     }
	//
	// To bind Objective-C properties, such as:
	//
	//     @property (nonatomic, readwrite, assign) CGPoint center;
	//
	// You would add a property definition in the C# interface like so:
	//
	//     [Export ("center")]
	//     CGPoint Center { get; set; }
	//
	// To bind an Objective-C method, such as:
	//
	//     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
	//
	// You would add a method definition to the C# interface like so:
	//
	//     [Export ("doSomething:atIndex:")]
	//     void DoSomething (NSObject object, nint index);
	//
	// Objective-C "constructors" such as:
	//
	//     -(id)initWithElmo:(ElmoMuppet *)elmo;
	//
	// Can be bound as:
	//
	//     [Export ("initWithElmo:")]
	//     NativeHandle Constructor (ElmoMuppet elmo);
	//
	// For more information, see https://aka.ms/ios-binding
	//

	// 6
	// typedef void (^ADJResolvedDeeplinkBlock)(NSString * _Nullable);
	delegate void ADJResolvedDeeplinkBlock ([NullAllowed] string resolvedLink);

	// typedef void (^ADJAttributionGetterBlock)(ADJAttribution * _Nullable);
	delegate void ADJAttributionGetterBlock ([NullAllowed] ADJAttribution attribution);

	// typedef void (^ADJIdfaGetterBlock)(NSString * _Nullable);
	delegate void ADJIdfaGetterBlock ([NullAllowed] string idfa);

	// typedef void (^ADJIdfvGetterBlock)(NSString * _Nullable);
	delegate void ADJIdfvGetterBlock ([NullAllowed] string idfv);

	// typedef void (^ADJSdkVersionGetterBlock)(NSString * _Nullable);
	delegate void ADJSdkVersionGetterBlock ([NullAllowed] string sdkVersion);

	// typedef void (^ADJLastDeeplinkGetterBlock)(NSURL * _Nullable);
	delegate void ADJLastDeeplinkGetterBlock ([NullAllowed] NSUrl deeplink);

	// typedef void (^ADJAdidGetterBlock)(NSString * _Nullable);
	delegate void ADJAdidGetterBlock ([NullAllowed] string adid);

	// typedef void (^ADJIsEnabledGetterBlock)(BOOL);
	delegate void ADJIsEnabledGetterBlock (bool arg0);

	// typedef void (^ADJVerificationResultBlock)(ADJPurchaseVerificationResult * _Nonnull);
	delegate void ADJVerificationResultBlock (ADJPurchaseVerificationResult purchaseVerificationResult);

	[Static]
	partial interface Constants
	{
		// extern NSString *const _Nonnull ADJEnvironmentSandbox;
		[Field ("ADJEnvironmentSandbox", "__Internal")]
		NSString ADJEnvironmentSandbox { get; }

		// extern NSString *const _Nonnull ADJEnvironmentProduction;
		[Field ("ADJEnvironmentProduction", "__Internal")]
		NSString ADJEnvironmentProduction { get; }
	}

    [BaseType(typeof(NSObject))]
    interface Adjust {
        [Static, Export("removeGlobalCallbackParameters")]
        void nativeRemoveGlobalCallbackParameters();

		// 55
		// +(void)initSdk:(ADJConfig * _Nullable)adjustConfig;
		[Static]
		[Export ("initSdk:")]
		void InitSdk ([NullAllowed] ADJConfig adjustConfig);

		// +(void)trackEvent:(ADJEvent * _Nullable)event;
		[Static]
		[Export ("trackEvent:")]
		void TrackEvent ([NullAllowed] ADJEvent @event);

		// +(void)enable;
		[Static]
		[Export ("enable")]
		void Enable ();

		// +(void)disable;
		[Static]
		[Export ("disable")]
		void Disable ();

		// +(void)isEnabledWithCompletionHandler:(ADJIsEnabledGetterBlock _Nonnull)completion;
		[Static]
		[Export ("isEnabledWithCompletionHandler:")]
		void IsEnabledWithCompletionHandler (ADJIsEnabledGetterBlock completion);

		// 80
		// +(void)processDeeplink:(ADJDeeplink * _Nonnull)deeplink;
		[Static]
		[Export ("processDeeplink:")]
		void ProcessDeeplink (ADJDeeplink deeplink);

		// +(void)processAndResolveDeeplink:(ADJDeeplink * _Nonnull)deeplink withCompletionHandler:(ADJResolvedDeeplinkBlock _Nonnull)completion;
		[Static]
		[Export ("processAndResolveDeeplink:withCompletionHandler:")]
		void ProcessAndResolveDeeplink (ADJDeeplink deeplink, ADJResolvedDeeplinkBlock completion);

		// +(void)setPushTokenAsString:(NSString * _Nonnull)pushToken;
		[Static]
		[Export ("setPushTokenAsString:")]
		void SetPushTokenAsString (string pushToken);

		// +(void)switchToOfflineMode;
		[Static]
		[Export ("switchToOfflineMode")]
		void SwitchToOfflineMode ();

		// +(void)switchBackToOnlineMode;
		[Static]
		[Export ("switchBackToOnlineMode")]
		void SwitchBackToOnlineMode ();

		// 110
		// +(void)idfaWithCompletionHandler:(ADJIdfaGetterBlock _Nonnull)completion;
		[Static]
		[Export ("idfaWithCompletionHandler:")]
		void IdfaWithCompletionHandler (ADJIdfaGetterBlock completion);

		// +(void)idfvWithCompletionHandler:(ADJIdfvGetterBlock _Nonnull)completion;
		[Static]
		[Export ("idfvWithCompletionHandler:")]
		void IdfvWithCompletionHandler (ADJIdfvGetterBlock completion);

		// +(void)adidWithCompletionHandler:(ADJAdidGetterBlock _Nonnull)completion;
		[Static]
		[Export ("adidWithCompletionHandler:")]
		void AdidWithCompletionHandler (ADJAdidGetterBlock completion);

		// +(void)attributionWithCompletionHandler:(ADJAttributionGetterBlock _Nonnull)completion;
		[Static]
		[Export ("attributionWithCompletionHandler:")]
		void AttributionWithCompletionHandler (ADJAttributionGetterBlock completion);

		// +(void)sdkVersionWithCompletionHandler:(ADJSdkVersionGetterBlock _Nonnull)completion;
		[Static]
		[Export ("sdkVersionWithCompletionHandler:")]
		void SdkVersionWithCompletionHandler (ADJSdkVersionGetterBlock completion);

		// 141
		// +(void)addGlobalCallbackParameter:(NSString * _Nonnull)param forKey:(NSString * _Nonnull)key;
		[Static]
		[Export ("addGlobalCallbackParameter:forKey:")]
		void AddGlobalCallbackParameter (string param, string key);

		// +(void)addGlobalPartnerParameter:(NSString * _Nonnull)param forKey:(NSString * _Nonnull)key;
		[Static]
		[Export ("addGlobalPartnerParameter:forKey:")]
		void AddGlobalPartnerParameter (string param, string key);

		// +(void)removeGlobalCallbackParameterForKey:(NSString * _Nonnull)key;
		[Static]
		[Export ("removeGlobalCallbackParameterForKey:")]
		void RemoveGlobalCallbackParameterForKey (string key);

		// +(void)removeGlobalPartnerParameterForKey:(NSString * _Nonnull)key;
		[Static]
		[Export ("removeGlobalPartnerParameterForKey:")]
		void RemoveGlobalPartnerParameterForKey (string key);

		// +(void)removeGlobalCallbackParameters;
		[Static]
		[Export ("removeGlobalCallbackParameters")]
		void RemoveGlobalCallbackParameters ();

		// +(void)removeGlobalPartnerParameters;
		[Static]
		[Export ("removeGlobalPartnerParameters")]
		void RemoveGlobalPartnerParameters ();

		// 171
		// +(void)gdprForgetMe;
		[Static]
		[Export ("gdprForgetMe")]
		void GdprForgetMe ();

		// +(void)trackThirdPartySharing:(ADJThirdPartySharing * _Nonnull)thirdPartySharing;
		[Static]
		[Export ("trackThirdPartySharing:")]
		void TrackThirdPartySharing (ADJThirdPartySharing thirdPartySharing);

		// +(void)trackMeasurementConsent:(BOOL)enabled;
		[Static]
		[Export ("trackMeasurementConsent:")]
		void TrackMeasurementConsent (bool enabled);

		// +(void)trackAdRevenue:(ADJAdRevenue * _Nonnull)adRevenue;
		[Static]
		[Export ("trackAdRevenue:")]
		void TrackAdRevenue (ADJAdRevenue adRevenue);

		// +(void)trackAppStoreSubscription:(ADJAppStoreSubscription * _Nonnull)subscription;
		[Static]
		[Export ("trackAppStoreSubscription:")]
		void TrackAppStoreSubscription (ADJAppStoreSubscription subscription);

		// 196
		// +(void)requestAppTrackingAuthorizationWithCompletionHandler:(void (^ _Nullable)(NSUInteger))completion;
		[Static]
		[Export ("requestAppTrackingAuthorizationWithCompletionHandler:")]
		void RequestAppTrackingAuthorizationWithCompletionHandler ([NullAllowed] Action<nuint> completion);

		// 201
		// +(int)appTrackingAuthorizationStatus;
		[Static]
		[Export ("appTrackingAuthorizationStatus")]
		int AppTrackingAuthorizationStatus { get; }

		// 207
		// +(void)updateSkanConversionValue:(NSInteger)conversionValue coarseValue:(NSString * _Nullable)coarseValue lockWindow:(NSNumber * _Nullable)lockWindow withCompletionHandler:(void (^ _Nullable)(NSError * _Nullable))completion;
		[Static]
		[Export ("updateSkanConversionValue:coarseValue:lockWindow:withCompletionHandler:")]
		void UpdateSkanConversionValue (nint conversionValue, [NullAllowed] string coarseValue, [NullAllowed] NSNumber lockWindow, [NullAllowed] Action<NSError> completion);

		// 212
		// +(void)lastDeeplinkWithCompletionHandler:(ADJLastDeeplinkGetterBlock _Nonnull)completion;
		[Static]
		[Export ("lastDeeplinkWithCompletionHandler:")]
		void LastDeeplinkWithCompletionHandler (ADJLastDeeplinkGetterBlock completion);

		// 217
		// +(void)verifyAppStorePurchase:(ADJAppStorePurchase * _Nonnull)purchase withCompletionHandler:(ADJVerificationResultBlock _Nonnull)completion;
		[Static]
		[Export ("verifyAppStorePurchase:withCompletionHandler:")]
		void VerifyAppStorePurchase (ADJAppStorePurchase purchase, ADJVerificationResultBlock completion);

		// +(void)verifyAndTrackAppStorePurchase:(ADJEvent * _Nonnull)event withCompletionHandler:(ADJVerificationResultBlock _Nonnull)completion;
		[Static]
		[Export ("verifyAndTrackAppStorePurchase:withCompletionHandler:")]
		void VerifyAndTrackAppStorePurchase (ADJEvent @event, ADJVerificationResultBlock completion);

		// +(void)setTestOptions:(NSDictionary * _Nullable)testOptions;
		[Static]
		[Export ("setTestOptions:")]
		void SetTestOptions ([NullAllowed] NSDictionary testOptions);

		// +(void)trackSubsessionStart;
		[Static]
		[Export ("trackSubsessionStart")]
		void TrackSubsessionStart ();

		// +(void)trackSubsessionEnd;
		[Static]
		[Export ("trackSubsessionEnd")]
		void TrackSubsessionEnd ();
	}

	// @interface ADJEvent : NSObject <NSCopying>
	[BaseType (typeof(NSObject))]
	interface ADJEvent : INSCopying
	{
		// 289
		// -(id _Nullable)initWithEventToken:(NSString * _Nonnull)eventToken;
		[Export ("initWithEventToken:")]
		NativeHandle Constructor (string eventToken);

		// -(void)setRevenue:(double)amount currency:(NSString * _Nonnull)currency;
		[Export ("setRevenue:currency:")]
		void SetRevenue (double amount, string currency);

		// -(void)addCallbackParameter:(NSString * _Nonnull)key value:(NSString * _Nonnull)value;
		[Export ("addCallbackParameter:value:")]
		void AddCallbackParameter (string key, string value);

		// -(void)addPartnerParameter:(NSString * _Nonnull)key value:(NSString * _Nonnull)value;
		[Export ("addPartnerParameter:value:")]
		void AddPartnerParameter (string key, string value);

		// -(void)setDeduplicationId:(NSString * _Nonnull)deduplicationId;
		[Export ("setDeduplicationId:")]
		void SetDeduplicationId (string deduplicationId);

		// -(void)setCallbackId:(NSString * _Nonnull)callbackId;
		[Export ("setCallbackId:")]
		void SetCallbackId (string callbackId);

		// -(void)setTransactionId:(NSString * _Nonnull)transactionId;
		[Export ("setTransactionId:")]
		void SetTransactionId (string transactionId);

		// -(void)setProductId:(NSString * _Nonnull)productId;
		[Export ("setProductId:")]
		void SetProductId (string productId);
	}

	// 327
	// @interface ADJThirdPartySharing : NSObject
	[BaseType (typeof(NSObject))]
	interface ADJThirdPartySharing
	{
		// -(id _Nullable)initWithIsEnabled:(NSNumber * _Nullable)isEnabled;
		[Export ("initWithIsEnabled:")]
		NativeHandle Constructor ([NullAllowed] NSNumber isEnabled);

		// -(void)addGranularOption:(NSString * _Nonnull)partnerName key:(NSString * _Nonnull)key value:(NSString * _Nonnull)value;
		[Export ("addGranularOption:key:value:")]
		void AddGranularOption (string partnerName, string key, string value);

		// -(void)addPartnerSharingSetting:(NSString * _Nonnull)partnerName key:(NSString * _Nonnull)key value:(BOOL)value;
		[Export ("addPartnerSharingSetting:key:value:")]
		void AddPartnerSharingSetting (string partnerName, string key, bool value);
	}

	// @protocol AdjustDelegate
	[Protocol, Model]
    [BaseType(typeof(NSObject))]
	interface AdjustDelegate
	{
		// 360
		// @optional -(void)adjustAttributionChanged:(ADJAttribution * _Nullable)attribution;
		[Export ("adjustAttributionChanged:")]
		void AdjustAttributionChanged ([NullAllowed] ADJAttribution attribution);

		// @optional -(void)adjustEventTrackingSucceeded:(ADJEventSuccess * _Nullable)eventSuccessResponse;
		[Export ("adjustEventTrackingSucceeded:")]
		void AdjustEventTrackingSucceeded ([NullAllowed] ADJEventSuccess eventSuccessResponse);

		// @optional -(void)adjustEventTrackingFailed:(ADJEventFailure * _Nullable)eventFailureResponse;
		[Export ("adjustEventTrackingFailed:")]
		void AdjustEventTrackingFailed ([NullAllowed] ADJEventFailure eventFailureResponse);

		// @optional -(void)adjustSessionTrackingSucceeded:(ADJSessionSuccess * _Nullable)sessionSuccessResponse;
		[Export ("adjustSessionTrackingSucceeded:")]
		void AdjustSessionTrackingSucceeded ([NullAllowed] ADJSessionSuccess sessionSuccessResponse);

		// @optional -(void)adjustSessionTrackingFailed:(ADJSessionFailure * _Nullable)sessionFailureResponse;
		[Export ("adjustSessionTrackingFailed:")]
		void AdjustSessionTrackingFailed ([NullAllowed] ADJSessionFailure sessionFailureResponse);

		// @optional -(BOOL)adjustDeferredDeeplinkReceived:(NSURL * _Nullable)deeplink;
		[Export ("adjustDeferredDeeplinkReceived:")]
		bool AdjustDeferredDeeplinkReceived ([NullAllowed] NSUrl deeplink);

		// @optional -(void)adjustSkanUpdatedWithConversionData:(NSDictionary<NSString *,NSString *> * _Nonnull)data;
		[Export ("adjustSkanUpdatedWithConversionData:")]
		void AdjustSkanUpdatedWithConversionData (NSDictionary<NSString, NSString> data);
	}

	// @interface ADJConfig : NSObject <NSCopying>
	[BaseType (typeof(NSObject))]
	interface ADJConfig : INSCopying
	{
		// 450
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		AdjustDelegate Delegate { get; set; }

		// @property (nonatomic, weak) NSObject<AdjustDelegate> * _Nullable delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		// 458
		// @property (copy, nonatomic) NSString * _Nullable sdkPrefix;
		[NullAllowed, Export ("sdkPrefix")]
		string SdkPrefix { get; set; }

		// 462
		// @property (assign, nonatomic) ADJLogLevel logLevel;
		[Export ("logLevel", ArgumentSemantic.Assign)]
		ADJLogLevel LogLevel { get; set; }

		// 466
		// @property (copy, nonatomic) NSString * _Nullable defaultTracker;
		[NullAllowed, Export ("defaultTracker")]
		string DefaultTracker { get; set; }

		// @property (assign, nonatomic) NSUInteger attConsentWaitingInterval;
		[Export ("attConsentWaitingInterval")]
		nuint AttConsentWaitingInterval { get; set; }

		// 470
		// @property (copy, nonatomic) NSString * _Nullable externalDeviceId;
		[NullAllowed, Export ("externalDeviceId")]
		string ExternalDeviceId { get; set; }

		// 478
		// @property (assign, nonatomic) NSInteger eventDeduplicationIdsMaxSize;
		[Export ("eventDeduplicationIdsMaxSize")]
		nint EventDeduplicationIdsMaxSize { get; set; }

		// 482
		// -(ADJConfig * _Nullable)initWithAppToken:(NSString * _Nonnull)appToken environment:(NSString * _Nonnull)environment;
		[Export ("initWithAppToken:environment:")]
		NativeHandle Constructor (string appToken, string environment);

		// 486
		// -(ADJConfig * _Nullable)initWithAppToken:(NSString * _Nonnull)appToken environment:(NSString * _Nonnull)environment suppressLogLevel:(BOOL)allowSuppressLogLevel;
		[Export ("initWithAppToken:environment:suppressLogLevel:")]
		NativeHandle Constructor (string appToken, string environment, bool allowSuppressLogLevel);

		// 495
		// -(void)disableAdServices;
		[Export ("disableAdServices")]
		void DisableAdServices ();

		// -(void)disableIdfaReading;
		[Export ("disableIdfaReading")]
		void DisableIdfaReading ();

		// -(void)disableIdfvReading;
		[Export ("disableIdfvReading")]
		void DisableIdfvReading ();

		// -(void)disableSkanAttribution;
		[Export ("disableSkanAttribution")]
		void DisableSkanAttribution ();

		// 511
		// -(void)enableSendingInBackground;
		[Export ("enableSendingInBackground")]
		void EnableSendingInBackground ();

		// -(void)enableLinkMe;
		[Export ("enableLinkMe")]
		void EnableLinkMe ();

		// 519
		// -(void)enableDeviceIdsReadingOnce;
		[Export ("enableDeviceIdsReadingOnce")]
		void EnableDeviceIdsReadingOnce ();

		// 523
		// -(void)enableCostDataInAttribution;
		[Export ("enableCostDataInAttribution")]
		void EnableCostDataInAttribution ();

		// 527
		// -(void)enableCoppaCompliance;
		[Export ("enableCoppaCompliance")]
		void EnableCoppaCompliance ();

		// 531
		// -(void)setUrlStrategy:(NSArray * _Nullable)urlStrategyDomains useSubdomains:(BOOL)useSubdomains isDataResidency:(BOOL)isDataResidency;
		[Export ("setUrlStrategy:useSubdomains:isDataResidency:")]
		//void SetUrlStrategy ([NullAllowed] NSObject[] urlStrategyDomains, bool useSubdomains, bool isDataResidency);
		void SetUrlStrategy ([NullAllowed] NSArray urlStrategyDomains, bool useSubdomains, bool isDataResidency);
	}

	// 605
	// @interface ADJAttribution : NSObject <NSCoding, NSCopying>
	[BaseType (typeof(NSObject))]
	interface ADJAttribution : INSCoding, INSCopying
	{
		// @property (copy, nonatomic) NSString * _Nullable trackerToken;
		[NullAllowed, Export ("trackerToken")]
		string TrackerToken { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable trackerName;
		[NullAllowed, Export ("trackerName")]
		string TrackerName { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable network;
		[NullAllowed, Export ("network")]
		string Network { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable campaign;
		[NullAllowed, Export ("campaign")]
		string Campaign { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable adgroup;
		[NullAllowed, Export ("adgroup")]
		string Adgroup { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable creative;
		[NullAllowed, Export ("creative")]
		string Creative { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable clickLabel;
		[NullAllowed, Export ("clickLabel")]
		string ClickLabel { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable costType;
		[NullAllowed, Export ("costType")]
		string CostType { get; set; }

		// @property (copy, nonatomic) NSNumber * _Nullable costAmount;
		[NullAllowed, Export ("costAmount", ArgumentSemantic.Copy)]
		NSNumber CostAmount { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable costCurrency;
		[NullAllowed, Export ("costCurrency")]
		string CostCurrency { get; set; }
	}

	// 663
	// @interface ADJAppStoreSubscription : NSObject <NSCopying>
	[BaseType (typeof(NSObject))]
	interface ADJAppStoreSubscription : INSCopying
	{
		// -(id _Nullable)initWithPrice:(NSDecimalNumber * _Nonnull)price currency:(NSString * _Nonnull)currency transactionId:(NSString * _Nonnull)transactionId;
		[Export ("initWithPrice:currency:transactionId:")]
		NativeHandle Constructor ([NullAllowed] NSDecimalNumber price, [NullAllowed] string currency, [NullAllowed] string transactionId);

		// -(void)setTransactionDate:(NSDate * _Nonnull)transactionDate;
		[Export ("setTransactionDate:")]
		void SetTransactionDate (NSDate transactionDate);

		// -(void)setSalesRegion:(NSString * _Nonnull)salesRegion;
		[Export ("setSalesRegion:")]
		void SetSalesRegion (string salesRegion);

		// -(void)addCallbackParameter:(NSString * _Nonnull)key value:(NSString * _Nonnull)value;
		[Export ("addCallbackParameter:value:")]
		void AddCallbackParameter (string key, string value);

		// -(void)addPartnerParameter:(NSString * _Nonnull)key value:(NSString * _Nonnull)value;
		[Export ("addPartnerParameter:value:")]
		void AddPartnerParameter (string key, string value);
	}

	// 715
	// @interface ADJEventSuccess : NSObject
	[BaseType (typeof(NSObject))]
	interface ADJEventSuccess
	{
		// @property (copy, nonatomic) NSString * message;
		[Export ("message")]
		string Message { get; set; }

		// @property (copy, nonatomic) NSString * timestamp;
		[Export ("timestamp")]
		string Timestamp { get; set; }

		// @property (copy, nonatomic) NSString * adid;
		[Export ("adid")]
		string Adid { get; set; }

		// @property (copy, nonatomic) NSString * eventToken;
		[Export ("eventToken")]
		string EventToken { get; set; }

		// @property (copy, nonatomic) NSString * callbackId;
		[Export ("callbackId")]
		string CallbackId { get; set; }

		// @property (nonatomic, strong) NSDictionary * jsonResponse;
		[Export ("jsonResponse", ArgumentSemantic.Strong)]
		NSDictionary JsonResponse { get; set; }
	}

	// @interface ADJEventFailure : NSObject
	[BaseType (typeof(NSObject))]
	interface ADJEventFailure
	{
		// @property (copy, nonatomic) NSString * message;
		[Export ("message")]
		string Message { get; set; }

		// @property (copy, nonatomic) NSString * timestamp;
		[Export ("timestamp")]
		string Timestamp { get; set; }

		// @property (copy, nonatomic) NSString * adid;
		[Export ("adid")]
		string Adid { get; set; }

		// @property (copy, nonatomic) NSString * eventToken;
		[Export ("eventToken")]
		string EventToken { get; set; }

		// @property (copy, nonatomic) NSString * callbackId;
		[Export ("callbackId")]
		string CallbackId { get; set; }

		// @property (assign, nonatomic) BOOL willRetry;
		[Export ("willRetry")]
		bool WillRetry { get; set; }

		// @property (nonatomic, strong) NSDictionary * jsonResponse;
		[Export ("jsonResponse", ArgumentSemantic.Strong)]
		NSDictionary JsonResponse { get; set; }
	}

	// @interface ADJSessionSuccess : NSObject <NSCopying>
	[BaseType (typeof(NSObject))]
	interface ADJSessionSuccess : INSCopying
	{
		// @property (copy, nonatomic) NSString * _Nullable message;
		[NullAllowed, Export ("message")]
		string Message { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable timestamp;
		[NullAllowed, Export ("timestamp")]
		string Timestamp { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable adid;
		[NullAllowed, Export ("adid")]
		string Adid { get; set; }

		// @property (nonatomic, strong) NSDictionary * _Nullable jsonResponse;
		[NullAllowed, Export ("jsonResponse", ArgumentSemantic.Strong)]
		NSDictionary JsonResponse { get; set; }
	}

	// @interface ADJSessionFailure : NSObject <NSCopying>
	[BaseType (typeof(NSObject))]
	interface ADJSessionFailure : INSCopying
	{
		// @property (copy, nonatomic) NSString * _Nullable message;
		[NullAllowed, Export ("message")]
		string Message { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable timestamp;
		[NullAllowed, Export ("timestamp")]
		string Timestamp { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable adid;
		[NullAllowed, Export ("adid")]
		string Adid { get; set; }

		// @property (assign, nonatomic) BOOL willRetry;
		[Export ("willRetry")]
		bool WillRetry { get; set; }

		// @property (nonatomic, strong) NSDictionary * _Nullable jsonResponse;
		[NullAllowed, Export ("jsonResponse", ArgumentSemantic.Strong)]
		NSDictionary JsonResponse { get; set; }
	}

	// 824
	// @interface ADJAdRevenue : NSObject <NSCopying>
	[BaseType (typeof(NSObject))]
	interface ADJAdRevenue : INSCopying
	{
		// -(id _Nullable)initWithSource:(NSString * _Nonnull)source;
		[Export ("initWithSource:")]
		NativeHandle Constructor (string source);

		// -(void)setRevenue:(double)amount currency:(NSString * _Nonnull)currency;
		[Export ("setRevenue:currency:")]
		void SetRevenue (double amount, string currency);

		// -(void)setAdImpressionsCount:(int)adImpressionsCount;
		[Export ("setAdImpressionsCount:")]
		void SetAdImpressionsCount (int adImpressionsCount);

		// -(void)setAdRevenueNetwork:(NSString * _Nonnull)adRevenueNetwork;
		[Export ("setAdRevenueNetwork:")]
		void SetAdRevenueNetwork (string adRevenueNetwork);

		// -(void)setAdRevenueUnit:(NSString * _Nonnull)adRevenueUnit;
		[Export ("setAdRevenueUnit:")]
		void SetAdRevenueUnit (string adRevenueUnit);

		// -(void)setAdRevenuePlacement:(NSString * _Nonnull)adRevenuePlacement;
		[Export ("setAdRevenuePlacement:")]
		void SetAdRevenuePlacement (string adRevenuePlacement);

		// -(void)addCallbackParameter:(NSString * _Nonnull)key value:(NSString * _Nonnull)value;
		[Export ("addCallbackParameter:value:")]
		void AddCallbackParameter (string key, string value);

		// -(void)addPartnerParameter:(NSString * _Nonnull)key value:(NSString * _Nonnull)value;
		[Export ("addPartnerParameter:value:")]
		void AddPartnerParameter (string key, string value);
	}

	// 912
	// @interface ADJAppStorePurchase : NSObject <NSCopying>
	[BaseType (typeof(NSObject))]
	interface ADJAppStorePurchase : INSCopying
	{
		// -(id _Nullable)initWithTransactionId:(NSString * _Nonnull)transactionId productId:(NSString * _Nonnull)productId;
		[Export ("initWithTransactionId:productId:")]
		NativeHandle Constructor (string transactionId, string productId);
	}

	// @interface ADJPurchaseVerificationResult : NSObject
	[BaseType (typeof(NSObject))]
	interface ADJPurchaseVerificationResult
	{
		// @property (copy, nonatomic) NSString * _Nonnull message;
		[Export ("message")]
		string Message { get; set; }

		// @property (assign, nonatomic) int code;
		[Export ("code")]
		int Code { get; set; }

		// @property (copy, nonatomic) NSString * _Nonnull verificationStatus;
		[Export ("verificationStatus")]
		string VerificationStatus { get; set; }
	}

	// 946
	// @interface ADJDeeplink : NSObject
	[BaseType (typeof(NSObject))]
	interface ADJDeeplink
	{
		// -(ADJDeeplink * _Nullable)initWithDeeplink:(NSURL * _Nonnull)deeplink;
		[Export ("initWithDeeplink:")]
		NativeHandle Constructor (NSUrl deeplink);
	}

}