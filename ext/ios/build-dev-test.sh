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
STATIC_FRAMEWORK=ext/ios/sdk-dev/Frameworks/Static/AdjustTestLibrary.framework
LIB_OUT_DIR=ios/Test/TestLib/Resources
PROJECT_DIR=ext/ios/sdk-dev/Adjust

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-TEST-IOS]:${GREEN} Removing old iOS SDK binary ... ${NC}"
cd ${ROOT_DIR}/ext/ios/sdk-dev
rm -rfv ${LIB_OUT_DIR}/libAdjustTest.a
echo -e "${CYAN}[ADJUST][BUILD-TEST-IOS]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-TEST-IOS]:${GREEN} Building new iOS SDK binary ... ${NC}"
cd ${ROOT_DIR}/ext/ios/sdk-dev/AdjustTests/AdjustTestLibrary
xcodebuild -target AdjustTestLibraryStatic -configuration Release clean build
echo -e "${CYAN}[ADJUST][BUILD-TEST-IOS]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-TEST-IOS]:${GREEN} Copying the generated Android SDK binary from ${STATIC_FRAMEWORK} to ${LIB_OUT_DIR} ... ${NC}"
cd ${ROOT_DIR}
cp -Rv ${STATIC_FRAMEWORK}/Versions/A/AdjustTestLibrary ${LIB_OUT_DIR}/libAdjustTest.a
echo -e "${CYAN}[ADJUST][BUILD-TEST-IOS]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-IOS]:${GREEN} Script completed! ${NC}"

# ======================================== #
