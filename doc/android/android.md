## Summary

This is the Xamarin SDK of adjust™. You can read more about adjust™ at [adjust.com].

## Table of contents

* [Example apps](#example-apps)
* [Basic integration](#basic-integration)
   * [Get the SDK](#sdk-get)
   * [Add the SDK to your project](#sdk-add)
   * [Add the SDK project reference to your app](#sdk-add-project)
   * [Add the SDK DLL reference to your app](#sdk-add-dll)
   * [Add Google Play Services to your app](#sdk-add-gps)
   * [Integrate the SDK into your app](#sdk-integrate)
   * [Session tracking](#session-tracking)
   	* [API level 14 and higher](#session-tracking-api14)
   	* [API level 9 until 13](#session-tracking-api9)
   * [Adjust logging](#adjust-logging)
   * [Build your app](#build-your-app)
* [Additional features](#additional-features)
   * [Event tracking](#event-tracking)
      * [Revenue tracking](#revenue-tracking)
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
   * [Pre-installed trackers](#pre-installed-trackers)
   * [Deep linking](#deeplinking)
      * [Standard deep linking scenario](#deeplinking-standard)
      * [Deferred deep linking scenario](#deeplinking-deferred)
      * [Reattribution via deep links](#deeplinking-reattribution)
* [License](#license)

## <a id="example-apps">Example apps

There is an iOS example app inside the [`Android` directory][demo-app-android]. You can open the Xamarin Studio project to 
see an example on how the adjust SDK can be integrated.

## <a id="basic-integration">Basic integration

We will describe the steps to integrate the adjust SDK into your Xamarin Studio project. We are going to assume that you use
Xamarin Studio for your Android development.

### <a id="sdk-get">Get the SDK

Download the latest version from our [releases page][releases]. Extract the archive into a directory of your choice.

If you want to use adjust bindings DLL you can start with [this step](#sdk-add-dll).

### <a id="sdk-add">Add the SDK to your project

Choose to add an existing project to your solution.

![][add_android_binding]

Select the `AdjustSdk.Xamarin.Android` project file and hit `Open`.

![][select_android_binding]

You will now have adjust Android bindings added as submodule to your solution.

![][submodule_android_binding]

### <a id="sdk-add-project">Add the SDK project reference to your app

After you have successfully added the adjust Android bindings project to your solution, you should also add a reference to 
it in your Android app project properties.

![][reference_android_binding]

In case you don't want to add reference to the adjust SDK via project reference, you can skip this step and add it as DLL 
reference to your app which is explained in the step below.

### <a id="sdk-add-dll">Add the SDK DLL reference to your app

The next step is to add a reference to the bindings DLL in your Android project properties. In the references window, choose
the `.Net Assembly` pane and select the `AdjustSdk.Xamarin.Android.dll` that you have downloaded.

![][select_android_dll]

### <a id="sdk-add-gps">Add Google Play Services to your app

Since the 1st of August of 2014, apps in the Google Play Store must use the Google Advertising ID to uniquely identify 
devices. To allow the adjust SDK to use the Google Advertising ID, you must integrate Google Play Services. If you haven't 
done this yet, follow these steps:

1. Choose to `Add Packages` by your `Packages` folder in Android app project.

	![][add_packages]

2. Search for `Xamarin Google Play Services - Analytics` and add them to your app.

	![][add_gps_to_app]

3. After you have added Google Play Services Analytics to your Android app project, 
the content of your `Packages` folder should look like this:

	![][gps_added]

### <a id="sdk-add-gps">Add permissions

In `Properties` folder, open the `AndroidManifest.xml` of your Android app project. Add the `INTERNET` permission if it's 
not already there.

![][permission_internet]

If you are _not_ targeting the Google Play Store, add the `INTERNET` and `ACCESS_WIFI_STATE` permissions:

![][permission_wifi_state]

### <a id="sdk-integrate">Integrate the SDK into your app

To start with, we'll set up basic session tracking.

We recommend using a global Android [Application][android_application] class to initialize the SDK. If don't have one in 
your app already, create a class that extends `Application`.

![][application_class]

In your `Application` class, find or create the `onCreate` method and add the following code to initialize the adjust SDK:

```cs
using Com.Adjust.Sdk;

// ...

string appToken = "{YourAppToken}";
string environment = AdjustConfig.EnvironmentSandbox;

var config = new AdjustConfig(this, appToken, environment);

Adjust.OnCreate(config);
```

Replace `{YourAppToken}` with your app token. You can find this in your [dashboard].

Depending on whether you build your app for testing or for production, you must set the `environment` with one of these 
values:

```cs
const String environment = AdjustConfig.EnvironmentSandbox;
const String environment = AdjustConfig.EnvironmentProduction;
```

**Important:** This value should be set to `AdjustConfig.EnvironmentSandbox` if and only if you or someone else is testing 
your app. Make sure to set the environment to `AdjustConfig.EnvironmentProduction` just before you publish the app. Set it 
back to `AdjustConfig.EnvironmentSandbox` when you start developing and testing it again.

We use this environment to distinguish between real traffic and test traffic from test devices. It is very important that 
you keep this value meaningful at all times! This is especially important if you are tracking revenue.

### <a id="session-tracking"></a>Session tracking

**Note**: This step is **really important** and please **make sure that you implement it properly in your app**. By 
implementing it, you will enable proper session tracking by the adjust SDK in your app.

#### <a id="session-tracking-api14"></a>API level 14 and higher

1. Add a private class that implements the `Java.Lang.Object` and  `IActivityLifecycleCallbacks` interface. If you don't 
have access to this interface, your app is targeting an Android API level inferior to 14. You will have to update manually 
each Activity by following these [instructions](#session-tracking-api9). If you had `Adjust.OnResume` and `Adjust.OnPause` 
calls on each Activity of your app before, you should remove them.

2. Edit the `OnActivityResumed(Activity activity)` method and add a call to `Adjust.OnResume()`. Edit the 
`OnActivityPaused(Activity activity)` method and add a call to `Adjust.OnPause()`.
    
3. After the adjust SDK is configured, add a call to `RegisterActivityLifecycleCallbacks` method with an instance of the 
created `ActivityLifecycleCallbacks` class.

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
    
    ![][session_tracking_new]

#### <a id="session-tracking-api9"></a>API level 9 until 13

If your app's minimal supported API level is between `9` and `13`, consider updating it to at least `14` to simplify the 
integration process in the long term. Consult the official Android [dashboard][android-dashboard] to know the latest market
share of the major versions.  

If you still want to target these lower API levels, in order to provide proper session tracking it is required to call 
certain adjust SDK methods every time any Activity resumes or pauses. Otherwise the SDK might miss a session start or 
session end. In order to do so you should **follow these steps for each Activity of your app**:

1. Open the source file of your Activity.
2. Add the `using` statement at the top of the file.
3. In your Activity's `OnResume` method call `Adjust.OnResume`. Create the method if needed.
4. In your Activity's `OnPause` method call `Adjust.OnPause`. Create the method if needed.

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

![][session_tracking_old]

Repeat these steps for **every** Activity of your app. Don't forget these steps when you create new Activities in the 
future. Depending on your coding style you might want to implement this in a common superclass of all your Activities.

### <a id="adjust-logging">Adjust logging

You can increase or decrease the amount of logs you see in tests by calling `SetLogLevel` on your `AdjustConfig` instance 
with one of the following parameters:

```cs
config.SetLogLevel(LogLevel.Verbose); // enable all logging
config.SetLogLevel(LogLevel.Debug);   // enable more logging
config.SetLogLevel(LogLevel.Info);    // the default
config.SetLogLevel(LogLevel.Warn);    // disable info logging
config.SetLogLevel(LogLevel.Error);   // disable warnings as well
config.SetLogLevel(LogLevel.Assert);  // disable errors as well
```

### <a id="build-your-app">Build your app

Build and run your app. If the build succeeds, you should carefully read the SDK logs in the console. After the app launched
for the first time, you should see the info log `Install tracked`.

![][run]

## <a id="additional-features">Additional features

Once you integrate the adjust SDK into your app, you can take advantage of the following features.

### <a id="event-tracking">Event tracking

You can use adjust to track events. Lets say you want to track every tap on a particular button. You would create a new 
event token in your [dashboard], which has an associated event token - looking something like `abc123`. In your button's 
`Click` handler you would then add the following lines to track the tap:

```cs
AdjustEvent eventClick = new AdjustEvent("abc123");

Adjust.TrackEvent(eventClick);
```

When tapping the button you should now see `Event tracked` in the logs.

The event instance can be used to configure the event further before tracking
it.

#### <a id="revenue-tracking">Revenue tracking

If your users can generate revenue by tapping on advertisements or making In-App Purchases, then you can track those 
revenues with events. Lets say a tap is worth one Euro cent. You could then track the revenue event like this:

```cs
AdjustEvent eventRevenue = new AdjustEvent("abc123");

adjustEvent.SetRevenue(0.01, "EUR");

Adjust.TrackEvent(adjustEvent);
```

When you set a currency token, adjust will automatically convert the incoming revenues into a reporting revenue of your 
choice. Read more about [currency conversion here.][currency-conversion]

You can read more about revenue and event tracking in the [event tracking guide][event-tracking].

#### <a id="iap-verification">In-App Purchase verification

In-App purchase verification can be done with Xamarin purchase SDK which is currently being developed and will soon be 
publicly available.

#### <a id="callback-parameters">Callback parameters

You can register a callback URL for your events in your [dashboard]. We will send a GET request to that URL whenever the 
event is tracked. You can add callback parameters to that event by calling `AddCallbackParameter` on the event before 
tracking it. We will then append these parameters to your callback URL.

For example, suppose you have registered the URL `http://www.adjust.com/callback` then track an event like this:

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

It should be mentioned that we support a variety of placeholders like `{gps_adid}` that can be used as parameter values. In 
the resulting callback, this placeholder would be replaced with the ID for Advertisers of the current device. Also note that
we don't store any of your custom parameters, but only append them to your callbacks. If you haven't registered a callback 
for an event, these parameters won't even be read.

You can read more about using URL callbacks, including a full list of available values, in our 
[callbacks guide][callbacks-guide].

#### <a id="partner-parameters">Partner parameters

You can also add parameters to be transmitted to network partners, for the integrations that have been activated in your 
adjust dashboard.

This works similarly to the callback parameters mentioned above, but can be added by calling the `AddPartnerParameter` 
method on your `AdjustEvent` instance.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.AddPartnerParameter("key", "value");
adjustEvent.AddPartnerParameter("foo", "bar");

Adjust.TrackEvent(adjustEvent);
```

You can read more about special partners and these integrations in our [guide to special partners][special-partners].

### <a id="attribution-callback">Attribution callback

You can register a callback to be notified of tracker attribution changes. Due to the different sources considered for 
attribution, this information can not by provided synchronously. Follow these steps to implement the optional callback in 
your app:

Please make sure to consider our [applicable attribution data policies][attribution-data].

1. Implement the `IOnAttributionChangedListener` interface in your Application class.

	```cs
	[Application (AllowBackup = true)]
	public class GlobalApplication : Application, IOnAttributionChangedListener
	{
	    ...
	}
	```
2. Override the `OnAttributionChanged` callback which will be triggered when the attribution has been changed.

	```cs
	public void OnAttributionChanged(AdjustAttribution attribution)
	{
	    Console.WriteLine("Attribution changed!");
	    Console.WriteLine("New attribution: {0}", attribution.ToString ());
	}
	```

3. Set your Application class instance as the listener in the `AdjustConfig` object.

	```cs
	AdjustConfig config = new AdjustConfig(this, yourAppToken, environment);
	
	config.SetOnAttributionChangedListener(this);
	
	Adjust.OnCreate (config);
	```

The callback function will be called when the SDK receives final attribution data. Within the callback function you have 
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
this should be done in your custom class which implements corresponding interface for each callback method.

Follow the same steps to implement the following callback function for successful tracked events. You need to set the 
listener object on `AdjustConfig` instance which implements `IOnEventTrackingSucceededListener` interface:

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

For failed tracked events, you need to set the listener object on `AdjustConfig` instance which implements 
`IOnEventTrackingFailedListener` interface:

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

For successful tracked sessions, you need to set the listener object on `AdjustConfig` instance which implements 
`IOnSessionTrackingSucceededListener` interface:

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

For failed tracked sessions, you need to set the listener object on `AdjustConfig` instance which implements 
`IOnSessionTrackingFailedListener` interface:

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

The callback functions will be called after the SDK tries to send a package to the server. Within the callback you have 
access to a response data object specifically for the callback. Here is a quick summary of the session response data 
properties:

- `string Message` the message from the server or the error logged by the SDK.
- `string Timestamp` timestamp from the server.
- `string Adid` a unique device identifier provided by adjust.
- `JSONObject JsonResponse` the JSON object with the response from the server.

Both event response data objects contain:

- `string EventToken` the event token, if the package tracked was an event.

And both event and session failed objects also contain:

- `bool WillRetry` indicates there will be an attempt to resend the package at a later time.

### <a id="disable-tracking">Disable tracking

You can disable the adjust SDK from tracking any activities of the current device by assigning parameter `false` to 
`Enabled` property. **This setting is remembered between sessions**, but it can only be activated after the first session.

```cs
Adjust.Enabled = false;
```

You can check if the adjust SDK is currently enabled by checking the `Enabled` property. It is always possible to activate 
the adjust SDK by invoking `Enabled` with the enabled parameter as `true`.

### <a id="offline-mode">Offline mode

You can put the adjust SDK in offline mode to suspend transmission to our servers, while still retaining tracked data to be 
sent later. While in offline mode, all information is saved in a file, so be careful to avoid triggering too many events 
while in offline mode.

You can activate offline mode by calling method `SetOfflineMode` with parameter `true`:

```cs
Adjust.SetOfflineMode (true);
```

Conversely, you can deactivate offline mode calling `SetOfflineMode` method with parameter `false`. When the adjust SDK is 
put back in online mode, all saved information is send to our servers with the correct time information.

Unlike disabling tracking, **this setting is not remembered** between sessions. This means that the SDK is in online mode 
whenever it is started, even if the app was terminated in offline mode.

### <a id="event-buffering">Event buffering

If your app makes heavy use of event tracking, then you might want to delay some HTTP requests in order to send them in a 
single batch per minute. 

You can enable event buffering with your `AdjustConfig` instance:

```cs
AdjustConfig config = new AdjustConfig(this, yourAppToken, environment);
	
config.SetEventBufferingEnabled((Java.Lang.Boolean)true);
	
Adjust.OnCreate(config);
```

If nothing is set, event buffering is **disabled by default**.

### <a id="background-tracking">Background tracking

The default behaviour of the adjust SDK is to **pause sending HTTP requests while the app is in the background**. You can 
change this in your `AdjustConfig` instance:

```cs
AdjustConfig config = new AdjustConfig(this, yourAppToken, environment);

config.SetSendInBackground(true);

Adjust.OnCreate(config);
```

If nothing set, sending in background is **disabled by default**.

### <a id="device-ids">Device IDs

Certain services (such as Google Analytics) require you to coordinate Device and Client IDs in order to prevent duplicate 
reporting. 

To obtain the Google Advertising Identifier, make a call to `GetGoogleAdId` method of the `Adjust` instance. In order to do 
that, second parameter of the method must be an object of a class which implements `IOnDeviceIdsRead` interface and 
overrides it's `OnGoogleAdIdRead` method:

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

### <a id="pre-installed-trackers">Pre-installed trackers

If you want to use the adjust SDK to recognize users that found your app pre-installed on their device, follow these steps.

1. Create a new tracker in your [dashboard].
2. Open your Application class and add set the default tracker of your `AdjustConfig`:

    ```cs
    AdjustConfig config = new AdjustConfig(this, yourAppToken, environment);

    config.SetDefaultTracker("{TrackerToken}");

    Adjust.OnCreate(config);
    ```

  Replace `{TrackerToken}` with the tracker token you created in step 2. Please note that the dashboard displays a tracker URL (including `http://app.adjust.com/`). In your source code, you should specify only the six-character token and not the
  entire URL.

3. Build and run your app. You should see a line like the following in app's log output:

    ```
    Default tracker: 'abc123'
    ```

### <a id="deeplinking"></a>Deep linking

If you are using the adjust tracker URL with an option to deep link into your app from the URL, there is the possibility to
get info about the deep link URL and its content. Hitting the URL can happen when the user has your app already installed 
(standard deep linking scenario) or if they don't have the app on their device (deferred deep linking scenario). In the 
standard deep linking scenario, Android platform natively offers the possibility for you to get the info about the deep 
link content. Deferred deep linking scenario is something which Android platform doesn't support out of box and for this 
case, the adjust SDK will offer you the mechanism to get the info about the deep link content.

#### <a id="deeplinking-standard">Standard deep linking scenario

If a user has your app installed and you want it to launch after hitting an adjust tracker URL with the `deep_link` 
parameter in it, you need enable deep linking in your app. This is being done by choosing a desired **unique scheme name** 
and assigning it to the Activity which you want to launch once the app opens after the user clicked on the link. This can be
done by setting certain properties on the Activity class which you would like to see launched once deep link has been 
clicked and your app opened. You need to set up proper intent filter and name the scheme:

```cs
[Activity(Label = "Example", MainLauncher = true)]
	[IntentFilter
	 (new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataScheme = "adjustExample")]
	public class MainActivity : Activity
	{
	    // ...
	}
```

With this being set, you need to use the assigned scheme name in the adjust tracker URL's `deep_link` parameter if you want
your app to launch once the tracker URL is clicked. A tracker URL without any information added to the deep link can be 
built to look something like this:

```
https://app.adjust.com/abc123?deep_link=adjustExample%3A%2F%2F
```

Please, have in mind that the `deep_link` parameter value in the URL **must be URL encoded**.

After clicking this tracker URL, and with the app set as described above, your app will launch along with the 
`MainActivity` intent. Inside the `MainActivity` class, you will automatically be provided with the information about the 
`deep_link` parameter content. Once this content is delivered to you, it **will not be encoded**, although it was encoded 
in the URL.

Depending on the `launchMode` setting of your Activity, information about the `deep_link` parameter content will be 
delivered to the appropriate place in the Activity file. For more information about the possible values of the `launchMode` 
property, check [the official Android documentation][android-launch-modes].

There are two possible places in which information about the deep link content will be delivered to your desired Activity 
via `Intent` object - either in the Activity's `OnCreate` or `OnNewIntent` method. After the app has launched and one of 
these methods is triggered, you will be able to get the actual deeplink passed in the `deep_link` parameter in the click 
URL. You can then use this information to do some additional logic in your app.

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

#### <a id="deeplinking-deferred">Deferred deep linking scenario

Deferred deep linking scenario happens when a user clicks on the adjust tracker URL with the `deep_link` parameter in it, 
but does not have the app installed on the device at the moment of click. After that, the user will get redirected to the 
Play Store to download and install your app. After opening it for the first time, the content of the `deep_link` parameter 
will be delivered to the app.

In order to get info about the `deep_link` parameter content in a deferred deep linking scenario, you should set a listener
method on the `AdjustConfig` object. Listener object needs to implement `IOnDeeplinkResponseListener` interface and override
it's `LaunchReceivedDeeplink` method. This will get triggered once the adjust SDK gets the info about the deep link content 
from the backend.

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

Once the adjust SDK receives the info about the deep link content from the backend, it will deliver you the info about its 
content in this listener and expect the `bool` return value from you. This return value represents your decision on 
whether the adjust SDK should launch the Activity to which you have assigned the scheme name from the deep link (like in 
the standard deep linking scenario) or not. 

If you return `true`, we will launch it and the exact same scenario which is described in the 
[Standard deep linking scenario chapter](#deeplinking-standard) will happen. If you do not want the SDK to launch the 
Activity, you can return `false` from this listener and based on the deep link content decide on your own what to do next 
in your app.

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
    
    Adjust.AppWillOpenUrl(data);
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
    
    Adjust.AppWillOpenUrl(data);
}
```

[dashboard]:	http://adjust.com
[adjust.com]:	http://adjust.com

[releases]: 		https://github.com/adjust/xamarin_sdk/releases
[event-tracking]: 	https://docs.adjust.com/en/event-tracking
[callbacks-guide]: 	https://docs.adjust.com/en/callbacks
[attribution-data]: 	https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[special-partners]: 	https://docs.adjust.com/en/special-partners
[demo-app-android]:	/./Android
[android-dashboard]:    http://developer.android.com/about/dashboards/index.html
[android_application]:	http://developer.android.com/reference/android/app/Application.html
[currency-conversion]:	https://docs.adjust.com/en/event-tracking/#tracking-purchases-in-different-currencies
[android-launch-modes]:	https://developer.android.com/guide/topics/manifest/activity-element.html

[reattribution-with-deeplinks]: https://docs.adjust.com/en/deeplinking/#manually-appending-attribution-data-to-a-deep-link

[run]: 			https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/run.png
[gps_added]: 		https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/gps_added.png
[add_packages]: 	https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/add_packages.png
[add_gps_to_app]: 	https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/add_gps_to_app.png
[application_class]: 	https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/application_class.png
[select_android_dll]: 	https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/select_android_dll.png
[permission_internet]: 	https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/permission_internet.png
[add_android_binding]:	https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/add_android_binding.png
[session_tracking_old]:	https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/session_tracking_old.png
[session_tracking_new]:	https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/session_tracking_new.png

[submodule_ios_binding]:     https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/submodule_ios_binding.png
[permission_wifi_state]:     https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/permission_wifi_state.png
[select_android_binding]:    https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/select_android_binding.png

[submodule_android_binding]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/submodule_android_binding.png
[reference_android_binding]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/reference_android_binding.png

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
