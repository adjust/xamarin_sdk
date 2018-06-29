#!/usr/bin/env bash

set -e

# ======================================== #

# Colors for output
NC='\033[0m'
RED='\033[0;31m'
CYAN='\033[1;36m'
GREEN='\033[0;32m'

# ======================================== #

# Script usage hint
if [ $# -ne 1 ]; then
	echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Please, pass either 'debug' or 'release' parameter to the script. ${NC}"
	echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Usage: ./build-sdk.sh [debug || release] ${NC}"
    exit 1
fi

BUILD_TYPE=$1

# ======================================== #

# Set directories of interest for the script
ROOT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
ROOT_DIR="$(dirname "$ROOT_DIR")"
ROOT_DIR="$(dirname "$ROOT_DIR")"
JAR_IN_DIR=ext/android/sdk-dev/Adjust/adjust/build/outputs
JAR_OUT_DIR=android/AdjustSdk.Xamarin.Android/Jars
PROJECT_DIR=ext/android/sdk-dev/Adjust
SDK_PREFIX='xamarin4.14.0'

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Appending SDK prefix to source code ... ${NC}"
cd ${ROOT_DIR}/${PROJECT_DIR}
grep -r 'adjustInstance.onCreate(adjustConfig)' -l --null . | LC_ALL=C xargs -0 sed -i '' "s#adjustInstance.onCreate(adjustConfig);#adjustConfig.setSdkPrefix(\"${SDK_PREFIX}\");adjustInstance.onCreate(adjustConfig);#g"
echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Starting Gradle tasks ... ${NC}"
cd ${ROOT_DIR}/${PROJECT_DIR}

if [ "$BUILD_TYPE" == "debug" ]; then
	echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Running Gradle tasks: clean makeDebugJar ... ${NC}"
    ./gradlew clean makeDebugJar
elif [ "$BUILD_TYPE" == "release" ]; then
	echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Running Gradle tasks: clean makeReleaseJar ... ${NC}"
    ./gradlew clean makeReleaseJar
fi
echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Removing SDK prefix from source code ... ${NC}"
cd ${ROOT_DIR}/${PROJECT_DIR}
grep -r 'adjustInstance.onCreate(adjustConfig)' -l --null . | LC_ALL=C xargs -0 sed -i '' "s#adjustConfig.setSdkPrefix(\"${SDK_PREFIX}\");adjustInstance.onCreate(adjustConfig);#adjustInstance.onCreate(adjustConfig);#g"
echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Moving the generated Android SDK JAR from ${JAR_IN_DIR} to ${JAR_OUT_DIR} ... ${NC}"
cd ${ROOT_DIR}
cp -v ${JAR_IN_DIR}/adjust-*.jar ${ROOT_DIR}/${JAR_OUT_DIR}/adjust-android.jar
echo -e "${CYAN}[ADJUST][BUILD-SDK-ANDROID]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-ANDROID]:${GREEN} Script completed! ${NC}"

# ======================================== #
