### Version 4.11.0 (5th January 2017)
#### Added
- Added `Adid` property to the attribution callback response.
- Added property `Adjust.Adid` to be able to get adid value at any time after obtaining it, not only when session/event callbacks have been triggered.
- Added property `Adjust.Attribution` to be able to get current attribution value at any time after obtaining it, not only when attribution callback has been triggered.
- Added sending of **Amazon Fire Advertising Identifier** for Android platform.
- Added possibility to set default tracker for the app by adding `adjust_config.properties` file to the `assets` folder of your app. Mostly meant to be used by the `Adjust Store & Pre-install Tracker Tool` (https://github.com/adjust/android_sdk/blob/master/doc/english/pre_install_tracker_tool.md).

#### Fixed
- Now reading push token value from activity state file when sending package.
- Fixed memory leak by closing network session for iOS platform.
- Fixed TARGET_OS_TV pre processer check for iOS platform.

#### Changed
- Firing attribution request as soon as install has been tracked, regardless of presence of attribution callback implementation in user's app.
- Saving iAd/AdSearch details to prevent sending duplicated `sdk_click` packages for iOS platform.
- Updated docs.
- Updated native iOS SDK to version **4.11.0**.
- Native SDKs stability updates and improvements.

---

### Version 4.10.1 (15th December 2016)
#### Fixed
- Deferred deep link arrival to the app is no longer dependent from implementation of the attribution callback.

#### Changed
- Updated native iOS SDK to version **4.10.3**.
- Updated native Android SDK to version **4.11.0**.
- Native SDKs stability updates and improvements.

---

### Version 4.10.0 (20th September 2016)
#### Added
- Added support for iOS 10.
- Added support for `Xamarin Studio 6`.
- Added support for iOS iAd v3.
- Added `Bitcode` support for iOS framework.
- Added a method for getting `IDFA` on iOS device.
- Added a method for getting `Google Play Services Ad Id` on Android device.
- Added an option for enabling/disabling tracking while app is in background.
- Added a callback to be triggered if event is successfully tracked.
- Added a callback callback to be triggered if event tracking failed.
- Added a callback to be triggered if session is successfully tracked.
- Added a callback to be triggered if session tracking failed.
- Added a callback to be triggered when deferred deeplink is received.
- Added revenue deduplication for Android platform.
- Added Changelog to the repository.
- Added possibility to set session callback and partner parameters on `Adjust` instance with `AddSessionCallbackParameter` and `AddSessionPartnerParameter` methods.
- Added possibility to remove session callback and partner parameters by key on `Adjust` instance with `RemoveSessionCallbackParameter` and `RemoveSessionPartnerParameter` methods.
- Added possibility to remove all session callback and partner parameters on `Adjust` instance with `ResetSessionCallbackParameters` and `ResetSessionPartnerParameters` methods.
- Added new `Suppress` log level and for it new adjust config object constructor which gets `bool` indicating whether suppress log level should be supported or not.
- Added possibility to delay initialisation of the SDK while maybe waiting to obtain some session callback or partner parameters with `delayed start` feature on adjust config instance.
- Added possibility to set user agent manually on adjust config instance.

#### Changed
- Deferred deep link info will now arrive as part of the attribution response and not as part of the answer to first session.
- Removed MAC MD5 tracking feature for iOS platform completely.
- Updated docs.
- Native SDKs stability updates and improvements.
- Updated native iOS SDK to version **4.10.1**.
- Updated native Android SDK to version **4.10.2**.


#### Fixed
- Fixed issue with `Xamarin Studio 6` which caused attribution callback not to get triggered.

---

### Version 4.0.2 (4th November 2015)
#### Changed
- Moved `ADJLogLevel` enum to `StructsAndEnums.cs` file.

---

### Version 4.0.1 (2nd November 2015)
#### Added
- Added support for iOS 9.
- Adding `iAd.framework` and `AdSupport.framework` automatically.

#### Changed
- Updated native iOS SDK to version **4.4.1**.
- Updated native Android SDK to version **4.1.3**.

---

### Version 4.0.0 (12th May 2015)
#### Added
- Initial release of the adjust SDK for Xamarin.
- Added support for iOS and Android platforms.
- Added native iOS SDK version **4.2.4**.
- Added native Android SDK version **4.0.6**.
