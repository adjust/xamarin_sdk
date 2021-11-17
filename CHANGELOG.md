### Version 4.29.0 (17th November 2021)
#### Added
- Added possibility to get cost data information in attribution callback.
- Added `NeedsCost` property to `AdjustConfig` to indicate if cost data is needed in attribution callback (by default cost data will not be part of attribution callback if not enabled with this setter method).
- Added `SetPreinstallTrackingEnabled` method to `AdjustConfig` to allow enabling of preinstall tracking in Android (this feature is OFF by default).
- Added preinstall tracking with usage of system installer receiver on Android platform (`SetPreinstallFilePath` method of the `AdjustConfig`).
- Added support for Apple Search Ads attribution with usage of `AdServices.framework`.
- Added `SetAllowAdServicesInfoReading` method to `AdjustConfig` to allow option for users to prevent SDK from performing any tasks related to Apple Search Ads attribution with usage of `AdServices.framework`.
- Added wrapper method `UpdateConversionValue` method to `Adjust` to allow updating SKAdNetwork conversion value via SDK API.
- Added `AppTrackingAuthorizationStatus` property to `Adjust` instance to be able to get current iOS app tracking status.
- Added improved measurement consent management and third party sharing mechanism.
- Added data residency feature. You can choose this setting by setting `UrlStrategy` property of `AdjustConfig` instance with `AdjustConfig.DataResidencyEU` (for EU data residency region), `AdjustConfig.DataResidencyTR` (for TR data residency region) or `AdjustConfig.DataResidencyUS` value (for US data residency region).
- Added `AdjustConversionValueUpdated` method to `AdjustDelegate` which can be used to set a callback to get information when Adjust SDK updates conversion value for the iOS user.

#### Native SDKs
- [iOS@v4.29.6][ios_sdk_v4.29.6]
- [Android@v4.28.7][android_sdk_v4.28.7]

---

### Version 4.28.0 (2nd April 2021)
#### Changed
- Removed native iOS legacy code.

#### Native SDKs
- [iOS@v4.28.0][ios_sdk_v4.28.0]
- [Android@v4.27.0][android_sdk_v4.27.0]

---

### Version 4.23.0 (22th October 2020)
#### Added
- Added communication with SKAdNetwork framework by default on iOS 14.
- Added method `deactivateSKAdNetworkHandling` to `ADJConfig` to switch off default communication with SKAdNetwork framework.
- Added wrapper method `requestTrackingAuthorizationWithCompletionHandler:` to `Adjust` to allow immediate propagation of user's choice to backend.
- Added handling of new iAd framework error codes introduced in iOS 14.
- Added sending of value of user's consent to be tracked with each package.
- Added `setUrlStrategy:` method in both android and ios `Config` classes to allow selection of URL strategy for specific market.
- Added reading of additional fields which Play Install Referrer API introduced in v2.0.

#### Native SDKs
- [iOS@v4.23.2][ios_sdk_v4.23.2]
- [Android@v4.24.1][android_sdk_v4.24.1]

---

### Version 4.22.0 (11th June 2020)
#### Added
- Added subscription tracking feature.
- Added support for Huawei App Gallery install referrer.

#### Changed
- Updated communication flow with `iAd.framework`.

#### Native SDKs
- [iOS@v4.22.1][ios_sdk_v4.22.1]
- [Android@v4.22.0][android_sdk_v4.22.0]

---

### Version 4.21.0 (20th March 2020)
#### Added
- Added `DisableThirdPartySharing` method to `Adjust` interface to allow disabling of data sharing with third parties outside of Adjust ecosystem.
- Added support for signature library as a plugin.
- Added more aggressive sending retry logic for install session package.
- Added additional parameters to `ad_revenue` package payload.
- Added external device ID support.

#### Native SDKs
- [iOS@v4.21.0][ios_sdk_v4.21.0]
- [Android@v4.21.0][android_sdk_v4.21.0]

---

### Version 4.18.0 (3rd July 2019)
#### Added
- Added `TrackAdRevenue` method to `Adjust` interface to allow tracking of ad revenue. With this release added support for `MoPub` ad revenue tracking.
- Added reading of Facebook anonymous ID if available on iOS platform.

#### Native SDKs
- [iOS@v4.18.0][ios_sdk_v4.18.0]
- [Android@v4.18.0][android_sdk_v4.18.0]

---

### Version 4.17.0 (14th January 2019)
#### Added
- Added `SdkVersion` property to `Adjust` interface to obtain current SDK version string.
- Added `SetCallbackId` method to `AdjustEvent` class for users to set custom ID on event object which will later be reported in event success/failure callbacks.
- Added `CallbackId` field to event tracking success callback object.
- Added `CallbackId` field to event tracking failure callback object.

#### Changed
- Marked `SetReadMobileEquipmentIdentity` method of `AdjustConfig` object as deprecated.
- SDK will now fire attribution request each time upon session tracking finished in case it lacks attribution info.

