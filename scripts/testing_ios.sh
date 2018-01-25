#!/usr/bin/env bash

# Exit if any errors occur
set -e

# Get the current directory (/scripts/ directory)
SCRIPTS_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Traverse up to get to the root directory
ROOT_DIR="$(dirname "$SCRIPTS_DIR")"

RED='\033[0;31m' 	# Red color
GREEN='\033[0;32m' 	# Green color
NC='\033[0m' 		# No Color

echo -e "${GREEN}>>> Running iOS build script ${NC}"
cd ${ROOT_DIR}
iOS/ext/build.sh

echo -e "${GREEN}>>> Build successful. Run it from Visual Studio (platforms/ios/) ${NC}"
