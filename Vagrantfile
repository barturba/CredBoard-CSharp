# CredBoard Windows VM for GUI Testing
# NOTE: This Vagrantfile is for x86_64 systems.
# For Apple Silicon (M1/M2/M3), use UTM instead (see README-VM-SETUP.md)

Vagrant.configure("2") do |config|
  # Use Windows Server 2022 (GUI enabled) - x86_64 only
  config.vm.box = "microsoft/windows-server-2022-standard"
  config.vm.box_version = "1.0.0"

  # VM Resources
  config.vm.provider "virtualbox" do |vb|
    vb.memory = "4096"
    vb.cpus = 2
    vb.name = "CredBoard-Test-VM"

    # Enable GUI
    vb.gui = true

    # Video settings for better GUI
    vb.customize ["modifyvm", :id, "--vram", "128"]
    vb.customize ["modifyvm", :id, "--graphicscontroller", "vmsvga"]
  end

  # Network settings
  config.vm.network "private_network", type: "dhcp"

  # Shared folder for app transfer
  config.vm.synced_folder ".", "/credboard-host"

  # Provisioning script
  config.vm.provision "shell", inline: <<-SHELL
    Write-Host "🚀 Setting up CredBoard Test Environment" -ForegroundColor Green
    Write-Host "===========================================" -ForegroundColor Green

    # Install Chocolatey
    Write-Host "📦 Installing Chocolatey..."
    Set-ExecutionPolicy Bypass -Scope Process -Force
    [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072
    Invoke-Expression ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

    # Install .NET 8.0
    Write-Host "🔧 Installing .NET 8.0 Runtime..."
    choco install dotnet-8.0-runtime -y

    # Create test directory
    New-Item -ItemType Directory -Path "C:\\CredBoard-Test" -Force
    Set-Location "C:\\CredBoard-Test"

    # Copy application from shared folder
    Write-Host "📋 Copying CredBoard application..."
    Copy-Item "C:\\credboard-host\\CredBoard.exe" "C:\\CredBoard-Test\\" -ErrorAction SilentlyContinue

    # Create test script
    $testScript = @"
# CredBoard GUI Test Script
Write-Host "🧪 CredBoard GUI Test Starting..." -ForegroundColor Cyan
Write-Host "===============================" -ForegroundColor Cyan

# Check if executable exists
if (Test-Path "CredBoard.exe") {
    `$fileInfo = Get-Item "CredBoard.exe"
    Write-Host "✅ CredBoard.exe found: `$(`$fileInfo.Length / 1MB) MB" -ForegroundColor Green

    # Take initial screenshot
    Write-Host "📸 Taking pre-launch screenshot..."
    Add-Type -AssemblyName System.Windows.Forms
    Add-Type -AssemblyName System.Drawing

    `$bounds = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds
    `$bitmap = New-Object System.Drawing.Bitmap `$bounds.Width, `$bounds.Height
    `$graphics = [System.Drawing.Graphics]::FromImage(`$bitmap)
    `$graphics.CopyFromScreen(`$bounds.X, `$bounds.Y, 0, 0, `$bounds.Size)
    `$bitmap.Save("C:\\CredBoard-Test\\pre-launch.png", [System.Drawing.Imaging.ImageFormat]::Png)

    Write-Host "✅ Pre-launch screenshot saved" -ForegroundColor Green

    # Note: Actual GUI testing would require manual interaction
    Write-Host ""
    Write-Host "🎯 GUI Test Setup Complete!" -ForegroundColor Green
    Write-Host "   • Application: Ready to launch"
    Write-Host "   • Screenshots: Pre-launch captured"
    Write-Host "   • Environment: Windows VM configured"
    Write-Host ""
    Write-Host "📋 Manual Testing Steps:" -ForegroundColor Yellow
    Write-Host "   1. Launch CredBoard.exe manually"
    Write-Host "   2. Test login screen"
    Write-Host "   3. Test client management"
    Write-Host "   4. Test credential operations"
    Write-Host "   5. Take screenshots of each screen"

} else {
    Write-Host "❌ CredBoard.exe not found in test directory" -ForegroundColor Red
    Write-Host "   Expected location: C:\\CredBoard-Test\\CredBoard.exe" -ForegroundColor Red
}

Write-Host ""
Write-Host "🎮 VM is ready for GUI testing!" -ForegroundColor Magenta
"@

    $testScript | Out-File -FilePath "C:\\CredBoard-Test\\test-gui.ps1" -Encoding UTF8

    # Create desktop shortcut
    $WshShell = New-Object -comObject WScript.Shell
    $Shortcut = $WshShell.CreateShortcut("C:\\Users\\vagrant\\Desktop\\CredBoard Test.lnk")
    $Shortcut.TargetPath = "C:\\CredBoard-Test\\CredBoard.exe"
    $Shortcut.WorkingDirectory = "C:\\CredBoard-Test"
    $Shortcut.Description = "CredBoard GUI Test Application"
    $Shortcut.Save()

    Write-Host "✅ CredBoard Test Environment Setup Complete!" -ForegroundColor Green
    Write-Host ""
    Write-Host "🎯 Next Steps:" -ForegroundColor Cyan
    Write-Host "   1. Run: vagrant ssh (to connect to VM)"
    Write-Host "   2. Run: C:\\CredBoard-Test\\test-gui.ps1"
    Write-Host "   3. Launch CredBoard.exe from desktop shortcut"
    Write-Host "   4. Test the GUI application"
    Write-Host "   5. Take screenshots as needed"
  SHELL
end
