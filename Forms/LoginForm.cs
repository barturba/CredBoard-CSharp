using System;
using System.Drawing;
using System.Windows.Forms;
using CredBoard.Utils;

namespace CredBoard.Forms
{
    /// <summary>
    /// Login form for user authentication
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly AuthManager _authManager;
        private TextBox? _passwordTextBox;
        private Button? _loginButton;
        private Button? _setupButton;
        private Label? _titleLabel;
        private Label? _instructionLabel;
        private CheckBox? _showPasswordCheckBox;
        private bool _isSetupMode;

        public event Action? OnAuthenticated;

        public LoginForm()
        {
            InitializeComponent();
            _authManager = AuthManager.Instance;
            _isSetupMode = !_authManager.IsMasterPasswordSet();
            UpdateUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Title label
            _titleLabel = new Label();
            _titleLabel.Text = "CredBoard";
            _titleLabel.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            _titleLabel.ForeColor = Color.FromArgb(33, 37, 41);
            _titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            _titleLabel.Location = new Point(0, 100);
            _titleLabel.Size = new Size(400, 50);
            _titleLabel.Anchor = AnchorStyles.Top;

            // Instruction label
            _instructionLabel = new Label();
            _instructionLabel.Font = new Font("Segoe UI", 12);
            _instructionLabel.ForeColor = Color.FromArgb(73, 80, 87);
            _instructionLabel.TextAlign = ContentAlignment.MiddleCenter;
            _instructionLabel.Location = new Point(0, 170);
            _instructionLabel.Size = new Size(400, 40);
            _instructionLabel.Anchor = AnchorStyles.Top;

            // Password text box
            _passwordTextBox = new TextBox();
            _passwordTextBox.Font = new Font("Segoe UI", 11);
            _passwordTextBox.PasswordChar = '•';
            _passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
            _passwordTextBox.Location = new Point(50, 230);
            _passwordTextBox.Size = new Size(300, 30);
            _passwordTextBox.Anchor = AnchorStyles.Top;
            _passwordTextBox.KeyDown += PasswordTextBox_KeyDown;

            // Show password checkbox
            _showPasswordCheckBox = new CheckBox();
            _showPasswordCheckBox.Text = "Show password";
            _showPasswordCheckBox.Font = new Font("Segoe UI", 9);
            _showPasswordCheckBox.ForeColor = Color.FromArgb(73, 80, 87);
            _showPasswordCheckBox.Location = new Point(50, 270);
            _showPasswordCheckBox.Size = new Size(120, 20);
            _showPasswordCheckBox.Anchor = AnchorStyles.Top;
            _showPasswordCheckBox.CheckedChanged += ShowPasswordCheckBox_CheckedChanged;

            // Login/Setup button
            _loginButton = new Button();
            _loginButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            _loginButton.BackColor = Color.FromArgb(0, 123, 255);
            _loginButton.ForeColor = Color.White;
            _loginButton.FlatStyle = FlatStyle.Flat;
            _loginButton.FlatAppearance.BorderSize = 0;
            _loginButton.Location = new Point(50, 310);
            _loginButton.Size = new Size(300, 40);
            _loginButton.Anchor = AnchorStyles.Top;
            _loginButton.Click += LoginButton_Click;

            // Setup button (only shown when no password is set)
            _setupButton = new Button();
            _setupButton.Text = "Set Up Master Password";
            _setupButton.Font = new Font("Segoe UI", 10);
            _setupButton.BackColor = Color.FromArgb(40, 167, 69);
            _setupButton.ForeColor = Color.White;
            _setupButton.FlatStyle = FlatStyle.Flat;
            _setupButton.FlatAppearance.BorderSize = 0;
            _setupButton.Location = new Point(50, 360);
            _setupButton.Size = new Size(300, 35);
            _setupButton.Anchor = AnchorStyles.Top;
            _setupButton.Click += SetupButton_Click;
            _setupButton.Visible = false;

            // Form setup
            ClientSize = new Size(400, 450);
            Controls.AddRange(new Control[] {
                _titleLabel,
                _instructionLabel,
                _passwordTextBox,
                _showPasswordCheckBox,
                _loginButton,
                _setupButton
            });

            BackColor = Color.FromArgb(248, 249, 250);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;

            this.ResumeLayout(false);
        }

        private void UpdateUI()
        {
            if (_isSetupMode)
            {
                _instructionLabel.Text = "Set up your master password to secure your credentials";
                _loginButton.Text = "Set Password";
                _loginButton.BackColor = Color.FromArgb(40, 167, 69);
                _setupButton.Visible = false;
            }
            else
            {
                _instructionLabel.Text = "Enter your master password to access your credentials";
                _loginButton.Text = "Login";
                _loginButton.BackColor = Color.FromArgb(0, 123, 255);
                _setupButton.Visible = true;
            }
        }

        private void ShowPasswordCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            _passwordTextBox.PasswordChar = _showPasswordCheckBox.Checked ? '\0' : '•';
        }

        private void PasswordTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_isSetupMode)
                    SetupPassword();
                else
                    PerformLogin();
            }
        }

        private void LoginButton_Click(object? sender, EventArgs e)
        {
            if (_isSetupMode)
                SetupPassword();
            else
                PerformLogin();
        }

        private void SetupButton_Click(object? sender, EventArgs e)
        {
            _isSetupMode = true;
            UpdateUI();
            _passwordTextBox.Clear();
            _passwordTextBox.Focus();
        }

        private void SetupPassword()
        {
            var password = _passwordTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Please enter a password");
                return;
            }

            if (password.Length < 8)
            {
                ShowError("Password must be at least 8 characters long");
                return;
            }

            var result = _authManager.SetupMasterPassword(password);
            if (result.Success)
            {
                ShowSuccess("Master password set up successfully!");
                OnAuthenticated?.Invoke();
            }
            else
            {
                ShowError(result.Error ?? "Failed to set up password");
            }
        }

        private void PerformLogin()
        {
            var password = _passwordTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Please enter your password");
                return;
            }

            var result = _authManager.Authenticate(password);
            if (result.Success)
            {
                ShowSuccess("Login successful!");
                OnAuthenticated?.Invoke();
            }
            else
            {
                ShowError(result.Error ?? "Login failed");
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
