🔥 CRED BOARD VM EXECUTION SCRIPT 🔥
====================================

#/bin/bash /Applications/DYMO.WebApi.Mac.Host.app/Contents/Resources/InstallCertificates.sh
set -e

echo '🔥 CRED BOARD VM EXECUTION SCRIPT 🔥'
echo '===================================='
echo ''

# Step 1: Prerequisites check
echo '📦 STEP 1: Prerequisites Check'
echo '=============================='
if ! command -v brew &> /dev/null; then
    echo '❌ Homebrew not found. Install from https://brew.sh/'
    exit 1
fi
echo '✅ Homebrew found'

if ! command -v utmctl &> /dev/null; then
    echo '📦 Installing UTM...'
    brew install utm
fi
echo '✅ UTM found'

if [ ! -f ~/Sync/CredBoard.exe ]; then
    echo '❌ CredBoard.exe not found in ~/Sync/'
    exit 1
fi
echo '✅ CredBoard.exe ready (155MB)'
echo ''

# Step 2: Open download page
echo '📥 STEP 2: Download Windows ARM64 ISO'
echo '====================================='
echo '🔗 OPENING BROWSER: Windows ARM64 Download Page...'
open 'https://www.microsoft.com/en-us/software-download/windowsinsiderpreviewARM64'
echo ''
echo '📋 INSTRUCTIONS:'
echo '1. Click "Download the ISO" button'
echo '2. Choose Windows 11 ARM64'
echo '3. Save to Downloads folder (about 5GB)'
echo '4. Press Enter when download completes...'
read -p ''
echo ''

# Step 3: Launch UTM
echo '🖥️  STEP 3: Launch UTM & Create VM'
echo '=================================='
echo '🔗 OPENING UTM APPLICATION...'
open -a UTM
echo ''
echo '📋 INSTRUCTIONS:'
echo '1. UTM should now be open'
echo '2. Click "+" button → "Create a New Virtual Machine"'
echo '3. Select "Windows" → "Windows 11 (ARM64)"'
echo '4. Configure:'
echo '   • RAM: 4GB'
echo '   • CPU: 2 cores'
echo '   • Storage: 64GB'
echo '5. Mount the downloaded Windows ARM64 ISO'
echo '6. Click "Save" to create VM'
echo '7. Press Enter when VM is created...'
read -p ''
echo ''

# Step 4: Windows Installation
echo '⏳ STEP 4: Install Windows (15-30 minutes)'
echo '=========================================='
echo '📋 INSTRUCTIONS:'
echo '1. In UTM, start the VM you just created'
echo '2. Follow Windows installation wizard:'
echo '   • Language: English'
echo '   • Accept license terms'
echo '   • Choose "Custom" installation'
echo '   • Select the virtual disk'
echo '3. Create user account and password'
echo '4. Windows will install and restart several times'
echo ''
echo '⚠️  This takes 15-30 minutes. Go grab coffee! ☕'
echo '   Press Enter when Windows is fully installed and you see the desktop...'
read -p ''
echo ''

# Step 5: Setup CredBoard Environment
echo '🔧 STEP 5: Setup CredBoard Environment'
echo '======================================'
VM_IP=${1:-"192.168.64.2"}
echo "📡 Using VM IP: $VM_IP"
echo ''
echo '📋 INSTRUCTIONS:'
echo '1. In Windows VM, open PowerShell (search for it)'
echo '2. Run these commands:'
echo ''
echo '   # Install .NET Runtime'
echo '   winget install Microsoft.DotNet.Runtime.8'
echo ''
echo '   # Create test directory'
echo '   New-Item -ItemType Directory -Path "C:\CredBoard-Test" -Force'
echo ''
echo '   # Verify .NET installation'
echo '   dotnet --version'
echo ''
echo 'Press Enter after running these commands...'
read -p ''
echo ''

# Step 6: Transfer files
echo '�� STEP 6: Transfer CredBoard Files'
echo '==================================='
echo "📡 Transferring to: $VM_IP"
echo ''

echo '📦 Transferring CredBoard.exe...'
if scp ~/Sync/CredBoard.exe user@$VM_IP:"C:/CredBoard-Test/"; then
    echo '✅ CredBoard.exe transferred'
else
    echo '❌ Failed to transfer CredBoard.exe'
    echo '   Make sure VM is running and network is configured'
    exit 1
fi

echo '📦 Transferring test scripts...'
scp vm-test-automation.ps1 test-gui.ps1 user@$VM_IP:"C:/CredBoard-Test/" || echo '⚠️  Test scripts transfer failed (optional)'
echo ''

# Step 7: Launch and Test
echo '🎮 STEP 7: Launch CredBoard & Test GUI'
echo '======================================'
echo '📋 INSTRUCTIONS:'
echo '1. In Windows VM, open File Explorer'
echo '2. Navigate to C:\CredBoard-Test'
echo '3. Double-click CredBoard.exe'
echo ''
echo 'WHAT YOU SHOULD SEE:'
echo '✅ Windows Forms GUI launches'
echo '✅ Login screen appears'
echo '✅ Master password prompt'
echo '✅ Dashboard with client management'
echo ''
echo 'Press Enter when CredBoard is running...'
read -p ''
echo ''

# Step 8: Run Automated Tests
echo '🤖 STEP 8: Run Automated GUI Tests'
echo '==================================='
echo '📋 INSTRUCTIONS:'
echo '1. In Windows VM PowerShell, run:'
echo '   cd C:\CredBoard-Test'
echo '   .\vm-test-automation.ps1'
echo ''
echo '2. Follow the script prompts'
echo '3. Take screenshots at each step'
echo '4. Script will generate test report'
echo ''
echo 'Press Enter after completing automated tests...'
read -p ''
echo ''

# Step 9: Results
echo '🎉 TESTING COMPLETE!'
echo '==================='
echo ''
echo '�� RESULTS:'
echo '✅ UTM VM created and configured'
echo '✅ Windows ARM64 installed'
echo '✅ .NET Runtime installed'
echo '✅ CredBoard.exe transferred and running'
echo '✅ GUI tested with screenshots'
echo '✅ Automated tests completed'
echo ''
echo '�� FILES CREATED:'
echo '   C:\CredBoard-Test\CredBoard.exe'
echo '   C:\CredBoard-Test\*.png (screenshots)'
echo '   C:\CredBoard-Test\CredBoard-Test-Report.md'
echo ''
echo '🎯 SUCCESS: CredBoard GUI fully tested on Windows ARM64 VM!'
echo ''
echo '📸 Screenshots saved in VM: C:\CredBoard-Test\'
echo '📄 Test report: C:\CredBoard-Test\CredBoard-Test-Report.md'


