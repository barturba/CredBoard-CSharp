# CredBoard - Secure Credential Management

A secure Windows desktop application for managing company credentials and login information, rewritten from React Native to C# Windows Forms.

## Features

- **Secure Authentication**: Master password protection with PBKDF2 key derivation
- **AES-256 Encryption**: All sensitive data is encrypted before storage
- **Windows Data Protection**: Uses Windows DPAPI for secure key storage
- **Client Management**: Organize credentials by company/client
- **Search Functionality**: Quickly find companies and credentials
- **Modern UI**: Clean, intuitive Windows Forms interface

## Architecture

### Core Components

- **Models**: `Client`, `Login`, `AppData` - Data structures
- **Utils**:
  - `CryptoHelper` - AES encryption/decryption and key derivation
  - `SecureStorage` - Windows DPAPI-based secure storage
  - `AuthManager` - Authentication and session management
- **Forms**:
  - `MainForm` - Main application window
  - `LoginForm` - Authentication interface
  - `DashboardForm` - Main credential management interface
  - `ClientModalForm` - Add/edit client dialog
  - `LoginModalForm` - Add/edit login credential dialog

## Security Features

1. **Master Password**: Required for application access
2. **AES-256 Encryption**: All credential data is encrypted
3. **PBKDF2 Key Derivation**: Secure key generation from passwords
4. **Windows Data Protection API**: Secure storage of encryption keys
5. **Isolated Storage**: Data stored in user's local application data folder

## Prerequisites

- Windows 10/11
- .NET 8.0 Runtime
- Visual Studio 2022 (for development)

## Building from Source

### Requirements
- .NET 8.0 SDK
- Visual Studio 2022 with Windows Forms support

### Build Steps

1. Clone or download the project files
2. Open `CredBoard.csproj` in Visual Studio
3. Restore NuGet packages:
   ```
   dotnet restore
   ```
4. Build the project:
   ```
   dotnet build --configuration Release
   ```
5. Publish for deployment:
   ```
   dotnet publish --configuration Release --runtime win-x64 --self-contained
   ```

### Build Configuration

The project includes:
- `app.manifest` - Windows application manifest for DPI awareness and compatibility
- `.cursorignore` - Cursor editor ignore patterns
- `CredBoard-CSharp.code-workspace` - VSCode workspace configuration

## Usage

1. **First Run**: Set up your master password
2. **Add Clients**: Create company/client entries
3. **Add Credentials**: Add login information for each client
4. **Search**: Use the search box to find companies quickly
5. **Manage**: Edit or delete clients and credentials as needed

## Data Storage

- **Location**: `%LOCALAPPDATA%\CredBoard\`
- **Files**:
  - `credboard.key` - Encrypted master password hash
  - `credboard.enc` - Encrypted encryption key
  - `credboard.dat` - Encrypted application data
- **Backup**: Copy the entire CredBoard folder to backup your data

## Security Notes

- Master password is required on every application start
- All data is encrypted with AES-256 before storage
- Encryption keys are protected by Windows Data Protection API
- No data is transmitted over network - everything is local
- Application data is stored in user's local profile

## Development

### Project Structure
```
CredBoard-CSharp/
├── CredBoard.csproj          # Project file
├── Program.cs               # Application entry point
├── App.config              # Application configuration
├── app.manifest            # Windows manifest
├── Models/                 # Data models
│   ├── Client.cs
│   ├── Login.cs
│   └── AppData.cs
├── Utils/                  # Utility classes
│   ├── AuthManager.cs
│   ├── CryptoHelper.cs
│   └── SecureStorage.cs
├── Forms/                  # Windows Forms
│   ├── MainForm.cs
│   ├── LoginForm.cs
│   ├── DashboardForm.cs
│   ├── ClientModalForm.cs
│   └── LoginModalForm.cs
├── Properties/             # Assembly properties
├── Resources/              # Application resources
└── .cursor/                # Editor configuration
```

### Key Differences from Original

The original React Native application has been converted to:

- **C# Windows Forms** instead of React Native
- **Windows Data Protection API** instead of Expo SecureStore
- **.NET AES implementation** instead of crypto-js
- **Windows Forms UI** instead of React components
- **Local file storage** instead of AsyncStorage

## License

This project maintains the same license as the original CredBoard application.

## Troubleshooting

### Build Issues
- Ensure .NET 8.0 SDK is installed
- Check that all NuGet packages are restored
- Verify Windows Forms workload is installed in Visual Studio

### Runtime Issues
- Ensure Windows Data Protection API is available (Windows 10+)
- Check write permissions to `%LOCALAPPDATA%\CredBoard\`
- Verify .NET 8.0 runtime is installed

### Data Issues
- Check that encryption keys haven't been corrupted
- Try deleting the CredBoard data folder to reset (will lose all data)
- Ensure master password is remembered correctly
