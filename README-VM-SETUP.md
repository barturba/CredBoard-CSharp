# 🖥️ CredBoard Windows VM Setup for Apple Silicon

## 🎯 Overview

Since you're running on **Apple Silicon (M1/M2/M3)**, traditional x86_64 Windows VMs won't work. Here are your options for GUI testing:

## 🚀 Option 1: UTM (Recommended for Apple Silicon)

### Prerequisites:
- **macOS Monterey 12.0+** with Apple Silicon
- **UTM** (free virtualization app): https://mac.getutm.app/

### Steps:

1. **Install UTM:**
   ```bash
   brew install utm
   # OR download from: https://mac.getutm.app/
   ```

2. **Download Windows ARM64 ISO:**
   - Windows 11 ARM64: https://www.microsoft.com/en-us/software-download/windowsinsiderpreviewARM64
   - Or Windows 10 ARM64 from Microsoft

3. **Create VM in UTM:**
   - Open UTM
   - Click "+" → "Create a New Virtual Machine"
   - Select "Windows" → "Windows 11 (ARM64)"
   - Allocate: 4GB RAM, 64GB storage
   - Enable "UEFI Boot"
   - Mount the Windows ISO

4. **Install Windows:**
   - Start VM and follow Windows installation
   - Install Windows normally

5. **Setup CredBoard Testing:**
   ```bash
   # In your VM, open PowerShell and run:
   winget install Microsoft.DotNet.Runtime.8
   ```

6. **Transfer CredBoard:**
   ```bash
   # From macOS terminal:
   scp ~/Sync/CredBoard.exe user@vm-ip:/path/to/desktop/
   ```

## 🐳 Option 2: Windows Docker (Limited)

### For Basic Testing Only:
```bash
# This can only test console functionality
docker run --platform linux/amd64 -it mcr.microsoft.com/windows/servercore:ltsc2022
```

**❌ Cannot run GUI applications in Docker**

## ☁️ Option 3: GitHub Codespaces (Web-based)

### For Development Testing:
1. Push your code to GitHub
2. Use GitHub Codespaces with Windows environment
3. Test GUI in browser-based Windows VM

## 🖥️ Option 4: Parallels Desktop (Paid)

### Professional Solution:
1. **Install Parallels Desktop 18+**
2. **Download Windows ARM64 ISO**
3. **Create ARM64 Windows VM**
4. **Full Windows environment with GUI**

## 📋 Current Setup Files

I've created these files for VM testing:

- `Vagrantfile` - Traditional x86_64 setup (won't work on Apple Silicon)
- `setup-vm.sh` - Automated setup script
- `test-gui.ps1` - GUI testing automation script

## 🎯 Recommended Approach for Apple Silicon

### Step 1: Install UTM
```bash
brew install utm
```

### Step 2: Download Windows ARM64
- Visit: https://www.microsoft.com/en-us/software-download/windowsinsiderpreviewARM64
- Download the ARM64 ISO

### Step 3: Create VM
1. Open UTM
2. Create new VM → Windows → Windows 11 ARM64
3. Configure with 4GB RAM, 64GB storage
4. Mount the downloaded ISO

### Step 4: Install Windows
- Boot VM and install Windows normally
- This will take 15-30 minutes

### Step 5: Setup Testing Environment
```powershell
# In Windows VM PowerShell:
winget install Microsoft.DotNet.Runtime.8
New-Item -ItemType Directory -Path "C:\CredBoard-Test" -Force
```

### Step 6: Transfer Application
```bash
# From macOS:
scp ~/Sync/CredBoard.exe user@vm-ip:C:/CredBoard-Test/
```

### Step 7: Test GUI
1. In VM: Double-click `CredBoard.exe`
2. Test login screen
3. Test client management
4. Take screenshots

## 📸 Screenshot Instructions

### In Windows VM:
```powershell
# Take screenshot of current screen
Add-Type -AssemblyName System.Windows.Forms, System.Drawing
$bounds = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds
$bitmap = New-Object System.Drawing.Bitmap $bounds.Width, $bounds.Height
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)
$graphics.CopyFromScreen($bounds.X, $bounds.Y, 0, 0, $bounds.Size)
$bitmap.Save("C:\CredBoard-Test\screenshot.png", [System.Drawing.Imaging.ImageFormat]::Png)
```

### Or use built-in tools:
- **Windows Key + Shift + S** (Snip & Sketch)
- **Windows Key + Print Screen** (Save to Pictures)

## 🔧 Troubleshooting

### UTM Issues:
- Ensure macOS is updated to latest version
- Check UTM is version 4.0+
- Verify ARM64 Windows ISO compatibility

### Network Issues:
- Use bridged networking in UTM for better connectivity
- Configure Windows Firewall to allow file sharing

### Performance Issues:
- Allocate at least 4GB RAM to VM
- Use SSD storage for better performance
- Close unnecessary applications on host

## ✅ Success Criteria

When setup is complete, you should be able to:
- ✅ Launch CredBoard.exe in Windows VM
- ✅ See the login screen GUI
- ✅ Create/test master password
- ✅ Access dashboard with client management
- ✅ Add/edit/delete clients and credentials
- ✅ Take screenshots of all screens

## 🎉 Ready for GUI Testing!

Once your Windows ARM64 VM is set up with UTM, you'll have a full Windows environment capable of running and testing the CredBoard GUI application with screenshots!

**Pro Tip:** UTM provides excellent performance on Apple Silicon and is completely free! 🚀
