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
JAR_IN_DIR=ext/android/sdk/Adjust/testlibrary/build/outputs
JAR_OUT_DIR=android/Test/TestLib/Jars
PROJECT_DIR=ext/android/sdk/Adjust

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-TEST-ANDROID]:${GREEN} Starting Gradle tasks ... ${NC}"
cd ${ROOT_DIR}/${PROJECT_DIR}

echo -e "${CYAN}[ADJUST][BUILD-TEST-ANDROID]:${GREEN} Running Gradle tasks: clean testlibrary:makeJar ... ${NC}"
./gradlew clean :testlibrary:makeJar
echo -e "${CYAN}[ADJUST][BUILD-TEST-ANDROID]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-TEST-ANDROID]:${GREEN} Moving the generated Android SDK JAR from ${JAR_IN_DIR} to ${JAR_OUT_DIR} ... ${NC}"
cd ${ROOT_DIR}
cp -v ${JAR_IN_DIR}/adjust-*.jar ${ROOT_DIR}/${JAR_OUT_DIR}/adjust-testing.jar
echo -e "${CYAN}[ADJUST][BUILD-TEST-ANDROID]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-TEST-ANDROID]:${GREEN} Script completed! ${NC}"

# ======================================== #
