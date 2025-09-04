using System;
using System.Security.Cryptography;
using System.Text;

namespace CredBoard.Utils
{
    /// <summary>
    /// Provides cryptographic utilities for encryption, decryption, and key derivation
    /// </summary>
    public static class CryptoHelper
    {
        private const int KeySize = 256;
        private const int BlockSize = 128;
        private const int SaltSize = 32; // 256 bits
        private const int Iterations = 10000;

        /// <summary>
        /// Encrypts a plaintext string using AES encryption
        /// </summary>
        /// <param name="plainText">The text to encrypt</param>
        /// <param name="key">The encryption key</param>
        /// <returns>Base64-encoded encrypted string</returns>
        public static string Encrypt(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Derive key and IV from the provided key
                var keyBytes = DeriveKey(key, aes.KeySize / 8);
                var iv = DeriveKey(key + "iv", aes.BlockSize / 8);

                aes.Key = keyBytes;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypts a Base64-encoded encrypted string using AES decryption
        /// </summary>
        /// <param name="cipherText">The encrypted text to decrypt</param>
        /// <param name="key">The decryption key</param>
        /// <returns>Decrypted plaintext string</returns>
        public static string Decrypt(string cipherText, string key)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Derive key and IV from the provided key
                var keyBytes = DeriveKey(key, aes.KeySize / 8);
                var iv = DeriveKey(key + "iv", aes.BlockSize / 8);

                aes.Key = keyBytes;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Derives a cryptographic key from a password using PBKDF2
        /// </summary>
        /// <param name="password">The password to derive from</param>
        /// <param name="keyLength">The desired key length in bytes</param>
        /// <returns>Derived key bytes</returns>
        public static byte[] DeriveKey(string password, int keyLength)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(keyLength);
            }
        }

        /// <summary>
        /// Derives a key from password and returns it as a hexadecimal string
        /// </summary>
        /// <param name="password">The password to derive from</param>
        /// <returns>Hexadecimal string representation of the derived key</returns>
        public static string DeriveKeyString(string password)
        {
            var keyBytes = DeriveKey(password, 32); // 256 bits
            return BitConverter.ToString(keyBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Generates a random salt string
        /// </summary>
        /// <returns>Base64-encoded random salt</returns>
        public static string GenerateSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
        }

        /// <summary>
        /// Computes SHA256 hash of the input string
        /// </summary>
        /// <param name="input">The string to hash</param>
        /// <returns>Hexadecimal string representation of the hash</returns>
        public static string ComputeSha256(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
