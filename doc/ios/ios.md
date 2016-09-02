## Summary

This is the Xamarin SDK of adjust™. You can read more about adjust™ at [adjust.com].

## Table of contents

* [Example apps](#example-apps)
* [Basic integration](#basic-integration)
   * [Get the SDK](#sdk-get)
   * [Add the SDK to your project](#sdk-add)
   * [Add the SDK project reference to your app](#sdk-add-project)
   * [Add the SDK DLL reference to your app](#sdk-add-dll)
   * [Integrate the SDK into your app](#sdk-integrate)
   * [Adjust logging](#adjust-logging)
   * [Additional settings](#additional-settings)
   * [Build your app](#build-your-app)
* [Additional features](#additional-features)
   * [Event tracking](#event-tracking)
      * [Revenue tracking](#revenue-tracking)
      * [Revenue deduplication](#revenue-deduplication)
      * [In-App Purchase verification](#iap-verification)
      * [Callback parameters](#callback-parameters)
      * [Partner parameters](#partner-parameters)
   * [Attribution callback](#attribution-callback)
   * [Session and event callbacks](#session-event-callbacks)
   * [Disable tracking](#disable-tracking)
   * [Offline mode](#offline-mode)
   * [Event buffering](#event-buffering)
   * [Background tracking](#background-tracking)
   * [Device IDs](#device-ids)
   * [Push notifications token](#push-token)
   * [AdWords Search and Mobile Web tracking](#adwords-tracking)
   * [Pre-installed trackers](#pre-installed-trackers)
   * [Deep linking](#deeplinking)
      * [Standard deep linking scenario](#deeplinking-standard)
      * [Deep linking on iOS 8 and earlier](#deeplinking-setup-old)
      * [Deep linking on iOS 9 and later](#deeplinking-setup-new)
      * [Deferred deep linking scenario](#deeplinking-deferred)
      * [Reattribution via deep links](#deeplinking-reattribution)
* [License](#license)

## <a id="example-apps">Example apps

There is an iOS example app inside the [`iOS` directory][demo-app-ios]. You can open the Xamarin Studio project to see an 
example on how the adjust SDK can be integrated.

## <a id="basic-integration">Basic integration

We will describe the steps to integrate the adjust SDK into your Xamarin Studio iOS project. We are going to assume that you
use Xamarin Studio for your iOS development.

### <a id="sdk-get">Get the SDK

Download the latest version from our [releases page][releases]. Extract the archive into a directory of your choice.

If you want to use adjust bindings DLL you can start with [this step](#sdk-add-dll).

### <a id="sdk-add">Add the SDK to your project

Choose to add an existing project to your solution.

![][add_ios_binding]

Select the `AdjustSdk.Xamarin.iOS` project file and hit `Open`.

![][select_ios_binding]

After this, you will have the adjust iOS bindings added as a submodule to your solution.

![][submodule_ios_binding]

### <a id="sdk-add-project">Add the SDK project reference to your app

After you have successfully added the adjust iOS bindings project to your solution, you should also add a reference to it in
your iOS app project properties.

![][reference_ios_binding]

In case you don't want to add reference to the adjust SDK via project reference, you can skip this step and add it as DLL 
reference to your app which is explained in the step below.

### <a id="sdk-add-dll">Add the SDK DLL reference to your app

The next step is to add a reference to the bindings DLL in your iOS project properties. In the references window, choose the
`.Net Assembly` pane and select the `AdjustSdk.Xamarin.iOS.dll` that you have downloaded.

![][select_ios_dll]

### <a id="sdk-integrate">Integrate the SDK into your app

To start with, we'll set up basic session tracking.

Open the source file of your app delegate. Add the `using` statement at the top of the file, then add the following call to 
`Adjust` in the `FinishedLaunching` method of your app delegate:

```cs
using AdjustBindingsiOS;

// ...

string yourAppToken = "{YourAppToken}";
string environment = AdjustConfig.EnvironmentSandbox;

var config = ADJConfig.ConfigWithAppToken(yourAppToken, environment);

Adjust.AppDidLaunch(adjustConfig);
```

Replace `{YourAppToken}` with your app token. You can find this in your [dashboard].

Depending on whether you build your app for testing or for production, you must set `environment` with one of these values:

```cs
string environment = AdjustConfig.EnvironmentSandbox;
string environment = AdjustConfig.EnvironmentProduction;
```

**Important:** This value should be set to `AdjustConfig.EnvironmentSandbox` if and only if you or someone else is testing 
your app. Make sure to set the environment to `AdjustConfig.EnvironmentProduction` just before you publish the app. Set it 
back to `AdjustConfig.EnvironmentSandbox` when you start developing and testing it again.

We use this environment to distinguish between real traffic and test traffic from test devices. It is very important that 
you keep this value meaningful at all times! This is especially important if you are tracking revenue.

### <a id="adjust-logging">Adjust logging

You can increase or decrease the amount of logs you see in tests by setting `LogLevel` property on your `ADJConfig` instance
with one of the following parameters:

```cs
config.LogLevel = ADJLogLevel.Verbose; // enable all logging
config.LogLevel = ADJLogLevel.Debug;   // enable more logging
config.LogLevel = ADJLogLevel.Info;    // the default
config.LogLevel = ADJLogLevel.Warn;    // disable info logging
config.LogLevel = ADJLogLevel.Error;   // disable warnings as well
config.LogLevel = ADJLogLevel.Assert;  // disable errors as well
```

### <a id="additional-settings">Additional settings

In order to get Xamarin iOS app project to recognize categories from adjust iOS bindings, you need to add additional mtouch 
arguments to your `iOS Build`. You can find this in the `Build` section of your `Project Options`. Add the `-gcc_flags` 
option followed by a quoted string, containing the `-ObjC` argument.

![][additional_flags]

### <a id="build-your-app">Build your app

Build and run your app. If the build succeeds, you should carefully read the SDK logs in the console. After the app launched
for the first time, you should see the info log `Install tracked`.

![][run]

## <a id="additional-features">Additional features

Once you integrate the adjust SDK into your app, you can take advantage of the following features.

### <a id="event-tracking">Event tracking

You can use adjust to track events. Let's say you want to track every tap on a particular button. You would create a new 
event token in your [dashboard], which has an associated event token - resembling something like `abc123`. In your button's 
`TouchUpInside` handler you would then add the following lines to track the tap:

```cs
var adjustEvent = ADJEvent.EventWithEventToken("abc123");

Adjust.TrackEvent(adjustEvent);
```

When tapping the button you should now see `Event tracked` in the logs.

#### <a id="revenue-tracking">Revenue tracking

If your users can generate revenue by tapping on advertisements or making In-App Purchases you can track those revenues with
events. Let's say a tap is worth one Euro cent. You could then track the revenue event like this:

```cs
var adjustEvent = ADJEvent.EventWithEventToken("abc123");

adjustEvent.SetRevenue(0.01, "EUR");

Adjust.TrackEvent(adjustEvent);
```

When you set a currency token, adjust will automatically convert the incoming revenues into a reporting revenue of your 
choice. Read more about [currency conversion here.][currency-conversion]

You can read more about revenue and event tracking in the [event tracking guide][event-tracking].

#### <a id="revenue-deduplication"></a> Revenue deduplication

You can also add an optional transaction ID to avoid tracking duplicate revenues. The last ten transaction IDs are
remembered, and revenue events with duplicate transaction IDs are skipped. This is especially useful for In-App Purchase
tracking. You can see an example below.

If you want to track in-app purchases, please make sure to call `TrackEvent` after `FinishTransaction` in 
`UpdatedTransaction` only if the state changed to `SKPaymentTransactionState.Purchased`. That way you can avoid tracking 
revenue that is not actually being generated.

```cs
public void UpdatedTransactions(SKPaymentQueue queue, SKPaymentTransaction[] transactions)
{
    foreach (SKPaymentTransaction transaction in transactions) 
    {
        switch (transaction.TransactionState)
        {
            case SKPaymentTransactionState.Purchased:
                SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);

                var adjustEvent = ADJEvent.EventWithEventToken("abc123");

                adjustEvent.SetRevenue(0.01, "{currency}");
                adjustEvent.SetTransactionId(transaction.TransactionIdentifier);

                Adjust.TrackEvent(adjustEvent);

                break;

                // More cases
        }
    }
}
```

#### <a id="iap-verification">In-App Purchase verification

In-App purchase verification can be done with Xamarin purchase SDK which is currently being developed and will soon be 
publicly available.

#### <a id="callback-parameters">Callback parameters

You can register a callback URL for your events in your [dashboard]. We will send a GET request to that URL whenever the 
event is tracked. You can add callback parameters to that event by calling `AddCallbackParameter` on the event before 
tracking it. We will then append these parameters to your callback URL.

For example, suppose you have registered the URL `http://www.adjust.com/callback` then track an event like this:

```csharp
var adjustEvent = ADJEvent.EventWithEventToken("abc123");

adjustEvent.AddCallbackParameter("key", "value");
adjustEvent.AddCallbackParameter("foo", "bar");

Adjust.TrackEvent(adjustEvent);
```

In that case we would track the event and send a request to:

```
http://www.adjust.com/callback?key=value&foo=bar
```

It should be mentioned that we support a variety of placeholders like `{idfa}` that can be used as parameter values. In the 
resulting callback this placeholder would be replaced with the ID for Advertisers of the current device. Also note that we 
don't store any of your custom parameters, but only append them to your callbacks. If you haven't registered a callback for 
an event, these parameters won't even be read.

You can read more about using URL callbacks, including a full list of available values, in our 
[callbacks guide][callbacks-guide].

#### <a id="partner-parameters">Partner parameters

You can also add parameters to be transmitted to network partners, for the integrations that have been activated in your 
adjust dashboard.

This works similarly to the callback parameters mentioned above, but can be added by calling the `AddPartnerParameter` 
method on your `ADJEvent` instance.

```cs
var adjustEvent = ADJEvent.EventWithEventToken("abc123");

adjustEvent.AddPartnerParameter("key", "value");
adjustEvent.AddPartnerParameter("foo", "bar");

Adjust.TrackEvent(adjustEvent);
```

You can read more about special partners and these integrations in our [guide to special partners.][special-partners]

### <a id="attribution-callback">Attribution callback

You can register a callback to be notified of tracker attribution changes. Due to the different sources considered for 
attribution, this information can not by provided synchronously. Follow these steps to implement the optional callback in 
your app:

Please make sure to consider our [applicable attribution data policies.][attribution-data]

1. Open `AppDelegate.cs` and create a class which inherits from `AdjustDelegate` and override its `AdjustAttributionChanged`
method.
    
    ```cs
    public class AdjustDelegateXamarin : AdjustDelegate
    {
        public override void AdjustAttributionChanged (ADJAttribution attribution)
        {
            Console.WriteLine ("Attribution changed!");
            Console.WriteLine ("New attribution: {0}", attribution.ToString ());
        }
    }
    ```

2. Add the private field of type `AdjustDelegateXamarin` to this `AppDelegate` class.

    ```cs
    private AdjustDelegateXamarin adjustDelegate = null;
    ```

3. Initialize and set the delegate with your `ADJConfig` instance:

    ```cs
    adjustDelegate = new AdjustDelegateXamarin();
    
    // ...
    
    adjustConfig.Delegate = adjustDelegate;
    ```
    
As the delegate callback is configured using the `ADJConfig` instance, you should set the `Delegate` property before calling
`Adjust.AppDidLaunch(adjustConfig)`.

The callback function will be called  when the SDK receives final attribution data. Within the callback function you have 
access to the `attribution` parameter. Here is a quick summary of its properties:

- `string TrackerToken` the tracker token of the current install.
- `string TrackerName` the tracker name of the current install.
- `string Network` the network grouping level of the current install.
- `string Campaign` the campaign grouping level of the current install.
- `string Adgroup` the ad group grouping level of the current install.
- `string Creative` the creative grouping level of the current install.
- `string ClickLabel` the click label of the current install.

### <a id="session-event-callbacks">Session and event callbacks

You can register a callback to be notified of successful and failed events and/or sessions. Like with attribution callback, 
this should be done in your custom class which inherits from `AdjustDelegate`.

Follow the same steps to implement the following callback function for successful tracked events:

```cs
public class AdjustDelegateXamarin : AdjustDelegate
{
    public override void AdjustEventTrackingSucceeded(ADJEventSuccess eventSuccessResponseData)
    {
        Console.WriteLine("Event tracking succeeded! Info: " + eventSuccessResponseData.ToString());
    }
}
```

The following callback function for failed tracked events:

```cs
public class AdjustDelegateXamarin : AdjustDelegate
{
    public override void AdjustEventTrackingFailed(ADJEventFailure eventFailureResponseData)
    {
        Console.WriteLine("Event tracking failed! Info: " + eventFailureResponseData.ToString());
    }
}
```

For successful tracked sessions:

```cs
public class AdjustDelegateXamarin : AdjustDelegate
{
    public override void AdjustSessionTrackingSucceeded(ADJSessionSuccess sessionSuccessResponseData)
    {
        Console.WriteLine("Session tracking succeeded! Info: " + sessionSuccessResponseData.ToString());
    }
}
```

And for failed tracked sessions:

```cs
public class AdjustDelegateXamarin : AdjustDelegate
{
    public override void AdjustSessionTrackingFailed(ADJSessionFailure sessionFailureResponseData)
    {
        Console.WriteLine("Session tracking failed! Info: " + sessionFailureResponseData.ToString());
    }
}
```

The callback functions will be called after the SDK tries to send a package to the server. Within the callback you have 
access to a response data object specifically for the callback. Here is a quick summary of the session response data 
properties:

- `string Message` the message from the server or the error logged by the SDK.
- `string Timestamp` timestamp from the server.
- `string Adid` a unique device identifier provided by adjust.
- `NSDictionary<string, object> JsonResponse` the JSON object with the response from the server.

Both event response data objects contain:

- `string EventToken` the event token, if the package tracked was an event.

And both event and session failed objects also contain:

- `bool WillRetry` indicates there will be an attempt to resend the package at a later time.

### <a id="disable-tracking">Disable tracking

You can disable the adjust SDK from tracking by invoking the method `SetEnabled` with the enabled parameter as `false`. 
**This setting is remembered between sessions**, but it can only be activated after the first session.

```cs
Adjust.SetEnabled(false);
```

You can verify if the adjust SDK is currently active with the property `IsEnabled`. It is always possible to activate the 
adjust SDK by invoking `SetEnabled` with the enabled parameter set to `true`.

### <a id="offline-mode">Offline mode

You can put the adjust SDK in offline mode to suspend transmission to our servers, while retaining tracked data to be sent 
later. When in offline mode, all information is saved in a file, so be careful not to trigger too many events.

You can activate offline mode by calling `SetOfflineMode` with the parameter `true`.

```cs
Adjust.SetOfflineMode(true);
```

Conversely, you can deactivate offline mode by calling `SetOfflineMode` with `false`. When the adjust SDK is put back in 
online mode, all saved information is send to our servers with the correct time information.

Unlike disabling tracking, **this setting is not remembered** between sessions. This means that the SDK is in online mode 
whenever it is started, even if the app was terminated in offline mode.

### <a id="event-buffering">Event buffering

If your app makes heavy use of event tracking, you might want to delay some HTTP requests in order to send them in one batch
every minute. You can enable event buffering with your `ADJConfig` instance:

```cs
var config = ADJConfig.ConfigWithAppToken(yourAppToken, environment);

config.EventBufferingEnabled = true;

Adjust.AppDidLaunch(config);
```

If nothing is set, event buffering is **disabled by default**.

### <a id="background-tracking">Background tracking

The default behaviour of the adjust SDK is to **pause sending HTTP requests while the app is in the background**. You can 
change this in your `ADJConfig` instance:

```cs
var config = ADJConfig.ConfigWithAppToken(yourAppToken, environment);

config.SendInBackground = true;

Adjust.AppDidLaunch(config);
```

If nothing set, sending in background is **disabled by default**.

### <a id="device-ids">Device IDs

Certain services (such as Google Analytics) require you to coordinate Device and Client IDs in order to prevent duplicate 
reporting. 

To obtain the IDFA, get the value of the `Idfa` property of the `Adjust` instance:

```cs
string idfa = Adjust.Idfa;
```

### <a id="push-token">Push notifications token

To send us the push notifications token, then add the following call to `Adjust` instance when ever you get your token in 
the app or when it gets updated:

```cs
NSData pushNotificationsToken;	// Obtain and assign your push notification token as NSData type.

Adjust.SetDeviceToken(pushNotificationsToken);
```

### <a id="adwords-tracking">AdWords Search and Mobile Web tracking

If you are initialising the adjust SDK manually and want to support deterministic tracking for all AdWords web inventories, 
you just need to call the `SendAdWordsRequest` on the `Adjust` instance **before initialising the SDK** with the 
`AppDidLaunch` method.

```cs
Adjust.SendAdWordsRequest();
```

Please have in mind that this feature is **supported on iOS 9 and above**.

### <a id="pre-installed-trackers">Pre-installed trackers

If you want to use the adjust SDK to recognize users that found your app pre-installed on their device, follow these steps.

1. Create a new tracker in your [dashboard].
2. Open your app delegate and add set the default tracker of your `ADJConfig`:

    ```cs
    var config = ADJConfig.ConfigWithAppToken(yourAppToken, environment);

    config.DefaultTracker = "{TrackerToken}";

    Adjust.AppDidLaunch(config);
    ```

  Replace `{TrackerToken}` with the tracker token you created in step 2. Please note that the dashboard displays a tracker URL (including `http://app.adjust.com/`). In your source code, you should specify only the six-character token and not the
  entire URL.

3. Build and run your app. You should see a line like the following in app's log output:

    ```
    Default tracker: 'abc123'
    ```

### <a id="deeplinking">Deep linking

If you are using the adjust tracker URL with an option to deep link into your app from the URL, there is the possibility to 
get info about the deep link URL and its content. Hitting the URL can happen when the user has your app already installed 
(standard deep linking scenario) or if they don't have the app on their device (deferred deep linking scenario).

For more detailed information about deep linking in iOS and how to enable it for your app, please check our 
[official iOS SDK README][ios-readme-deeplinking].

#### <a id="deeplinking-standard">Standard deep linking scenario

If your user already has the app installed and hits the tracker URL with deep link information in it, your application will 
get opened and the content of the deep link will be sent to your app so that you can parse it and decide what to do next in 
your app. As you have read in our official iOS SDK README, deep linking in iOS at this moment can be done with usage of 
custom URL scheme in tracker URL (iOS 8 and earlier) or with usage of universal links (iOS 9 and newer).

Depending on which scenario you want to use for your app (or if you want to use them both to support wide range of devices),
you need to set up your app to handle one of these or both scenarios.

#### <a id="deeplinking-setup-old"> Deep linking on iOS 8 and earlier

In order to set deep linking support for iOS 8 and earlier devices, you need to set up custom URL scheme in your app's 
`Info.plist` file. Open your `Info.plist` file and go to `Advanced` tab. In there, fill in your `Bundle ID` value in 
`Identifier` field and custom URL scheme for your app in `URL Schemes` field.

![][deeplinking_custom_url_scheme]

With this completed, when you app gets opened by click on tracker URL with deep link info which contains your custom URL 
scheme in it, `OpenUrl` method in your `AppDelegate` class will get called and you will get the deep link info in there.

```cs
public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
{
    // url -> This is your deep link content.

    return true;
}
```

With this setup, you have successfully set up deep linking handling for iOS devices with iOS 8 and earlier versions.

#### <a id="deeplinking-setup-new"> Deep linking on iOS 9 and later

In order to set deep linking support for iOS 9 and later devices, you need to enable your app to handle Apple universal 
links. If you followed our official iOS SDK README instructions, you have successfully enabled `Associated Domains` for your
app in Apple Developer portal. In order to enable this in your app project, please open `Entitlements.plist`. In there you 
will see `Associated Domains` part which you need to enable by checking the `Enable Associated Domains` check box. After 
this you only need to add domain which was generated for you in the adjust dashboard.

![][deeplinking_universal_links]

With this completed, when your app gets opened by click on universal link, `ContinueUserActivity` method in your 
`AppDelegate` class will get called and you will get the deep link info in there.

```cs
public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
{
    if (userActivity.ActivityType == NSUserActivityType.BrowsingWeb)
    {
        // userActivity.WebPageUrl -> This is your deep link content.
    }

    return true;
}
```

With this setup, you have successfully set up deep linking handling for iOS devices with iOS 9 and later versions.

#### <a id="deeplinking-deferred">Deferred deep linking scenario

In order to get info about the URL content in a deferred deep linking scenario, you should implement a callback method in 
the same way like you were setting callbacks for attribution, events and sessions. You need to override 
`AdjustDeeplinkResponse` in the class which inherits from `AdjustDelegate`:

```cs
public class AdjustDelegateXamarin : AdjustDelegate
{
    public override bool AdjustDeeplinkResponse(NSUrl deeplink)
    {
        Console.WriteLine("adjust: Deferred deep link received! URL = " + deeplink.ToString());

        return true;
        // return false;
    }
}
```

In deferred deep linking scenario, there is one additional setting which can be set in the `AdjustDeeplinkResponse` method 
via it's return value. Once the adjust SDK gets the deferred deep link info, we are offering you the possibility to choose  
whether our SDK should open this URL or not. You can choose to set this option by returning either `true` if you want us to 
open the URL or `false` if you don't want us to do anything.

If this callback is not implemented, **the adjust SDK will always try to launch the URL by default**.

#### <a id="deeplinking-reattribution">Reattribution via deep links

Adjust enables you to run re-engagement campaigns with usage of deep links. For more information on how to do that, please 
check our [official docs][reattribution-with-deeplinks]. 

If you are using this feature, in order for your user to be properly reattributed, you need to make one additional call to 
the adjust SDK in your app.

Once you have received deep link content information in your app, add a call to `Adjust.AppWillOpenUrl` method. By making 
this call, the adjust SDK will try to find if there is any new attribution info inside of the deep link and if any, it will
be sent to the adjust backend. If your user should be reattributed due to a click on the adjust tracker URL with deep link 
content in it, you will see the [attribution callback](#attribution-callback) in your app being triggered with new 
attribution info for this user.

The call to `Adjust.AppWillOpenUrl` should be done like this to support deeplinking in all iOS versions:

```cs
public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
{
    // url -> This is your deep link content.
    Adjust.AppWillOpenUrl(url);

    return true;
}
```

```cs
public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
{
    if (userActivity.ActivityType == NSUserActivityType.BrowsingWeb)
    {
        // userActivity.WebPageUrl -> This is your deep link content.
        Adjust.AppWillOpenUrl(userActivity.WebPageUrl);
    }

    return true;
}
```

[dashboard]: 	http://adjust.com
[adjust.com]:	http://adjust.com

[releases]:             https://github.com/adjust/xamarin_sdk/releases
[demo-app-ios]:         /./iOS
[event-tracking]:       https://docs.adjust.com/en/event-tracking
[callbacks-guide]:      https://docs.adjust.com/en/callbacks
[special-partners]:     https://docs.adjust.com/en/special-partners
[attribution-data]:     https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[currency-conversion]:  https://docs.adjust.com/en/event-tracking/#tracking-purchases-in-different-currencies

[ios-readme-deeplinking]:	        https://github.com/adjust/ios_sdk/#deeplink-reattributions
[reattribution-with-deeplinks]:   https://docs.adjust.com/en/deeplinking/#manually-appending-attribution-data-to-a-deep-link

[run]: 			        https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/run.png
[select_ios_dll]: 	https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/select_ios_dll.png
[add_ios_binding]: 	https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/add_ios_binding.png
[additional_flags]:	https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/additional_flags.png

[select_ios_binding]: 		https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/select_ios_binding.png
[submodule_ios_binding]: 	https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/submodule_ios_binding.png
[reference_ios_binding]: 	https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/reference_ios_binding.png

[deeplinking_universal_links]:	https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/deeplinking_universal_links.png
[deeplinking_custom_url_scheme]:	https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/deeplinking_custom_url_scheme.png

## <a id="license">License

The adjust SDK is licensed under the MIT License.

Copyright (c) 2012-2016 adjust GmbH, http://www.adjust.com

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
