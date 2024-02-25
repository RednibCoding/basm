#!/bin/bash

# Check if the build directory exists
if [ -d "./build" ]; then
    # Remove the build directory
    rm -rf ./build
fi

# Create the build directory
mkdir ./build

# Your custom build command here
echo "Building application..."
bflat build -o ./build/basm.exe --no-reflection --no-stacktrace-data --no-globalization --no-exception-messages --no-pie --separate-symbols --no-debug-info

if [ $? -eq 0 ]; then
    echo "Build completed successfully!"
else
    echo "Error during build. Check above for the error message."
    exit 1
fi
