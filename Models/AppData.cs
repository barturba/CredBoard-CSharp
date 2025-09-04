using System;
using System.Collections.Generic;

namespace CredBoard.Models
{
    /// <summary>
    /// Represents the complete application data structure
    /// </summary>
    public class AppData
    {
        /// <summary>
        /// List of all clients in the application
        /// </summary>
        public List<Client> Clients { get; set; } = new List<Client>();

        /// <summary>
        /// When the data was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
