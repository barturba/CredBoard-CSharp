using System;
using System.Collections.Generic;

namespace CredBoard.Models
{
    /// <summary>
    /// Represents a client/company with associated login credentials
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Unique identifier for the client
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Name of the client/company
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional email address for the client
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// List of login credentials for this client
        /// </summary>
        public List<Login> Logins { get; set; } = new List<Login>();

        /// <summary>
        /// When the client was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
