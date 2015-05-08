## Summary

This is the Xamarin SDK of adjust™. You can read more about adjust™ at
[adjust.com].

## Example apps

There is an Android example app inside the [`AdjustDemoAndroid` directory][AdjustDemoAndroid]. 
You can open the Xamarin Studio project to see an example on how the adjust SDK can be integrated.

## Basic integration into Xamarin Android project

We will describe the steps to integrate the adjust SDK into your Xamarin Studio project.
We are going to assume that you use Xamarin Studio for your Android development.

### 1. Get the SDK

Download the latest version from our [releases page][releases]. Extract the
archive into a directory of your choice.

### 2. Add adjust Android bindings project to your solution

Choose to add an exising project to your solution.

![][add_android_binding]

Select `AdjustBindingsAndroid` project file and select Open.

![][select_android_binding]

After this, you will have adjust Android bindings added as submodule to your solution.

<a href="url"><img src="https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/submodule_android_binding.png" align="center" height="500" width="300" ></a>

### 3. Add reference to adjust Android bindings project

After you have successfully added adjust Android bindings project to your solution, you should add a reference to it in your Android app project properties.

<a href="url"><img src="https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/reference_android_binding.png" align="center" height="400" width="650" ></a>

### 4. Add Google Play Services

Since the 1st of August of 2014, apps in the Google Play Store must use the Google Advertising ID to uniquely identify devices. To allow the adjust SDK to use the Google Advertising ID, you must integrate the Google Play Services. If you haven't done this yet, follow these steps:

1. Choose to `Get More Components` by your `Components` folder in Android app project.

	![][get_more_components]

2. Search for `Google Play Services` and add them to your app.

	![][add_gps_to_app]

3. After you have added Google Play Services to your Android app project, your `Components` and `Packages`
folders content should look like this:

	<a href="url"><img src="https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/gps_added.png" align="center" height="500" width="300" ></a>

### 5. Add permissions

In `Properties` folder open the `AndroidManifest.xml` of your Android app project. Add the INTERNET permission 
if it's not present already.

![][permission_internet]

If you are _not_ targeting the Google Play Store, add INTERNET and ACCESS_WIFI_STATE permissions:

![][permission_wifi_state]

### 6. Integrate Adjust into your app

To start with, we'll set up basic session tracking.

#### Basic Setup

We recommend using a global android [Application][android_application] class to
initialize the SDK. If don't have one in your app already, create a class that extends `Application`.

![][application_class]

In your `Application` class find or create the `onCreate` method and add the
following code to initialize the adjust SDK:

```csharp
using Com.Adjust.Sdk;
// ...
const String appToken = "{YourAppToken}";
const String environment = AdjustConfig.EnvironmentSandbox;
AdjustConfig config = new AdjustConfig(this, appToken, environment);
Adjust.OnCreate (config);
```

Replace `{YourAppToken}` with your app token. You can find this in your
[dashboard].

Depending on whether you build your app for testing or for production, you must
set `environment` with one of these values:

```csharp
const String environment = AdjustConfig.EnvironmentSandbox;
const String environment = AdjustConfig.EnvironmentProduction;
```

**Important:** This value should be set to `AdjustConfig.EnvironmentSandbox` if and only
if you or someone else is testing your app. Make sure to set the environment to
`AdjustConfig.EnvironmentProduction` just before you publish the app. Set it back to
`AdjustConfig.EnvironmentSandbox` when you start developing and testing it again.

We use this environment to distinguish between real traffic and test traffic
from test devices. It is very important that you keep this value meaningful at
all times! This is especially important if you are tracking revenue.

#### Adjust Logging

You can increase or decrease the amount of logs you see in tests by calling `SetLogLevel`
on your `AdjustConfig` instance with one of the following parameters:

