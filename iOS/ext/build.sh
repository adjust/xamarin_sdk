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

RED='\033[0;31m' # Red color
GREEN='\033[0;32m' # Green color
NC='\033[0m' # No Color

echo -e "${GREEN}>>> Removing old framework ${NC}"
cd ${ROOT_DIR}/iOS/ext/sdk
rm -rfv ${LIB_OUT_DIR}/libAdjust.a

echo -e "${GREEN}>>> building new framework ${NC}"
cd ${ROOT_DIR}/iOS/ext/sdk
xcodebuild -target AdjustStatic -configuration Release clean build

echo -e "${GREEN}>>> Copy built framework to designated location ${NC}"
cd ${ROOT_DIR}
\cp -Rv ${STATIC_FRAMEWORK}/Versions/A/AdjustSdk ${LIB_OUT_DIR}/libAdjust.a
