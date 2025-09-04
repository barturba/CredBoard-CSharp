using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CredBoard.Models;

namespace CredBoard.Utils
{
    /// <summary>
    /// Provides secure storage functionality using Windows Data Protection API
    /// </summary>
    public static class SecureStorage
    {
        private const string StorageFileName = "credboard.dat";
        private const string MasterKeyFileName = "credboard.key";
        private const string EncryptionKeyFileName = "credboard.enc";

        private static readonly string AppDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CredBoard"
        );

        static SecureStorage()
        {
            // Ensure the application data directory exists
            Directory.CreateDirectory(AppDataPath);
        }

        /// <summary>
        /// Gets the full path for a storage file
        /// </summary>
        private static string GetStoragePath(string fileName)
        {
            return Path.Combine(AppDataPath, fileName);
        }

        /// <summary>
        /// Protects data using platform-specific secure storage
        /// </summary>
        private static byte[] ProtectData(byte[] data)
        {
#if WINDOWS
            return ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
#else
            // On non-Windows platforms, return data as-is (not recommended for production)
            return data;
#endif
        }

        /// <summary>
        /// Unprotects data using platform-specific secure storage
        /// </summary>
        private static byte[] UnprotectData(byte[] data)
        {
#if WINDOWS
            return ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
#else
            // On non-Windows platforms, return data as-is (not recommended for production)
            return data;
#endif
        }

        /// <summary>
        /// Saves a string value securely
        /// </summary>
        public static void SaveSecureString(string key, string value)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(value);
                var protectedData = ProtectData(data);
                File.WriteAllBytes(GetStoragePath(key), protectedData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save secure data for key '{key}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads a string value securely
        /// </summary>
        public static string? LoadSecureString(string key)
        {
            try
            {
                var filePath = GetStoragePath(key);
                if (!File.Exists(filePath))
                    return null;

                var protectedData = File.ReadAllBytes(filePath);
                var data = UnprotectData(protectedData);
                return Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load secure data for key '{key}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a securely stored value
        /// </summary>
        public static void DeleteSecureString(string key)
        {
            try
            {
                var filePath = GetStoragePath(key);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete secure data for key '{key}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Saves the master password hash
        /// </summary>
        public static void SaveMasterPasswordHash(string hash)
        {
            SaveSecureString(MasterKeyFileName, hash);
        }

        /// <summary>
        /// Loads the master password hash
        /// </summary>
        public static string? LoadMasterPasswordHash()
        {
            return LoadSecureString(MasterKeyFileName);
        }

        /// <summary>
        /// Saves the encryption key
        /// </summary>
        public static void SaveEncryptionKey(string key)
        {
            SaveSecureString(EncryptionKeyFileName, key);
        }

        /// <summary>
        /// Loads the encryption key
        /// </summary>
        public static string? LoadEncryptionKey()
        {
            return LoadSecureString(EncryptionKeyFileName);
        }

        /// <summary>
        /// Clears all stored authentication data
        /// </summary>
        public static void ClearAuthData()
        {
            try
            {
                DeleteSecureString(MasterKeyFileName);
                DeleteSecureString(EncryptionKeyFileName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to clear authentication data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Saves application data (encrypted)
        /// </summary>
        public static void SaveAppData(AppData data, string encryptionKey)
        {
            try
            {
                var jsonData = System.Text.Json.JsonSerializer.Serialize(data);
                var encryptedData = CryptoHelper.Encrypt(jsonData, encryptionKey);

                // Split large data into chunks if needed (for compatibility with storage limits)
                const int chunkSize = 1800; // Leave buffer below any potential limits

                if (encryptedData.Length <= chunkSize)
                {
                    File.WriteAllText(GetStoragePath(StorageFileName), encryptedData);
                }
                else
                {
                    // Split into chunks
                    var chunks = SplitIntoChunks(encryptedData, chunkSize);
                    File.WriteAllText(GetStoragePath(StorageFileName + ".chunks"), chunks.Length.ToString());

                    for (int i = 0; i < chunks.Length; i++)
                    {
                        File.WriteAllText(GetStoragePath($"{StorageFileName}.chunk.{i}"), chunks[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save application data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads application data (decrypted)
        /// </summary>
        public static AppData? LoadAppData(string encryptionKey)
        {
            try
            {
                string encryptedData;

                // Check if data is stored in chunks
                var chunksFile = GetStoragePath(StorageFileName + ".chunks");
                if (File.Exists(chunksFile))
                {
                    var chunkCount = int.Parse(File.ReadAllText(chunksFile));
                    var chunks = new string[chunkCount];

                    for (int i = 0; i < chunkCount; i++)
                    {
                        var chunkFile = GetStoragePath($"{StorageFileName}.chunk.{i}");
                        if (File.Exists(chunkFile))
                        {
                            chunks[i] = File.ReadAllText(chunkFile);
                        }
                        else
                        {
                            throw new Exception($"Missing data chunk {i}");
                        }
                    }

                    encryptedData = string.Join("", chunks);
                }
                else
                {
                    var dataFile = GetStoragePath(StorageFileName);
                    if (!File.Exists(dataFile))
                        return null;

                    encryptedData = File.ReadAllText(dataFile);
                }

                var jsonData = CryptoHelper.Decrypt(encryptedData, encryptionKey);
                return System.Text.Json.JsonSerializer.Deserialize<AppData>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load application data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clears all application data
        /// </summary>
        public static void ClearAppData()
        {
            try
            {
                var dataFile = GetStoragePath(StorageFileName);
                if (File.Exists(dataFile))
                    File.Delete(dataFile);

                // Clear chunks if they exist
                var chunksFile = GetStoragePath(StorageFileName + ".chunks");
                if (File.Exists(chunksFile))
                {
                    var chunkCount = int.Parse(File.ReadAllText(chunksFile));
                    for (int i = 0; i < chunkCount; i++)
                    {
                        var chunkFile = GetStoragePath($"{StorageFileName}.chunk.{i}");
                        if (File.Exists(chunkFile))
                            File.Delete(chunkFile);
                    }
                    File.Delete(chunksFile);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to clear application data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Splits a string into chunks of specified size
        /// </summary>
        private static string[] SplitIntoChunks(string data, int chunkSize)
        {
            var chunks = new System.Collections.Generic.List<string>();
            for (int i = 0; i < data.Length; i += chunkSize)
            {
                chunks.Add(data.Substring(i, Math.Min(chunkSize, data.Length - i)));
            }
            return chunks.ToArray();
        }
    }
}
