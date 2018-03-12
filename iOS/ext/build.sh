#!/usr/bin/env bash

# End script if one of the lines fails
set -e

# Get the current directory (iOS/ext)
ROOT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
# Traverse up to get to the root directory
ROOT_DIR="$(dirname "$ROOT_DIR")"
ROOT_DIR="$(dirname "$ROOT_DIR")"

STATIC_FRAMEWORK=iOS/ext/sdk/Frameworks/Static/AdjustSdk.framework
LIB_OUT_DIR=iOS/AdjustSdk.Xamarin.iOS/Resources
PROJECT_DIR=iOS/ext/sdk/Adjust

SDK_PREFIX='xamarin4.12.0'

RED='\033[0;31m' # Red color
GREEN='\033[0;32m' # Green color
NC='\033[0m' # No Color

echo -e "${GREEN}>>> Removing old framework ${NC}"
cd ${ROOT_DIR}/iOS/ext/sdk
rm -rfv ${LIB_OUT_DIR}/libAdjust.a
echo success

echo -e "${GREEN}>>> Appending prefix to source ${NC}"
cd ${ROOT_DIR}/${PROJECT_DIR}
grep -r 'activityHandlerWithConfig:adjustConfig' -l --null . | LC_ALL=C xargs -0 sed -i '' "s#self.activityHandler = \[ADJAdjustFactory activityHandlerWithConfig:adjustConfig#[adjustConfig setSdkPrefix:@\"${SDK_PREFIX}\"];self.activityHandler = [ADJAdjustFactory activityHandlerWithConfig:adjustConfig#g"
echo success

echo -e "${GREEN}>>> building new framework ${NC}"
cd ${ROOT_DIR}/iOS/ext/sdk
xcodebuild -target AdjustStatic -configuration Release clean build
echo success

echo -e "${GREEN}>>> Removing prefix from source ${NC}"
cd ${ROOT_DIR}/${PROJECT_DIR}
grep -r 'activityHandlerWithConfig:adjustConfig' -l --null . | LC_ALL=C xargs -0 sed -i '' "s#\[adjustConfig setSdkPrefix:@\".*\"\];self.activityHandler = \[ADJAdjustFactory activityHandlerWithConfig:adjustConfig#self.activityHandler = \[ADJAdjustFactory activityHandlerWithConfig:adjustConfig#g"
echo success

echo -e "${GREEN}>>> Copy built framework to designated location ${NC}"
cd ${ROOT_DIR}
\cp -Rv ${STATIC_FRAMEWORK}/Versions/A/AdjustSdk ${LIB_OUT_DIR}/libAdjust.a
echo success
