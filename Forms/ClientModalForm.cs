using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CredBoard.Models;

namespace CredBoard.Forms
{
    /// <summary>
    /// Modal dialog for adding/editing clients and their login credentials
    /// </summary>
    public partial class ClientModalForm : Form
    {
        private Client? _originalClient;
        private Client _client;
        private TextBox? _nameTextBox;
        private TextBox? _emailTextBox;
        private ListBox? _loginsListBox;
        private Button? _addLoginButton;
        private Button? _editLoginButton;
        private Button? _deleteLoginButton;
        private Button? _saveButton;
        private Button? _cancelButton;
        private Label? _titleLabel;

        public Client? Client => _client;

        public ClientModalForm(Client? client)
        {
            _originalClient = client;
            _client = client != null
                ? new Client
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    Logins = client.Logins.Select(l => new Login
                    {
                        Id = l.Id,
                        Username = l.Username,
                        Password = l.Password,
                        Website = l.Website,
                        Notes = l.Notes,
                        CreatedAt = l.CreatedAt
                    }).ToList(),
                    CreatedAt = client.CreatedAt
                }
                : new Client();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form setup
            Text = _originalClient == null ? "Add New Client" : "Edit Client";
            Size = new Size(600, 500);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;

            // Title
            _titleLabel = new Label();
            _titleLabel.Text = _originalClient == null ? "Add New Client" : "Edit Client";
            _titleLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            _titleLabel.ForeColor = Color.FromArgb(33, 37, 41);
            _titleLabel.Location = new Point(20, 20);
            _titleLabel.AutoSize = true;

            // Client name
            var nameLabel = new Label();
            nameLabel.Text = "Company Name:";
            nameLabel.Font = new Font("Segoe UI", 11);
            nameLabel.ForeColor = Color.FromArgb(73, 80, 87);
            nameLabel.Location = new Point(20, 60);
            nameLabel.AutoSize = true;

            _nameTextBox = new TextBox();
            _nameTextBox.Font = new Font("Segoe UI", 11);
            _nameTextBox.BorderStyle = BorderStyle.FixedSingle;
            _nameTextBox.Location = new Point(20, 85);
            _nameTextBox.Size = new Size(250, 30);
            _nameTextBox.Text = _client.Name;

            // Client email
            var emailLabel = new Label();
            emailLabel.Text = "Email (optional):";
            emailLabel.Font = new Font("Segoe UI", 11);
            emailLabel.ForeColor = Color.FromArgb(73, 80, 87);
            emailLabel.Location = new Point(290, 60);
            emailLabel.AutoSize = true;

            _emailTextBox = new TextBox();
            _emailTextBox.Font = new Font("Segoe UI", 11);
            _emailTextBox.BorderStyle = BorderStyle.FixedSingle;
            _emailTextBox.Location = new Point(290, 85);
            _emailTextBox.Size = new Size(270, 30);
            _emailTextBox.Text = _client.Email ?? "";

            // Logins section
            var loginsLabel = new Label();
            loginsLabel.Text = "Login Credentials:";
            loginsLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            loginsLabel.ForeColor = Color.FromArgb(33, 37, 41);
            loginsLabel.Location = new Point(20, 130);
            loginsLabel.AutoSize = true;

            _loginsListBox = new ListBox();
            _loginsListBox.Font = new Font("Segoe UI", 10);
            _loginsListBox.BorderStyle = BorderStyle.FixedSingle;
            _loginsListBox.Location = new Point(20, 160);
            _loginsListBox.Size = new Size(420, 200);
            _loginsListBox.DrawMode = DrawMode.OwnerDrawFixed;
            _loginsListBox.DrawItem += LoginsListBox_DrawItem;
            _loginsListBox.ItemHeight = 50;
            _loginsListBox.SelectedIndexChanged += LoginsListBox_SelectedIndexChanged;

