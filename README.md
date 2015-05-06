## Summary

This is the Xamarin SDK of adjust™. You can read more about adjust™ at
[adjust.com].

## Example apps

There is an iOS example app inside the [`AdjustDemoiOS` directory][AdjustDemoiOS] 
and an Android example app inside the [`AdjustDemoAndroid` directory][AdjustDemoAndroid]. 
You can open the Xamarin Studio project to see an example on how the adjust SDK can be integrated.

## Basic integration into Xamarin iOS project

We will describe the steps to integrate the adjust SDK into your Xamarin Studio project.
We are going to assume that you use Xamarin Studio for your iOS development.

### 1. Get the SDK

Download the latest version from our [releases page][releases]. Extract the
archive into a directory of your choice.

### 2. Add adjust iOS binding project to your solution

Choose to add an exising project to your solution.

![][add_ios_binding]

Select AdjustBindingsiOS project file and select Open.

![][select_ios_binding]

After this, you will have adjust iOS bindings added as submodule to your solution.

![][submodule_ios_binding]

### 3. Add reference to adjust iOS binding project

After you have successfully added adjust iOS bindings project to your solution, you should add a reference to it in your iOS application project properties.

![][reference_ios_binding]

### 4. Integrate Adjust into your app

To start with, we'll set up basic session tracking.

#### Basic Setup

In the Project Navigator, open the source file of your application delegate.
Add the `using` statement at the top of the file, then add the following call
to `Adjust` in the `FinishedLaunching` method of your app delegate:

```csharp
using AdjustBindingsiOS;
// ...
String yourAppToken = "{YourAppToken}";
String environment = Constants.ADJEnvironmentSandbox;
ADJConfig adjustConfig = new ADJConfig (yourAppToken, environment);
Adjust.AppDidLaunch (adjustConfig);
```

Replace `{YourAppToken}` with your app token. You can find this in your
[dashboard].

Depending on whether you build your app for testing or for production, you must
set `environment` with one of these values:

```csharp
String environment = Constants.ADJEnvironmentSandbox;
String environment = Constants.ADJEnvironmentProduction;
```

**Important:** This value should be set to `Constants.ADJEnvironmentSandbox` if and only
if you or someone else is testing your app. Make sure to set the environment to
`Constants.ADJEnvironmentProduction` just before you publish the app. Set it back to
`Constants.ADJEnvironmentSandbox` when you start developing and testing it again.

We use this environment to distinguish between real traffic and test traffic
from test devices. It is very important that you keep this value meaningful at
all times! This is especially important if you are tracking revenue.

#### Adjust Logging

You can increase or decrease the amount of logs you see in tests by calling
setting `LogLevel` property on your `ADJConfig` instance with one of the following
parameters:

```csharp
adjustConfig.LogLevel = ADJLogLevel.Verbose; // enable all logging
adjustConfig.LogLevel = ADJLogLevel.Debug;   // enable more logging
adjustConfig.LogLevel = ADJLogLevel.Info;    // the default
adjustConfig.LogLevel = ADJLogLevel.Warn;    // disable info logging
adjustConfig.LogLevel = ADJLogLevel.Error;   // disable warnings as well
adjustConfig.LogLevel = ADJLogLevel.Assert;  // disable errors as well
```

### 5. Additional flags

In order to get Xamarion iOS application project to recognize categories from adjust bundle, you need to add in “iPhone Build’s” aditional mtouch argument (these are part of your project options) the “-gcc_flags” option followed by a quoted string. You need to add `-ObjC` argument.

![][additional_flags]

### 6. Build your app

Build and run your app. If the build succeeds, you should carefully read the
SDK logs in the console. After the app launched for the first time, you should
see the info log `Install tracked`.

![][run]

## Additional features

Once you integrate the adjust SDK into your project, you can take advantage of
the following features.

### 7. Set up event tracking

You can use adjust to track events. Lets say you want to track every tap on a
particular button. You would create a new event token in your [dashboard],
which has an associated event token - looking something like `abc123`. In your
button's `TouchUpInside` method you would then add the following lines to track
the tap:

```csharp
ADJEvent adjustEvent = new ADJEvent("abc123");
Adjust.TrackEvent(adjustEvent);
```

When tapping the button you should now see `Event tracked` in the logs.

The event instance can be used to configure the event even more before tracking
it.

#### Add callback parameters

You can register a callback URL for your events in your [dashboard]. We will
send a GET request to that URL whenever the event gets tracked. You can add
callback parameters to that event by calling `AddCallbackParameter` on the
event before tracking it. We will then append these parameters to your callback
URL.

For example, suppose you have registered the URL
`http://www.adjust.com/callback` then track an event like this:

```csharp
ADJEvent adjustEvent = new ADJEvent("abc123");
adjustEvent.AddCallbackParameter("key", "value");
adjustEvent.AddCallbackParameter("foo", "bar");
Adjust.TrackEvent(adjustEvent);
```

In that case we would track the event and send a request to:

    http://www.adjust.com/callback?key=value&foo=bar

It should be mentioned that we support a variety of placeholders like `{idfa}`
that can be used as parameter values. In the resulting callback this
placeholder would be replaced with the ID for Advertisers of the current
device. Also note that we don't store any of your custom parameters, but only
append them to your callbacks. If you haven't registered a callback for an
event, these parameters won't even be read.

You can read more about using URL callbacks, including a full list of available
values, in our [callbacks guide][callbacks-guide].

#### Track revenue

If your users can generate revenue by tapping on advertisements or making
in-app purchases you can track those revenues with events. Lets say a tap is
worth one Euro cent. You could then track the revenue event like this:

```csharp
ADJEvent adjustEvent = new ADJEvent("abc123");
adjustEvent.SetRevenue(0.01, "EUR");
Adjust.TrackEvent(adjustEvent);
```

This can be combined with callback parameters of course.

When you set a currency token, adjust will automatically convert the incoming revenues into a reporting revenue of your choice. Read more about [currency conversion here.][currency-conversion]

You can read more about revenue and event tracking in the [event tracking guide.][event-tracking]

[adjust.com]: http://adjust.com
[dashboard]: http://adjust.com
[AdjustDemoiOS]: https://github.com/adjust/xamarin_sdk/tree/master/AdjustDemoiOS
[AdjustDemoAndroid]: https://github.com/adjust/xamarin_sdk/tree/master/AdjustDemoAndroid
[releases]: https://github.com/adjust/xamarin_sdk/releases
[add_ios_binding]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/ios/add_ios_binding.png
[select_ios_binding]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/ios/select_ios_binding.png
[submodule_ios_binding]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/ios/submodule_ios_binding.png
[reference_ios_binding]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/ios/reference_ios_binding.png
[additional_flags]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/ios/additional_flags.png
[run]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/ios/run.png
[callbacks-guide]: https://docs.adjust.com/en/callbacks
[event-tracking]: https://docs.adjust.com/en/event-tracking
[currency-conversion]: https://docs.adjust.com/en/event-tracking/#tracking-purchases-in-different-currencies
