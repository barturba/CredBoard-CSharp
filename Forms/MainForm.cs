using System;
using System.Windows.Forms;
using CredBoard.Utils;

namespace CredBoard.Forms
{
    /// <summary>
    /// Main application window that manages navigation between login and dashboard
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly AuthManager _authManager;
        private Form? _currentForm;

        public MainForm()
        {
            InitializeComponent();
            _authManager = AuthManager.Instance;

            // Set up the main form
            Text = "CredBoard - Secure Credential Management";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new System.Drawing.Size(1200, 800);
            MinimumSize = new System.Drawing.Size(800, 600);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = true;

            // Try to restore authentication on startup
            var authResult = _authManager.TryRestoreAuthentication();
            if (authResult.Success)
            {
                ShowDashboard();
            }
            else
            {
                ShowLoginScreen();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main form setup is done in constructor
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Name = "MainForm";

            this.ResumeLayout(false);
        }

        /// <summary>
        /// Shows the login screen
        /// </summary>
        public void ShowLoginScreen()
        {
            HideCurrentForm();

            var loginForm = new LoginForm();
            loginForm.OnAuthenticated += () => ShowDashboard();
            ShowForm(loginForm);
        }

        /// <summary>
        /// Shows the dashboard
        /// </summary>
        public void ShowDashboard()
        {
            HideCurrentForm();

            var dashboardForm = new DashboardForm();
            dashboardForm.OnLogout += () => ShowLoginScreen();
            ShowForm(dashboardForm);
        }

        /// <summary>
        /// Shows a form as the current content
        /// </summary>
        private void ShowForm(Form form)
        {
            _currentForm = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            Controls.Add(form);
            form.Show();
            form.BringToFront();
        }

        /// <summary>
        /// Hides the currently displayed form
        /// </summary>
        private void HideCurrentForm()
        {
            if (_currentForm != null)
            {
                Controls.Remove(_currentForm);
                _currentForm.Dispose();
                _currentForm = null;
            }
        }

        /// <summary>
        /// Handles form closing to ensure proper cleanup
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            HideCurrentForm();
            base.OnFormClosing(e);
        }
    }
}
