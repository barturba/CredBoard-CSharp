using System;

namespace CredBoard.Utils
{
    /// <summary>
    /// Manages authentication and session state for the application
    /// </summary>
    public class AuthManager
    {
        private static AuthManager? _instance;
        private string? _encryptionKey;
        private bool _isAuthenticated;

        /// <summary>
        /// Gets the singleton instance of AuthManager
        /// </summary>
        public static AuthManager Instance => _instance ??= new AuthManager();

        /// <summary>
        /// Gets whether the user is currently authenticated
        /// </summary>
        public bool IsAuthenticated => _isAuthenticated;

        /// <summary>
        /// Gets the current encryption key (only available when authenticated)
        /// </summary>
        public string? EncryptionKey => _encryptionKey;

        private AuthManager()
        {
            // Private constructor for singleton pattern
        }

        /// <summary>
        /// Result of an authentication operation
        /// </summary>
        public class AuthResult
        {
            public bool Success { get; set; }
            public string? Error { get; set; }

            public static AuthResult Successful() => new AuthResult { Success = true };
            public static AuthResult Failed(string error) => new AuthResult { Success = false, Error = error };
        }

        /// <summary>
        /// Checks if a master password has been set up
        /// </summary>
        public bool IsMasterPasswordSet()
        {
            try
            {
                var hash = SecureStorage.LoadMasterPasswordHash();
                return !string.IsNullOrEmpty(hash);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Sets up the master password for the first time
        /// </summary>
        public AuthResult SetupMasterPassword(string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                    return AuthResult.Failed("Password cannot be empty");

                if (password.Length < 8)
                    return AuthResult.Failed("Password must be at least 8 characters long");

                var hashedPassword = CryptoHelper.ComputeSha256(password);
                SecureStorage.SaveMasterPasswordHash(hashedPassword);

                // Generate and store encryption key
                _encryptionKey = CryptoHelper.DeriveKeyString(password + CryptoHelper.GenerateSalt());
                SecureStorage.SaveEncryptionKey(_encryptionKey);

                _isAuthenticated = true;

                return AuthResult.Successful();
            }
            catch (Exception ex)
            {
                return AuthResult.Failed($"Failed to set up master password: {ex.Message}");
            }
        }

        /// <summary>
        /// Authenticates the user with the master password
        /// </summary>
        public AuthResult Authenticate(string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                    return AuthResult.Failed("Password cannot be empty");

                var storedHash = SecureStorage.LoadMasterPasswordHash();
                if (string.IsNullOrEmpty(storedHash))
                    return AuthResult.Failed("No master password set up");

                var inputHash = CryptoHelper.ComputeSha256(password);
                if (inputHash != storedHash)
                    return AuthResult.Failed("Invalid password");

                // Load the stored encryption key
                _encryptionKey = SecureStorage.LoadEncryptionKey();
                if (string.IsNullOrEmpty(_encryptionKey))
                {
                    // Fallback: regenerate encryption key if somehow missing
                    _encryptionKey = CryptoHelper.DeriveKeyString(password + CryptoHelper.GenerateSalt());
                    SecureStorage.SaveEncryptionKey(_encryptionKey);
                }

                _isAuthenticated = true;

                return AuthResult.Successful();
            }
            catch (Exception ex)
            {
                return AuthResult.Failed($"Authentication failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the current encryption key (throws if not authenticated)
        /// </summary>
        public string GetEncryptionKey()
        {
            if (!_isAuthenticated || string.IsNullOrEmpty(_encryptionKey))
                throw new InvalidOperationException("Not authenticated or encryption key not available");

            return _encryptionKey;
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        public void Logout()
        {
            _encryptionKey = null;
            _isAuthenticated = false;
        }

        /// <summary>
        /// Clears all authentication data (for reset functionality)
        /// </summary>
        public AuthResult ClearAllData()
        {
            try
            {
                SecureStorage.ClearAuthData();
                SecureStorage.ClearAppData();
                Logout();
                return AuthResult.Successful();
            }
            catch (Exception ex)
            {
                return AuthResult.Failed($"Failed to clear data: {ex.Message}");
            }
        }

        /// <summary>
        /// Attempts to restore authentication state on application startup
        /// </summary>
        public AuthResult TryRestoreAuthentication()
        {
            try
            {
                if (!IsMasterPasswordSet())
                    return AuthResult.Failed("No master password set up");

                var encryptionKey = SecureStorage.LoadEncryptionKey();
                if (string.IsNullOrEmpty(encryptionKey))
                    return AuthResult.Failed("Encryption key not found");

                _encryptionKey = encryptionKey;
                _isAuthenticated = true;

                return AuthResult.Successful();
            }
            catch (Exception ex)
            {
                return AuthResult.Failed($"Failed to restore authentication: {ex.Message}");
            }
        }
    }
}
