#!/usr/bin/env bash

set -e

# ======================================== #

# Colors for output
NC='\033[0m'
RED='\033[0;31m'
CYAN='\033[1;36m'
GREEN='\033[0;32m'

# ======================================== #

# Set directories of interest for the script
ROOT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
ROOT_DIR="$(dirname "$ROOT_DIR")"
ROOT_DIR="$(dirname "$ROOT_DIR")"
STATIC_FRAMEWORK=ext/ios/sdk/Frameworks/Static/AdjustSdk.framework
LIB_OUT_DIR=ios/AdjustSdk.Xamarin.iOS/Resources
PROJECT_DIR=ext/ios/sdk/Adjust
SDK_PREFIX='xamarin4.14.0'

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-SDK-IOS]:${GREEN} Removing old iOS SDK binary ... ${NC}"
cd ${ROOT_DIR}/ext/ios/sdk
rm -rfv ${LIB_OUT_DIR}/libAdjust.a
echo -e "${CYAN}[ADJUST][BUILD-SDK-IOS]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-SDK-IOS]:${GREEN} Appending SDK prefix to source code ... ${NC}"
cd ${ROOT_DIR}/${PROJECT_DIR}
grep -r 'activityHandlerWithConfig:adjustConfig' -l --null . | LC_ALL=C xargs -0 sed -i '' "s#self.activityHandler = \[ADJAdjustFactory activityHandlerWithConfig:adjustConfig#[adjustConfig setSdkPrefix:@\"${SDK_PREFIX}\"];self.activityHandler = [ADJAdjustFactory activityHandlerWithConfig:adjustConfig#g"
echo -e "${CYAN}[ADJUST][BUILD-SDK-IOS]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-SDK-IOS]:${GREEN} Building new iOS SDK binary ... ${NC}"
cd ${ROOT_DIR}/ext/ios/sdk
xcodebuild -target AdjustStatic -configuration Release clean build
echo -e "${CYAN}[ADJUST][BUILD-SDK-IOS]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-SDK-IOS]:${GREEN} Removing SDK prefix from source code ... ${NC}"
cd ${ROOT_DIR}/${PROJECT_DIR}
grep -r 'activityHandlerWithConfig:adjustConfig' -l --null . | LC_ALL=C xargs -0 sed -i '' "s#\[adjustConfig setSdkPrefix:@\".*\"\];self.activityHandler = \[ADJAdjustFactory activityHandlerWithConfig:adjustConfig#self.activityHandler = \[ADJAdjustFactory activityHandlerWithConfig:adjustConfig#g"
echo -e "${CYAN}[ADJUST][BUILD-SDK-IOS]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${GREEN}>>> Copy built framework to designated location ${NC}"
echo -e "${CYAN}[ADJUST][BUILD-SDK-IOS]:${GREEN} Copying the generated Android SDK binary from ${STATIC_FRAMEWORK} to ${LIB_OUT_DIR} ... ${NC}"
cd ${ROOT_DIR}
cp -Rv ${STATIC_FRAMEWORK}/Versions/A/AdjustSdk ${LIB_OUT_DIR}/libAdjust.a
echo -e "${CYAN}[ADJUST][BUILD-SDK-IOS]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-IOS]:${GREEN} Script completed! ${NC}"

# ======================================== #
