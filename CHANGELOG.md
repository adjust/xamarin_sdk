### Version 4.10.0 (xth September 2016)
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
- Updated native Android SDK to version **4.10.1**.


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
