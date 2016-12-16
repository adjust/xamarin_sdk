#!/usr/bin/env bash

set -e

MVNDIR=./sdk/Adjust
JARINDIR=./sdk/Adjust/target
JAROUTDIR=../AdjustSdk.Xamarin.Android/Jars

(cd $MVNDIR; mvn clean)
(cd $MVNDIR; mvn package)

rm -v -f $JAROUTDIR/adjust-android*; \
cp -v $JARINDIR/adjust-android-*.*.*.jar $JAROUTDIR; \
rm -v -f $JAROUTDIR/*-javadoc.jar; \
rm -v -f $JAROUTDIR/*-sources.jar; \
mv -v $JAROUTDIR/adjust-android-*.*.*.jar $JAROUTDIR/adjust-android.jar