```csharp
config.SetLogLevel(LogLevel.Verbose); // enable all logging
config.SetLogLevel(LogLevel.Debug);   // enable more logging
config.SetLogLevel(LogLevel.Info);    // the default
config.SetLogLevel(LogLevel.Warn);    // disable info logging
config.SetLogLevel(LogLevel.Error);   // disable warnings as well
config.SetLogLevel(LogLevel.Assert);  // disable errors as well
```

### 7. Update your activities

To provide proper session tracking it is required to call certain Adjust methods every time 
any Activity resumes or pauses. Otherwise the SDK might miss a session start or session end. 
In order to do so you should follow these steps for **each** Activity of your app:

1. Open the source file of your Activity.
2. Add the `import` statement at the top of the file.
3. In your Activity's `OnResume` method call `Adjust.OnResume`. Create the method if needed.
4. In your Activity's `OnPause` method call `Adjust.OnPause`. Create the method if needed.

After these steps your activity should look like this:

![][on_resume_on_pause]

### 8. Build your app

Build and run your app. If the build succeeds, you should carefully read the
SDK logs in the console. After the app launched for the first time, you should
see the info log `Install tracked`.

![][run_android]

## Additional features

Once you integrate the adjust SDK into your project, you can take advantage of
the following features.

### 9. Add tracking of custom events

You can use adjust to track events. Lets say you want to track every tap on a
particular button. You would create a new event token in your [dashboard],
which has an associated event token - looking something like `abc123`. In your
button's `Click` handler you would then add the following lines to track the tap:

```csharp
AdjustEvent eventClick = new AdjustEvent("abc123");
Adjust.TrackEvent(eventClick);
```

When tapping the button you should now see `Event tracked` in the logs.

The event instance can be used to configure the event even more before tracking
it.

### 10. Add callback parameters

You can register a callback URL for your events in your [dashboard]. We will
send a GET request to that URL whenever the event gets tracked. You can add
callback parameters to that event by calling `AddCallbackParameter` on the
event before tracking it. We will then append these parameters to your callback URL.

For example, suppose you have registered the URL
`http://www.adjust.com/callback` then track an event like this:

