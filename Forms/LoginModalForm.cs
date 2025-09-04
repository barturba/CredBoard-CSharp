using System;
using System.Drawing;
using System.Windows.Forms;
using CredBoard.Models;

namespace CredBoard.Forms
{
    /// <summary>
    /// Modal dialog for adding/editing login credentials
    /// </summary>
    public partial class LoginModalForm : Form
    {
        private Login? _originalLogin;
        private Login _login;
        private TextBox? _usernameTextBox;
        private TextBox? _passwordTextBox;
        private TextBox? _websiteTextBox;
        private TextBox? _notesTextBox;
        private CheckBox? _showPasswordCheckBox;
        private Button? _saveButton;
        private Button? _cancelButton;
        private Label? _titleLabel;

        public Login? Login => _login;

        public LoginModalForm(Login? login)
        {
            _originalLogin = login;
            _login = login != null
                ? new Login
                {
                    Id = login.Id,
                    Username = login.Username,
                    Password = login.Password,
                    Website = login.Website,
                    Notes = login.Notes,
                    CreatedAt = login.CreatedAt
                }
                : new Login();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form setup
            Text = _originalLogin == null ? "Add Login Credential" : "Edit Login Credential";
            Size = new Size(450, 400);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;

            // Title
            _titleLabel = new Label();
            _titleLabel.Text = _originalLogin == null ? "Add Login Credential" : "Edit Login Credential";
            _titleLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            _titleLabel.ForeColor = Color.FromArgb(33, 37, 41);
            _titleLabel.Location = new Point(20, 20);
            _titleLabel.AutoSize = true;

            // Username
            var usernameLabel = new Label();
            usernameLabel.Text = "Username/Email:";
            usernameLabel.Font = new Font("Segoe UI", 11);
            usernameLabel.ForeColor = Color.FromArgb(73, 80, 87);
            usernameLabel.Location = new Point(20, 60);
            usernameLabel.AutoSize = true;

            _usernameTextBox = new TextBox();
            _usernameTextBox.Font = new Font("Segoe UI", 11);
            _usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            _usernameTextBox.Location = new Point(20, 85);
            _usernameTextBox.Size = new Size(400, 30);
            _usernameTextBox.Text = _login.Username;

            // Password
            var passwordLabel = new Label();
            passwordLabel.Text = "Password:";
            passwordLabel.Font = new Font("Segoe UI", 11);
            passwordLabel.ForeColor = Color.FromArgb(73, 80, 87);
            passwordLabel.Location = new Point(20, 125);
            passwordLabel.AutoSize = true;

            _passwordTextBox = new TextBox();
            _passwordTextBox.Font = new Font("Segoe UI", 11);
            _passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
            _passwordTextBox.PasswordChar = '•';
            _passwordTextBox.Location = new Point(20, 150);
            _passwordTextBox.Size = new Size(400, 30);
            _passwordTextBox.Text = _login.Password;

            // Show password checkbox
            _showPasswordCheckBox = new CheckBox();
            _showPasswordCheckBox.Text = "Show password";
            _showPasswordCheckBox.Font = new Font("Segoe UI", 9);
            _showPasswordCheckBox.ForeColor = Color.FromArgb(108, 117, 125);
            _showPasswordCheckBox.Location = new Point(20, 185);
            _showPasswordCheckBox.Size = new Size(120, 20);
            _showPasswordCheckBox.CheckedChanged += ShowPasswordCheckBox_CheckedChanged;

            // Website
            var websiteLabel = new Label();
            websiteLabel.Text = "Website/Service:";
            websiteLabel.Font = new Font("Segoe UI", 11);
            websiteLabel.ForeColor = Color.FromArgb(73, 80, 87);
            websiteLabel.Location = new Point(20, 215);
            websiteLabel.AutoSize = true;

            _websiteTextBox = new TextBox();
            _websiteTextBox.Font = new Font("Segoe UI", 11);
            _websiteTextBox.BorderStyle = BorderStyle.FixedSingle;
            _websiteTextBox.Location = new Point(20, 240);
            _websiteTextBox.Size = new Size(400, 30);
            _websiteTextBox.Text = _login.Website;
            _websiteTextBox.PlaceholderText = "e.g., accounts.google.com, portal.azure.com";

            // Notes
            var notesLabel = new Label();
            notesLabel.Text = "Notes (optional):";
            notesLabel.Font = new Font("Segoe UI", 11);
            notesLabel.ForeColor = Color.FromArgb(73, 80, 87);
            notesLabel.Location = new Point(20, 280);
            notesLabel.AutoSize = true;

            _notesTextBox = new TextBox();
            _notesTextBox.Font = new Font("Segoe UI", 11);
            _notesTextBox.BorderStyle = BorderStyle.FixedSingle;
            _notesTextBox.Multiline = true;
            _notesTextBox.Location = new Point(20, 305);
            _notesTextBox.Size = new Size(400, 50);
            _notesTextBox.Text = _login.Notes ?? "";

            // Action buttons
            _saveButton = new Button();
            _saveButton.Text = "Save";
            _saveButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            _saveButton.BackColor = Color.FromArgb(40, 167, 69);
            _saveButton.ForeColor = Color.White;
            _saveButton.FlatStyle = FlatStyle.Flat;
            _saveButton.FlatAppearance.BorderSize = 0;
            _saveButton.Size = new Size(100, 40);
            _saveButton.Location = new Point(270, 365);
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
            _cancelButton.Location = new Point(150, 365);
            _cancelButton.Click += CancelButton_Click;
            _cancelButton.DialogResult = DialogResult.Cancel;

            // Add controls
            Controls.AddRange(new Control[] {
                _titleLabel,
                usernameLabel, _usernameTextBox,
                passwordLabel, _passwordTextBox,
                _showPasswordCheckBox,
                websiteLabel, _websiteTextBox,
                notesLabel, _notesTextBox,
                _saveButton, _cancelButton
            });

            this.AcceptButton = _saveButton;
            this.CancelButton = _cancelButton;

            this.ResumeLayout(false);
        }

        private void ShowPasswordCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (_passwordTextBox != null && _showPasswordCheckBox != null)
            {
                _passwordTextBox.PasswordChar = _showPasswordCheckBox.Checked ? '\0' : '•';
            }
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            // Validate input
            var username = _usernameTextBox?.Text.Trim() ?? "";
            var password = _passwordTextBox?.Text.Trim() ?? "";
            var website = _websiteTextBox?.Text.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a username or email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _usernameTextBox?.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter a password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _passwordTextBox?.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(website))
            {
                MessageBox.Show("Please enter a website or service name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _websiteTextBox?.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            // Update login
            _login.Username = username;
            _login.Password = password;
            _login.Website = website;
            _login.Notes = string.IsNullOrWhiteSpace(_notesTextBox?.Text) ? null : (_notesTextBox?.Text.Trim());

            // Set creation date if new login
            if (_originalLogin == null)
            {
                _login.CreatedAt = DateTime.Now;
            }
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
