### Version 4.11.2 (15th May 2017)
#### Added
- **[IOS][AND]** Added check if `sdk_click` package response contains attribution information.
- **[IOS][AND]** Added sending of attributable parameters with every `sdk_click` package.

#### Changed
- **[IOS][AND]** Replaced `assert` level logs with `warn` level.
- **[REPO]** Updated example apps.

---

### Version 4.11.1 (25th April 2017)
#### Added
- **[iOS]** Added nullability annotations to public headers for Swift 3.0 compatibility.
- **[iOS]** Added `BITCODE_GENERATION_MODE` to iOS framework for `Carthage` support.
- **[iOS]** Added support for iOS 10.3.
- **[iOS][AND]** Added sending of the app's install time.
- **[iOS][AND]** Added sending of the app's update time.

#### Fixed
- **[iOS]** Fixed not processing of `sdk_info` package type causing logs not to print proper package name once tracked.
- **[AND]** Fixed query string parsing.
- **[AND]** Fixed issue of creating and destroying lots of threads on certain Android API levels (https://github.com/adjust/android_sdk/issues/265).
- **[AND]** Protected `Package Manager` from throwing unexpected exceptions (https://github.com/adjust/android_sdk/issues/266).

#### Changed
- **[AND]** Refactored native networking code.
- **[iOS]** Updated native iOS SDK to version **4.11.3**.
- **[AND]** Updated native Android SDK to version **4.11.3**.
- **[REPO]** Introduced `[iOS]`, `[AND]`, `[WIN]` and `[REPO]` tags to `CHANGELOG` to highlight the platform the change is referring to.
- **[REPO]** Updated example apps.

---

### Version 4.11.0 (5th January 2017)
#### Added
- **[iOS][AND]** Added `Adid` property to the attribution callback response.
- **[iOS][AND]** Added property `Adjust.Adid` to be able to get adid value at any time after obtaining it, not only when session/event callbacks have been triggered.
- **[iOS][AND]** Added property `Adjust.Attribution` to be able to get current attribution value at any time after obtaining it, not only when attribution callback has been triggered.
- **[AND]** Added sending of **Amazon Fire Advertising Identifier** for Android platform.
- **[AND]** Added possibility to set default tracker for the app by adding `adjust_config.properties` file to the `assets` folder of your app. Mostly meant to be used by the `Adjust Store & Pre-install Tracker Tool` (https://github.com/adjust/android_sdk/blob/master/doc/english/pre_install_tracker_tool.md).

#### Fixed
- **[iOS][AND]** Now reading push token value from activity state file when sending package.
- **[iOS]** Fixed memory leak by closing network session for iOS platform.
- **[iOS]** Fixed `TARGET_OS_TV` pre processor check for iOS platform.

#### Changed
- **[iOS][AND]** Firing attribution request as soon as install has been tracked, regardless of presence of attribution callback implementation in user's app.
- **[iOS]** Saving iAd/AdSearch details to prevent sending duplicated `sdk_click` packages for iOS platform.
- **[iOS]** Updated native iOS SDK to version **4.11.0**.
- **[REPO]** Updated docs.

---

### Version 4.10.1 (15th December 2016)
#### Fixed
- **[iOS][AND]** Deferred deep link arrival to the app is no longer dependent from implementation of the attribution callback.

#### Changed
- **[iOS]** Updated native iOS SDK to version **4.10.3**.
- **[AND]** Updated native Android SDK to version **4.11.0**.

---

### Version 4.10.0 (20th September 2016)
#### Added
- **[iOS]** Added support for iOS 10.
- **[iOS]** Added support for iOS iAd v3.
- **[iOS]** Added `Bitcode` support for iOS framework.
- **[iOS]** Added a method for getting `IDFA` on iOS device.
- **[AND]** Added a method for getting `Google Play Services Ad Id` on Android device.
- **[AND]** Added revenue deduplication for Android platform.
- **[iOS][AND]** Added support for `Xamarin Studio 6`.
- **[iOS][AND]** Added an option for enabling/disabling tracking while app is in background.
- **[iOS][AND]** Added a callback to be triggered if event is successfully tracked.
- **[iOS][AND]** Added a callback callback to be triggered if event tracking failed.
- **[iOS][AND]** Added a callback to be triggered if session is successfully tracked.
- **[iOS][AND]** Added a callback to be triggered if session tracking failed.
- **[iOS][AND]** Added a callback to be triggered when deferred deeplink is received.
- **[iOS][AND]** Added possibility to set session callback and partner parameters on `Adjust` instance with `AddSessionCallbackParameter` and `AddSessionPartnerParameter` methods.
- **[iOS][AND]** Added possibility to remove session callback and partner parameters by key on `Adjust` instance with `RemoveSessionCallbackParameter` and `RemoveSessionPartnerParameter` methods.
- **[iOS][AND]** Added possibility to remove all session callback and partner parameters on `Adjust` instance with `ResetSessionCallbackParameters` and `ResetSessionPartnerParameters` methods.
- **[iOS][AND]** Added new `Suppress` log level and for it new adjust config object constructor which gets `bool` indicating whether suppress log level should be supported or not.
- **[iOS][AND]** Added possibility to delay initialisation of the SDK while maybe waiting to obtain some session callback or partner parameters with `delayed start` feature on adjust config instance.
- **[iOS][AND]** Added possibility to set user agent manually on adjust config instance.
- **[REPO]** Added `CHANGELOG` to the repository.

#### Fixed
- **[iOS][AND]** Fixed issue with `Xamarin Studio 6` which caused attribution callback not to get triggered.

#### Changed
- **[iOS][AND]** Deferred deep link info will now arrive as part of the attribution response and not as part of the answer to first session.
- **[iOS]** Removed MAC MD5 tracking feature for iOS platform completely.
- **[iOS]** Updated native iOS SDK to version **4.10.1**.
- **[AND]** Updated native Android SDK to version **4.10.2**.
- **[REPO]** Updated docs.

---

### Version 4.0.2 (4th November 2015)
#### Changed
- **[iOS][AND]** Moved `ADJLogLevel` enum to `StructsAndEnums.cs` file.

---

### Version 4.0.1 (2nd November 2015)
#### Added
- **[iOS]** Added support for iOS 9.
- **[iOS]** Adding `iAd.framework` and `AdSupport.framework` automatically.

#### Changed
- **[iOS]** Updated native iOS SDK to version **4.4.1**.
- **[AND]** Updated native Android SDK to version **4.1.3**.

---

### Version 4.0.0 (12th May 2015)
#### Added
- **[iOS][AND]** Initial release of the adjust SDK for Xamarin. Added support for iOS and Android platforms.
- **[iOS]** Added native iOS SDK version **4.2.4**.
- **[AND]** Added native Android SDK version **4.0.6**.
