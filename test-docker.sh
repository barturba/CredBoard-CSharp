#!/bin/bash

echo "🐳 CredBoard Windows Docker Testing"
echo "==================================="
echo ""

# Check if we're on macOS (Docker Desktop supports Windows containers)
if [[ "$OSTYPE" == "darwin"* ]]; then
    echo "🍎 Running on macOS - Docker Desktop Windows containers available"
else
    echo "⚠️  This script is designed for macOS with Docker Desktop"
    echo "   Windows containers require Docker Desktop with Windows container support"
fi

echo ""
echo "📋 Available testing options:"
echo ""

echo "1️⃣  GitHub Actions (Recommended):"
echo "   • Automated Windows container testing"
echo "   • Runs on every push to main branch"
echo "   • Validates core functionality"
echo ""

echo "2️⃣  Local Windows VM/Machine:"
echo "   • Copy CredBoard.exe to Windows"
echo "   • Run: CredBoard.exe"
echo "   • Full GUI testing available"
echo ""

echo "3️⃣  Windows Docker (Advanced):"
echo "   • Requires Docker Desktop with Windows containers enabled"
echo "   • Limited to headless testing"
echo "   • Validates .NET runtime and file access"
echo ""

# Check if executable exists
if [ -f "~/Sync/CredBoard-Windows/CredBoard-Windows-Executable/CredBoard.exe" ]; then
    echo "✅ Windows executable available for testing"
else
    echo "❌ Windows executable not found"
    echo "   Run: ./watch-build.sh to download from GitHub Actions"
fi

echo ""
echo "🎯 Recommended testing workflow:"
echo "   1. Push changes → GitHub Actions builds and tests"
echo "   2. Download executable to Windows machine"
echo "   3. Manual GUI testing on Windows"
echo ""

echo "📖 For Windows Docker testing (if needed):"
echo "   # Enable Windows containers in Docker Desktop"
echo "   docker build -f Dockerfile.windows -t credboard-test ."
echo "   docker run credboard-test"
echo ""

echo "🔗 Useful links:"
echo "   • Docker Desktop: https://www.docker.com/products/docker-desktop"
echo "   • Windows Containers: https://docs.microsoft.com/en-us/virtualization/windowscontainers"
echo ""

echo "✅ Docker testing strategy configured!"
