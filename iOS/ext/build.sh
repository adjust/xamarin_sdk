#!/usr/bin/env bash

set -e

SRCDIR=./sdk
LIBOUTDIR=../AdjustSdk.Xamarin.iOS/Resources
STATICFRAMEWORK=$SRCDIR/Frameworks/Static/AdjustSdk.framework

(cd $SRCDIR; xcodebuild -target AdjustStatic -configuration Release clean build)
cp -v $STATICFRAMEWORK/Versions/A/AdjustSdk $LIBOUTDIR/libAdjust.a