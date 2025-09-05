# CredBoard - Windows GUI Application

## 🚫 Cannot Run GUI Apps in Docker Containers

**Technical Limitations:**
- Docker containers are **headless** (no display server)
- GUI applications require Windows environment with display
- GitHub Actions containers don't support GUI rendering

## ✅ Alternative: Run on Windows Machine

### Prerequisites:
- Windows 10/11 machine
- The CredBoard.exe file

### Steps:
1. Copy CredBoard.exe to Windows machine
2. Double-click CredBoard.exe
3. Application will launch with full GUI

### Expected UI Flow:
1. **Login Screen** - Master password authentication
2. **Dashboard** - Client and credential management
3. **Modal Dialogs** - Add/edit clients and credentials

## 🔧 Console Version for Docker Testing

Would you like me to create a console version that can run in Docker to demonstrate functionality?

