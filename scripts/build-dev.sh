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
SCRIPTS_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
ROOT_DIR="$(dirname "$SCRIPTS_DIR")"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-DEV]:${GREEN} Running Android build script(s) ... ${NC}"
cd ${ROOT_DIR}
ext/android/build-dev-sdk.sh release
ext/android/build-dev-test.sh
echo -e "${CYAN}[ADJUST][BUILD-DEV]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-DEV]:${GREEN} Running iOS build script(s) ... ${NC}"
cd ${ROOT_DIR}
ext/ios/build-dev-sdk.sh
ext/ios/build-dev-test.sh
echo -e "${CYAN}[ADJUST][BUILD-DEV]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][BUILD-DEV]:${GREEN} Script completed! ${NC}"

# ======================================== #