```csharp
AdjustEvent adjustEvent = new AdjustEvent("abc123");
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

### 11. Partner parameters

You can also add parameters to be transmitted to network partners, for the
integrations that have been activated in your adjust dashboard.

This works similarly to the callback parameters mentioned above, but can
be added by calling the `AddPartnerParameter` method on your `AdjustEvent` instance.

```objc
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.AddPartnerParameter("key", "value");
Adjust.TrackEvent(adjustEvent);
```

You can read more about special partners and these integrations in our
[guide to special partners.][special-partners]

### 12. Add tracking of revenue

If your users can generate revenue by tapping on advertisements or making
in-app purchases you can track those revenues with events. Lets say a tap is
worth one Euro cent. You could then track the revenue event like this:

```csharp
AdjustEvent eventRevenue = new AdjustEvent("abc123");
adjustEvent.SetRevenue(0.01, "EUR");
Adjust.TrackEvent(adjustEvent);
```

This can be combined with callback parameters of course.

When you set a currency token, adjust will automatically convert the incoming revenues into a reporting revenue of your choice. Read more about [currency conversion here.][currency-conversion]

You can read more about revenue and event tracking in the [event tracking guide.][event-tracking]

### 13. Set up deep link reattributions

You can set up the adjust SDK to handle deep links that are used to open your
app via a custom URL scheme. We will only read certain adjust specific
parameters. This is essential if you are planning to run retargeting or
re-engagement campaigns with deep links.

For each activity that accepts deep links, find the `OnCreate` method and add the folowing call to adjust:

```csharp
protected override void OnCreate (Bundle savedInstanceState)
{
	base.OnCreate (savedInstanceState);

	Intent intent = this.Intent;
	var data = intent.Data;
	Adjust.AppWillOpenUrl(data);
	...
}
```

### 14. Enable event buffering

If your app makes heavy use of event tracking, you might want to delay some
HTTP requests in order to send them in one batch every minute. 

You can enable event buffering with your `AdjustConfig` instance:

```csharp
config.SetEventBufferingEnabled((Java.Lang.Boolean)true);
```

### 15. Set listener for attribution changes

You can register a callback listener to be notified of tracker attribution
changes. Due to the different sources considered for attribution, this
information can not by provided synchronously. Follow these steps to implement
the optional listener in your app:

Please make sure to consider our [applicable attribution data
policies.][attribution-data]

1. Make your Application class to implement `IOnAttributionChangedListener` interface.

	```csharp
	[Application (AllowBackup = true)]
	public class GlobalApplication : Application, IOnAttributionChangedListener
	{
		...
	}
	```
2. Override `OnAttributionChanged` callback which will be triggered when attribution has been changed.

	```csharp
	public void OnAttributionChanged (AdjustAttribution attribution)
	{
		Console.WriteLine ("Attribution changed!");
		Console.WriteLine ("New attribution: {0}", attribution.ToString ());
	}
	```

3. Set your Application class instance as listener in `AdjustConfig` object.

	```csharp
	AdjustConfig config = new AdjustConfig(this, appToken, environment);
	config.SetOnAttributionChangedListener(this);
	Adjust.OnCreate (config);
	```

The callback function will get when the SDK receives final attribution data.
Within the callback function you have access to the `attribution` parameter.
Here is a quick summary of its properties:

- `string TrackerToken` the tracker token of the current install.
- `string TrackerName` the tracker name of the current install.
- `string Network` the network grouping level of the current install.
- `string Campaign` the campaign grouping level of the current install.
- `string Adgroup` the ad group grouping level of the current install.
- `string Creative` the creative grouping level of the current install.
- `string ClickLabel` the click label of the current install.

### 16. Disable tracking

You can disable the adjust SDK from tracking any activities of the current
device by assigning parameter `false` to `Enabled` property. This setting 
is remembered between sessions, but it can only be activated after the first session.

```csharp
Adjust.Enabled = false;
```

You can check if the adjust SDK is currently enabled by checking the
`Enabled` property. It is always possible to activate the adjust SDK by invoking
`Enabled` with the enabled parameter as `true`.

[adjust.com]: http://adjust.com
[dashboard]: http://adjust.com
[AdjustDemoiOS]: https://github.com/adjust/xamarin_sdk/tree/master/AdjustDemoiOS
[AdjustDemoAndroid]: https://github.com/adjust/xamarin_sdk/tree/master/AdjustDemoAndroid
[releases]: https://github.com/adjust/xamarin_sdk/releases
[add_android_binding]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/add_android_binding.png
[get_more_components]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/get_more_components.png
[application_class]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/application_class.png
[add_gps_to_app]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/add_gps_to_app.png
[gps_added]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/gps_added.png
[permission_internet]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/permission_internet.png
[permission_wifi_state]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/permission_wifi_state.png
[on_resume_on_pause]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/on_resume_on_pause.png
[select_ios_binding]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/ios/select_ios_binding.png
[select_android_binding]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/select_android_binding.png
[submodule_ios_binding]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/ios/submodule_ios_binding.png
[submodule_android_binding]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/submodule_android_binding.png
[reference_ios_binding]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/ios/reference_ios_binding.png
[reference_android_binding]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/reference_android_binding.png
[additional_flags]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/ios/additional_flags.png
[run_ios]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/ios/run.png
[run_android]: https://github.com/adjust/sdks/blob/xamarin/Resources/xamarin/android/run.png
[callbacks-guide]: https://docs.adjust.com/en/callbacks
[event-tracking]: https://docs.adjust.com/en/event-tracking
[currency-conversion]: https://docs.adjust.com/en/event-tracking/#tracking-purchases-in-different-currencies
[attribution-data]: https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[special-partners]: https://docs.adjust.com/en/special-partners
[android_application]: http://developer.android.com/reference/android/app/Application.html

## License

The adjust-SDK is licensed under the MIT License.

Copyright (c) 2012-2015 adjust GmbH,
http://www.adjust.com

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
