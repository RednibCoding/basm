@echo off
echo Running tests on all .bsm files...

REM Set the path to the examples directory
set "EXAMPLES_DIR=./examples"

REM Check if the directory exists
if not exist "%EXAMPLES_DIR%" (
    echo The specified examples directory "%EXAMPLES_DIR%" does not exist.
    exit /b 1
)

REM Iterate over all .bsm files in the examples directory
for %%f in ("%EXAMPLES_DIR%\*.bsm") do (
    echo.
    echo -------------------------------
    echo %%~nxf
    REM Execute the basm.exe with each .bsm file
    build\basm.exe "%%f"
    if %ERRORLEVEL% neq 0 (
        echo Test failed with error code %ERRORLEVEL% for file %%~nxf.
        REM Optional: exit on first error
        REM exit /b %ERRORLEVEL%
    )
)

echo.
echo All tests completed.
