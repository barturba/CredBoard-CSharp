#!/bin/bash

echo "🖥️  CredBoard Windows VM Setup for GUI Testing"
echo "=============================================="

# Detect Apple Silicon
if [[ "$(uname -m)" == "arm64" ]]; then
    echo "🍎 Apple Silicon (M1/M2/M3) detected!"
    echo "   Traditional Vagrant + VirtualBox won't work on Apple Silicon."
    echo ""
    echo "📖 Please see: README-VM-SETUP.md for Apple Silicon setup instructions"
    echo "   Recommended: Use UTM for Windows ARM64 VM"
    echo ""
    echo "🔗 Quick Start:"
    echo "   1. brew install utm"
    echo "   2. Download Windows 11 ARM64 ISO"
    echo "   3. Create VM in UTM"
    echo "   4. Copy CredBoard.exe to VM"
    echo "   5. Test GUI application"
    echo ""
    exit 0
fi

# Check if Vagrant is installed
if ! command -v vagrant &> /dev/null; then
    echo "❌ Vagrant not found. Installing..."
    echo ""
    echo "📦 Please install Vagrant first:"
    echo "   • macOS: brew install vagrant"
    echo "   • Ubuntu: sudo apt install vagrant"
    echo "   • Windows: Download from https://www.vagrantup.com/"
    echo ""
    echo "Also install VirtualBox:"
    echo "   • macOS: brew install virtualbox"
    echo "   • Ubuntu: sudo apt install virtualbox"
    echo "   • Windows: Download from https://www.virtualbox.org/"
    exit 1
fi

# Check if VirtualBox is installed
if ! command -v vboxmanage &> /dev/null; then
    echo "❌ VirtualBox not found. Please install VirtualBox first."
    exit 1
fi

echo "✅ Prerequisites check passed"

# Copy the executable to current directory
if [ -f "~/Sync/CredBoard.exe" ]; then
    echo "📋 Copying CredBoard.exe to current directory..."
    cp ~/Sync/CredBoard.exe ./CredBoard.exe
    echo "✅ Executable copied"
else
    echo "⚠️  CredBoard.exe not found in ~/Sync/"
    echo "   Make sure to run: cp ~/Sync/CredBoard.exe ./CredBoard.exe"
fi

echo ""
echo "🚀 Starting CredBoard Windows VM..."
echo "===================================="
echo ""
echo "⚠️  This will:"
echo "   • Download Windows Server 2022 (several GB)"
echo "   • Create a VM with 4GB RAM and GUI support"
echo "   • Set up .NET runtime and test environment"
echo "   • May take 10-20 minutes to complete"
echo ""
read -p "Continue? (y/N): " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo "❌ Setup cancelled"
    exit 1
fi

echo ""
echo "🏗️  Initializing Vagrant VM..."
vagrant up

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ VM Setup Complete!"
    echo "==================="
    echo ""
    echo "🎯 VM Access:"
    echo "   • Connect: vagrant ssh"
    echo "   • Desktop: VM will show GUI window"
    echo "   • Test Script: C:\\CredBoard-Test\\test-gui.ps1"
    echo ""
    echo "🖱️  GUI Testing:"
    echo "   1. Look for 'CredBoard Test' shortcut on desktop"
    echo "   2. Double-click to launch application"
    echo "   3. Test login, client management, credentials"
    echo "   4. Take screenshots of the interface"
    echo ""
    echo "🛑 To stop VM: vagrant halt"
    echo "🗑️  To destroy VM: vagrant destroy"
    echo ""
    echo "📸 Screenshots will be saved to: C:\\CredBoard-Test\\"
else
    echo ""
    echo "❌ VM Setup Failed"
    echo "=================="
    echo ""
    echo "🔧 Troubleshooting:"
    echo "   • Check VirtualBox is running"
    echo "   • Ensure virtualization is enabled in BIOS"
    echo "   • Try: vagrant destroy && vagrant up"
    echo "   • Check VM console for detailed errors"
fi
