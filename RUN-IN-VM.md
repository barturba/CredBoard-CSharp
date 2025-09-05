# 🚀 RUN CREDBOARD IN VM - STEP BY STEP

## **YOU NEED TO DO THIS YOURSELF:**

### **1. Install UTM (if not already done):**
```bash
brew install utm
```

### **2. Download Windows ARM64 ISO:**
- Open: https://www.microsoft.com/en-us/software-download/windowsinsiderpreviewARM64
- Download: Windows 11 ARM64 ISO

### **3. Create VM in UTM:**
1. Open UTM app
2. Click '+' → 'Create a New Virtual Machine'
3. Select 'Windows' → 'Windows 11 (ARM64)'
4. Configure: 4GB RAM, 2 CPUs, 64GB storage
5. Mount the downloaded ISO

### **4. Install Windows:**
1. Start VM
2. Follow Windows installation (15-30 minutes)
3. Set up user account

### **5. Setup CredBoard:**
```powershell
# In Windows VM PowerShell:
winget install Microsoft.DotNet.Runtime.8
New-Item -ItemType Directory -Path "C:\CredBoard-Test" -Force
```

### **6. Transfer CredBoard:**
```bash
# From your Mac (replace VM-IP with actual IP):
scp ~/Sync/CredBoard.exe user@VM-IP:C:/CredBoard-Test/
```

### **7. RUN CREDBOARD:**
```powershell
# In Windows VM:
Start-Process "C:\CredBoard-Test\CredBoard.exe"
```

## **WHAT YOU'LL SEE:**
1. **Login Screen** - Master password prompt
2. **Dashboard** - Client management interface  
3. **Modal Dialogs** - Add/edit clients
4. **Full GUI** - Working Windows Forms app

## **TAKE SCREENSHOTS:**
- Win + Shift + S (Snip & Sketch)
- Or use the test script:
```powershell
& "C:\CredBoard-Test	est-gui.ps1"
```

## **EXECUTABLE LOCATION:**
~/Sync/CredBoard.exe (155MB, ready to transfer)


