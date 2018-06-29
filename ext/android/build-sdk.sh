#!/usr/bin/env bash

# - Build the JAR file
# - Copy the JAR file to the root dir

# End script if one of the lines fails
set -e

if [ $# -ne 1 ]; then
    echo $0: "usage: ./build.sh [debug || release]"
    exit 1
fi

BUILD_TYPE=$1

# Get the current directory (Android/ext/)
ROOT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Traverse up to get to the root directory
ROOT_DIR="$(dirname "$ROOT_DIR")"
ROOT_DIR="$(dirname "$ROOT_DIR")"
JAR_IN_DIR=ext/android/sdk/Adjust/adjust/build/outputs
JAR_OUT_DIR=android/AdjustSdk.Xamarin.Android/Jars
PROJECT_DIR=ext/android/sdk/Adjust

SDK_PREFIX='xamarin4.14.0'

RED='\033[0;31m' # Red color
GREEN='\033[0;32m' # Green color
NC='\033[0m' # No Color

echo -e "${GREEN}>>> Appending prefix to source ${NC}"
cd ${ROOT_DIR}/${PROJECT_DIR}
grep -r 'adjustInstance.onCreate(adjustConfig)' -l --null . | LC_ALL=C xargs -0 sed -i '' "s#adjustInstance.onCreate(adjustConfig);#adjustConfig.setSdkPrefix(\"${SDK_PREFIX}\");adjustInstance.onCreate(adjustConfig);#g"
echo success

echo -e "${GREEN}>>> Running Gradle ${NC}"
cd ${ROOT_DIR}/${PROJECT_DIR}

if [ "$BUILD_TYPE" == "debug" ]; then
    echo -e "${GREEN}>>> Running Gradle tasks: makeDebugJar${NC}"
    ./gradlew clean makeDebugJar

elif [ "$BUILD_TYPE" == "release" ]; then
    echo -e "${GREEN}>>> Running Gradle tasks: makeReleaseJar${NC}"
    ./gradlew clean makeReleaseJar
fi
echo success

echo -e "${GREEN}>>> Removing prefix from source ${NC}"
cd ${ROOT_DIR}/${PROJECT_DIR}
grep -r 'adjustInstance.onCreate(adjustConfig)' -l --null . | LC_ALL=C xargs -0 sed -i '' "s#adjustConfig.setSdkPrefix(\"${SDK_PREFIX}\");adjustInstance.onCreate(adjustConfig);#adjustInstance.onCreate(adjustConfig);#g"
echo success

echo -e "${GREEN}>>> Moving the jar from ${JAR_IN_DIR} to ${JAR_OUT_DIR} ${NC}"
cd ${ROOT_DIR}
cp -v ${JAR_IN_DIR}/adjust-*.jar ${ROOT_DIR}/${JAR_OUT_DIR}/adjust-android.jar
echo success
