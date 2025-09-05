# CredBoard GUI Testing Script for Windows VM
Write-Host "🖥️  CredBoard GUI Testing in Windows VM" -ForegroundColor Magenta
Write-Host "=========================================" -ForegroundColor Magenta

# Function to take screenshot
function Take-Screenshot {
    param([string]$filename)
    Add-Type -AssemblyName System.Windows.Forms
    Add-Type -AssemblyName System.Drawing

    $bounds = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds
    $bitmap = New-Object System.Drawing.Bitmap $bounds.Width, $bounds.Height
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.CopyFromScreen($bounds.X, $bounds.Y, 0, 0, $bounds.Size)
    $bitmap.Save("$PSScriptRoot\$filename.png", [System.Drawing.Imaging.ImageFormat]::Png)
    $bitmap.Dispose()
    $graphics.Dispose()

    Write-Host "📸 Screenshot saved: $filename.png" -ForegroundColor Green
}

# Check environment
Write-Host "🔍 Checking Environment..." -ForegroundColor Cyan
$dotnetVersion = & dotnet --version 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ .NET Runtime: $dotnetVersion" -ForegroundColor Green
} else {
    Write-Host "❌ .NET Runtime not found" -ForegroundColor Red
}

if (Test-Path "CredBoard.exe") {
    $fileInfo = Get-Item "CredBoard.exe"
    Write-Host "✅ CredBoard.exe found: $([math]::Round($fileInfo.Length / 1MB, 2)) MB" -ForegroundColor Green
} else {
    Write-Host "❌ CredBoard.exe not found" -ForegroundColor Red
    Write-Host "   Please ensure CredBoard.exe is in the current directory" -ForegroundColor Red
    exit 1
}

# Take initial screenshot
Write-Host ""
Write-Host "📸 Taking initial screenshot..." -ForegroundColor Yellow
Take-Screenshot "vm-desktop-initial"

# Display instructions
Write-Host ""
Write-Host "🎯 GUI Testing Instructions:" -ForegroundColor Green
Write-Host "==========================" -ForegroundColor Green
Write-Host ""
Write-Host "1️⃣  Launch Application:" -ForegroundColor Cyan
Write-Host "   • Double-click CredBoard.exe"
Write-Host "   • Or use desktop shortcut 'CredBoard Test'"
Write-Host ""
Write-Host "2️⃣  Test Login Screen:" -ForegroundColor Cyan
Write-Host "   • Should see master password prompt"
Write-Host "   • Take screenshot of login screen"
Write-Host ""
Write-Host "3️⃣  Test Setup Mode:" -ForegroundColor Cyan
Write-Host "   • Click 'Setup Password' button"
Write-Host "   • Enter a test password"
Write-Host "   • Confirm password"
Write-Host ""
Write-Host "4️⃣  Test Dashboard:" -ForegroundColor Cyan
Write-Host "   • Should see main application window"
Write-Host "   • Test client management features"
Write-Host "   • Take screenshot of dashboard"
Write-Host ""
Write-Host "5️⃣  Test Modal Dialogs:" -ForegroundColor Cyan
Write-Host "   • Click 'Add Client' button"
Write-Host "   • Fill out client information"
Write-Host "   • Test credential management"
Write-Host ""

# Automated screenshot taking (if application is launched)
Write-Host "🔄 Automated Screenshot Capture:" -ForegroundColor Yellow
Write-Host "================================" -ForegroundColor Yellow
Write-Host ""
Write-Host "The following PowerShell commands can be run manually to take screenshots:"
Write-Host ""
Write-Host "# Take screenshot of current screen:" -ForegroundColor Gray
Write-Host "Add-Type -AssemblyName System.Windows.Forms, System.Drawing" -ForegroundColor Gray
Write-Host '$bounds = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds' -ForegroundColor Gray
Write-Host '$bitmap = New-Object System.Drawing.Bitmap $bounds.Width, $bounds.Height' -ForegroundColor Gray
Write-Host '$graphics = [System.Drawing.Graphics]::FromImage($bitmap)' -ForegroundColor Gray
Write-Host '$graphics.CopyFromScreen($bounds.X, $bounds.Y, 0, 0, $bounds.Size)' -ForegroundColor Gray
Write-Host '$bitmap.Save("$PSScriptRoot\screenshot.png", [System.Drawing.Imaging.ImageFormat]::Png)' -ForegroundColor Gray
Write-Host ""

Write-Host "📁 Screenshots will be saved to:" -ForegroundColor Green
Write-Host "   $PSScriptRoot\" -ForegroundColor Green
Write-Host ""
Write-Host "🎮 VM is ready for CredBoard GUI testing!" -ForegroundColor Magenta
Write-Host ""
Write-Host "💡 Pro Tip: Use Windows Snipping Tool or Snip & Sketch for easy screenshots" -ForegroundColor Blue
