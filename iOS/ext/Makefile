SRCDIR = ./sdk
LIBOUTDIR = ../AdjustSdk.Xamarin.iOS/Resources
STATICFRAMEWORK = $(SRCDIR)/Frameworks/Static/AdjustSdk.framework

default: copy

build:
	cd $(SRCDIR); \
	xcodebuild -target AdjustStatic -configuration Release

copy: build
	cp $(STATICFRAMEWORK)/Versions/A/AdjustSdk $(LIBOUTDIR)/libAdjust.a;
