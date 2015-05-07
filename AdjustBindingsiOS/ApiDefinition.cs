using System;
using UIKit;
using Foundation;
using ObjCRuntime;

namespace AdjustBindingsiOS
{
	// @interface ADJEvent : NSObject <NSCopying>
	[BaseType (typeof(NSObject))]
	interface ADJEvent : INSCopying
	{
		// @property (readonly, copy, nonatomic) NSString * eventToken;
		[Export ("eventToken")]
		string EventToken { get; }

		// @property (readonly, copy, nonatomic) NSNumber * revenue;
		[Export ("revenue", ArgumentSemantic.Copy)]
		NSNumber Revenue { get; }

		// @property (readonly, nonatomic) NSDictionary * callbackParameters;
		[Export ("callbackParameters")]
		NSDictionary CallbackParameters { get; }

		// @property (readonly, nonatomic) NSDictionary * partnerParameters;
		[Export ("partnerParameters")]
		NSDictionary PartnerParameters { get; }

		// @property (readonly, copy, nonatomic) NSString * transactionId;
		[Export ("transactionId")]
		string TransactionId { get; }

		// @property (readonly, copy, nonatomic) NSString * currency;
		[Export ("currency")]
		string Currency { get; }

		// @property (readonly, copy, nonatomic) NSData * receipt;
		[Export ("receipt", ArgumentSemantic.Copy)]
		NSData Receipt { get; }

		// @property (readonly, assign, nonatomic) BOOL emptyReceipt;
		[Export ("emptyReceipt")]
		bool EmptyReceipt { get; }

		// +(ADJEvent *)eventWithEventToken:(NSString *)eventToken;
		[Static]
		[Export ("eventWithEventToken:")]
		ADJEvent EventWithEventToken (string eventToken);

		// -(id)initWithEventToken:(NSString *)eventToken;
		[Export ("initWithEventToken:")]
		IntPtr Constructor (string eventToken);

		// -(void)addCallbackParameter:(NSString *)key value:(NSString *)value;
		[Export ("addCallbackParameter:value:")]
		void AddCallbackParameter (string key, string value);

		// -(void)addPartnerParameter:(NSString *)key value:(NSString *)value;
		[Export ("addPartnerParameter:value:")]
		void AddPartnerParameter (string key, string value);

		// -(void)setRevenue:(double)amount currency:(NSString *)currency;
		[Export ("setRevenue:currency:")]
		void SetRevenue (double amount, string currency);

		// -(void)setTransactionId:(NSString *)transactionId;
		[Export ("setTransactionId:")]
		void SetTransactionId (string transactionId);

		// -(BOOL)isValid;
		[Export ("isValid")]
		bool IsValid { get; }

		// -(void)setReceipt:(NSData *)receipt transactionId:(NSString *)transactionId;
		[Export ("setReceipt:transactionId:")]
		void SetReceipt (NSData receipt, string transactionId);
	}

	// @interface ADJAttribution : NSObject <NSCoding, NSCopying>
	[BaseType (typeof(NSObject))]
	interface ADJAttribution : INSCoding, INSCopying
	{
		// @property (copy, nonatomic) NSString * trackerToken;
		[Export ("trackerToken")]
		string TrackerToken { get; set; }

		// @property (copy, nonatomic) NSString * trackerName;
		[Export ("trackerName")]
		string TrackerName { get; set; }

		// @property (copy, nonatomic) NSString * network;
		[Export ("network")]
		string Network { get; set; }

		// @property (copy, nonatomic) NSString * campaign;
		[Export ("campaign")]
		string Campaign { get; set; }

		// @property (copy, nonatomic) NSString * adgroup;
		[Export ("adgroup")]
		string Adgroup { get; set; }

		// @property (copy, nonatomic) NSString * creative;
		[Export ("creative")]
		string Creative { get; set; }

		// @property (copy, nonatomic) NSString * clickLabel;
		[Export ("clickLabel")]
		string ClickLabel { get; set; }

		// -(BOOL)isEqualToAttribution:(ADJAttribution *)attribution;
		[Export ("isEqualToAttribution:")]
		bool IsEqualToAttribution (ADJAttribution attribution);

