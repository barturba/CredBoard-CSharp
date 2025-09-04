using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CredBoard.Models;
using CredBoard.Utils;

namespace CredBoard.Forms
{
    /// <summary>
    /// Main dashboard form for managing clients and credentials
    /// </summary>
    public partial class DashboardForm : Form
    {
        private readonly AuthManager _authManager;
        private List<Client> _clients;
        private Client? _selectedClient;
        private TextBox? _searchTextBox;
        private ListBox? _clientsListBox;
        private ListBox? _loginsListBox;
        private Button? _addClientButton;
        private Button? _editClientButton;
        private Button? _deleteClientButton;
        private Button? _logoutButton;
        private Label? _titleLabel;
        private Label? _selectedClientLabel;
        private Panel? _searchPanel;
        private Panel? _contentPanel;
        private Panel? _clientsPanel;
        private Panel? _loginsPanel;

        public event Action? OnLogout;

        public DashboardForm()
        {
            InitializeComponent();
            _authManager = AuthManager.Instance;
            _clients = new List<Client>();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main layout panels
            _searchPanel = new Panel();
            _searchPanel.Dock = DockStyle.Top;
            _searchPanel.Height = 60;
            _searchPanel.BackColor = Color.FromArgb(248, 249, 250);

            _contentPanel = new Panel();
            _contentPanel.Dock = DockStyle.Fill;
            _contentPanel.BackColor = Color.White;

            // Header with title and logout
            var headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 70;
            headerPanel.BackColor = Color.FromArgb(248, 249, 250);
            headerPanel.BorderStyle = BorderStyle.FixedSingle;

            _titleLabel = new Label();
            _titleLabel.Text = "CredBoard";
            _titleLabel.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            _titleLabel.ForeColor = Color.FromArgb(33, 37, 41);
            _titleLabel.Location = new Point(20, 15);
            _titleLabel.AutoSize = true;

            _logoutButton = new Button();
            _logoutButton.Text = "Logout";
            _logoutButton.Font = new Font("Segoe UI", 10);
            _logoutButton.BackColor = Color.FromArgb(108, 117, 125);
            _logoutButton.ForeColor = Color.White;
            _logoutButton.FlatStyle = FlatStyle.Flat;
            _logoutButton.FlatAppearance.BorderSize = 0;
            _logoutButton.Size = new Size(80, 35);
            _logoutButton.Location = new Point(this.ClientSize.Width - 100, 18);
            _logoutButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _logoutButton.Click += LogoutButton_Click;

            headerPanel.Controls.AddRange(new Control[] { _titleLabel, _logoutButton });

            // Search panel
            var searchLabel = new Label();
            searchLabel.Text = "Search companies:";
            searchLabel.Font = new Font("Segoe UI", 11);
            searchLabel.ForeColor = Color.FromArgb(73, 80, 87);
            searchLabel.Location = new Point(20, 15);
            searchLabel.AutoSize = true;

            _searchTextBox = new TextBox();
            _searchTextBox.Font = new Font("Segoe UI", 11);
            _searchTextBox.BorderStyle = BorderStyle.FixedSingle;
            _searchTextBox.Location = new Point(150, 12);
            _searchTextBox.Size = new Size(250, 30);
            _searchTextBox.TextChanged += SearchTextBox_TextChanged;

            _addClientButton = new Button();
            _addClientButton.Text = "+ New Item";
            _addClientButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _addClientButton.BackColor = Color.FromArgb(0, 123, 255);
            _addClientButton.ForeColor = Color.White;
            _addClientButton.FlatStyle = FlatStyle.Flat;
            _addClientButton.FlatAppearance.BorderSize = 0;
            _addClientButton.Size = new Size(100, 35);
            _addClientButton.Location = new Point(this.ClientSize.Width - 120, 12);
            _addClientButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _addClientButton.Click += AddClientButton_Click;

            _searchPanel.Controls.AddRange(new Control[] { searchLabel, _searchTextBox, _addClientButton });

            // Content panels
            _clientsPanel = new Panel();
            _clientsPanel.Size = new Size(400, this.ClientSize.Height - 130);
            _clientsPanel.Location = new Point(0, 0);
            _clientsPanel.BorderStyle = BorderStyle.FixedSingle;

            _loginsPanel = new Panel();
            _loginsPanel.Size = new Size(this.ClientSize.Width - 400, this.ClientSize.Height - 130);
            _loginsPanel.Location = new Point(400, 0);
            _loginsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _loginsPanel.BorderStyle = BorderStyle.FixedSingle;

            _contentPanel.Controls.AddRange(new Control[] { _clientsPanel, _loginsPanel });

            // Clients panel content
            var clientsTitleLabel = new Label();
            clientsTitleLabel.Text = "Companies";
            clientsTitleLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            clientsTitleLabel.ForeColor = Color.FromArgb(73, 80, 87);
            clientsTitleLabel.Location = new Point(15, 15);
            clientsTitleLabel.AutoSize = true;

            _clientsListBox = new ListBox();
            _clientsListBox.Font = new Font("Segoe UI", 10);
            _clientsListBox.BorderStyle = BorderStyle.None;
            _clientsListBox.Location = new Point(15, 50);
            _clientsListBox.Size = new Size(370, _clientsPanel.Height - 60);
            _clientsListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _clientsListBox.SelectedIndexChanged += ClientsListBox_SelectedIndexChanged;
            _clientsListBox.DrawMode = DrawMode.OwnerDrawFixed;
            _clientsListBox.DrawItem += ClientsListBox_DrawItem;
            _clientsListBox.ItemHeight = 50;

            _clientsPanel.Controls.AddRange(new Control[] { clientsTitleLabel, _clientsListBox });

            // Logins panel content
            _selectedClientLabel = new Label();
            _selectedClientLabel.Text = "Select a company";
            _selectedClientLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            _selectedClientLabel.ForeColor = Color.FromArgb(73, 80, 87);
            _selectedClientLabel.Location = new Point(15, 15);
            _selectedClientLabel.AutoSize = true;

            var editButtonPanel = new Panel();
            editButtonPanel.Size = new Size(_loginsPanel.Width - 30, 40);
            editButtonPanel.Location = new Point(15, 45);
            editButtonPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            _editClientButton = new Button();
            _editClientButton.Text = "Edit";
            _editClientButton.Font = new Font("Segoe UI", 10);
            _editClientButton.BackColor = Color.FromArgb(255, 193, 7);
            _editClientButton.ForeColor = Color.Black;
            _editClientButton.FlatStyle = FlatStyle.Flat;
            _editClientButton.FlatAppearance.BorderSize = 0;
            _editClientButton.Size = new Size(60, 30);
            _editClientButton.Location = new Point(editButtonPanel.Width - 140, 5);
            _editClientButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _editClientButton.Click += EditClientButton_Click;
            _editClientButton.Enabled = false;

            _deleteClientButton = new Button();
            _deleteClientButton.Text = "Delete";
            _deleteClientButton.Font = new Font("Segoe UI", 10);
            _deleteClientButton.BackColor = Color.FromArgb(220, 53, 69);
            _deleteClientButton.ForeColor = Color.White;
            _deleteClientButton.FlatStyle = FlatStyle.Flat;
            _deleteClientButton.FlatAppearance.BorderSize = 0;
            _deleteClientButton.Size = new Size(70, 30);
            _deleteClientButton.Location = new Point(editButtonPanel.Width - 65, 5);
            _deleteClientButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _deleteClientButton.Click += DeleteClientButton_Click;
            _deleteClientButton.Enabled = false;

            editButtonPanel.Controls.AddRange(new Control[] { _editClientButton, _deleteClientButton });

            _loginsListBox = new ListBox();
            _loginsListBox.Font = new Font("Segoe UI", 10);
            _loginsListBox.BorderStyle = BorderStyle.None;
            _loginsListBox.Location = new Point(15, 95);
            _loginsListBox.Size = new Size(_loginsPanel.Width - 30, _loginsPanel.Height - 105);
            _loginsListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _loginsListBox.DrawMode = DrawMode.OwnerDrawFixed;
            _loginsListBox.DrawItem += LoginsListBox_DrawItem;
            _loginsListBox.ItemHeight = 60;

            _loginsPanel.Controls.AddRange(new Control[] { _selectedClientLabel, editButtonPanel, _loginsListBox });

            // Add all panels to form
            Controls.AddRange(new Control[] { headerPanel, _searchPanel, _contentPanel });

            // Form setup
            BackColor = Color.FromArgb(248, 249, 250);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;

            this.Resize += DashboardForm_Resize;

            this.ResumeLayout(false);
        }

