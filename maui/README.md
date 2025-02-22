## Summary

This is the MAUI SDK of Adjust™. It supports iOS and Android targets. You can read more about Adjust™ at [adjust.com]. 

## Table of contents

### Quick start

   * [Getting started](#qs-getting-started)
      * [Get the SDK](#qs-get-sdk)
      * [Add the SDK to your project](#qs-add-sdk)
      * [Integrate the SDK into your app](#qs-integrate-sdk)
      * [Adjust logging](#qs-adjust-logging)

### Deeplinking
   * [Deep linking](#dl)
      * [Standard deep linking scenario](#dl-standard)
      * [Deferred deep linking scenario](#dl-deferred)
      * [Reattribution via deep links](#dl-reattribution)
      * [Resolve Adjust short links](#dl-short-links-resolution)
      * [Get last deep link](#dl-get-last-deep-link)

### Event Sending

   * [Send event information](#es-sending)
   * [Event revenue](#es-revenue)
   * [Deduplicate revenue](#es-revenue-deduplication)

### Custom parameters

   * [Custom parameters overview](#cp)
   * [Event parameters](#cp-event-parameters)
      * [Event callback parameters](#cp-event-callback-parameters)
      * [Event partner parameters](#cp-event-partner-parameters)
      * [Event callback identifier](#cp-event-callback-id)
   * [Session parameters](#cp-session-parameters)
      * [Session callback parameters](#cp-session-callback-parameters)
      * [Session partner parameters](#cp-session-partner-parameters)

### Additional features

   * [AppTrackingTransparency framework](#ad-att-framework)
      * [App-tracking authorisation wrapper](#ad-ata-wrapper)
      * [Get current authorisation status](#ad-ata-getter)
   * [SKAdNetwork framework](#ad-skadn-framework)
      * [Update SKAdNetwork conversion value](#ad-skadn-update-conversion-value)
      * [Conversion value updated callback](#ad-skadn-cv-updated-callback)
   * [Push token (uninstall tracking)](#ad-push-token)
   * [Attribution callback](#ad-attribution-callback)
   * [Ad revenue tracking](#ad-ad-revenue)
   * [Subscription tracking](#ad-subscriptions)
   * [Session and event callbacks](#ad-session-event-callbacks)
   * [User attribution](#ad-user-attribution)
   * [Device IDs](#ad-device-ids)
      * [iOS advertising identifier](#ad-idfa)
      * [Google Play Services advertising identifier](#ad-gps-adid)
      * [Amazon advertising identifier](#ad-amazon-adid)
      * [Adjust device identifier](#ad-adid)
   * [Set external device ID](#set-external-device-id)
   * [Preinstalled apps](#ad-preinstalled-apps)
   * [Offline mode](#ad-offline-mode)
   * [Disable tracking](#ad-disable-tracking)
   * [Background tracking](#ad-background-tracking)
   * [GDPR right to be forgotten](#ad-gdpr-forget-me)
   * [Third-party sharing](#ad-third-party-sharing)
      * [Disable third-party sharing](#ad-disable-third-party-sharing)
      * [Enable third-party sharing](#ad-enable-third-party-sharing)
   * [Measurement consent](#ad-measurement-consent)
   * [Data residency](#ad-data-residency)
   * [COPPA compliance](#ad-coppa-compliance)
   * [Play Store Kids Apps](#ad-play-store-kids-apps)
   * [Disable AdServices information reading](#ad-disable-ad-services)

### License
  * [License agreement](#license)


## Quick start

### <a id="qs-getting-started"></a>Getting started

To integrate the Adjust SDK into your MAUI project, follow these steps.

### <a id="qs-get-sdk"></a>Get the SDK

You can download the latest version from our [releases page][releases].

### <a id="qs-add-sdk"></a>Add the SDK to your project

1. Download the `adjust-maui-beta.zip` file and unzip it to a folder above (not inside) your project.
2. In your `.csproj` project file, add a project reference like so:
    ```xml
    <ItemGroup>
        <ProjectReference Include="..\adjust-maui\adjustSdk\adjustSdk.csproj" />
    </ItemGroup>
    ```
3. Build the adjust projects with the following commands:
    ```zsh
    dotnet build adjust-maui/android/adjustSdk.AndroidBinding
    dotnet build adjust-maui/iOs/adjustSdk.iOSBinding
    dotnet build adjust-maui/adjustSdk/
    ```
4. Build your project

### <a id="qs-integrate-sdk"></a>Integrate the SDK into your app

In order to initialize SDK, you will need to specify app token and environment. Follow [these steps](https://help.adjust.com/en/dashboard/apps/app-settings#view-your-app-token) to find it in the dashboard. Depending on whether you are building your app for testing or for production, change the `Environment` setting to either 'Sandbox' or 'Production'.

**Important:** Set the value to `Sandbox` if you or someone else is testing your app. Make sure to set the `Environment` to `Production` before you publish the app. Set it back to `Sandbox` if you start testing again. Also, have in mind that by default Adjust dashboard is showing production traffic of your app, so in case you want to see traffic you generated while testing in sandbox mode, make sure to switch to sandbox traffic view within dashboard.

We use the environment setting to distinguish between real traffic and artificial traffic from test devices. Please make sure to keep your environment setting updated.

On the MainPage you can configure the sdk the following wa

```cs
using adjustSdk;

//...

var adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
Adjust.InitSdk(adjustConfig);
```

### <a id="qs-adjust-logging"></a>Adjust logging

You can increase or decrease the granularity of the logs you see by changing the value of `Log Level` to one of the following:

- `VERBOSE` - enable all logs
- `DEBUG` - disable verbose logs
- `INFO` - disable debug logs (default)
- `WARN` - disable info logs
- `ERROR` - disable warning logs
- `ASSERT` - disable error logs
- `SUPPRESS` - disable all logs

If you want to disable all of your log output when initializing the Adjust SDK, set the log level to suppress and use a constructor for the `AdjustConfig` object. This opens a boolean parameter where you can enter whether the suppress log level should be supported or not:

```cs
AdjustConfig config = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox, true);
config.setLogLevel(AdjustLogLevel.SUPPRESS);
Adjust.InitSdk(config);
```

## <a id="dl"></a>Deep linking

If you are using the Adjust campaign link URL with an option to deep link into your app from the URL, there is the possibility to get information about the deep link URL and its content. Hitting the URL can happen when the user has your app already installed (standard deep linking scenario) or if they don't have the app on their device (deferred deep linking scenario).

### <a id="dl-standard"></a>Standard deep linking scenario

The standard deep linking scenario is a platform specific feature in which an app gets opened when user clicks on the deep link. This guide is going to assume that you are aware how deep linking should be set up in your app for both, iOS and Android platforms.

### <a id="dl-deferred"></a>Deferred deep linking scenario

While deferred deep linking is not supported out of the box on Android and iOS, our Adjust SDK makes it possible.

In order to get information about the URL content in a deferred deep linking scenario, you should set a callback method on the `DeferredDeeplinkDelegate` property of the `AdjustConfig` object

This delegate function receives the deep link as a string argument.

If you want to open the deep link, return true in your delegate function. If you don’t want to open it, return false.


```cs
var adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
adjustConfig.DeferredDeeplinkDelegate = (string deeplink) => {
    Debug.Log("Deferred deep link callback called!");
    Debug.Log("Deep link URL: " + deeplink);

    return true;
};
```

If nothing is set, **the Adjust SDK will always try to launch the URL by default**.

### <a id="dl-reattribution"></a>Reattribution via deep links

Adjust enables you to run re-engagement campaigns through deep links.

If you are using this feature, in order for your user to be properly reattributed, you need to make one additional call to the Adjust SDK in your app.

Once you have received deep link content information in your app, add a call to the `ProcessDeeplink` method passing the `AdjustDeeplink` record with the deeplink as its constructor argument.
By making this call, the Adjust SDK will try to find if there is any new attribution information inside of the deep link. If there is any, it will be sent to the Adjust backend. If your user should be reattributed due to a click on the Adjust campaign link URL with deep link content, you will see the [attribution callback](#attribution-callback) in your app being triggered with new attribution info for this user.

```cs
Adjust.ProcessDeeplink(new ("your deeplink here"));
```

### <a id="dl-short-links-resolution"></a>Resolve Adjust short links

To resolve an Adjust shortened deep link, instantiate an `AdjustDeeplink` record with your shortened deep link and pass it to the `ProcessAndResolveDeeplink` method.
In addition to this, you need to declare a callback method that will get triggered once the link gets resolved.

```cs
Adjust.ProcessAndResolveDeeplink(new ("your deeplink here"),
    (string deeplink) => {
        Debug.Log("Resolved deep link URL: " + deeplink);
    });
```

> Note: If the link passed to the `ProcessAndResolveDeeplink` method was shortened, the callback function receives the extended original link. Otherwise, the callback function receives the link you passed.

### <a id="dl-get-last-deep-link"></a>Get last deep link

You can return the last deep link URL resolved by the `Adjust.ProcessDeeplink()` or `Adjust.ProcessAndResolveDeepLink()` method by calling the `Adjust.GetLastDeeplink()` method with a corresponding callback. This method returns the last resolved deep link as a deep link object.

```cs
Adjust.GetLastDeeplink((string? lastDeeplink) => {
    // ...
});
```

## Event tracking

### <a id="es-sending"></a>Send event information

You can use Adjust to track any event in your app. If you want to track every tap on a button, [create a new event token](https://help.adjust.com/en/tracking/in-app-events/basic-event-setup#generate-event-tokens-in-the-adjust-dashboard) in your dashboard. Let's say that the event token is `abc123`. In your button's click handler method, add the following lines to track the click:

```cs
var adjustEvent = new AdjustEvent("abc123");
Adjust.TrackEvent(adjustEvent);
```

### <a id="es-revenue"></a>Event revenue

If your users generate revenue by engaging with advertisements or making in-app purchases, you can track this with events. For example: if one add tap is worth one Euro cent, you can track the revenue event like this:

```cs
var adjustEvent = new AdjustEvent("abc123");
adjustEvent.SetRevenue(0.01, "EUR");
Adjust.TrackEvent(adjustEvent);
```

When you set a currency token, Adjust will automatically convert the incoming revenues using the openexchange API into a reporting revenue of your choice. [Read more about currency conversion here](http://help.adjust.com/tracking/revenue-events/currency-conversion).

If you want to track in-app purchases, please make sure to call `TrackEvent` only if the purchase is finished and the item has been purchased. This is important in order to avoid tracking revenue your users did not actually generate.

### <a id="es-revenue-deduplication"></a>Revenue deduplication

Add an optional transaction ID to avoid tracking duplicated revenues. The SDK remembers the last ten transaction IDs and skips revenue events with duplicate transaction IDs. This is especially useful for tracking in-app purchases. 

```cs
var adjustEvent = new AdjustEvent("abc123");
adjustEvent.SetRevenue(0.01, "EUR");
adjustEvent.DeduplicationId = "transactionId";
Adjust.TrackEvent(adjustEvent);
```

## Custom parameters

### <a id="cp"></a>Custom parameters overview

In addition to the data points the Adjust SDK collects by default, you can use the Adjust SDK to track and add as many custom values as you need (user IDs, product IDs, etc.) to the event or session. Custom parameters are only available as raw data and will **not** appear in your Adjust dashboard.

Use [callback parameters](https://help.adjust.com/en/manage-data/export-raw-data/callbacks/best-practices-callbacks) for the values you collect for your own internal use, and partner parameters for those you share with external partners. If a value (e.g. product ID) is tracked both for internal use and external partner use, we recommend using both callback and partner parameters.

### <a id="cp-event-parameters"></a>Event parameters

### <a id="cp-event-callback-parameters"></a>Event callback parameters

If you register a callback URL for events in your [dashboard], we will send a GET request to that URL whenever the event is tracked. You can also put key-value pairs in an object and pass it to the `TrackEvent` method. We will then append these parameters to your callback URL.

For example, if you've registered the URL `http://www.example.com/callback`, then you would track an event like this:

```cs
var adjustEvent = new AdjustEvent("abc123");
adjustEvent.AddCallbackParameter("key", "value");
adjustEvent.AddCallbackParameter("foo", "bar");
Adjust.TrackEvent(adjustEvent);
```

In this case we would track the event and send a request to:

```
http://www.example.com/callback?key=value&foo=bar
```

Adjust supports a variety of placeholders, for example `{idfa}` for iOS or `{gps_adid}` for Android, which can be used as parameter values.  Using this example, in the resulting callback we would replace the placeholder with the IDFA/ Google Play Services ID of the current device. Read more about [real-time callbacks](https://help.adjust.com/en/manage-data/export-raw-data/callbacks) and see our full list of [placeholders](https://partners.adjust.com/placeholders/). 

**Note:** We don't store any of your custom parameters. We only append them to your callbacks. If you haven't registered a callback for an event, we will not read these parameters.

### <a id="cp-event-partner-parameters"></a>Event partner parameters

Once your parameters are activated in the dashboard, you can send them to your network partners. Read more about [module partners](https://docs.adjust.com/en/special-partners/) and their extended integration.

This works the same way as callback parameters; add them by calling the `addPartnerParameter` method on your `AdjustEvent` instance.

```cs
var adjustEvent = new AdjustEvent("abc123");
adjustEvent.AddPartnerParameter("key", "value");
adjustEvent.AddPartnerParameter("foo", "bar");
Adjust.TrackEvent(adjustEvent);
```

You can read more about special partners and these integrations in our [guide to special partners][special-partners].

### <a id="cp-event-callback-id"></a>Event callback identifier

You can add custom string identifiers to each event you want to track. We report this identifier in your event callbacks, letting you know which event was successfully tracked. Set the identifier by setting the `CallbackId` property on your `AdjustEvent` instance:

```cs
var adjustEvent = new AdjustEvent("abc123");
adjustEvent.CallbackId = "Your-Custom-Id";
Adjust.TrackEvent(adjustEvent);
```

### <a id="cp-session-parameters"></a>Session parameters

Session parameters are saved locally and sent with every Adjust SDK **event and session**. Whenever you add these parameters, we save them (so you don't need to add them again). Adding the same parameter twice will have no effect.

### <a id="cp-session-callback-parameters"></a>Session callback parameters

You can save event callback parameters to be sent with every Adjust SDK session.

The session callback parameters' interface is similar to the one for event callback parameters. Instead of adding the key and its value to an event, add them via a call to the `AddGlobalCallbackParameter` method of the `Adjust` instance:

```cs
Adjust.AddGlobalCallbackParameter("foo", "bar");
```

Session callback parameters merge with event callback parameters, sending all of the information as one, but event callback parameters take precedence over session callback parameters. If you add an event callback parameter with the same key as a session callback parameter, we will show the event value.

You can remove a specific session callback parameter by passing the desired key to the `RemoveGlobalCallbackParameter` method of the `Adjust` instance.

```cs
Adjust.RemoveGlobalCallbackParameter("foo");
```

To remove all keys and their corresponding values from the session callback parameters, you can reset them with the `RemoveGlobalCallbackParameters` method of the `Adjust` instance.

```cs
Adjust.RemoveGlobalCallbackParameters();
```

### <a id="cp-session-partner-parameters"></a>Session partner parameters

In the same way that [session callback parameters](#cp-session-callback-parameters) are sent with every event or session that triggers our SDK, there are also session partner parameters.

These are transmitted to network partners for all of the integrations activated in your [dashboard].

The session partner parameters interface is similar to the event partner parameters interface, however instead of adding the key and its value to an event, add it by calling the `AddGlobalPartnerParameter` method of the `Adjust` instance.

```cs
Adjust.AddGlobalPartnerParameter("foo", "bar");
```

Session partner parameters merge with event partner parameters. However, event partner parameters take precedence over session partner parameters. If you add an event partner parameter with the same key as a session partner parameter, we will show the event value.

To remove a specific session partner parameter, pass the desired key to the `RemoveGlobalPartnerParameter` method of the `Adjust` instance.

```cs
Adjust.RemoveGlobalPartnerParameter("foo");
```

To remove all keys and their corresponding values from the session partner parameters, reset it with the `RemoveGlobalPartnerParameters` method of the `Adjust` instance.

```cs
Adjust.RemoveGlobalPartnerParameters();
```

## Additional features

Once you integrate the Adjust SDK into your project, you can take advantage of the following features:

### <a id="ad-att-framework"></a>AppTrackingTransparency framework

**Note**: This feature exists only in iOS platform.

For each package sent, the Adjust backend receives one of the following four (4) states of consent for access to app-related data that can be used for tracking the user or the device:

- Authorized
- Denied
- Not Determined
- Restricted

After a device receives an authorization request to approve access to app-related data, which is used for user device tracking, the returned status will either be Authorized or Denied.

Before a device receives an authorization request for access to app-related data, which is used for tracking the user or device, the returned status will be Not Determined.

If authorization to use app tracking data is restricted, the returned status will be Restricted.

The SDK has a built-in mechanism to receive an updated status after a user responds to the pop-up dialog, in case you don't want to customize your displayed dialog pop-up. To conveniently and efficiently communicate the new state of consent to the backend, Adjust SDK offers a wrapper around the app tracking authorization method described in the following chapter, App-tracking authorization wrapper.

### <a id="ad-ata-wrapper"></a>App-tracking authorisation wrapper

**Note**: This feature exists only in iOS platform.

Adjust SDK offers the possibility to use it for requesting user authorization in accessing their app-related data. Adjust SDK has a wrapper built on top of the [requestTrackingAuthorizationWithCompletionHandler:](https://developer.apple.com/documentation/apptrackingtransparency/attrackingmanager/3547037-requesttrackingauthorizationwith?language=objc) method, where you can as well define the callback method to get information about a user's choice. In order for this method to work, you need to specify a text which is going to be displayed as part of the tracking request dialog to your user. This setting is located inside of your iOS app `Info.plist` file under `NSUserTrackingUsageDescription` key. In case you don't want to add specify this on your own in your Xcode project, you can check Adjust prefab settings in inspector and specify this text under `User Tracking Description`. If specified there, Adjust iOS post-build process will make sure to add this setting into your app's `Info.plist` file.

Also, with the use of this wrapper, as soon as a user responds to the pop-up dialog, it's then communicated back using your callback method. The SDK will also inform the backend of the user's choice. The `NSUInteger` value will be delivered via your callback method with the following meaning:

- 0: `ATTrackingManagerAuthorizationStatusNotDetermined`
- 1: `ATTrackingManagerAuthorizationStatusRestricted`
- 2: `ATTrackingManagerAuthorizationStatusDenied`
- 3: `ATTrackingManagerAuthorizationStatusAuthorized`

To use this wrapper, you can call it as such:

```cs
#if IOS
Adjust.RequestAppTrackingAuthorization((status) =>
{
    switch (status)
    {
        case 0:
            // ATTrackingManagerAuthorizationStatusNotDetermined case
            break;
        case 1:
            // ATTrackingManagerAuthorizationStatusRestricted case
            break;
        case 2:
            // ATTrackingManagerAuthorizationStatusDenied case
            break;
        case 3:
            // ATTrackingManagerAuthorizationStatusAuthorized case
            break;
    }
});
#endif
```

### <a id="ad-ata-getter"></a>Get current authorisation status

**Note**: This feature exists only in iOS platform.

To get the current app tracking authorization status you can call `GetAppTrackingAuthorizationStatus` method of `Adjust` class that will return one of the following possibilities:

* `0`: The user hasn't been asked yet
* `1`: The user device is restricted
* `2`: The user denied access to IDFA
* `3`: The user authorized access to IDFA
* `-1`: The status is not available

### <a id="ad-skadn-framework"></a>SKAdNetwork framework

**Note**: This feature exists only in iOS platform.

If you have implemented the Adjust iOS SDK v4.23.0 or above and your app is running on iOS 14 and above, the communication with SKAdNetwork will be set on by default, although you can choose to turn it off. When set on, Adjust automatically registers for SKAdNetwork attribution when the SDK is initialized. If events are set up in the Adjust dashboard to receive conversion values, the Adjust backend sends the conversion value data to the SDK. The SDK then sets the conversion value. After Adjust receives the SKAdNetwork callback data, it is then displayed in the dashboard.

In case you don't want the Adjust SDK to automatically communicate with SKAdNetwork, you can disable that by setting the following property on configuration object:

```cs
#if IOS
adjustConfig.IsSkanAttributionEnabled = false;
#endif
```

### <a id="ad-skadn-update-conversion-value"></a>Update SKAdNetwork conversion value

**Note**: This feature exists only in iOS platform.

Conversion values are a mechanism used to measure user behavior in SKAdNetwork. You can map 64 conditions to values from 0 through 63 and send this integer value to SKAdNetwork on user install. This gives you insight into how your users interact with your app in the first few days.

If you manage your conversion values with Adjust, the servers update this value in the SDK. You can also update this value by using the `UpdateSkanConversionValue` method.


```cs
#if IOS
Adjust.UpdateSkanConversionValue(
    1,      // Your conversion value. Must be between 0 and 63.
    "low",  // "low", "medium" or "high" 
    false,  // Whether to send the postback before the conversion window ends.
    (string nsErrorString) => ...); // An optional completion handler you provide to catch and handle any errors
#endif
```

### <a id="ad-skadn-cv-updated-callback"></a>Conversion value updated callback

If you use Adjust to manage conversion values, the Adjust’s servers send conversion value updates to the SDK. You can set up a delegate function to listen for these changes assigning a function to the `SkanUpdatedDelegate` property of your AdjustConfig instance.

```cs
#if IOS
var adjustConfig = new AdjustConfig("{Your App Token}", 
    AdjustEnvironment.Sandbox);
adjustConfig.SkanUpdatedDelegate = (Dictionary<string,string> response) => {
    Debug.Log("Conversion value updated. Callback received");
    Debug.Log("Conversion value: " + response["fine_value"]);
    Debug.Log("Coarse conversion value: " + response["coarse_value"]);
    Debug.Log ("Will send before conversion value window ends: " 
        + response["lock_window"]);
    Debug.Log("Error message: " + response["error"]);
};
Adjust.InitSdk(config);
#endif
```

### <a id="ad-push-token"></a>Push token (uninstall tracking)

Push tokens are used for Audience Builder and client callbacks; they are also required for uninstall and reinstall tracking.

To send us a push notification token, call the `SetPushToken` method on the `Adjust` instance when you obtain your app's push notification token (or whenever its value changes):

```cs
Adjust.SetPushToken("YourPushNotificationToken");
```

### <a id="ad-attribution-callback"></a>Attribution callback

You can set up a callback to be notified about attribution changes. We consider a variety of different sources for attribution, so we provide this information asynchronously. Make sure to consider [applicable attribution data policies][attribution_data] before sharing any of your data with third-parties. 

Follow these steps to add the optional callback in your application:

1. Create a method with the signature of the delegate `Action<AdjustAttribution>`.

2. After creating the `AdjustConfig` object, set the `adjustConfig.AttributionChangedDelegate` property with the previously created method. You can also use a lambda with the same signature.
\
Because the callback is configured using the `AdjustConfig` instance, set `adjustConfig.AttributionChangedDelegate` before calling `Adjust.start`.

```cs
var adjustConfig = new AdjustConfig("{Your App Token}", 
    AdjustEnvironment.Sandbox);
adjustConfig.AttributionChangedDelegate = (AdjustAttribution adjustAttribution) => {
    Debug.Log("Attribution changed");
    // ...
};
Adjust.InitSdk(config);
```

The callback function will be called when the SDK receives final attribution data. Within the callback function you have access to the `attribution` parameter. Here is a quick summary of its properties:

- `string trackerToken` the tracker token of the current attribution
- `string trackerName` the tracker name of the current attribution
- `string network` the network grouping level of the current attribution
- `string campaign` the campaign grouping level of the current attribution
- `string adgroup` the ad group grouping level of the current attribution
- `string creative` the creative grouping level of the current attribution
- `string clickLabel` the click label of the current attribution
- `string adid` the Adjust device identifier
- `string costType` the cost type string
- `double? costAmount` the cost amount
- `string costCurrency` the cost currency string
- `string fbInstallReferrer` the Facebook install referrer information

**Note**: The cost data - `costType`, `costAmount` & `costCurrency` are only available when configured in `AdjustConfig` by setting the `IsCostDataInAttributionEnabled` property to `true`. If not configured or configured, but not being part of the attribution, these fields will have value `null`. This feature is available in SDK v4.24.0 and above.

### <a id="ad-ad-revenue"></a>Ad revenue tracking

You can track ad revenue information with Adjust SDK by invoking the following method:

```cs
// initialise with AppLovin MAX source
AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue("source");
// set revenue and currency
adjustAdRevenue.SetRevenue(1.00, "USD");
// optional parameters
adjustAdRevenue.AdImpressionsCount = 10;
adjustAdRevenue.AdRevenueNetwork = "network";
adjustAdRevenue.AdRevenueUnit = "unit";
adjustAdRevenue.AdRevenuePlacement = "placement";
// callback & partner parameters
adjustAdRevenue.AddCallbackParameter("key", "value");
adjustAdRevenue.AddPartnerParameter("key", "value");
// track ad revenue
Adjust.TrackAdRevenue(adjustAdRevenue);
```

Some of the sources we support the below `source` parameter values:

- `"applovin_max_sdk"` - representing AppLovin MAX platform
- `"mopub"` - representing MoPub platform
- `"admob_sdk"` - representing AdMob platform
- `"ironsource_sdk"` - representing IronSource platform
- `"admost_sdk"` - representing Admost platform
- `"unity_sdk"` - representing Unity platform
- `"helium_chartboost_sdk"` - representing Helium Chartboost
- `"topon_sdk"` - representing TopOn
- `"adx_sdk"` - representing AD(X)

**Note**: Additional documentation which explains detailed integration with every of the supported sources will be provided outside of this README. Also, in order to use this feature, additional setup is needed for your app in Adjust dashboard, so make sure to get in touch with our support team to make sure that everything is set up correctly before you start to use this feature.

### <a id="ad-subscriptions"></a>Subscription tracking

**Note**: This feature is only available in the SDK v4.22.0 and above.

You can track App Store and Play Store subscriptions and verify their validity with the Adjust SDK. After a subscription has been successfully purchased, make the following call to the Adjust SDK:

**For App Store subscription:**

```cs
#if IOS
var appStoreSubscription = new AdjustAppStoreSubscription(
    price,
    currency,
    transactionId,
    receipt);
appStoreSubscription.TransactionDate = transactionDate;
appStoreSubscription.SalesRegion = salesRegion;

Adjust.TrackAppStoreSubscription(appStoreSubscription);
#endif
```

**For Play Store subscription:**

```cs
#if ANDROID
var playStoreSubscription = new AdjustPlayStoreSubscription(
    price,
    currency,
    sku,
    orderId,
    signature,
    purchaseToken);
playStoreSubscription.PurchaseTime = purchaseTime;

Adjust.TrackPlayStoreSubscription(playStoreSubscription);
#endif
```

Subscription tracking parameters for App Store subscription:

- [price](https://developer.apple.com/documentation/storekit/skproduct/1506094-price?language=objc)
- currency (you need to pass [currencyCode](https://developer.apple.com/documentation/foundation/nslocale/1642836-currencycode?language=objc) of the [priceLocale](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc) object)
- [transactionId](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411288-transactionidentifier?language=objc)
- receipt(you need to pass properly formatted JSON `receipt` field of your purchased object returned from Unity IAP API)
- [transactionDate](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411273-transactiondate?language=objc)
- salesRegion (you need to pass [countryCode](https://developer.apple.com/documentation/foundation/nslocale/1643060-countrycode?language=objc) of the [priceLocale](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc) object)

Subscription tracking parameters for Play Store subscription:

- [price](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpriceamountmicros)
- [currency](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpricecurrencycode)
- [sku](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsku)
- [orderId](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getorderid)
- [signature](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsignature)
- [purchaseToken](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetoken)
- [purchaseTime](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetime)

Just like with event tracking, you can attach callback and partner parameters to the subscription object as well:

**For App Store subscription:**

```cs
#if IOS
var appStoreSubscription = new AdjustAppStoreSubscription(
    price,
    currency,
    transactionId,
    receipt);
appStoreSubscription.TransactionDate = transactionDate;
appStoreSubscription.SalesRegion = salesRegion;

// add callback parameters
appStoreSubscription.addCallbackParameter("key", "value");
appStoreSubscription.addCallbackParameter("foo", "bar");

// add partner parameters
appStoreSubscription.addPartnerParameter("key", "value");
appStoreSubscription.addPartnerParameter("foo", "bar");

Adjust.TrackAppStoreSubscription(appStoreSubscription);
#endif
```

**For Play Store subscription:**

```cs
#if ANDROID
var playStoreSubscription = new AdjustPlayStoreSubscription(
    price,
    currency,
    sku,
    orderId,
    signature,
    purchaseToken);
playStoreSubscription.PurchaseTime = purchaseTime;

// add callback parameters
playStoreSubscription.addCallbackParameter("key", "value");
playStoreSubscription.addCallbackParameter("foo", "bar");

// add partner parameters
playStoreSubscription.addPartnerParameter("key", "value");
playStoreSubscription.addPartnerParameter("foo", "bar");

Adjust.TrackPlayStoreSubscription(playStoreSubscription);
#endif
```

### <a id="ad-session-event-callbacks"></a>Session and event callbacks

You can set up callbacks to notify you of successful and failed events and/or sessions.

Follow these steps to add the callback function for successfully tracked events:

```cs
AdjustConfig adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
adjustConfig.EventSuccessDelegate = (AdjustEventSuccess adjustEventSuccess) => {
    // ...
});
Adjust.InitSdk(adjustConfig);
```

Add the following callback function for failed tracked events:

```cs
AdjustConfig adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
adjustConfig.EventFailureDelegate = (AdjustEventFailure adjustEventFailure) => {
    // ...
});
Adjust.InitSdk(adjustConfig);
```

For successfully tracked sessions:

```cs
AdjustConfig adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
adjustConfig.SessionSuccessDelegate = (AdjustSessionSuccess adjustSessionSuccess) => {
    // ...
});
Adjust.InitSdk(adjustConfig);
```

For failed tracked sessions:

```cs
AdjustConfig adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
adjustConfig.SessionFailureDelegate = (AdjustSessionFailure adjustSessionFailure) => {
    // ...
});
Adjust.InitSdk(adjustConfig);
```

Callback functions will be called after the SDK tries to send a package to the server. Within the callback you have access to a response data object specifically for the callback. Here is a quick summary of the session response data properties:

- `string Message` the message from the server or the error logged by the SDK
- `string Timestamp` timestamp from the server
- `string Adid` a unique device identifier provided by Adjust
- `JsonResponse` the JSON object with the response from the server, of type `Org.Json.JSONObject` for Android and `NSDictionary` for iOs

Both event response data objects contain:

- `string EventToken` the event token, if the package tracked was an event
- `string CallbackId` the custom defined callback ID set on an event object

Both event and session failed objects also contain:

- `bool WillRetry` indicates there will be an attempt to resend the package at a later time

### <a id="ad-user-attribution"></a>User attribution

When a user installs your app, Adjust attributes the install to a campaign. The Adjust SDK gives you access to campaign attribution details for your install. To return this information, pass a listener function to the `Adjust.GetAttribution` method. The SDK fetches the information asynchronously and passes it to your listener function as an AdjustAttribution object.

```cs
Adjust.GetAttribution((AdjustAttribution adjustAttribution) => {
    //...
});
```

### <a id="ad-device-ids"></a>Device IDs

The Adjust SDK lets you receive device identifiers.

### <a id="ad-idfa">iOS Advertising Identifier

The IDFA (ID for Advertisers) is a device-specific identifier for Apple devices. You can return the device’s IDFA by calling the `Adjust.GetIdfa` method with a completion handler. The SDK fetches the information asynchronously and passes it to your completion handler.


```cs
#if IOS
Adjust.GetIdfa((string idfa) => {
    //...
});
#endif
```

### <a id="ad-gps-adid"></a>Google Play Services advertising identifier
  
The Google Play Services Advertising Identifier (Google advertising ID) is a unique identifier for a device. Users can opt out of sharing their Google advertising ID by toggling the "Opt out of Ads Personalization" setting on their device. When a user has enabled this setting, the Adjust SDK returns a string of zeros when trying to read the Google advertising ID.
  
> **Important**: If you are targeting Android 12 and above (API level 31), you need to add the [`com.google.android.gms.AD_ID` permission](#gps-adid-permission) to your app. If you do not add this permission, you will not be able to read the Google advertising ID even if the user has not opted out of sharing their ID.

The Google advertising ID can only be read in a background thread. If you call the method `getGoogleAdId` of the `Adjust` instance with an `Action<string>` delegate, it will work in any situation:

```cs
#if ANDROID
Adjust.GetGoogleAdId((string googleAdid) => {
    //...
});
#endif
```

### <a id="ad-amazon-adid"></a>Amazon advertising identifier

The Amazon Advertising ID (Amazon Ad ID) is a device-specific identifier for Android devices. You can request this information asynchronously from the Adjust SDK by passing a callback method to the `Adjust.GetAmazonAdId` method.

```cs
#if ANDROID
Adjust.GetAmazonAdId((string amazonAdid) => {
    //...
});
#endif
```

### <a id="ad-adid"></a>Adjust device identifier

Adjust generates a unique Adjust Device ID (ADID) for each device. You can request this information asynchronously from the Adjust SDK by passing a callback method to the `Adjust.GetAdid` method.

```cs
Adjust.GetAdid((string adjustAdid) => {
    //...
});
```

Information about the adid is only available after our backend tracks the app install. It is not possible to access the adid value before the SDK has been initialized and the installation of your app has been successfully tracked.
  
### <a id="set-external-device-id"></a>Set external device ID

> **Note** If you want to use external device IDs, please contact your Adjust representative. They will talk you through the best approach for your use case.

An external device identifier is a custom value that you can assign to a device or user. They can help you to recognize users across sessions and platforms. They can also help you to deduplicate installs by user so that a user isn't counted as multiple new installs.

You can also use an external device ID as a custom identifier for a device. This can be useful if you use these identifiers elsewhere and want to keep continuity.

Check out our [external device identifiers article](https://help.adjust.com/en/article/external-device-identifiers) for more information.


To set an external device ID, assign the identifier to the `externalDeviceId` property of your config instance. Do this before you initialize the Adjust SDK.

```cs
adjustConfig.ExternalDeviceId = "{Your-External-Device-Id}";
```

> **Important** You need to make sure this ID is **unique to the user or device** depending on your use-case. Using the same ID across different users or devices could lead to duplicated data. Talk to your Adjust representative for more information.

If you want to use the external device ID in your business analytics, you can pass it as a session callback parameter. See the section on [session callback parameters](#cp-session-parameters) for more information.

You can import existing external device IDs into Adjust. This ensures that the backend matches future data to your existing device records. If you want to do this, please contact your Adjust representative.  

### <a id="ad-preinstalled-apps"></a>Preinstalled apps

You can use the Adjust SDK to recognize users whose devices had your app preinstalled during manufacturing. Adjust offers two solutions: one which uses the system payload, and one which uses a default tracker. 

In general, we recommend using the system payload solution. However, there are certain use cases which may require the tracker. First check the available [implementation methods](https://help.adjust.com/en/article/pre-install-tracking#Implementation_methods) and your preinstall partner’s preferred method. If you are unsure which solution to implement, reach out to integration@adjust.com

#### Use the system payload

- The Content Provider, System Properties, or File System method is supported from SDK v4.23.0 and above.

- The System Installer Receiver method is supported from SDK v4.27.0 and above.

Enable the Adjust SDK to recognise preinstalled apps by setting `IsPreinstallTrackingEnabled` with the value `true` after creating the config object:

```cs
#if ANDROID
adjustConfig.IsPreinstallTrackingEnabled = true;
#endif
```

#### Use a default tracker

- Create a new tracker in your [dashboard].
- Open your app delegate and set the default tracker of your config:

  ```cs
  adjustConfig.DefaultTracker = "{TrackerToken}";
  ```

- Replace `{TrackerToken}` with the tracker token you created in step one. Please note that the dashboard displays a tracker URL (including `http://app.adjust.com/`). In your source code, you should specify only the six or seven-character token and not the entire URL.

- Build and run your app. You should see a line like the following in your LogCat:

  ```
  Default tracker: 'abc123'
  ```
  
### <a id="ad-offline-mode"></a>Offline mode

Offline mode suspends transmission to our servers while retaining tracked data to be sent at a later point. While the Adjust SDK is in offline mode, all information is saved in a file. Please be careful not to trigger too many events in offline mode.

Activate offline mode by calling `SwitchToOfflineMode`.

```cs
Adjust.SwitchToOfflineMode();
```

Deactivate offline mode by calling `SwitchBackToOnlineMode`. When you put the Adjust SDK back into online mode, all saved information is sent to our servers with the correct time information.

This setting is not remembered between sessions, meaning that the SDK is in online mode whenever it starts, even if the app was terminated in offline mode.

### <a id="ad-disable-tracking"></a>Disable tracking

You can disable Adjust SDK tracking by invoking the method `Disable`. This setting is remembered between sessions, but it can only be activated after the first session.

```cs
Adjust.Disable();
```

You can check if the Adjust SDK is currently active with the method `IsEnabled`. It is always possible to activate the Adjust SDK by invoking `Enable`.

### <a id="ad-background-tracking"></a>Background tracking

The default behaviour of the Adjust SDK is to pause sending network requests while the app is in the background. You can change this in your `AdjustConfig` instance:

```cs
var adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
adjustConfig.IsSendingInBackgroundEnabled = true;
Adjust.InitSdk(adjustConfig);
```

### <a id="ad-gdpr-forget-me"></a>GDPR right to be forgotten

In accordance with article 17 of the EU's General Data Protection Regulation (GDPR), you can notify Adjust when a user has exercised their right to be forgotten. Calling the following method will instruct the Adjust SDK to communicate the user's choice to be forgotten to the Adjust backend:

```cs
Adjust.GdprForgetMe();
```

Upon receiving this information, Adjust will erase the user's data and the Adjust SDK will stop tracking the user. No requests from this device will be sent to Adjust in the future.

Please note that even when testing, this decision is permanent. It is not reversible.

## <a id="ad-third-party-sharing"></a>Third-party sharing for specific users

You can notify Adjust when a user disables, enables, and re-enables data sharing with third-party partners.

### <a id="ad-disable-third-party-sharing"></a>Disable third-party sharing for specific users

Call the following method to instruct the Adjust SDK to communicate the user's choice to disable data sharing to the Adjust backend:

```cs
var adjustThirdPartySharing = new AdjustThirdPartySharing(false);
Adjust.TrackThirdPartySharing(adjustThirdPartySharing);
```

Upon receiving this information, Adjust will block the sharing of that specific user's data to partners and the Adjust SDK will continue to work as usual.

### <a id="ad-enable-third-party-sharing">Enable or re-enable third-party sharing for specific users</a>

Call the following method to instruct the Adjust SDK to communicate the user's choice to share data or change data sharing, to the Adjust backend:

```cs
var adjustThirdPartySharing = new AdjustThirdPartySharing(true);
Adjust.TrackThirdPartySharing(adjustThirdPartySharing);
```

Upon receiving this information, Adjust changes sharing the specific user's data to partners. The Adjust SDK will continue to work as expected.

Call the following method to instruct the Adjust SDK to send the granular options to the Adjust backend:

```cs
var adjustThirdPartySharing = new AdjustThirdPartySharing(null);
adjustThirdPartySharing.AddGranularOption("PartnerA", "foo", "bar");
Adjust.TrackThirdPartySharing(adjustThirdPartySharing);
```

### <a id="ad-measurement-consent"></a>Consent measurement for specific users

You can notify Adjust when a user exercises their right to change data sharing with partners for marketing purposes, but they allow data sharing for statistical purposes. 

Call the following method to instruct the Adjust SDK to communicate the user's choice to change data sharing, to the Adjust backend:

```cs
Adjust.TrackMeasurementConsent(true);
```

Upon receiving this information, Adjust changes sharing the specific user's data to partners. The Adjust SDK will continue to work as expected.

### <a id="ad-data-residency"></a>Data residency

The URL strategy feature allows you to set either:

- The country in which Adjust stores your data (data residency).
- The endpoint to which the Adjust SDK sends traffic (URL strategy).

This is useful if you’re operating in a country with strict privacy requirements. When you set your URL strategy, Adjust stores data in the selected data residency region or sends traffic to the chosen domain.

To set your country of data residency, call the `AdjustConfig.setUrlStrategy` method following parameters:

- `domains` (`List<String>`): The country or countries of data residence, or the endpoints to which you want to send SDK traffic.
- `useSubdomains` (`boolean`): Whether the source should prefix a subdomain.
- `isDataResidency` (`boolean`): Whether the domain should be used for data residency.


```cs
var adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
adjustConfig.SetUrlStrategy(["adjust.net.in", "adjust.com"], true, false);
Adjust.InitSdk(adjustConfig);

```

### <a id="ad-coppa-compliance"></a>COPPA compliance

By default Adjust SDK doesn't mark app as COPPA compliant. In order to mark your app as COPPA compliant, make sure to call `setCoppaCompliantEnabled` method of `AdjustConfig` instance with boolean parameter `true`:

```cs
var adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
adjustConfig.IsCoppaComplianceEnabled = true;
Adjust.InitSdk(adjustConfig);
```

**Note:** By enabling this feature, third-party sharing will be automatically disabled for the users. If later during the app lifetime you decide not to mark app as COPPA compliant anymore, third-party sharing **will not be automatically re-enabled**. Instead, next to not marking your app as COPPA compliant anymore, you will need to explicitly re-enable third-party sharing in case you want to do that.

### <a id="ad-play-store-kids-apps"></a>Play Store Kids Apps

By default Adjust SDK doesn't mark Android app as Play Store Kids App. In order to mark your app as the app which is targetting kids in Play Store, make sure to set `IsPlayStoreKidsComplianceEnabled` property of `AdjustConfig` instance with `true`:

```cs
var adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
adjustConfig.IsPlayStoreKidsComplianceEnabled = true;
Adjust.InitSdk(adjustConfig);
```

### <a id="ad-disable-ad-services"></a>Disable AdServices information reading

The SDK is enabled by default to try to communicate with `AdServices.framework` on iOS in order to try to obtain attribution token which is later being used for handling Apple Search Ads attribution. In case you would not like Adjust to show information from Apple Search Ads campaigns, you can disable this in the SDK by setting `IsAdServicesEnabled` property of `AdjustConfig` to `false`:

```cs
#if IOS
var adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
adjustConfig.IsAdServicesEnabled = false;
Adjust.InitSdk(adjustConfig);
#endif
```


[dashboard]:  http://dash.adjust.com
[adjust.com]: http://adjust.com

[en-readme]:  README.md
[zh-readme]:  doc/chinese/README.md
[ja-readme]:  doc/japanese/README.md
[ko-readme]:  doc/korean/README.md

[sdk2sdk-mopub]:    doc/english/sdk-to-sdk/mopub.md

[ios]:                     https://github.com/adjust/ios_sdk
[android]:                 https://github.com/adjust/android_sdk
[releases]:                https://github.com/adjust/xamarin_sdk/releases
[google_ad_id]:            https://developer.android.com/google/play-services/id.html
[ios-deeplinking]:         https://github.com/adjust/ios_sdk/#deeplinking-reattribution
[attribution_data]:        https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[special-partners]:        https://docs.adjust.com/en/special-partners
[unity-purchase-sdk]:      https://github.com/adjust/unity_purchase_sdk
[android-deeplinking]:     https://github.com/adjust/android_sdk#deep-linking
[google_play_services]:    http://developer.android.com/google/play-services/setup.html
[android_sdk_download]:    https://developer.android.com/sdk/index.html#Other
[install-referrer-aar]:    https://maven.google.com/com/android/installreferrer/installreferrer/2.2/installreferrer-2.2.aar
[android-custom-receiver]: https://github.com/adjust/android_sdk/blob/master/doc/english/referrer.md

[menu_android]:             https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/menu_android.png
[adjust_editor]:            https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/adjust_editor.png
[import_package]:           https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/import_package.png
[android_sdk_location]:     https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download.png
[android_sdk_location_new]: https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download_new.png

[prefab-sdk-settings]:        https://raw.github.com/adjust/sdks/master/Resources/unity/prefab-sdk-settings.png
[prefab-post-build-settings]: https://raw.github.com/adjust/sdks/master/Resources/unity/prefab-post-build-settings.png

## License

### <a id="license"></a>License

The Adjust SDK is licensed under the MIT License.

Copyright (c) 2012-Present Adjust GmbH, http://www.adjust.com

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
