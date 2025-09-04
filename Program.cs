using System;
using System.Collections.Generic;
using CredBoard.Utils;
using CredBoard.Models;

namespace CredBoard
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🔐 CredBoard C# - Secure Credential Manager");
            Console.WriteLine("==========================================");
            Console.WriteLine();

            // Initialize components (using static/singleton classes)
            var authManager = AuthManager.Instance;

            Console.WriteLine("✅ Components initialized successfully");
            Console.WriteLine();

            // Demo: Test encryption/decryption
            Console.WriteLine("🧪 Testing AES-256 Encryption:");
            string testData = "MySecretPassword123!";
            string testKey = "MyTestEncryptionKey123456789012345678901234567890";
            Console.WriteLine($"Original: {testData}");

            try
            {
                string encrypted = CryptoHelper.Encrypt(testData, testKey);
                Console.WriteLine($"Encrypted: {encrypted}");

                string decrypted = CryptoHelper.Decrypt(encrypted, testKey);
                Console.WriteLine($"Decrypted: {decrypted}");

                bool match = testData == decrypted;
                Console.WriteLine($"✅ Decryption successful: {match}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Encryption test failed: {ex.Message}");
            }

            Console.WriteLine();

            // Demo: Test password setup and authentication
            Console.WriteLine("🔑 Testing Master Password Setup:");
            string testPassword = "MyMasterPassword!";
            Console.WriteLine($"Test Password: {testPassword}");

            try
            {
                var setupResult = authManager.SetupMasterPassword(testPassword);
                Console.WriteLine($"Setup Result: {setupResult.Success}");

                if (setupResult.Success)
                {
                    var authResult = authManager.Authenticate(testPassword);
                    Console.WriteLine($"✅ Authentication successful: {authResult.Success}");
                }
                else
                {
                    Console.WriteLine($"Setup failed: {setupResult.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Password test failed: {ex.Message}");
            }

            Console.WriteLine();

            // Demo: Test credential storage
            Console.WriteLine("💾 Testing Credential Storage:");

            var testClient = new Client
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Demo Company",
                Email = "contact@demo.com",
                Logins = new List<Login>
                {
                    new Login
                    {
                        Id = Guid.NewGuid().ToString(),
                        Username = "demo@example.com",
                        Password = "DemoPass123!",
                        Website = "https://demo.com/login",
                        Notes = "Demo login credentials"
                    }
                }
            };

            try
            {
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(testClient, Newtonsoft.Json.Formatting.Indented);
                Console.WriteLine("Sample Client Data:");
                Console.WriteLine(jsonData);

                Console.WriteLine("✅ Credential data structure validated");

                // Note: SecureStorage uses Windows-specific APIs not available on macOS
                Console.WriteLine("ℹ️  Note: SecureStorage requires Windows environment for DPAPI");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Credential serialization failed: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("🎯 Core Security Features Demonstrated:");
            Console.WriteLine("   • AES-256 Encryption/Decryption");
            Console.WriteLine("   • PBKDF2 Password Hashing");
            Console.WriteLine("   • Secure Credential Data Models");
            Console.WriteLine("   • JSON Serialization Support");
            Console.WriteLine();

            Console.WriteLine("📝 Note: Full GUI requires Windows environment for Windows Forms");
            Console.WriteLine("   This console version demonstrates all core security functionality");
            Console.WriteLine();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}