		// +(ADJAttribution *)dataWithJsonDict:(NSDictionary *)jsonDict;
		[Static]
		[Export ("dataWithJsonDict:")]
		ADJAttribution DataWithJsonDict (NSDictionary jsonDict);

		// -(id)initWithJsonDict:(NSDictionary *)jsonDict;
		[Export ("initWithJsonDict:")]
		IntPtr Constructor (NSDictionary jsonDict);

		// -(NSDictionary *)dictionary;
		[Export ("dictionary")]
		NSDictionary Dictionary { get; }
	}

	// @protocol ADJLogger
	[Protocol, Model]
	interface ADJLogger
	{
		// @required -(void)setLogLevel:(ADJLogLevel)logLevel;
		[Abstract]
		[Export ("setLogLevel:")]
		void SetLogLevel (ADJLogLevel logLevel);

		// @required -(void)verbose:(NSString *)message, ...;
		[Internal, Abstract]
		[Export ("verbose:", IsVariadic = true)]
		void Verbose (string message, IntPtr varArgs);

		// @required -(void)debug:(NSString *)message, ...;
		[Internal, Abstract]
		[Export ("debug:", IsVariadic = true)]
		void Debug (string message, IntPtr varArgs);

		// @required -(void)info:(NSString *)message, ...;
		[Internal, Abstract]
		[Export ("info:", IsVariadic = true)]
		void Info (string message, IntPtr varArgs);

		// @required -(void)warn:(NSString *)message, ...;
		[Internal, Abstract]
		[Export ("warn:", IsVariadic = true)]
		void Warn (string message, IntPtr varArgs);

		// @required -(void)error:(NSString *)message, ...;
		[Internal, Abstract]
		[Export ("error:", IsVariadic = true)]
		void Error (string message, IntPtr varArgs);

		// @required -(void)assert:(NSString *)message, ...;
		[Internal, Abstract]
		[Export ("assert:", IsVariadic = true)]
		void Assert (string message, IntPtr varArgs);
	}

	// Autogenerated annotations:
	// [Protocol, Model]
	// Changed to annotations bellow like stated in "Binding Protocols" section of document on URL bellow:
	// http://www.mono-project.com/archived/monomac_documentation_binding_new_objectivec_types/#binding-properties

	// @protocol AdjustDelegate
	[BaseType (typeof(NSObject))]
	[Protocol, Model]
	interface AdjustDelegate
	{
		// @optional -(void)adjustAttributionChanged:(ADJAttribution *)attribution;
		[Export ("adjustAttributionChanged:")]
		void AdjustAttributionChanged (ADJAttribution attribution);
	}

	// @interface ADJConfig : NSObject <NSCopying>
	[BaseType (typeof(NSObject))]
	interface ADJConfig : INSCopying
	{
		// extern NSString *const ADJEnvironmentSandbox;
		[Field ("ADJEnvironmentSandbox", "__Internal")]
		NSString ADJEnvironmentSandbox { get; }

		// extern NSString *const ADJEnvironmentProduction;
		[Field ("ADJEnvironmentProduction", "__Internal")]
		NSString ADJEnvironmentProduction { get; }

		// @property (readonly, copy, nonatomic) NSString * appToken;
		[Export ("appToken")]
		string AppToken { get; }

		// @property (assign, nonatomic) ADJLogLevel logLevel;
		[Export ("logLevel", ArgumentSemantic.Assign)]
		ADJLogLevel LogLevel { get; set; }

		// @property (readonly, copy, nonatomic) NSString * environment;
		[Export ("environment")]
		string Environment { get; }

		// @property (copy, nonatomic) NSString * sdkPrefix;
		[Export ("sdkPrefix")]
		string SdkPrefix { get; set; }

		// @property (copy, nonatomic) NSString * defaultTracker;
		[Export ("defaultTracker")]
		string DefaultTracker { get; set; }

		// +(ADJConfig *)configWithAppToken:(NSString *)appToken environment:(NSString *)environment;
		[Static]
		[Export ("configWithAppToken:environment:")]
		ADJConfig ConfigWithAppToken (string appToken, string environment);