        private void DashboardForm_Resize(object? sender, EventArgs e)
        {
            // Adjust logout button position
            if (_logoutButton != null) _logoutButton.Location = new Point(this.ClientSize.Width - 100, 18);

            // Adjust add client button position
            if (_addClientButton != null) _addClientButton.Location = new Point(this.ClientSize.Width - 120, 12);

            // Adjust logins panel size
            if (_loginsPanel != null) _loginsPanel.Size = new Size(this.ClientSize.Width - 400, this.ClientSize.Height - 130);
        }

        private void LoadData()
        {
            try
            {
                var encryptionKey = _authManager.GetEncryptionKey();
                var appData = SecureStorage.LoadAppData(encryptionKey);

                if (appData != null)
                {
                    _clients = appData.Clients;
                }
                else
                {
                    // Create sample data if none exists
                    _clients = CreateSampleData();
                    SaveData();
                }

                UpdateClientsList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<Client> CreateSampleData()
        {
            return new List<Client>
            {
                new Client
                {
                    Name = "Google",
                    Email = "admin@company.com",
                    Logins = new List<Login>
                    {
                        new Login { Username = "admin@company.com", Password = "securePass123!", Website = "accounts.google.com", Notes = "Main admin account" },
                        new Login { Username = "support@company.com", Password = "supportPass456!", Website = "console.cloud.google.com" }
                    }
                },
                new Client
                {
                    Name = "Microsoft",
                    Email = "it@company.com",
                    Logins = new List<Login>
                    {
                        new Login { Username = "it@company.com", Password = "azurePass789!", Website = "portal.azure.com", Notes = "Azure subscription" }
                    }
                }
            };
        }

        private void SaveData()
        {
            try
            {
                var encryptionKey = _authManager.GetEncryptionKey();
                var appData = new AppData
                {
                    Clients = _clients,
                    LastUpdated = DateTime.Now
                };
                SecureStorage.SaveAppData(appData, encryptionKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateClientsList()
        {
            if (_clientsListBox != null) _clientsListBox.Items.Clear();
            var filteredClients = string.IsNullOrWhiteSpace(_searchTextBox?.Text)
                ? _clients
                : _clients.Where(c =>
                    c.Name.Contains(_searchTextBox?.Text ?? "", StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(c.Email) && c.Email.Contains(_searchTextBox?.Text ?? "", StringComparison.OrdinalIgnoreCase)))
                  .ToList();

            foreach (var client in filteredClients)
            {
                if (_clientsListBox != null) _clientsListBox.Items.Add(client);
            }

            if (_clientsListBox != null && _clientsListBox.Items.Count > 0 && _clientsListBox.SelectedIndex == -1)
            {
                _clientsListBox.SelectedIndex = 0;
            }
        }

        private void UpdateLoginsList()
        {
            if (_loginsListBox != null)
            {
                _loginsListBox.Items.Clear();

                if (_selectedClient != null)
                {
                    foreach (var login in _selectedClient.Logins)
                    {
                        _loginsListBox.Items.Add(login);
                    }
                }
            }
        }

        private void ClientsListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_clientsListBox?.SelectedItem is Client client)
            {
                _selectedClient = client;
                if (_selectedClientLabel != null) _selectedClientLabel.Text = $"{client.Name} Logins";
                if (_editClientButton != null) _editClientButton.Enabled = true;
                if (_deleteClientButton != null) _deleteClientButton.Enabled = true;
                UpdateLoginsList();
            }
        }

        private void ClientsListBox_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            if (_clientsListBox != null && e.Index < _clientsListBox.Items.Count)
            {
                var client = (Client)_clientsListBox.Items[e.Index];
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Background
            e.DrawBackground();

            // Selection highlight
            if (isSelected)
            {
                using (var brush = new SolidBrush(Color.FromArgb(0, 123, 255)))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }

            // Text
            var textColor = isSelected ? Color.White : Color.FromArgb(33, 37, 41);
            using (var brush = new SolidBrush(textColor))
            {
                var nameFont = new Font("Segoe UI", 12, FontStyle.Bold);
                var emailFont = new Font("Segoe UI", 9);

                // Client name
                e.Graphics.DrawString(client.Name, nameFont, brush, e.Bounds.Left + 10, e.Bounds.Top + 8);

                // Email (if available)
                if (!string.IsNullOrEmpty(client.Email))
                {
                    using (var emailBrush = new SolidBrush(isSelected ? Color.FromArgb(200, 200, 200) : Color.FromArgb(108, 117, 125)))
                    {
                        e.Graphics.DrawString(client.Email, emailFont, emailBrush, e.Bounds.Left + 10, e.Bounds.Top + 30);
                    }
                }

                // Login count
                var countText = $"{client.Logins.Count} login{(client.Logins.Count != 1 ? "s" : "")}";
                using (var countBrush = new SolidBrush(isSelected ? Color.FromArgb(200, 200, 200) : Color.FromArgb(108, 117, 125)))
                {
                    var countSize = e.Graphics.MeasureString(countText, emailFont);
                    e.Graphics.DrawString(countText, emailFont, countBrush,
                        e.Bounds.Right - countSize.Width - 10, e.Bounds.Top + 30);
                }
            }
            }

            e.DrawFocusRectangle();
        }

        private void LoginsListBox_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            if (_loginsListBox != null && e.Index < _loginsListBox.Items.Count)
            {
                var login = (Login)_loginsListBox.Items[e.Index];

            // Background
            e.DrawBackground();

            // Border
            using (var pen = new Pen(Color.FromArgb(222, 226, 230)))
            {
                e.Graphics.DrawRectangle(pen, e.Bounds);
            }

            // Content
            using (var brush = new SolidBrush(Color.FromArgb(33, 37, 41)))
            {
                var titleFont = new Font("Segoe UI", 10, FontStyle.Bold);
                var detailFont = new Font("Segoe UI", 9);

                // Username
                e.Graphics.DrawString($"Username: {login.Username}", titleFont, brush, e.Bounds.Left + 10, e.Bounds.Top + 8);

                // Website
                using (var websiteBrush = new SolidBrush(Color.FromArgb(0, 123, 255)))
                {
                    e.Graphics.DrawString($"Website: {login.Website}", detailFont, websiteBrush, e.Bounds.Left + 10, e.Bounds.Top + 28);
                }

                // Password (masked)
                var maskedPassword = new string('•', Math.Min(login.Password.Length, 20));
                e.Graphics.DrawString($"Password: {maskedPassword}", detailFont, brush, e.Bounds.Left + 10, e.Bounds.Top + 43);

                // Notes (if available)
                if (!string.IsNullOrEmpty(login.Notes))
                {
                    using (var notesBrush = new SolidBrush(Color.FromArgb(108, 117, 125)))
                    {
                        e.Graphics.DrawString($"Notes: {login.Notes}", detailFont, notesBrush,
                            e.Bounds.Left + 200, e.Bounds.Top + 28);
                    }
                }
            }
            }

            e.DrawFocusRectangle();
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            UpdateClientsList();
        }

        private void AddClientButton_Click(object? sender, EventArgs e)
        {
            var clientModal = new ClientModalForm(null);
            if (clientModal.ShowDialog() == DialogResult.OK)
            {
                var newClient = clientModal.Client;
                if (newClient != null)
                {
                    _clients.Add(newClient);
                    SaveData();
                    UpdateClientsList();
                }
            }
        }

        private void EditClientButton_Click(object? sender, EventArgs e)
        {
            if (_selectedClient != null)
            {
                var clientModal = new ClientModalForm(_selectedClient);
                if (clientModal.ShowDialog() == DialogResult.OK)
                {
                    var updatedClient = clientModal.Client;
                    if (updatedClient != null)
                    {
                        var index = _clients.FindIndex(c => c.Id == _selectedClient.Id);
                        if (index >= 0)
                        {
                            _clients[index] = updatedClient;
                            _selectedClient = updatedClient;
                            SaveData();
                            UpdateClientsList();
                            UpdateLoginsList();
                        }
                    }
                }
            }
        }

        private void DeleteClientButton_Click(object? sender, EventArgs e)
        {
            if (_selectedClient != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{_selectedClient.Name}'?\n\nThis will permanently delete all associated login credentials.",
                    "Delete Client",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                // Ensure _selectedClient is still not null before using it below
                if (_selectedClient == null) return;

                if (result == DialogResult.Yes)
                {
                    _clients.Remove(_selectedClient);
                    _selectedClient = null;
                    _selectedClientLabel.Text = "Select a company";
                    _editClientButton.Enabled = false;
                    _deleteClientButton.Enabled = false;
                    SaveData();
                    UpdateClientsList();
                    UpdateLoginsList();
                }
            }
        }

        private void LogoutButton_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                _authManager.Logout();
                OnLogout?.Invoke();
            }
        }
    }
}
