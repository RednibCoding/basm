@echo off
echo Building application...

REM Check if the build directory exists and remove it
if exist build rmdir /S /Q build

REM Create the build directory
mkdir build

REM Your custom build command here
REM Example placeholder command, replace it with your actual build command
bflat build -o ./build/basm.exe --no-reflection --no-stacktrace-data --no-globalization --no-exception-messages --no-pie --separate-symbols --no-debug-info

if %ERRORLEVEL% == 0 (
    echo Build completed successfully.
) else (
    echo Build failed with error code %ERRORLEVEL%.
)