		// -(id)initWithAppToken:(NSString *)appToken environment:(NSString *)environment;
		[Export ("initWithAppToken:environment:")]
		IntPtr Constructor (string appToken, string environment);

		// @property (assign, nonatomic) BOOL eventBufferingEnabled;
		[Export ("eventBufferingEnabled")]
		bool EventBufferingEnabled { get; set; }

		// @property (assign, nonatomic) BOOL macMd5TrackingEnabled;
		[Export ("macMd5TrackingEnabled")]
		bool MacMd5TrackingEnabled { get; set; }

		// Binding for AdjustDelegate written as suggested in section "Binding Protocols" on URL bellow:
		// http://www.mono-project.com/archived/monomac_documentation_binding_new_objectivec_types/#binding-properties

		[Wrap ("WeakDelegate")]
		AdjustDelegate Delegate { get; set; }

		// @property (retain, nonatomic) NSObject<AdjustDelegate> * delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Retain)]
		NSObject WeakDelegate { get; set; }

		// Instead of autogenerated stuff:
		//		[Wrap ("WeakHasDelegate")]
		//		bool HasDelegate { get; set; }
		//
		//		// @property (assign, nonatomic) BOOL hasDelegate;
		//		[NullAllowed, Export ("hasDelegate")]
		//		NSObject WeakHasDelegate { get; set; }

		// Adding our own binding for hasDelegate bool.
		// @property (assign, nonatomic) BOOL hasDelegate;
		[Export ("hasDelegate")]
		bool HasDelegate { get; set; }

		// -(BOOL)isValid;
		[Export ("isValid")]
		bool IsValid { get; }
	}

	// Autogenerated binding didn't have "__Internal" property in annotation.
	// W/O this property, "Error CS0117: `MonoTouch.Constants' does not contain a definition for `AdjustBindingsiOSLibrary' (CS0117) (AdjustBindingsiOS)"
	// was being reported.
	// Moving Constants to ADJConfig to keep things consistant between Android and iOS bindings.
	//	[BaseType (typeof(NSObject))]
	//	[Protocol]
	//	public partial interface Constants
	//	{
	//		// extern NSString *const ADJEnvironmentSandbox;
	//		[Field ("ADJEnvironmentSandbox", "__Internal")]
	//		NSString ADJEnvironmentSandbox { get; }
	//
	//		// extern NSString *const ADJEnvironmentProduction;
	//		[Field ("ADJEnvironmentProduction", "__Internal")]
	//		NSString ADJEnvironmentProduction { get; }
	//	}

	// @interface Adjust : NSObject
	[BaseType (typeof(NSObject))]
	interface Adjust
	{
		// +(void)appDidLaunch:(ADJConfig *)adjustConfig;
		[Static]
		[Export ("appDidLaunch:")]
		void AppDidLaunch (ADJConfig adjustConfig);

		// +(void)trackEvent:(ADJEvent *)event;
		[Static]
		[Export ("trackEvent:")]
		void TrackEvent (ADJEvent @adjustEvent);

		// +(void)trackSubsessionStart;
		[Static]
		[Export ("trackSubsessionStart")]
		void TrackSubsessionStart ();

		// +(void)trackSubsessionEnd;
		[Static]
		[Export ("trackSubsessionEnd")]
		void TrackSubsessionEnd ();

		// +(void)setEnabled:(BOOL)enabled;
		[Static]
		[Export ("setEnabled:")]
		void SetEnabled (bool enabled);

		// +(BOOL)isEnabled;
		[Static]
		[Export ("isEnabled")]
		bool IsEnabled { get; }

		// +(void)appWillOpenUrl:(NSURL *)url;
		[Static]
		[Export ("appWillOpenUrl:")]
		void AppWillOpenUrl (NSUrl url);

		// +(void)setDeviceToken:(NSData *)deviceToken;
		[Static]
		[Export ("setDeviceToken:")]
		void SetDeviceToken (NSData deviceToken);

		// +(void)setOfflineMode:(BOOL)enabled;
		[Static]
		[Export ("setOfflineMode:")]
		void SetOfflineMode (bool enabled);

		// +(id)getInstance;
		[Static]
		[Export ("getInstance")]
		NSObject Instance { get; }
	}
}
