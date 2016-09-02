### Version 4.8.0 (xth September 2016)
#### Added
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

#### Changed
- Removed MAC MD5 tracking feature for iOS platform completely.
- Updated docs.
- Updated native iOS SDK to version **4.8.5**.
- Updated native Android SDK to version **4.7.0**.


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
