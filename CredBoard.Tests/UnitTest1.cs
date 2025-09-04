using CredBoard.Utils;
using CredBoard.Models;
using System.Security.Cryptography;

namespace CredBoard.Tests;

public class UnitTest1
{
    [Fact]
    public void CryptoHelper_EncryptDecrypt_RoundTrip()
    {
        // Arrange
        string originalText = "MySecretPassword123!";
        string key = "TestKey123456789012345678901234567890";

        // Act
        string encrypted = CryptoHelper.Encrypt(originalText, key);
        string decrypted = CryptoHelper.Decrypt(encrypted, key);

        // Assert
        Assert.NotEqual(originalText, encrypted);
        Assert.Equal(originalText, decrypted);
    }

    [Fact]
    public void CryptoHelper_DifferentKeys_ProduceDifferentResults()
    {
        // Arrange
        string originalText = "TestPassword";
        string key1 = "Key1_123456789012345678901234567890";
        string key2 = "Key2_123456789012345678901234567890";

        // Act
        string encrypted1 = CryptoHelper.Encrypt(originalText, key1);
        string encrypted2 = CryptoHelper.Encrypt(originalText, key2);

        // Assert
        Assert.NotEqual(encrypted1, encrypted2);
    }

    [Fact]
    public void CryptoHelper_WrongKey_Fails()
    {
        // Arrange
        string originalText = "TestPassword";
        string correctKey = "CorrectKey123456789012345678901234567890";
        string wrongKey = "WrongKey123456789012345678901234567890";

        // Act
        string encrypted = CryptoHelper.Encrypt(originalText, correctKey);

        // Assert
        Assert.Throws<CryptographicException>(() =>
            CryptoHelper.Decrypt(encrypted, wrongKey));
    }

    [Fact]
    public void Client_Model_CanBeCreated()
    {
        // Arrange & Act
        var client = new Client
        {
            Name = "Test Company",
            Email = "test@company.com"
        };

        // Assert
        Assert.Equal("Test Company", client.Name);
        Assert.Equal("test@company.com", client.Email);
        Assert.NotEqual(Guid.Empty, Guid.Parse(client.Id));
        Assert.NotNull(client.Logins);
        Assert.Empty(client.Logins);
    }

    [Fact]
    public void Login_Model_CanBeCreated()
    {
        // Arrange & Act
        var login = new Login
        {
            Username = "testuser",
            Password = "testpass",
            Website = "https://example.com"
        };

        // Assert
        Assert.Equal("testuser", login.Username);
        Assert.Equal("testpass", login.Password);
        Assert.Equal("https://example.com", login.Website);
        Assert.NotEqual(Guid.Empty, Guid.Parse(login.Id));
    }

    [Fact]
    public void Client_CanHaveMultipleLogins()
    {
        // Arrange
        var client = new Client { Name = "Test Company" };
        var login1 = new Login { Username = "user1", Password = "pass1" };
        var login2 = new Login { Username = "user2", Password = "pass2" };

        // Act
        client.Logins.Add(login1);
        client.Logins.Add(login2);

        // Assert
        Assert.Equal(2, client.Logins.Count);
        Assert.Contains(login1, client.Logins);
        Assert.Contains(login2, client.Logins);
    }

    [Fact]
    public void AppData_Model_CanBeCreated()
    {
        // Arrange & Act
        var appData = new AppData();

        // Assert
        Assert.NotNull(appData.Clients);
        Assert.Empty(appData.Clients);
    }

    [Fact]
    public void Models_CanBeSerializedToJson()
    {
        // Arrange
        var client = new Client
        {
            Name = "Test Company",
            Email = "test@company.com",
            Logins =
            [
                new Login
                {
                    Username = "testuser",
                    Password = "testpass",
                    Website = "https://example.com",
                    Notes = "Test notes"
                }
            ]
        };

        // Act
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(client, Newtonsoft.Json.Formatting.Indented);

        // Assert
        Assert.Contains("Test Company", json);
        Assert.Contains("testuser", json);
        Assert.Contains("testpass", json);
        Assert.Contains("https://example.com", json);
        Assert.Contains("Test notes", json);
    }

    [Fact]
    public void Models_CanBeDeserializedFromJson()
    {
        // Arrange
        string json = @"{
            ""Id"": ""test-id"",
            ""Name"": ""Test Company"",
            ""Email"": ""test@company.com"",
            ""Logins"": [
                {
                    ""Id"": ""login-id"",
                    ""Username"": ""testuser"",
                    ""Password"": ""testpass"",
                    ""Website"": ""https://example.com"",
                    ""Notes"": ""Test notes""
                }
            ]
        }";

        // Act
        var client = Newtonsoft.Json.JsonConvert.DeserializeObject<Client>(json);

        // Assert
        Assert.NotNull(client);
        Assert.Equal("Test Company", client!.Name);
        Assert.Equal("test@company.com", client.Email);
        Assert.Single(client.Logins);
        Assert.Equal("testuser", client.Logins[0].Username);
        Assert.Equal("testpass", client.Logins[0].Password);
    }
}