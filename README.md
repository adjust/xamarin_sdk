## Summary

This is the Xamarin SDK of Adjust™. You can read more about Adjust™ at [adjust.com].

## Table of contents

* [Example apps](#example-apps)
* [Basic integration](#basic-integration)
   * [Get the SDK](#sdk-get)
   * [Add the SDK from NuGet package manager](#sdk-add-nuget)
   * [Add the SDK project reference to your app](#sdk-add-project)
   * [Add the SDK DLL reference to your app](#sdk-add-dll)
   * [Additional settings](#sdk-additional-settings)
      * [[Android] Add Google Play Services](#android-gps)
      * [[Android] Add permissions](#android-permissions)
      * [[Android] Install referrer](#android-referrer)
         * [Google Play Referrer API](#android-referrer-gpr-api)
         * [Google Play Store intent](#android-referrer-gps-intent)
      * [[Android] Proguard settings](#android-proguard)
      * [[iOS] Additional linker flags](#ios-linker-flags)
   * [Integrate the SDK into your app](#sdk-integrate)
   * [Android session tracking](#session-tracking-android)
      * [API level 14 and higher](#session-tracking-api14)
      * [API level 9 until 13](#session-tracking-api9)
   * [Adjust logging](#adjust-logging)
   * [Build your app](#build-your-app)
* [Additional features](#additional-features)
   * [AppTrackingTransparency framework](#att-framework)
      * [App-tracking authorisation wrapper](#ata-wrapper)
      * [Get current authorisation status](#ata-getter)
   * [SKAdNetwork framework](#skadn-framework)
      * [Update SKAdNetwork conversion value](#skadn-update-conversion-value)
      * [Conversion value updated callback](#skadn-cv-updated-callback)
   * [Event tracking](#event-tracking)
      * [Revenue tracking](#revenue-tracking)
      * [Revenue deduplication](#revenue-deduplication)
      * [Callback parameters](#callback-parameters)
      * [Partner parameters](#partner-parameters)
      * [Callback identifier](#callback-id)
   * [Subscription tracking](#subscription-tracking)
   * [Session parameters](#session-parameters)
      * [Session callback parameters](#session-callback-parameters)
      * [Session partner parameters](#session-partner-parameters)
      * [Delay start](#delay-start)
   * [Attribution callback](#attribution-callback)
   * [Session and event callbacks](#session-event-callbacks)
   * [Disable tracking](#disable-tracking)
   * [Offline mode](#offline-mode)
   * [Event buffering](#event-buffering)
   * [GDPR right to be forgotten](#gdpr-forget-me)
   * [Disable third-party sharing](#disable-third-party-sharing)
   * [SDK signature](#sdk-signature)
   * [Background tracking](#background-tracking)
   * [Device IDs](#device-ids)
      * [iOS Advertising Identifier](#di-idfa)
      * [Google Play Services advertising identifier](#di-gps-adid)
      * [Amazon advertising identifier](#di-fire-adid)
      * [Adjust device identifier](#di-adid)
   * [Set external device ID](#set-external-device-id)
   * [Push token](#push-token)
   * [Pre-installed trackers](#pre-installed-trackers)
   * [Deep linking](#deeplinking)
      * [Standard deep linking scenario](#deeplinking-standard)
      * [Deep linking on iOS 8 and earlier](#deeplinking-setup-old)
      * [Deep linking on iOS 9 and later](#deeplinking-setup-new)
      * [Deferred deep linking scenario](#deeplinking-deferred)
      * [Reattribution via deep links](#deeplinking-reattribution)
   * [Data residency](#data-residency)
* [License](#license)

## <a id="example-apps"></a>Example apps

There is an iOS example app inside the [`iOS` directory][demo-app-ios] and Android example app inside the [`Android` directory][demo-app-android]. You can open the Xamarin Studio / Visual Studio for Mac project to see an example on how the Adjust SDK can be integrated.

## <a id="basic-integration"></a>Basic integration

We will describe the steps to integrate the Adjust SDK into your Xamarin project. We are going to assume that you use Xamarin Studio or Visual Studio for your development.

### <a id="sdk-get"></a>Get the SDK

Download the latest version from our [releases page][releases] or via NuGet package manager. In case you want to add Adjust SDK to your app:

- From NuGet package manager, continue from [this step](#sdk-add-nuget). 
- As project reference, continue from [this step](#sdk-add-project).
- As DLL reference, continue from [this step](#sdk-add-dll).

#### <a id="sdk-add-nuget"></a>Add the SDK from NuGet package manager

Right click on the `Packages` under the project in the Solution Explorer, then click on `Add Packages...`. In the newly opened `Add Packages` window, type in the search box `"Adjust Xamarin Android"` for Android project, or `"Adjust Xamarin iOS"` for iOS project. The Adjust Xamarin Android/iOS package should be the first search result. Click on it, and in the bottom right corner, click on `Add Package`.

### <a id="sdk-add-project"></a>Add the SDK as project reference

Choose to add an existing project to your solution. Select the `AdjustSdk.Xamarin.iOS` project file and hit `Open`. Repeat the same process for `AdjustSdk.Xamarin.Android` project. After this, you will have the Adjust SDK bindings added as a submodule to your solution. After you have successfully added the Adjust SDK bindings project to your solution, you should also add a reference to it in your app project properties. In case you don't want to add reference to the Adjust SDK via project reference, you can skip this step and add it as DLL reference to your app which is explained in the step below.

### <a id="sdk-add-dll"></a>Add the SDK as DLL reference

The next step is to add a reference to the bindings DLL in your project properties. In the references window, choose the `.Net Assembly` pane and select the `AdjustSdk.Xamarin.iOS.dll` and `AdjustSdk.Xamarin.Android.dll` that you have downloaded.

### <a id="sdk-additional-settings"></a>Additional settings

Once the Adjust SDK has been added to your app, certain tweaks need to be performed so that the Adjust SDK can work properly. Below you can find a description of every additional thing that you need to do after you've added the Adjust SDK into to your app.

### <a id="android-gps"></a>[Android] Add Google Play Services

Since the 1st of August of 2014, apps in the Google Play Store must use the Google Advertising ID to uniquely identify devices. To allow the Adjust SDK to use the Google Advertising ID, you must integrate Google Play Services. If you haven't done this yet, follow these steps:

- Choose to `Add Packages` by your `Packages` folder in Android app project.
- Search for `Xamarin Google Play Services - Analytics` and add them to your app.

### <a id="android-permissions"></a>[Android] Add permissions

In `Properties` folder, open the `AndroidManifest.xml` of your Android app project. Add the `Internet` and `AccessNetworkState` permission if it's not already there.

If you are **not targeting the Google Play Store**, add the `AccessWifiState` permission as well.

#### <a id="gps-adid-permission"></a>Add permission to gather Google advertising ID

If you are targeting Android 12 and above (API level 31), you need to add the `com.google.android.gms.AD_ID` permission to read the device's advertising ID. Add the following line to your `AndroidManifest.xml` to enable the permission.

```xml
<uses-permission android:name="com.google.android.gms.permission.AD_ID"/>
```

For more information, see [Google's `AdvertisingIdClient.Info` documentation](https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient.Info#public-string-getid).

### <a id="android-referrer"></a>[Android] Install referrer

In order to correctly attribute an install of your Android app to its source, Adjust needs information about the **install referrer**. This can be obtained by using the **Google Play Referrer API** or by catching the **Google Play Store intent** with a broadcast receiver.

**Important**: The Google Play Referrer API is newly introduced by Google with the express purpose of providing a more reliable and secure way of obtaining install referrer information and to aid attribution providers in the fight against click injection. It is **strongly advised** that you support this in your application. The Google Play Store intent is a less secure way of obtaining install referrer information. It will continue to exist in parallel with the new Google Play Referrer API temporarily, but it is set to be deprecated in future.

#### <a id="android-referrer-gpr-api"></a>Google Play Referrer API

Google Play Referrer API is already included in Adjust SDK. Nothing more needs to be done.

Also, make sure that you have paid attention to the [Proguard settings](#android-proguard) chapter and that you have added all the rules mentioned in it, especially the one needed for this feature:

```
-keep public class com.android.installreferrer.** { *; }
```

This feature is supported if you are using the **Adjust SDK v4.12.0 or above**.

#### <a id="android-referrer-gps-intent"></a>Google Play Store intent

The Google Play Store `INSTALL_REFERRER` intent should be captured with a broadcast receiver. The Adjust install referrer broadcast receiver is added to your app by default. For more information, you can check our native [Android SDK README][broadcast-receiver]. We've included a class called `AdjustReferrerReceiver.cs` in our plugin which automatically captures that intent. The full name of the class is `com.adjust.binding.AdjustReferrerReceiver.cs`. Feel free to use it for testing install referrers by triggering it manually. More on that [here][testing-broadcast-receivers].

Please bear in mind that if you are using your own broadcast receiver which handles the `INSTALL_REFERRER` intent, you don't need to use Adjust broadcast receiver. Simply add the call to the Adjust broadcast receiver as described in our [Android guide][broadcast-receiver-custom] in your custom receiver.

### <a id="android-proguard"></a>[Android] Proguard settings

If you are using Proguard, add these lines to your Proguard file:

```
-keep class com.adjust.sdk.** { *; }
-keep class com.google.android.gms.common.ConnectionResult {
    int SUCCESS;
}
-keep class com.google.android.gms.ads.identifier.AdvertisingIdClient {
    com.google.android.gms.ads.identifier.AdvertisingIdClient$Info getAdvertisingIdInfo(android.content.Context);
}
-keep class com.google.android.gms.ads.identifier.AdvertisingIdClient$Info {
    java.lang.String getId();
    boolean isLimitAdTrackingEnabled();
}
-keep public class com.android.installreferrer.** { *; }
```

If you are **not targeting the Google Play Store**, you can remove the `com.google.android.gms` rules.

### <a id="ios-linker-flags"></a>[iOS] Additional linker flags

Adjust SDK is able to get additional information by default linking weakly additional iOS frameworks to your app:

- `AdSupport.framework` - This framework is needed so that SDK can access to IDFA value and (prior to iOS 14) LAT information.
- `iAd.framework` - This framework is needed so that SDK can automatically handle attribution for ASA campaigns you might be running.
- `AdServices.framework` - For devices running iOS 14.3 or higher, this framework allows the SDK to automatically handle attribution for ASA campaigns. It is required when leveraging the Apple Ads Attribution API.
- `StoreKit.framework` - This framework is needed for access to `SKAdNetwork` framework and for Adjust SDK to handle communication with it automatically in iOS 14 or later.
- `AppTrackingTransparency.framework` - This framework is needed in iOS 14 and later for SDK to be able to wrap user's tracking consent dialog and access to value of the user's consent to be tracked or not.

### <a id="sdk-integrate"></a>Integrate the SDK into your app

To start with, we'll set up basic session tracking.

---

**For iOS app:**

Open the source file of your app delegate. Add the `using` statement at the top of the file, then add the following call to `Adjust` in the `FinishedLaunching` method of your app delegate:

```cs
using AdjustBindingsiOS;

// ...

string yourAppToken = "{YourAppToken}";
string environment = AdjustConfig.EnvironmentSandbox;

var config = ADJConfig.ConfigWithAppToken(yourAppToken, environment);

Adjust.AppDidLaunch(config);
```

---

**For Android app:**

We recommend using a global Android [Application][android_application] class to initialize the SDK. If don't have one in your app already, create a class that extends `Application`. In your `Application` class, find or create the `onCreate` method and add the following code to initialize the Adjust SDK:

```cs
using Com.Adjust.Sdk;

// ...

string appToken = "{YourAppToken}";
string environment = AdjustConfig.EnvironmentSandbox;

var config = new AdjustConfig(this, appToken, environment);

Adjust.OnCreate(config);
```

Please, pay special attention to [Android session tracking](#session-tracking-android) chapter as well, since it is vital for enabling proper session tracking for your Android app.

---

Replace `{YourAppToken}` with your app token. You can find this in your [dashboard].

Depending on whether you build your app for testing or for production, you must set `environment` with one of these values:

```cs
string environment = AdjustConfig.EnvironmentSandbox;
string environment = AdjustConfig.EnvironmentProduction;
```

**Important:** This value should be set to `AdjustConfig.EnvironmentSandbox` if and only if you or someone else is testing your app. Make sure to set the environment to `AdjustConfig.EnvironmentProduction` just before you publish the app. Set it back to `AdjustConfig.EnvironmentSandbox` when you start developing and testing it again.

We use this environment to distinguish between real traffic and test traffic from test devices. It is very important that you keep this value meaningful at all times! This is especially important if you are tracking revenue.

### <a id="session-tracking-android"></a>Android session tracking

**Note**: This step is **really important** and please **make sure that you implement it properly in your app**. By implementing it, you will enable proper session tracking by the Adjust SDK in your app.

### <a id="session-tracking-api14"></a>API level 14 and higher

- Add a private class that implements the `Java.Lang.Object` and  `IActivityLifecycleCallbacks` interface. If you don't have access to this interface, your app is targeting an Android API level inferior to 14. You will have to update manually each Activity by following these [instructions](#session-tracking-api9). If you had `Adjust.OnResume` and `Adjust.OnPause` calls on each Activity of your app before, you should remove them.
- Edit the `OnActivityResumed(Activity activity)` method and add a call to `Adjust.OnResume()`. Edit the `OnActivityPaused(Activity activity)` method and add a call to `Adjust.OnPause()`.
- After the adjust SDK is configured, add a call to the `RegisterActivityLifecycleCallbacks` method with an instance of the created `ActivityLifecycleCallbacks` class.

    ```cs
    using Com.Adjust.Sdk;

    // ...

    [Application(AllowBackup = true)]
    public class GlobalApplication : Application
    {
        public GlobalApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            string appToken = "{YourAppToken}";
            string environment = AdjustConfig.EnvironmentSandbox;

            var config = new AdjustConfig(this, appToken, environment);

            Adjust.OnCreate(config);

            RegisterActivityLifecycleCallbacks(new ActivityLifecycleCallbacks());
        }

        private class ActivityLifecycleCallbacks : Java.Lang.Object, IActivityLifecycleCallbacks
        {
            public void OnActivityPaused(Activity activity)
            {
                Adjust.OnPause();
            }

            public void OnActivityResumed(Activity activity)
            {
                Adjust.OnResume()
            }

            public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
            {
            }

            public void OnActivityDestroyed(Activity activity)
            {
            }

            public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
            {
            }

            public void OnActivityStarted(Activity activity)
            {
            }

            public void OnActivityStopped(Activity activity)
            {
            }
        }
    }
    ```

### <a id="session-tracking-api9"></a>API level 9 until 13

If your app's minimal supported API level is between `9` and `13`, consider updating it to at least `14` to simplify the integration process in the long term. Consult the official Android [dashboard][android-dashboard] to know the latest market share of the major versions.

If you still want to target these lower API levels, in order to provide proper session tracking it is required to call certain Adjust SDK methods every time any Activity resumes or pauses. Otherwise the SDK might miss a session start or session end. In order to do so you should **follow these steps for each Activity of your app**:

- Open the source file of your Activity.
- Add the `using` statement at the top of the file.
- In your Activity's `OnResume` method call `Adjust.OnResume`. Create the method if needed.
- In your Activity's `OnPause` method call `Adjust.OnPause`. Create the method if needed.

After these steps your activity should look like this:

```cs
using Com.Adjust.Sdk;

// ...

[Activity(Label = "Example", MainLauncher = true)]
public class MainActivity : Activity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }

    protected override void OnResume()
    {
        base.OnResume();

        Adjust.OnResume();
    }

    protected override void OnPause()
    {
        base.OnPause();

        Adjust.OnPause();
    }
}
```

Repeat these steps for **every** Activity of your app. Don't forget these steps when you create new Activities in the future. Depending on your coding style you might want to implement this in a common superclass of all your Activities.

### <a id="adjust-logging"></a>Adjust logging

**For iOS app:**

You can increase or decrease the amount of logs you see in tests by setting `LogLevel` property on your `ADJConfig` instance with one of the following parameters:

```cs
config.LogLevel = ADJLogLevel.Verbose;  // enable all logging
config.LogLevel = ADJLogLevel.Debug;    // enable more logging
config.LogLevel = ADJLogLevel.Info;     // the default
config.LogLevel = ADJLogLevel.Warn;     // disable info logging
config.LogLevel = ADJLogLevel.Error;    // disable warnings as well
config.LogLevel = ADJLogLevel.Assert;   // disable errors as well
config.LogLevel = ADJLogLevel.Suppress; // disable all logging
```

If you don't want your app in production to display any logs coming from the Adjust SDK, then you should select `ADJLogLevel.Suppress` and in addition to that, initialise `ADJConfig` object with another constructor where you should enable suppress log level mode:

```cs
using AdjustBindingsiOS;

// ...

string yourAppToken = "{YourAppToken}";
string environment = AdjustConfig.EnvironmentSandbox;

var config = ADJConfig.ConfigWithAppToken(yourAppToken, environment);
config.LogLevel = ADJLogLevel.Suppress;

Adjust.AppDidLaunch(config);
```

---

**For Android app:**

You can increase or decrease the amount of logs you see in tests by calling `SetLogLevel` on your `AdjustConfig` instance with one of the following parameters:

```cs
config.SetLogLevel(LogLevel.Verbose);     // enable all logging
config.SetLogLevel(LogLevel.Debug);       // enable more logging
config.SetLogLevel(LogLevel.Info);        // the default
config.SetLogLevel(LogLevel.Warn);        // disable info logging
config.SetLogLevel(LogLevel.Error);       // disable warnings as well
config.SetLogLevel(LogLevel.Assert);      // disable errors as well
config.SetLogLevel(LogLevel.Supress);    // disable all logging
```

If you don't want your app in production to display any logs coming from the adjust SDK, then you should select `LogLevel.Supress` and in addition to that, initialise `AdjustConfig` object with another constructor where you should enable suppress log level mode:

```cs
using Com.Adjust.Sdk;

// ...

string appToken = "{YourAppToken}";
string environment = AdjustConfig.EnvironmentSandbox;

AdjustConfig config = new AdjustConfig(this, appToken, environment, true);
config.SetLogLevel(LogLevel.Supress);

Adjust.OnCreate(config);
```

### <a id="build-your-app"></a>Build your app

Build and run your app. If the build succeeds, you should carefully read the SDK logs in the console. After the app launched for the first time, you should see the info log `Install tracked`.

## <a id="additional-features"></a>Additional features

Once you integrate the Adjust SDK into your app, you can take advantage of the following features.

### <a id="att-framework"></a>[iOS] AppTrackingTransparency framework

For each package sent, the Adjust backend receives one of the following four (4) states of consent for access to app-related data that can be used for tracking the user or the device:

- Authorized
- Denied
- Not Determined
- Restricted

After a device receives an authorization request to approve access to app-related data, which is used for user device tracking, the returned status will either be Authorized or Denied.

Before a device receives an authorization request for access to app-related data, which is used for tracking the user or device, the returned status will be Not Determined.

If authorization to use app tracking data is restricted, the returned status will be Restricted.

The SDK has a built-in mechanism to receive an updated status after a user responds to the pop-up dialog, in case you don't want to customize your displayed dialog pop-up. To conveniently and efficiently communicate the new state of consent to the backend, Adjust SDK offers a wrapper around the app tracking authorization method described in the following chapter, App-tracking authorization wrapper.

### <a id="ata-wrapper"></a>[iOS] App-tracking authorisation wrapper

Adjust SDK offers the possibility to use it for requesting user authorization in accessing their app-related data. Adjust SDK has a wrapper built on top of the [requestTrackingAuthorizationWithCompletionHandler:](https://developer.apple.com/documentation/apptrackingtransparency/attrackingmanager/3547037-requesttrackingauthorizationwith?language=objc) method, where you can as well define the callback method to get information about a user's choice. Also, with the use of this wrapper, as soon as a user responds to the pop-up dialog, it's then communicated back using your callback method. The SDK will also inform the backend of the user's choice. The `nuint` status value will be delivered via your callback method with the following meaning:

- 0: `ATTrackingManagerAuthorizationStatusNotDetermined`
- 1: `ATTrackingManagerAuthorizationStatusRestricted`
- 2: `ATTrackingManagerAuthorizationStatusDenied`
- 3: `ATTrackingManagerAuthorizationStatusAuthorized`

To use this wrapper, you can call it as such:

```cs
Adjust.RequestTrackingAuthorization((status) => {
    switch (status) {
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
```

### <a id="ata-getter"></a>[iOS] Get current authorisation status

To get the current app tracking authorization status you can get the value of `AppTrackingAuthorizationStatus` property of `Adjust` class that will return one of the following possibilities:

* `0`: The user hasn't been asked yet
* `1`: The user device is restricted
* `2`: The user denied access to IDFA
* `3`: The user authorized access to IDFA
* `-1`: The status is not available

### <a id="skadn-framework"></a>[iOS] SKAdNetwork framework

If you have implemented the Adjust Xamarin SDK v4.23.0 or above and your app is running on iOS 14 and above, the communication with SKAdNetwork will be set on by default, although you can choose to turn it off. When set on, Adjust automatically registers for SKAdNetwork attribution when the SDK is initialized. If events are set up in the Adjust dashboard to receive conversion values, the Adjust backend sends the conversion value data to the SDK. The SDK then sets the conversion value. After Adjust receives the SKAdNetwork callback data, it is then displayed in the dashboard.

In case you don't want the Adjust SDK to automatically communicate with SKAdNetwork, you can disable that by calling the following method on configuration object:

```cs
adjustConfig.DeactivateSKAdNetworkHandling();
```

### <a id="skadn-update-conversion-value"></a>[iOS] Update SKAdNetwork conversion value

You can use Adjust SDK wrapper method `UpdateConversionValue` to update SKAdNetwork conversion value for your user:

```cs
Adjust.UpdateConversionValue(6);
```

### <a id="skadn-cv-updated-callback"></a>[iOS] Conversion value updated callback

You can register callback to get notified each time when Adjust SDK updates conversion value for the user.

```cs
public class AdjustDelegateXamarin : AdjustDelegate
{
    public override void AdjustConversionValueUpdated(NSNumber conversionValue)
    {
        Console.WriteLine("adjust: Conversion value updated! Covnersion value: " + conversionValue.ToString());
    }
}
```

### <a id="event-tracking"></a>Event tracking

You can use Adjust to track events. Let's say you want to track every tap on a particular button. You would create a new event token in your [dashboard], which has an associated event token - resembling something like `abc123`. In your button's `TouchUpInside` handler you would then add the following lines to track the tap:

**For iOS app:**

```cs
var adjustEvent = ADJEvent.EventWithEventToken("abc123");
Adjust.TrackEvent(adjustEvent);
```

**For Android app:**

```cs
AdjustEvent eventClick = new AdjustEvent("abc123");
Adjust.TrackEvent(eventClick);
```

When tapping the button you should now see `Event tracked` in the logs.

### <a id="revenue-tracking"></a>Revenue tracking

If your users can generate revenue by tapping on advertisements or making In-App Purchases you can track those revenues with events. Let's say a tap is worth one Euro cent. You could then track the revenue event like this:

**For iOS app:**

```cs
var adjustEvent = ADJEvent.EventWithEventToken("abc123");
adjustEvent.SetRevenue(0.01, "EUR");
Adjust.TrackEvent(adjustEvent);
```

**For Android app:**

```cs
AdjustEvent eventRevenue = new AdjustEvent("abc123");
adjustEvent.SetRevenue(0.01, "EUR");
Adjust.TrackEvent(adjustEvent);
```

When you set a currency token, Adjust will automatically convert the incoming revenues into a reporting revenue of your choice. Read more about [currency conversion here][currency-conversion].

You can read more about revenue and event tracking in the [event tracking guide][event-tracking].

### <a id="revenue-deduplication"></a>Revenue deduplication

You can also add an optional transaction ID to avoid tracking duplicate revenues. The last ten transaction IDs are remembered, and revenue events with duplicate transaction IDs are skipped. This is especially useful for In-App Purchase tracking. You can see an example below. Please make sure to call the `TrackEvent` only if the transaction is finished and item is purchased.

**For iOS app:**

```cs
var adjustEvent = ADJEvent.EventWithEventToken("abc123");
adjustEvent.SetRevenue(0.01, "EUR");
adjustEvent.SetTransactionId("{TransactionIdentifier}");
Adjust.TrackEvent(adjustEvent);
```

**For Android app:**

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.SetRevenue(0.01, "EUR");
adjustEvent.SetOrderId("YourOrderId");
Adjust.TrackEvent(adjustEvent);
```

### <a id="callback-parameters"></a>Callback parameters

You can register a callback URL for your events in your [dashboard]. We will send a GET request to that URL whenever the event is tracked. You can add callback parameters to that event by calling `AddCallbackParameter` on the event before tracking it. We will then append these parameters to your callback URL.

For example, suppose you have registered the URL `http://www.adjust.com/callback` then track an event like this:

**For iOS app:**

```cs
var adjustEvent = ADJEvent.EventWithEventToken("abc123");
adjustEvent.AddCallbackParameter("key", "value");
adjustEvent.AddCallbackParameter("foo", "bar");
Adjust.TrackEvent(adjustEvent);
```

**For Android app:**

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.AddCallbackParameter("key", "value");
adjustEvent.AddCallbackParameter("foo", "bar");
Adjust.TrackEvent(adjustEvent);
```

In that case we would track the event and send a request to:

```
http://www.adjust.com/callback?key=value&foo=bar
```

It should be mentioned that we support a variety of placeholders like `{idfa}` or `{gps_adid}` that can be used as parameter values. In the resulting callback this placeholder would be replaced with the ID for Advertisers of the current device. Also note that we don't store any of your custom parameters, but only append them to your callbacks. If you haven't registered a callback for an event, these parameters won't even be read.

You can read more about using URL callbacks, including a full list of available values, in our [callbacks guide][callbacks-guide].

### <a id="partner-parameters"></a>Partner parameters

You can also add parameters to be transmitted to network partners, for the integrations that have been activated in your Adjust dashboard. This works similarly to the callback parameters mentioned above, but can be added by calling the `AddPartnerParameter` method on your `ADJEvent`/`AdjustEvent` instance.

**For iOS app:**

```cs
var adjustEvent = ADJEvent.EventWithEventToken("abc123");
adjustEvent.AddPartnerParameter("key", "value");
adjustEvent.AddPartnerParameter("foo", "bar");
Adjust.TrackEvent(adjustEvent);
```

**For Android app:**
```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.AddPartnerParameter("key", "value");
adjustEvent.AddPartnerParameter("foo", "bar");
Adjust.TrackEvent(adjustEvent);
```

You can read more about special partners and these integrations in our [guide to special partners][special-partners].

### <a id="callback-id"></a>Callback identifier

You can also add custom string identifier to each event you want to track. This identifier will later be reported in event success and/or event failure callbacks to enable you to keep track on which event was successfully tracked or not. You can set this identifier by calling the `SetCallbackId` method on your `ADJEvent`/`AdjustEvent` instance:

**For iOS apps:**

```cs
var adjustEvent = ADJEvent.EventWithEventToken("abc123");
adjustEvent.SetCallbackId("Your-Custom-Id");
Adjust.TrackEvent(adjustEvent);
```

**For Android apps:**

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.SetCallbackId("Your-Custom-Id");
Adjust.TrackEvent(adjustEvent);
```

### <a id="subscription-tracking"></a>Subscription tracking

**Note**: This feature is only available in the SDK v4.22.0 and above.

You can track App Store and Play Store subscriptions and verify their validity with the Adjust SDK. After a subscription has been successfully purchased, make the following call to the Adjust SDK:

**For App Store subscription:**

```cs
ADJSubscription subscription = new ADJSubscription(price, currency, transactionId, receipt);
subscription.SetTransactionDate(transactionDate);
subscription.SetSalesRegion(salesRegion);

Adjust.TrackSubscription(subscription);
```

**For Play Store subscription:**

```cs
AdjustPlayStoreSubscription subscription = new AdjustPlayStoreSubscription(
    price,
    currency,
    sku,
    orderId,
    signature,
    purchaseToken);
subscription.SetPurchaseTime(purchaseTime);

Adjust.TrackPlayStoreSubscription(subscription);
```

Subscription tracking parameters for App Store subscription:

- [price](https://developer.apple.com/documentation/storekit/skproduct/1506094-price?language=objc)
- currency (you need to pass [currencyCode](https://developer.apple.com/documentation/foundation/nslocale/1642836-currencycode?language=objc) of the [priceLocale](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc) object)
- [transactionId](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411288-transactionidentifier?language=objc)
- [receipt](https://developer.apple.com/documentation/foundation/nsbundle/1407276-appstorereceipturl)
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
ADJSubscription subscription = new ADJSubscription(price, currency, transactionId, receipt);
subscription.SetTransactionDate(transactionDate);
subscription.SetSalesRegion(salesRegion);

// add callback parameters
subscription.AddCallbackParameter("key", "value");
subscription.AddCallbackParameter("foo", "bar");

// add partner parameters
subscription.AddPartnerParameter("key", "value");
subscription.AddPartnerParameter("foo", "bar");

Adjust.TrackSubscription(subscription);
```

**For Play Store subscription:**

```cs
AdjustPlayStoreSubscription subscription = new AdjustPlayStoreSubscription(
    price,
    currency,
    sku,
    orderId,
    signature,
    purchaseToken);
subscription.SetPurchaseTime(purchaseTime);

// add callback parameters
subscription.AddCallbackParameter("key", "value");
subscription.AddCallbackParameter("foo", "bar");

// add partner parameters
subscription.AddPartnerParameter("key", "value");
subscription.AddPartnerParameter("foo", "bar");

Adjust.TrackPlayStoreSubscription(subscription);
```

### <a id="session-parameters"></a>Session parameters

Some parameters are saved to be sent in every event and session of the Adjust SDK. Once you have added any of these parameters, you don't need to add them every time, since they will be saved locally. If you add the same parameter twice, there will be no effect.

These session parameters can be called before the Adjust SDK is launched to make sure they are sent even on install. If you need to send them with an install, but can only obtain the needed values after launch, it's possible to [delay](#delay-start) the first launch of the Adjust SDK to allow this behaviour.

### <a id="session-callback-parameters"></a>Session callback parameters

The same callback parameters that are registered for [events](#callback-parameters) can be also saved to be sent in every event or session of the Adjust SDK.

The session callback parameters have a similar interface of the event callback parameters. Instead of adding the key and it's value to an event, it's added through a call to `Adjust` method `AddSessionCallbackParameter`:

```cs
Adjust.AddSessionCallbackParameter("foo", "bar");
```

The session callback parameters will be merged with the callback parameters added to an event. The callback parameters added to an event have precedence over the session callback parameters. Meaning that, when adding a callback parameter to an event with the same key to one added from the session, the value that prevails is the callback parameter added to the event.

It's possible to remove a specific session callback parameter by passing the desiring key to the method `RemoveSessionCallbackParameter`.

```cs
Adjust.RemoveSessionCallbackParameter("foo");
```

If you wish to remove all key and values from the session callback parameters, you can reset it with the method `ResetSessionCallbackParameters`.

```cs
Adjust.ResetSessionCallbackParameters();
```

### <a id="session-partner-parameters"></a>Session partner parameters

In the same way that there is [session callback parameters](#session-callback-parameters) that are sent every in event or session of the Adjust SDK, there is also session partner parameters.

These will be transmitted to network partners, for the integrations that have been activated in your Adjust [dashboard].

The session partner parameters have a similar interface to the event partner parameters. Instead of adding the key and it's value to an event, it's added through a call to `Adjust` method `AddSessionPartnerParameter`:

```cs
Adjust.AddSessionPartnerParameter("foo", "bar");
```

The session partner parameters will be merged with the partner parameters added to an event. The partner parameters added to an event have precedence over the session partner parameters. Meaning that, when adding a partner parameter to an event with the same key to one added from the session, the value that prevails is the partner parameter added to the event.

It's possible to remove a specific session partner parameter by passing the desiring key to the method `RemoveSessionPartnerParameter`.

```cs
Adjust.RemoveSessionPartnerParameter("foo");
```

If you wish to remove all key and values from the session partner parameters, you can reset it with the method `ResetSessionPartnerParameters`.

```cs
Adjust.ResetSessionPartnerParameters();
```

### <a id="delay-start"></a>Delay start

Delaying the start of the Adjust SDK allows your app some time to obtain session parameters, such as unique identifiers, to be send on install.

Set the initial delay time in seconds with the `DelayStart` property of the `ADJConfig`/`AdjustConfig` instance:

**For iOS app:**

```cs
config.DelayStart = 5.5;
```

**For Android app:**

```cs
config.SetDelayStart(5.5);
```

In this case this will make the Adjust SDK not send the initial install session and any event created for 5.5 seconds. After this time is expired or if you call `Adjust.SendFirstPackages()` in the meanwhile, every session parameter will be added to the delayed install session and events and the Adjust SDK will resume as usual.

**The maximum delay start time of the adjust SDK is 10 seconds**.

### <a id="attribution-callback"></a>Attribution callback

You can register a callback to be notified of tracker attribution changes. Due to the different sources considered for attribution, this information can not by provided synchronously. Follow these steps to implement the optional callback in your app:

Please make sure to consider our [applicable attribution data policies][attribution-data].

**For iOS app:**

- Open `AppDelegate.cs` and create a class which inherits from `AdjustDelegate` and override its `AdjustAttributionChanged` method.
    
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

- Add the private field of type `AdjustDelegateXamarin` to this `AppDelegate` class.

    ```cs
    private AdjustDelegateXamarin adjustDelegate = null;
    ```

- Initialize and set the delegate with your `ADJConfig` instance:

    ```cs
    adjustDelegate = new AdjustDelegateXamarin();
    // ...
    adjustConfig.Delegate = adjustDelegate;
    ```
    
**For Android app:**

- Implement the `IOnAttributionChangedListener` interface in your Application class.

    ```cs
    [Application (AllowBackup = true)]
    public class GlobalApplication : Application, IOnAttributionChangedListener
    {
        ...
    }
    ```
- Override the `OnAttributionChanged` callback which will be triggered when the attribution has been changed.

    ```cs
    public void OnAttributionChanged(AdjustAttribution attribution)
    {
        Console.WriteLine("Attribution changed!");
        Console.WriteLine("New attribution: {0}", attribution.ToString ());
    }
    ```

- Set your Application class instance as the listener in the `AdjustConfig` object.

    ```cs
    AdjustConfig config = new AdjustConfig(this, yourAppToken, environment);
    config.SetOnAttributionChangedListener(this);
    Adjust.OnCreate (config);
    ```

The callback function will be called  when the SDK receives final attribution data. Within the callback function you have access to the `attribution` parameter. Here is a quick summary of its properties:

- `TrackerToken`    the tracker token of the current attribution.
- `TrackerName`     the tracker name of the current attribution.
- `Network`         the network grouping level of the current attribution.
- `Campaign`        the campaign grouping level of the current attribution.
- `Adgroup`         the ad group grouping level of the current attribution.
- `Creative`        the creative grouping level of the current attribution.
- `ClickLabel`      the click label of the current attribution.
- `Adid`            the Adjust device identifier.
- `CostType`        the cost type.
- `CostAmount`      the cost amount.
- `CostCurrency`    the cost currency.

**Note**: The cost data - `CostType`, `CostAmount` & `CostCurrency` are only available when configured in `AdjustConfig` by seting `NeedsCost` property. If not configured or configured, but not being part of the attribution, these fields will have value `null`. This feature is available in SDK v4.29.0 and above.

**For iOS apps:**

```cs
config.NeedsCost = true;
```

**For Android apps:**
```cs
config.SetNeedsCost(true);
```

### <a id="session-event-callbacks"></a>Session and event callbacks

---

**For iOS app:** 

You can register a callback to be notified of successful and failed events and/or sessions. Like with attribution callback, this should be done in your custom class which inherits from `AdjustDelegate`.

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

The callback functions will be called after the SDK tries to send a package to the server. Within the callback you have access to a response data object specifically for the callback. Here is a quick summary of the session response data properties:

- `string Message` the message from the server or the error logged by the SDK.
- `string Timestamp` timestamp from the server.
- `string Adid` a unique device identifier provided by adjust.
- `NSDictionary<string, object> JsonResponse` the JSON object with the response from the server.

Both event response data objects contain:

- `string EventToken` the event token, if the package tracked was an event.
- `string CallbackId` the custom defined callback ID set on event object.

And both event and session failed objects also contain:

- `bool WillRetry` indicates there will be an attempt to resend the package at a later time.

---

**For Android app:**

You can register a callback to be notified of successful and failed events and/or sessions. Like with attribution callback, this should be done in your custom class which implements corresponding interface for each callback method.

Follow the same steps to implement the following callback function for successful tracked events. You need to set the listener object on `AdjustConfig` instance which implements `IOnEventTrackingSucceededListener` interface:

```cs
[Application(AllowBackup = true)]
public class GlobalApplication : Application, IOnEventTrackingSucceededListener
{
    public override void OnCreate()
    {
        base.OnCreate();

        string appToken = "{YourAppToken}";
        string environment = AdjustConfig.EnvironmentSandbox;
        AdjustConfig config = new AdjustConfig(this, appToken, environment);

        // Set event tracking success callback.
        config.SetOnEventTrackingSucceededListener(this);

        Adjust.OnCreate(config);
    }

    public void OnFinishedEventTrackingSucceeded(AdjustEventSuccess eventSuccess)
    {
        Console.WriteLine("Event tracking succeeded! " + eventSuccess.ToString());
    }
}
```

For failed tracked events, you need to set the listener object on `AdjustConfig` instance which implements `IOnEventTrackingFailedListener` interface:

```cs
[Application(AllowBackup = true)]
public class GlobalApplication : Application, IOnEventTrackingFailedListener
{
    public override void OnCreate()
    {
        base.OnCreate();

        string appToken = "{YourAppToken}";
        string environment = AdjustConfig.EnvironmentSandbox;
        AdjustConfig config = new AdjustConfig(this, appToken, environment);

        // Set event tracking failure callback.
        config.SetOnEventTrackingFailedListener(this);

        Adjust.OnCreate(config);
    }

    public void OnFinishedEventTrackingFailed(AdjustEventFailure eventFailure)
    {
        Console.WriteLine("Event tracking failed! " + eventFailure.ToString());
    }
}
```

For successful tracked sessions, you need to set the listener object on `AdjustConfig` instance which implements `IOnSessionTrackingSucceededListener` interface:

```cs
[Application(AllowBackup = true)]
public class GlobalApplication : Application, IOnSessionTrackingSucceededListener
{
    public override void OnCreate()
    {
        base.OnCreate();

        string appToken = "{YourAppToken}";
        string environment = AdjustConfig.EnvironmentSandbox;
        AdjustConfig config = new AdjustConfig(this, appToken, environment);

        // Set session tracking success callback.
        config.SetOnSessionTrackingSucceededListener(this);

        Adjust.OnCreate(config);
    }

    public void OnFinishedSessionTrackingSucceeded(AdjustSessionSuccess sessionSuccess)
    {
        Console.WriteLine("Session tracking succeeded! " + sessionSuccess.ToString());
    }
}
```

For failed tracked sessions, you need to set the listener object on `AdjustConfig` instance which implements `IOnSessionTrackingFailedListener` interface:

```cs
[Application(AllowBackup = true)]
public class GlobalApplication : Application, IOnSessionTrackingFailedListener
{
    public override void OnCreate()
    {
        base.OnCreate();

        string appToken = "{YourAppToken}";
        string environment = AdjustConfig.EnvironmentSandbox;
        AdjustConfig config = new AdjustConfig(this, appToken, environment);

        // Set session tracking failure callback.
        config.SetOnSessionTrackingFailedListener(this);

        Adjust.OnCreate(config);
    }

    public void OnFinishedSessionTrackingFailed(AdjustSessionFailure sessionFailure)
    {
        Console.WriteLine("Session tracking failed! " + sessionFailure.ToString());
    }
}
```

The callback functions will be called after the SDK tries to send a package to the server. Within the callback you have access to a response data object specifically for the callback. Here is a quick summary of the session response data properties:

- `string Message` the message from the server or the error logged by the SDK.
- `string Timestamp` timestamp from the server.
- `string Adid` a unique device identifier provided by adjust.
- `JSONObject JsonResponse` the JSON object with the response from the server.

Both event response data objects contain:

- `string EventToken` the event token, if the package tracked was an event.
- `string CallbackId` the custom defined callback ID set on event object.

And both event and session failed objects also contain:

- `bool WillRetry` indicates there will be an attempt to resend the package at a later time.

---

### <a id="disable-tracking"></a>Disable tracking

You can disable the Adjust SDK from tracking by invoking the method `SetEnabled` with the enabled parameter as `false`. **This setting is remembered between sessions**, but it can only be activated after the first session.

```cs
Adjust.SetEnabled(false);
```

You can verify if the Adjust SDK is currently active with the property `IsEnabled`. It is always possible to activate the Adjust SDK by invoking `SetEnabled` with the enabled parameter set to `true`.

### <a id="offline-mode"></a>Offline mode

You can put the adjust SDK in offline mode to suspend transmission to our servers, while retaining tracked data to be sent later. When in offline mode, all information is saved in a file, so be careful not to trigger too many events.

You can activate offline mode by calling `SetOfflineMode` with the parameter `true`.

```cs
Adjust.SetOfflineMode(true);
```

Conversely, you can deactivate offline mode by calling `SetOfflineMode` with `false`. When the Adjust SDK is put back in online mode, all saved information is send to our servers with the correct time information.

Unlike disabling tracking, **this setting is not remembered** between sessions. This means that the SDK is in online mode whenever it is started, even if the app was terminated in offline mode.

### <a id="event-buffering"></a>Event buffering

If your app makes heavy use of event tracking, you might want to delay some HTTP requests in order to send them in one batch every minute. You can enable event buffering with your `ADJConfig`/`AdjustConfig` instance:

**For iOS app:**

```cs
var config = ADJConfig.ConfigWithAppToken(yourAppToken, environment);
config.EventBufferingEnabled = true;
Adjust.AppDidLaunch(config);
```

**For Android app:**

```cs
AdjustConfig config = new AdjustConfig(this, yourAppToken, environment);
config.SetEventBufferingEnabled((Java.Lang.Boolean)true);
Adjust.OnCreate(config);
```

If nothing is set, event buffering is **disabled by default**.

### <a id="gdpr-forget-me"></a>GDPR right to be forgotten

In accordance with article 17 of the EU's General Data Protection Regulation (GDPR), you can notify Adjust when a user has exercised their right to be forgotten. Calling the following method will instruct the Adjust SDK to communicate the user's choice to be forgotten to the Adjust backend:

```cs
Adjust.GdprForgetMe();
```

Upon receiving this information, Adjust will erase the user's data and the Adjust SDK will stop tracking the user. No requests from this device will be sent to Adjust in the future.

### <a id="disable-third-party-sharing"></a>Disable third-party sharing for specific users

You can now notify Adjust when a user has exercised their right to stop sharing their data with partners for marketing purposes, but has allowed it to be shared for statistics purposes. 

Call the following method to instruct the Adjust SDK to communicate the user's choice to disable data sharing to the Adjust backend:

**For iOS app:**

```cs
Adjust.DisableThirdPartySharing();
```

**For Android app:**

```cs
Adjust.DisableThirdPartySharing(this);
```

Upon receiving this information, Adjust will block the sharing of that specific user's data to partners and the Adjust SDK will continue to work as usual.

### <a id="sdk-signature"></a>SDK signature

An account manager must activate the Adjust SDK signature. Contact Adjust support (support@adjust.com) if you are interested in using this feature.

If the SDK signature has already been enabled on your account and you have access to App Secrets in your Adjust Dashboard, please use the method below to integrate the SDK signature into your app.

An App Secret is set by passing all secret parameters (`secretId`, `info1`, `info2`, `info3`, `info4`) to `SetAppSecret` method of `ADJConfig`/`AdjustConfig` instance:

**For iOS app:**

```cs
var config = ADJConfig.ConfigWithAppToken(appToken, environment);
config.SetAppSecret(secretId, info1, info2, info3, info4);
Adjust.AppDidLaunch(config);
```

**For Android app:**

```cs
AdjustConfig config = AdjustConfig(this, appToken, environment);
config.SetAppSecret(secretId, info1, info2, info3, info4);
Adjust.OnCreate(config);
```

### <a id="background-tracking"></a>Background tracking

The default behaviour of the Adjust SDK is to **pause sending HTTP requests while the app is in the background**. You can change this in your `ADJConfig`/`AdjustConfig` instance:

**For iOS apps:**

```cs
var config = ADJConfig.ConfigWithAppToken(yourAppToken, environment);
config.SendInBackground = true;
Adjust.AppDidLaunch(config);
```

**For Android apps:**

```cs
AdjustConfig config = new AdjustConfig(this, yourAppToken, environment);
config.SetSendInBackground(true);
Adjust.OnCreate(config);
```

If nothing set, sending in background is **disabled by default**.

### <a id="device-ids"></a>Device IDs

The adjust SDK offers the possibility to obtain some device identifiers.

### <a id="di-idfa"></a>iOS Advertising Identifier

Certain services (such as Google Analytics) require you to coordinate device and client IDs in order to prevent duplicate reporting.

To obtain the IDFA device identifier, access the `Idfa` property of the `Adjust` instance:

```cs
string idfa = Adjust.Idfa;
```

### <a id="di-gps-adid"></a>Google Play Services advertising identifier

The Google Play Services Advertising Identifier (Google advertising ID) is a unique identifier for a device. Users can opt out of sharing their Google advertising ID by toggling the "Opt out of Ads Personalization" setting on their device. When a user has enabled this setting, the Adjust SDK returns a string of zeros when trying to read the Google advertising ID.

> **Important**: If you are targeting Android 12 and above (API level 31), you need to add the [`com.google.android.gms.AD_ID` permission](#gps-adid-permission) to your app. If you do not add this permission, you will not be able to read the Google advertising ID even if the user has not opted out of sharing their ID.

Certain services (such as Google Analytics) require you to coordinate Device and Client IDs in order to prevent duplicated reporting.

If you need to obtain the Google Advertising ID, there is a restriction that only allows it to be read in a background thread. If you call the function `getGoogleAdId` with the context and a `OnDeviceIdsRead` instance, it will work in any situation:

```cs
using Com.Adjust.Sdk;

// ...

[Application(AllowBackup = true)]
public class GlobalApplication : Application, IOnDeviceIdsRead
{
    public override void OnCreate()
    {
        base.OnCreate();

        string appToken = "{YourAppToken}";
        string environment = AdjustConfig.EnvironmentSandbox;

        var config = new AdjustConfig(this, appToken, environment);

        Adjust.OnCreate(config);

        Adjust.GetGoogleAdId(this, this);
    }

    public void OnGoogleAdIdRead(string googleAdId)
    {
        Console.WriteLine("Google Ad Id read: " + googleAdId);
    }
}
```

Inside the method `onGoogleAdIdRead` of the `OnDeviceIdsRead` instance, you will have access to the Google Advertising ID through the variable `googleAdId`.

### <a id="di-fire-adid"></a>Amazon advertising identifier

If you need to obtain the Amazon advertising ID, you can call the `GetAmazonAdId` method of the `Adjust` instance and pass your callback as a parameter to which the Amazon advertising ID value will be sent once obtained:

```csharp
Console.WriteLine("Amazon Ad Id: " + Adjust.GetAmazonAdId(this));
```

### <a id="di-adid"></a>Adjust device identifier

For each device with your app installed, the Adjust backend generates a unique **Adjust device identifier** (**adid**). In order to obtain this identifier, you can access the following property of the `Adjust` instance:

```cs
stirng adid = Adjust.Adid;
```

**Note**: Information about the **adid** is available after the app's installation has been tracked by the Adjust backend. From that moment on, the Adjust SDK has information about the device **adid** and you can access it with this method. So, **it is not possible** to access the **adid** before the SDK has been initialised and the installation of your app has been successfully tracked.

### <a id="set-external-device-id"></a>Set external device ID

> **Note** If you want to use external device IDs, please contact your Adjust representative. They will talk you through the best approach for your use case.

An external device identifier is a custom value that you can assign to a device or user. They can help you to recognize users across sessions and platforms. They can also help you to deduplicate installs by user so that a user isn't counted as multiple new installs.

You can also use an external device ID as a custom identifier for a device. This can be useful if you use these identifiers elsewhere and want to keep continuity.

Check out our [external device identifiers article](https://help.adjust.com/en/article/external-device-identifiers) for more information.

> **Note** This setting requires Adjust SDK v4.21.0 or later.

To set an external device ID, assign the identifier to the `ExternalDeviceId` property of your config instance. Do this before you initialize the Adjust SDK.

**For iOS apps:**

```csharp
var config = ADJConfig.ConfigWithAppToken(yourAppToken, environment);
config.ExternalDeviceId = "{Your-External-Device-Id}";
Adjust.AppDidLaunch(config);
```

**For Android apps:**

```csharp
AdjustConfig config = new AdjustConfig(this, yourAppToken, environment);
config.ExternalDeviceId = "{Your-External-Device-Id}";
Adjust.OnCreate(config);
```

> **Important**: You need to make sure this ID is **unique to the user or device** depending on your use-case. Using the same ID across different users or devices could lead to duplicated data. Talk to your Adjust representative for more information.

If you want to use the external device ID in your business analytics, you can pass it as a session callback parameter. See the section on [session callback parameters](#session-callback-parameters) for more information.

You can import existing external device IDs into Adjust. This ensures that the backend matches future data to your existing device records. If you want to do this, please contact your Adjust representative.

### <a id="user-attribution"></a>User attribution

The attribution callback will be triggered as described in the [attribution callback section](#attribution-callback), providing you with information about any new attribution whenever it changes. If you want to access information about a user's current attribution at any other time, you can do so through the following property of the `Adjust` instance:

```cs
ADJAttribution attribution = Adjust.Attribution;
```

### <a id="push-token"></a>Push token

Push tokens are used for Audience Builder and client callbacks, and they are required for uninstall and reinstall tracking.

To send us the push notification token, add the following call to Adjust once you have obtained your token or when ever it's value is changed:

```cs
Adjust.SetPushToken("YourPushNotificationsToken");
```

**Note for iOS**: `Adjust.SetDeviceToken(NSData)` method is marked as deprecated as of Android SDK v4.14.0. Please, use `Adjust.SetPushToken(string)` method instead.

### <a id="pre-installed-trackers"></a>Pre-installed trackers

If you want to use the Adjust SDK to recognize users that found your app pre-installed on their device, follow these steps.

- Create a new tracker in your [dashboard].
- Open your app delegate and add set the default tracker of your `ADJConfig`:

    **For iOS apps:**
    
    ```cs
    var config = ADJConfig.ConfigWithAppToken(yourAppToken, environment);
    config.DefaultTracker = "{TrackerToken}";
    Adjust.AppDidLaunch(config);
    ```
    
    **For Android apps:**
    
    ```cs
    AdjustConfig config = new AdjustConfig(this, yourAppToken, environment);
    config.SetDefaultTracker("{TrackerToken}");
    Adjust.OnCreate(config);
    ```

  Replace `{TrackerToken}` with the tracker token you created in step 2. Please note that the dashboard displays a tracker 
  URL (including `http://app.adjust.com/`). In your source code, you should specify only the six-character token and not the
  entire URL.

- Build and run your app. You should see a line like the following in app's log output:

    ```
    Default tracker: 'abc123'
    ```

### <a id="deeplinking"></a>Deep linking

If you are using the Adjust tracker URL with an option to deep link into your app from the URL, there is the possibility to get information about the deep link URL and its content. Hitting the URL can happen when the user already has your app installed (standard deep linking scenario) or if they do not have the app on their device (deferred deep linking scenario). In the standard deep linking scenario, the iOS and Android platforms offer native support for you to get the info about the deep link content. The deferred deep linking scenario is something that the platforms does not support out of the box. In this scenario, the Adjust SDK will offer you the tools you need to get the information about the deep link content.

### <a id="deeplinking-standard"></a>Standard deep linking scenario

---

**For iOS apps:**

If your user already has the app installed and hits the tracker URL with deep link information in it, your application will get opened and the content of the deep link will be sent to your app so that you can parse it and decide what to do next in your app. As you have read in our official iOS SDK README, deep linking in iOS at this moment can be done with usage of custom URL scheme in tracker URL (iOS 8 and earlier) or with usage of universal links (iOS 9 and newer).

Depending on which scenario you want to use for your app (or if you want to use them both to support wide range of devices), you need to set up your app to handle one of these or both scenarios.

---

**For Android apps:**

If a user has your app installed and you want it to launch after hitting an Adjust tracker URL with the `deep_link` parameter in it, you need enable deep linking in your app. This is done by choosing a desired **unique scheme name** and assigning it to the Activity which you want to launch once the app opens after the user has clicked on the link. This can be done by setting certain properties on the Activity class which you would like to see launched once the deep link has been clicked and your app has opened. You need to set up a proper intent filter and name the scheme:

```cs
[Activity(Label = "Example", MainLauncher = true)]
    [IntentFilter
     (new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "adjust-example")]
    public class MainActivity : Activity
    {
        // ...
    }
```

With this now set, you need to use the assigned scheme name in the Adjust tracker URL's `deep_link` parameter. A tracker URL without any information added to the deep link can be built to look something like this:

```
https://app.adjust.com/abc123?deep_link=adjust-example%3A%2F%2F
```

Please, have in mind that the `deep_link` parameter value in the URL **must be URL encoded**.

After clicking this tracker URL, and with the app set as described above, your app will launch along with the `MainActivity` intent. Inside the `MainActivity` class, you will automatically be provided with the information about the `deep_link` parameter content. Once this content is delivered to you, it **will not be encoded**, although it was encoded in the URL.

Depending on the `launchMode` setting of your Activity, information about the `deep_link` parameter content will be delivered to the appropriate place in the Activity file. For more information about the possible values of the `launchMode` property, check [the official Android documentation][android-launch-modes].

There are two possible places in which information about the deep link content will be delivered to your desired Activity via `Intent` object - either in the Activity's `OnCreate` or `OnNewIntent` method. After the app has launched and one of these methods is triggered, you will be able to get the actual deeplink passed in the `deep_link` parameter in the click URL. You can then use this information to do some additional logic in your app.

You can extract the deep link content from these two methods like this:

```cs
using Android.Content;
using Com.Adjust.Sdk;

// ...

protected override void OnCreate(Bundle savedInstanceState)
{
    base.OnCreate(savedInstanceState);

    Intent intent = this.Intent;
    var data = intent.Data;

    // data.ToString() -> This is your deep_link parameter value.
}
```

```cs
using Android.Content;
using Com.Adjust.Sdk;

// ...

protected override void OnNewIntent(Android.Content.Intent intent)
{
    base.OnNewIntent(intent);

    var data = intent.Data;

    // data.ToString() -> This is your deep_link parameter value.
}
```

### <a id="deeplinking-setup-old"></a>Deep linking on iOS 8 and earlier

In order to set deep linking support for iOS 8 and earlier devices, you need to set up custom URL scheme in your app's `Info.plist` file. Open your `Info.plist` file and go to `Advanced` tab. In there, fill in your `Bundle ID` value in `Identifier` field and custom URL scheme for your app in `URL Schemes` field.

With this completed, when you app gets opened by click on tracker URL with deep link info which contains your custom URL scheme in it, `OpenUrl` method in your `AppDelegate` class will get called and you will get the deep link info in there.

```cs
public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
{
    // url -> This is your deep link content.

    return true;
}
```

With this setup, you have successfully set up deep linking handling for iOS devices with iOS 8 and earlier versions.

### <a id="deeplinking-setup-new"></a>Deep linking on iOS 9 and later

In order to set deep linking support for iOS 9 and later devices, you need to enable your app to handle Apple universal links. If you followed our official iOS SDK README instructions, you have successfully enabled `Associated Domains` for your app in Apple Developer portal. In order to enable this in your app project, please open `Entitlements.plist`. In there you will see `Associated Domains` part which you need to enable by checking the `Enable Associated Domains` check box. After this you only need to add domain which was generated for you in the Adjust dashboard.

With this completed, when your app gets opened by click on universal link, `ContinueUserActivity` method in your `AppDelegate` class will get called and you will get the deep link info in there.

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

### <a id="deeplinking-deferred"></a>Deferred deep linking scenario

---

**For iOS apps:**

In order to get info about the URL content in a deferred deep linking scenario, you should implement a callback method in the same way like you were setting callbacks for attribution, events and sessions. You need to override `AdjustDeeplinkResponse` in the class which inherits from `AdjustDelegate`:

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

In deferred deep linking scenario, there is one additional setting which can be set in the `AdjustDeeplinkResponse` method via it's return value. Once the Adjust SDK gets the deferred deep link info, we are offering you the possibility to choose whether our SDK should open this URL or not. You can choose to set this option by returning either `true` if you want us to open the URL or `false` if you don't want us to do anything.

If this callback is not implemented, **the Adjust SDK will always try to launch the URL by default**.

---

**For Android apps:**

Deferred deep linking scenario happens when a user clicks on the Adjust tracker URL with the `deep_link` parameter in it, but does not have the app installed on the device at the moment of click. After that, the user will get redirected to the Play Store to download and install your app. After opening it for the first time, the content of the `deep_link` parameter will be delivered to the app.

In order to get info about the `deep_link` parameter content in a deferred deep linking scenario, you should set a listener method on the `AdjustConfig` object. Listener object needs to implement `IOnDeeplinkResponseListener` interface and override it's `LaunchReceivedDeeplink` method. This will get triggered once the Adjust SDK gets the info about the deep link content from the backend.

```cs
[Application(AllowBackup = true)]
public class GlobalApplication : Application, IOnDeeplinkResponseListener
{
    public override void OnCreate()
    {
        base.OnCreate();

        string appToken = "{YourAppToken}";
        string environment = AdjustConfig.EnvironmentSandbox;
        AdjustConfig config = new AdjustConfig(this, appToken, environment);

        // Set deferred deeplink callback.
        config.SetOnDeeplinkResponseListener(this);

        Adjust.OnCreate(config);
    }

    public bool LaunchReceivedDeeplink(Android.Net.Uri deeplink)
    {
        Console.WriteLine("Deferred deeplink arrived! URL = " + deeplink.ToString());

        return true;
        // return false;
    }
}
```

Once the aAdjust SDK receives the info about the deep link content from the backend, it will deliver you the info about its content in this listener and expect the `bool` return value from you. This return value represents your decision on whether the Adjust SDK should launch the Activity to which you have assigned the scheme name from the deep link (like in the standard deep linking scenario) or not.

If you return `true`, we will launch it and the exact same scenario which is described in the [Standard deep linking scenario chapter](#deeplinking-standard) will happen. If you do not want the SDK to launch the Activity, you can return `false` from this listener and based on the deep link content decide on your own what to do next in your app.

### <a id="deeplinking-reattribution"></a>Reattribution via deep links

Adjust enables you to run re-engagement campaigns with usage of deep links. For more information on how to do that, please check our [official docs][reattribution-with-deeplinks]. 

If you are using this feature, in order for your user to be properly reattributed, you need to make one additional call to the adjust SDK in your app.

Once you have received deep link content information in your app, add a call to `Adjust.AppWillOpenUrl` method. By making this call, the Adjust SDK will try to find if there is any new attribution info inside of the deep link and if any, it will be sent to the Adjust backend. If your user should be reattributed due to a click on the Adjust tracker URL with deep link content in it, you will see the [attribution callback](#attribution-callback) in your app being triggered with new attribution info for this user.

**For iOS apps:**

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

**For Androoid apps:**

The call to `Adjust.AppWillOpenUrl` should be done like this:

```cs
using Android.Content;
using Com.Adjust.Sdk;

// ...

protected override void OnCreate(Bundle savedInstanceState)
{
    base.OnCreate(savedInstanceState);

    Intent intent = this.Intent;
    var data = intent.Data;

    // data.ToString() -> This is your deep_link parameter value.

    Adjust.AppWillOpenUrl(data, this);
}
```

```cs
using Android.Content;
using Com.Adjust.Sdk;

// ...

protected override void OnNewIntent(Android.Content.Intent intent)
{
    base.OnNewIntent(intent);

    var data = intent.Data;

    // data.ToString() -> This is your deep_link parameter value.

    Adjust.AppWillOpenUrl(data, this);
}
```

**Note for Android**: `Adjust.AppWillOpenUrl(Android.Net.Uri)` method is marked as deprecated as of Android SDK v4.14.0. Please, use `Adjust.appWillOpenUrl(Android.Net.Uri, Context)` method instead.

### <a id="data-residency"></a>Data residency

In order to enable data residency feature, make sure to set URL strategy setting of the `AdjustConfig` instance with one of the following constants:

**For iOS apps:**

```cs
config.UrlStrategy = AdjustConfig.DataResidencyEU; // for EU data residency region
config.UrlStrategy = AdjustConfig.DataResidencyTR; // for Turkey data residency region
config.UrlStrategy = AdjustConfig.DataResidencyUS; // for US data residency region
```

**For Android apps:**

```cs
config.SetUrlStrategy(AdjustConfig.DataResidencyEu); // for EU data residency region
config.SetUrlStrategy(AdjustConfig.DataResidencyTr); // for Turkey data residency region
config.SetUrlStrategy(AdjustConfig.DataResidencyUs); // for US data residency region
```

[dashboard]:                    http://adjust.com
[adjust.com]:                   http://adjust.com
[demo-app-ios]:                 ./ios
[demo-app-android]:             ./android
[releases]:                     https://github.com/adjust/xamarin_sdk/releases
[event-tracking]:               https://docs.adjust.com/en/event-tracking
[callbacks-guide]:              https://docs.adjust.com/en/callbacks
[special-partners]:             https://docs.adjust.com/en/special-partners
[attribution-data]:             https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[android-dashboard]:            http://developer.android.com/about/dashboards/index.html
[android_application]:          http://developer.android.com/reference/android/app/Application.html
[currency-conversion]:          https://docs.adjust.com/en/event-tracking/#tracking-purchases-in-different-currencies
[android-launch-modes]:         https://developer.android.com/guide/topics/manifest/activity-element.html
[ios-readme-deeplinking]:       https://github.com/adjust/ios_sdk/#deeplink-reattributions
[reattribution-with-deeplinks]: https://docs.adjust.com/en/deeplinking/#manually-appending-attribution-data-to-a-deep-link

## <a id="license"></a>License

The Adjust SDK is licensed under the MIT License.

Copyright (c) 2012-2021 Adjust GmbH, http://www.adjust.com

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
