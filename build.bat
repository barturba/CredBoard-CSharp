@echo off
echo Building CredBoard C# Application...
echo.

REM Build in Release mode
dotnet publish -c Release --self-contained -r win-x64 -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

echo.
echo Build complete! Check the 'bin\Release\net8.0-windows\win-x64\publish\' directory for the executable.
echo.

pause
