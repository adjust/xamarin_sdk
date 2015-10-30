## Summary

This is the Xamarin SDK of adjust™. You can read more about adjust™ at
[adjust.com].

## Example apps

There is an iOS example app inside the [`AdjustDemoiOS` directory][AdjustDemoiOS]. 
You can open the Xamarin Studio project to see an example on how the adjust SDK can be integrated.

## Basic integration into Xamarin iOS project

We will describe the steps to integrate the adjust SDK into your Xamarin Studio iOS project.
We are going to assume that you use Xamarin Studio for your iOS development.

### 1. Get the SDK

Download the latest version from our [releases page][releases]. Extract the
archive into a directory of your choice.

If you want to use adjust bindings DLL you can start with [step 3](#step3dll).

### 2. Add adjust iOS bindings project to your solution

Choose to add an existing project to your solution.

![][add_ios_binding]

Select the `AdjustBindingsiOS` project file and hit Open.

![][select_ios_binding]

After this, you will have the adjust iOS bindings added as a submodule to your solution.

![][submodule_ios_binding]

### 3. Add reference to adjust iOS bindings project

#### Adding the adjust bindings project

After you have successfully added the adjust iOS bindings project to your solution, you should 
also add a reference to it in your iOS app project properties.

![][reference_ios_binding]

#### <a id="step3dll"></a>Adding the adjust bindings DLL

The next step is to add a reference to the bindings DLL in your iOS project properties. In the references window, choose the `.Net Assembly` pane and select the  `AdjustBindingsiOS.dll` that you have downloaded.

![][select_ios_dll]

### 4. Integrate Adjust into your iOS app

To start with, we'll set up basic session tracking.

#### Basic Setup

Open the source file of your app delegate. Add the `using` statement at the top of the file, 
then add the following call to `Adjust` in the `FinishedLaunching` method of your app delegate:

```csharp
using AdjustBindingsiOS;
// ...
String yourAppToken = "{YourAppToken}";
String environment = AdjustConfig.EnvironmentSandbox;
ADJConfig adjustConfig = new ADJConfig (yourAppToken, environment);
Adjust.AppDidLaunch (adjustConfig);
```

Replace `{YourAppToken}` with your app token. You can find this in your
[dashboard].

Depending on whether you build your app for testing or for production, you must
set `environment` with one of these values:

```csharp
String environment = AdjustConfig.EnvironmentSandbox;
String environment = AdjustConfig.EnvironmentProduction;
```

**Important:** This value should be set to `AdjustConfig.EnvironmentSandbox` if and only
if you or someone else is testing your app. Make sure to set the environment to
`AdjustConfig.EnvironmentProduction` just before you publish the app. Set it back to
`AdjustConfig.EnvironmentSandbox` when you start developing and testing it again.

We use this environment to distinguish between real traffic and test traffic
from test devices. It is very important that you keep this value meaningful at
all times! This is especially important if you are tracking revenue.

#### Adjust Logging

You can increase or decrease the amount of logs you see in tests by setting `LogLevel` 
property on your `ADJConfig` instance with one of the following parameters:

```csharp
config.LogLevel = ADJLogLevel.Verbose; // enable all logging
config.LogLevel = ADJLogLevel.Debug;   // enable more logging
config.LogLevel = ADJLogLevel.Info;    // the default
config.LogLevel = ADJLogLevel.Warn;    // disable info logging
config.LogLevel = ADJLogLevel.Error;   // disable warnings as well
config.LogLevel = ADJLogLevel.Assert;  // disable errors as well
```

### 5. Additional settings

In order to get Xamarin iOS app project to recognize categories from adjust iOS bindings, 
you need to add additional mtouch arguments to your `iOS Build`. You can find this in 
the `Build` section of your `Project Options`. Add the `-gcc_flags` option followed by a 
quoted string, containing the `-ObjC` argument.

![][additional_flags]

### 6. Build your app

Build and run your app. If the build succeeds, you should carefully read the
SDK logs in the console. After the app launched for the first time, you should
see the info log `Install tracked`.

![][run_ios]

## Additional features

Once you integrate the adjust SDK into your project, you can take advantage of
the following features.

### 7. Add tracking of custom events

You can use adjust to track events. Lets say you want to track every tap on a
particular button. You would create a new event token in your [dashboard],
which has an associated event token - looking something like `abc123`. In your
button's `TouchUpInside` handler you would then add the following lines to track the tap:

```csharp
ADJEvent adjustEvent = new ADJEvent("abc123");
Adjust.TrackEvent(adjustEvent);
```

When tapping the button you should now see `Event tracked` in the logs.

The event instance can be used to configure the event further before tracking
it.

### 8. Add revenue tracking

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

#### <a id="deduplication"></a> Revenue deduplication

You can also pass in an optional transaction ID to avoid tracking duplicate
revenues. The last ten transaction IDs are remembered and revenue events with
duplicated transaction IDs are skipped. This is especially useful for in-app
purchase tracking. See an example below.

If you want to track in-app purchases, please make sure to call `TrackEvent`
after `FinishTransaction` in `UpdatedTransaction` only if the
state changed to `SKPaymentTransactionState.Purchased`. That way you can avoid
tracking revenue that is not actually being generated.

```csharp
public void UpdatedTransactions (SKPaymentQueue queue, SKPaymentTransaction[] transactions)
{
	foreach (SKPaymentTransaction transaction in transactions) 
	{
		switch (transaction.TransactionState)
		{
			case SKPaymentTransactionState.Purchased:
				SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);
		        
				ADJEvent adjustEvent = new ADJEvent ("abc123");
				adjustEvent.SetRevenue (0.01, "{currency}");
				adjustEvent.SetTransactionId (transaction.TransactionIdentifier);
				Adjust.TrackEvent (adjustEvent);

				break;

			// more cases
		}
	}
}
```

#### Receipt verification

If you track in-app purchases, you can also attach the receipt to the tracked
event. In that case our servers will verify that receipt with Apple and discard
the event if the verification failed. To make this work, you also need to send
us the transaction ID of the purchase. The transaction ID will also be used for
SDK side deduplication as explained [above](#deduplication):

```csharp
NSUrl receiptUrl = NSBundle.MainBundle.AppStoreReceiptUrl;
NSData receipt = NSData.FromUrl (receiptUrl);

ADJEvent adjustEvent = new ADJEvent ("abc123");
adjustEvent.SetRevenue (0.01, "{currency}");
adjustEvent.SetReceipt (receipt, transaction.TransactionIdentifier);
Adjust.TrackEvent (adjustEvent);
```

### 9. Callback parameters

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

### 10. Partner parameters

You can also add parameters to be transmitted to network partners, for the
integrations that have been activated in your adjust dashboard.

This works similarly to the callback parameters mentioned above, but can
be added by calling the `AddPartnerParameter` method on your `ADJEvent` instance.

```objc
ADJEvent adjustEvent = new ADJEvent("abc123");
adjustEvent.AddPartnerParameter("key", "value");
Adjust.TrackEvent(adjustEvent);
```

You can read more about special partners and these integrations in our
[guide to special partners.][special-partners]

### 11. Implement the attribution callback

You can register a delegate callback to be notified of tracker attribution
changes. Due to the different sources considered for attribution, this
information can not by provided synchronously. Follow these steps to implement
the optional delegate protocol in your app:

Please make sure to consider our [applicable attribution data
policies.][attribution-data]

1. Open `AppDelegate.cs` and create a class which inherits from `AdjustDelegate` and override its `AdjustAttributionChanged` method.
    
    ```csharp
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

    ```csharp
    private AdjustDelegateXamarin adjustDelegate = null;
    ```

3. Initialize and set the delegate with your `ADJConfig` instance:

    ```csharp
    adjustDelegate = new AdjustDelegateXamarin();
    ...
    adjustConfig.Delegate = adjustDelegate;
    ```
    
As the delegate callback is configured using the `ADJConfig` instance, you
should set the `Delegate` property before calling `Adjust.AppDidLaunch (adjustConfig)`.

The callback function will be called  when the SDK receives final attribution data.
Within the callback function you have access to the `attribution` parameter.
Here is a quick summary of its properties:

- `string TrackerToken` the tracker token of the current install.
- `string TrackerName` the tracker name of the current install.
- `string Network` the network grouping level of the current install.
- `string Campaign` the campaign grouping level of the current install.
- `string Adgroup` the ad group grouping level of the current install.
- `string Creative` the creative grouping level of the current install.
- `string ClickLabel` the click label of the current install.

### 12. Set up deep link reattributions

You can set up the adjust SDK to handle deep links that are used to open your
app via a custom URL scheme. We will only read certain adjust specific
parameters. This is essential if you are planning to run retargeting or
re-engagement campaigns with deep links.

Open the source file your Application Delegate. Find or add the method `OpenURL` 
and add the following call to adjust:

```csharp
public override bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
{
	Adjust.AppWillOpenUrl (url);

	return true;
}
```

### 13. Enable event buffering

If your app makes heavy use of event tracking, you might want to delay some
HTTP requests in order to send them in one batch every minute. 

You can enable event buffering with your `ADJConfig` instance:

```csharp
config.EventBufferingEnabled = true;
```

### 14. Disable tracking

You can disable the adjust SDK from tracking any activities of the current
device by calling `SetEnabled` with parameter `false`. This setting is remembered
between sessions, but it can only be activated after the first session.

```csharp
Adjust.SetEnabled(false);
```

You can check if the adjust SDK is currently enabled by checking the
`IsEnabled` property. It is always possible to activate the adjust SDK by invoking
`SetEnabled` with the enabled parameter as `true`.

### 15. Offline mode

You can put the adjust SDK in offline mode to suspend transmission to our servers, 
while retaining tracked data to be sent later. While in offline mode, all information is saved
in a file, so be careful not to trigger too many events while in offline mode.

You can activate offline mode by calling `SetOfflineMode` with the parameter `true`.

```csharp
Adjust.SetOfflineMode(true);
```

Conversely, you can deactivate offline mode by calling `SetOfflineMode` with `false`.
When the adjust SDK is put back in online mode, all saved information is send to our servers 
with the correct time information.

Unlike disabling tracking, this setting is *not remembered*
bettween sessions. This means that the SDK is in online mode whenever it is started,
even if the app was terminated in offline mode.

[adjust.com]: http://adjust.com
[dashboard]: http://adjust.com
[AdjustDemoiOS]: https://github.com/adjust/xamarin_sdk/tree/master/AdjustDemoiOS
[AdjustDemoAndroid]: https://github.com/adjust/xamarin_sdk/tree/master/AdjustDemoAndroid
[releases]: https://github.com/adjust/xamarin_sdk/releases
[add_ios_binding]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/add_ios_binding.png
[select_ios_binding]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/select_ios_binding.png
[select_ios_dll]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/select_ios_dll.png
[submodule_ios_binding]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/submodule_ios_binding.png
[submodule_android_binding]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/submodule_android_binding.png
[reference_ios_binding]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/reference_ios_binding.png
[reference_android_binding]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/reference_android_binding.png
[additional_flags]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/additional_flags.png
[run_ios]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/ios/run.png
[run_android]: https://github.com/adjust/sdks/blob/master/Resources/xamarin/android/run.png
[callbacks-guide]: https://docs.adjust.com/en/callbacks
[event-tracking]: https://docs.adjust.com/en/event-tracking
[currency-conversion]: https://docs.adjust.com/en/event-tracking/#tracking-purchases-in-different-currencies
[attribution-data]: https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[special-partners]: https://docs.adjust.com/en/special-partners

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