            // Login buttons
            _addLoginButton = new Button();
            _addLoginButton.Text = "Add Login";
            _addLoginButton.Font = new Font("Segoe UI", 10);
            _addLoginButton.BackColor = Color.FromArgb(0, 123, 255);
            _addLoginButton.ForeColor = Color.White;
            _addLoginButton.FlatStyle = FlatStyle.Flat;
            _addLoginButton.FlatAppearance.BorderSize = 0;
            _addLoginButton.Size = new Size(100, 35);
            _addLoginButton.Location = new Point(450, 160);
            _addLoginButton.Click += AddLoginButton_Click;

            _editLoginButton = new Button();
            _editLoginButton.Text = "Edit";
            _editLoginButton.Font = new Font("Segoe UI", 10);
            _editLoginButton.BackColor = Color.FromArgb(255, 193, 7);
            _editLoginButton.ForeColor = Color.Black;
            _editLoginButton.FlatStyle = FlatStyle.Flat;
            _editLoginButton.FlatAppearance.BorderSize = 0;
            _editLoginButton.Size = new Size(70, 35);
            _editLoginButton.Location = new Point(450, 205);
            _editLoginButton.Click += EditLoginButton_Click;
            _editLoginButton.Enabled = false;

            _deleteLoginButton = new Button();
            _deleteLoginButton.Text = "Delete";
            _deleteLoginButton.Font = new Font("Segoe UI", 10);
            _deleteLoginButton.BackColor = Color.FromArgb(220, 53, 69);
            _deleteLoginButton.ForeColor = Color.White;
            _deleteLoginButton.FlatStyle = FlatStyle.Flat;
            _deleteLoginButton.FlatAppearance.BorderSize = 0;
            _deleteLoginButton.Size = new Size(70, 35);
            _deleteLoginButton.Location = new Point(450, 250);
            _deleteLoginButton.Click += DeleteLoginButton_Click;
            _deleteLoginButton.Enabled = false;

            // Action buttons
            _saveButton = new Button();
            _saveButton.Text = "Save";
            _saveButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            _saveButton.BackColor = Color.FromArgb(40, 167, 69);
            _saveButton.ForeColor = Color.White;
            _saveButton.FlatStyle = FlatStyle.Flat;
            _saveButton.FlatAppearance.BorderSize = 0;
            _saveButton.Size = new Size(100, 40);
            _saveButton.Location = new Point(410, 430);
            _saveButton.Click += SaveButton_Click;
            _saveButton.DialogResult = DialogResult.OK;

            _cancelButton = new Button();
            _cancelButton.Text = "Cancel";
            _cancelButton.Font = new Font("Segoe UI", 11);
            _cancelButton.BackColor = Color.FromArgb(108, 117, 125);
            _cancelButton.ForeColor = Color.White;
            _cancelButton.FlatStyle = FlatStyle.Flat;
            _cancelButton.FlatAppearance.BorderSize = 0;
            _cancelButton.Size = new Size(100, 40);
            _cancelButton.Location = new Point(290, 430);
            _cancelButton.Click += CancelButton_Click;
            _cancelButton.DialogResult = DialogResult.Cancel;

            // Add controls
            Controls.AddRange(new Control[] {
                _titleLabel,
                nameLabel, _nameTextBox,
                emailLabel, _emailTextBox,
                loginsLabel,
                _loginsListBox,
                _addLoginButton, _editLoginButton, _deleteLoginButton,
                _saveButton, _cancelButton
            });

            // Load logins
            UpdateLoginsList();

            this.AcceptButton = _saveButton;
            this.CancelButton = _cancelButton;

            this.ResumeLayout(false);
        }

        private void UpdateLoginsList()
        {
            if (_loginsListBox != null)
            {
                _loginsListBox.Items.Clear();
                foreach (var login in _client.Logins)
                {
                    _loginsListBox.Items.Add(login);
                }
            }
        }

        private void LoginsListBox_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            if (_loginsListBox == null || e.Index >= _loginsListBox.Items.Count) return;
            var login = (Login)_loginsListBox.Items[e.Index];
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

