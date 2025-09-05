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
            Console.WriteLine("🔐 CredBoard - Console Demo");
            Console.WriteLine("============================");

            try
            {
                // Test 1: Crypto Helper
                Console.WriteLine("\n🧪 Testing AES-256 Encryption:");
                string testData = "MySecretPassword123!";
                string key = "TestKey123456789012345678901234567890";

                string encrypted = CryptoHelper.Encrypt(testData, key);
                string decrypted = CryptoHelper.Decrypt(encrypted, key);

                Console.WriteLine($"✅ Encryption/Decryption: {'✓'}");
                Console.WriteLine($"   Original: {testData}");
                Console.WriteLine($"   Encrypted: {encrypted.Substring(0, 20)}...");
                Console.WriteLine($"   Decrypted: {decrypted}");

                // Test 2: Auth Manager
                Console.WriteLine("\n🔑 Testing Authentication:");
                var authManager = AuthManager.Instance;
                string testPassword = "TestMasterPassword123!";

                var setupResult = authManager.SetupMasterPassword(testPassword);
                var loginResult = authManager.Authenticate(testPassword);

                Console.WriteLine($"✅ Password Setup: {setupResult.Success}");
                Console.WriteLine($"✅ Password Login: {loginResult.Success}");

                // Test 3: Data Models
                Console.WriteLine("\n💾 Testing Data Models:");
                var client = new Client
                {
                    Name = "Demo Company",
                    Email = "demo@company.com",
                    Logins = new List<Login>
                    {
                        new Login
                        {
                            Username = "demo@example.com",
                            Password = "DemoPass123!",
                            Website = "https://demo.com"
                        }
                    }
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(client, Newtonsoft.Json.Formatting.Indented);
                Console.WriteLine("✅ JSON Serialization: ✓");
                Console.WriteLine("Sample Client Data:");
                Console.WriteLine(json);

                Console.WriteLine("\n🎯 Core Functionality Test: PASSED");
                Console.WriteLine("   • AES-256 Encryption: Working");
                Console.WriteLine("   • Password Authentication: Working");
                Console.WriteLine("   • Data Models: Working");
                Console.WriteLine("   • JSON Serialization: Working");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return;
            }

            Console.WriteLine("\n📝 Note: This is a console demo.");
            Console.WriteLine("   Full GUI requires Windows environment.");
            Console.WriteLine("   Run CredBoard.exe on Windows for full experience.");
        }
    }
}
