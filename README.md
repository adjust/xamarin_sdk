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
