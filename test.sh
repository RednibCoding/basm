#!/bin/bash

echo "Running tests on all .bsm files..."

# Set the path to the examples directory
EXAMPLES_DIR="./examples"

# Check if the examples directory exists
if [ ! -d "$EXAMPLES_DIR" ]; then
    echo "The specified examples directory $EXAMPLES_DIR does not exist."
    exit 1
fi

# Iterate over all .bsm files in the examples directory
for file in "$EXAMPLES_DIR"/*.bsm; do
    echo "-------------------------------"
    echo "$(basename "$file")"
    # Execute basm with each .bsm file
    ./build/basm "$file"
    if [ $? -ne 0 ]; then
        echo "Test failed with error code $? for file $(basename "$file")."
        # Optional: uncomment to exit on first error
        # exit 1
    fi
done

echo
echo "All tests completed."