#### Native SDKs
- [iOS@v4.17.1][ios_sdk_v4.17.1]
- [Android@v4.17.0][android_sdk_v4.17.0]

---

### Version 4.14.0 (18th June 2018)
#### Added
- Added deep link caching in case `AppWillOpenUrl` method is called before SDK is initialised.
- Added `SetPushToken(string)` method to `Adjust` interface for iOS platform. This method should be used instead of `SetDeviceToken(NSData)` for passing push token to Adjust SDK as of v4.14.0.
- Added new method `AppWillOpenUrl(Android.Net.Uri, Context)` to `Adjust` interface for Android platform. This method should be used instead of `AppWillOpenUrl(Android.Net.Uri)` as of v4.14.0.

#### Changed
- Marked `SetDeviceToken(NSData)` method of the `Adjust` interface for iOS platform as deprecated.
- Marked `AppWillOpenUrl(Android.Net.Uri)` method of the Adjust interface for Android platform as deprecated.

#### Native SDKs
- [iOS@v4.14.1][ios_sdk_v4.14.1]
- [Android@v4.14.0][android_sdk_v4.14.0]

---

### Version 4.13.0 (16th May 2018)
#### Added
- Added `GdprForgetMe` method to `Adjust` interface for Android and iOS to enable possibility for user to be forgotten in accordance with GDPR law.

#### Native SDKs
- [iOS@v4.13.0][ios_sdk_v4.13.0]
- [Android@v4.13.0][android_sdk_v4.13.0]

---

### Version 4.12.0 (12th March 2018)
#### Added
- Added `GetAmazonAdId` method to `Adjust` interface for Android.
- Added `SetReadMobileEquipmentIdentity` method to `AdjustConfig` interface for Android.
- Added `SetAppSecret` method to `AdjustConfig` interface.

#### Native SDKs
- [iOS@v4.12.3][ios_sdk_v4.12.3]
- [Android@v4.12.4][android_sdk_v4.12.4]

---

### Version 4.11.3 (28th September 2017)
#### Added
- **[iOS]** Improved iOS 11 support.

#### Changed
- **[iOS]** Removed iOS connection validity checks.
- **[iOS]** Updated native iOS SDK to version **4.11.5**.

---

### Version 4.11.2 (15th May 2017)
#### Added
- **[iOS][AND]** Added check if `sdk_click` package response contains attribution information.
- **[iOS][AND]** Added sending of attributable parameters with every `sdk_click` package.

#### Changed
- **[iOS][AND]** Replaced `assert` level logs with `warn` level.
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

[ios_sdk_v4.12.1]: https://github.com/adjust/ios_sdk/tree/v4.12.1
[ios_sdk_v4.12.3]: https://github.com/adjust/ios_sdk/tree/v4.12.3
[ios_sdk_v4.13.0]: https://github.com/adjust/ios_sdk/tree/v4.13.0
[ios_sdk_v4.14.1]: https://github.com/adjust/ios_sdk/tree/v4.14.1
[ios_sdk_v4.17.1]: https://github.com/adjust/ios_sdk/tree/v4.17.1
[ios_sdk_v4.18.0]: https://github.com/adjust/ios_sdk/tree/v4.18.0
[ios_sdk_v4.21.0]: https://github.com/adjust/ios_sdk/tree/v4.21.0
[ios_sdk_v4.21.2]: https://github.com/adjust/ios_sdk/tree/v4.21.2
[ios_sdk_v4.22.1]: https://github.com/adjust/ios_sdk/tree/v4.22.1
[ios_sdk_v4.23.2]: https://github.com/adjust/ios_sdk/tree/v4.23.2
[ios_sdk_v4.28.0]: https://github.com/adjust/ios_sdk/tree/v4.28.0
[ios_sdk_v4.29.6]: https://github.com/adjust/ios_sdk/tree/v4.29.6

[android_sdk_v4.12.0]: https://github.com/adjust/android_sdk/tree/v4.12.0
[android_sdk_v4.12.4]: https://github.com/adjust/android_sdk/tree/v4.12.4
[android_sdk_v4.13.0]: https://github.com/adjust/android_sdk/tree/v4.13.0
[android_sdk_v4.14.0]: https://github.com/adjust/android_sdk/tree/v4.14.0
[android_sdk_v4.17.0]: https://github.com/adjust/android_sdk/tree/v4.17.0
[android_sdk_v4.18.0]: https://github.com/adjust/android_sdk/tree/v4.18.0
[android_sdk_v4.21.0]: https://github.com/adjust/android_sdk/tree/v4.21.0
[android_sdk_v4.21.1]: https://github.com/adjust/android_sdk/tree/v4.21.1
[android_sdk_v4.22.0]: https://github.com/adjust/android_sdk/tree/v4.22.0
[android_sdk_v4.24.1]: https://github.com/adjust/android_sdk/tree/v4.24.1
[android_sdk_v4.27.0]: https://github.com/adjust/android_sdk/tree/v4.27.0
[android_sdk_v4.28.7]: https://github.com/adjust/android_sdk/tree/v4.28.7