            // Content
            var textColor = isSelected ? Color.White : Color.FromArgb(33, 37, 41);
            using (var brush = new SolidBrush(textColor))
            {
                var titleFont = new Font("Segoe UI", 10, FontStyle.Bold);
                var detailFont = new Font("Segoe UI", 9);

                // Username
                e.Graphics.DrawString(login.Username, titleFont, brush, e.Bounds.Left + 10, e.Bounds.Top + 8);

                // Website
                using (var websiteBrush = new SolidBrush(isSelected ? Color.FromArgb(200, 200, 200) : Color.FromArgb(0, 123, 255)))
                {
                    e.Graphics.DrawString(login.Website, detailFont, websiteBrush, e.Bounds.Left + 10, e.Bounds.Top + 28);
                }

                // Password (masked)
                var maskedPassword = new string('•', Math.Min(login.Password.Length, 15));
                using (var passwordBrush = new SolidBrush(isSelected ? Color.FromArgb(200, 200, 200) : Color.FromArgb(108, 117, 125)))
                {
                    e.Graphics.DrawString($"Password: {maskedPassword}", detailFont, passwordBrush,
                        e.Bounds.Left + 200, e.Bounds.Top + 8);
                }

                // Notes (if available)
                if (!string.IsNullOrEmpty(login.Notes))
                {
                    using (var notesBrush = new SolidBrush(isSelected ? Color.FromArgb(200, 200, 200) : Color.FromArgb(108, 117, 125)))
                    {
                        var notes = login.Notes.Length > 30 ? login.Notes.Substring(0, 27) + "..." : login.Notes;
                        e.Graphics.DrawString($"Notes: {notes}", detailFont, notesBrush,
                            e.Bounds.Left + 200, e.Bounds.Top + 28);
                    }
                }
            }

            e.DrawFocusRectangle();
        }

        private void LoginsListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_loginsListBox != null)
            {
                var hasSelection = _loginsListBox.SelectedIndex >= 0;
                if (_editLoginButton != null) _editLoginButton.Enabled = hasSelection;
                if (_deleteLoginButton != null) _deleteLoginButton.Enabled = hasSelection;
            }
        }

        private void AddLoginButton_Click(object? sender, EventArgs e)
        {
            var loginModal = new LoginModalForm(null);
            if (loginModal.ShowDialog() == DialogResult.OK)
            {
                var newLogin = loginModal.Login;
                if (newLogin != null)
                {
                    _client.Logins.Add(newLogin);
                    UpdateLoginsList();
                }
            }
        }

        private void EditLoginButton_Click(object? sender, EventArgs e)
        {
            if (_loginsListBox.SelectedItem is Login selectedLogin)
            {
                var loginModal = new LoginModalForm(selectedLogin);
                if (loginModal.ShowDialog() == DialogResult.OK)
                {
                    var updatedLogin = loginModal.Login;
                    if (updatedLogin != null)
                    {
                        var index = _client.Logins.FindIndex(l => l.Id == selectedLogin.Id);
                        if (index >= 0)
                        {
                            _client.Logins[index] = updatedLogin;
                            UpdateLoginsList();
                        }
                    }
                }
            }
        }

        private void DeleteLoginButton_Click(object? sender, EventArgs e)
        {
            if (_loginsListBox.SelectedItem is Login selectedLogin)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to delete this login credential?",
                    "Delete Login",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    _client.Logins.Remove(selectedLogin);
                    UpdateLoginsList();
                }
            }
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            // Validate input
            var name = _nameTextBox?.Text.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a company name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _nameTextBox?.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            // Update client
            _client.Name = name;
            _client.Email = string.IsNullOrWhiteSpace(_emailTextBox?.Text) ? null : _emailTextBox?.Text.Trim();

            // Set creation date if new client
            if (_originalClient == null)
            {
                _client.CreatedAt = DateTime.Now;
            }
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
