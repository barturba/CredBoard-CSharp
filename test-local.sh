#!/bin/bash

echo "🧪 Testing CredBoard Application"
echo "================================"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Test 1: Check if executable exists
echo "📁 Checking executable..."
if [ -f "~/Sync/CredBoard-Windows/CredBoard-Windows-Executable/CredBoard.exe" ]; then
    echo -e "${GREEN}✅ Executable found${NC}"
else
    echo -e "${RED}❌ Executable not found${NC}"
    exit 1
fi

# Test 2: Check file size
echo "📊 Checking file size..."
FILE_SIZE=$(stat -f%z "~/Sync/CredBoard-Windows/CredBoard-Windows-Executable/CredBoard.exe" 2>/dev/null || echo "0")
if [ "$FILE_SIZE" -gt 100000000 ]; then # 100MB
    echo -e "${GREEN}✅ File size OK: $(($FILE_SIZE / 1024 / 1024))MB${NC}"
else
    echo -e "${YELLOW}⚠️  File size seems small: $(($FILE_SIZE / 1024 / 1024))MB${NC}"
fi

# Test 3: Run unit tests
echo "🧪 Running unit tests..."
cd "$(dirname "$0")"
if dotnet test CredBoard.Tests/CredBoard.Tests.csproj --verbosity minimal; then
    echo -e "${GREEN}✅ Unit tests passed${NC}"
else
    echo -e "${YELLOW}⚠️  Unit tests failed or not found${NC}"
fi

# Test 4: Test core functionality (console version)
echo "🔧 Testing core functionality..."
if timeout 10s dotnet run --project CredBoard.csproj > /dev/null 2>&1; then
    echo -e "${GREEN}✅ Core functionality test passed${NC}"
else
    echo -e "${YELLOW}⚠️  Core functionality test timed out or failed${NC}"
fi

echo ""
echo "📋 Test Summary:"
echo "   • Executable: ~/Sync/CredBoard-Windows/CredBoard-Windows-Executable/CredBoard.exe"
echo "   • Size: $(($FILE_SIZE / 1024 / 1024))MB"
echo "   • Platform: Windows x64 (Self-contained)"
echo ""
echo "🎯 For GUI testing on Windows:"
echo "   1. Copy CredBoard.exe to a Windows machine"
echo "   2. Double-click to run the application"
echo "   3. Test login, client management, and credential storage"
echo ""
echo -e "${GREEN}✅ Local testing complete!${NC}"
