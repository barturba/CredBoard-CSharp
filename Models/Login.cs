using System;

namespace CredBoard.Models
{
    /// <summary>
    /// Represents a login credential for a specific website/service
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Unique identifier for the login
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Username for the login
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Password for the login
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Website or service URL
        /// </summary>
        public string Website { get; set; } = string.Empty;

        /// <summary>
        /// Optional notes about the login
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// When the login was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
