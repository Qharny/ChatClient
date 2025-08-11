using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class MainForm : Form
    {
        private ClientConnection _connection;
        private string _currentUsername;
        private List<string> _onlineUsers = new List<string>();

        // UI Controls
        private TextBox txtServerIP;
        private TextBox txtPort;
        private TextBox txtUsername;
        private Button btnConnect;
        private ListBox lstChatMessages;
        private TextBox txtMessage;
        private Button btnSend;
        private ListBox lstOnlineUsers;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private Label lblServerIP;
        private Label lblPort;
        private Label lblUsername;
        private Label lblChat;
        private Label lblOnlineUsers;
        private Label lblMessage;

        public MainForm()
        {
            InitializeComponent();
            InitializeConnection();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Chat Client";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Create controls
            CreateControls();
            LayoutControls();

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void CreateControls()
        {
            // Connection panel
            lblServerIP = new Label
            {
                Text = "Server IP:",
                Location = new Point(10, 15),
                Size = new Size(70, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            txtServerIP = new TextBox
            {
                Location = new Point(85, 12),
                Size = new Size(120, 23),
                Text = "127.0.0.1"
            };

            lblPort = new Label
            {
                Text = "Port:",
                Location = new Point(220, 15),
                Size = new Size(40, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            txtPort = new TextBox
            {
                Location = new Point(265, 12),
                Size = new Size(60, 23),
                Text = "8888"
            };

            lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(340, 15),
                Size = new Size(70, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            txtUsername = new TextBox
            {
                Location = new Point(415, 12),
                Size = new Size(120, 23),
                Text = "User" + new Random().Next(1000, 9999)
            };

            btnConnect = new Button
            {
                Text = "Connect",
                Location = new Point(550, 10),
                Size = new Size(80, 28)
            };

            // Chat messages
            lblChat = new Label
            {
                Text = "Chat Messages:",
                Location = new Point(10, 50),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            lstChatMessages = new ListBox
            {
                Location = new Point(10, 75),
                Size = new Size(500, 300),
                Font = new Font("Consolas", 9F)
            };

            // Online users
            lblOnlineUsers = new Label
            {
                Text = "Online Users:",
                Location = new Point(520, 50),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            lstOnlineUsers = new ListBox
            {
                Location = new Point(520, 75),
                Size = new Size(250, 300)
            };

            // Message input
            lblMessage = new Label
            {
                Text = "Message:",
                Location = new Point(10, 390),
                Size = new Size(60, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            txtMessage = new TextBox
            {
                Location = new Point(75, 387),
                Size = new Size(435, 23),
                Multiline = false
            };

            btnSend = new Button
            {
                Text = "Send",
                Location = new Point(520, 385),
                Size = new Size(80, 28),
                Enabled = false
            };

            // Status strip
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel("Disconnected");
            statusStrip.Items.Add(lblStatus);

            // Add controls to form
            this.Controls.AddRange(new Control[]
            {
                lblServerIP, txtServerIP,
                lblPort, txtPort,
                lblUsername, txtUsername,
                btnConnect,
                lblChat, lstChatMessages,
                lblOnlineUsers, lstOnlineUsers,
                lblMessage, txtMessage, btnSend,
                statusStrip
            });

            // Wire up events
            btnConnect.Click += BtnConnect_Click;
            btnSend.Click += BtnSend_Click;
            txtMessage.KeyPress += TxtMessage_KeyPress;
            lstOnlineUsers.DoubleClick += LstOnlineUsers_DoubleClick;
        }

        private void LayoutControls()
        {
            // Set form minimum size
            this.MinimumSize = new Size(800, 600);
        }

        private void InitializeConnection()
        {
            _connection = new ClientConnection();
            _connection.MessageReceived += Connection_MessageReceived;
            _connection.ConnectionStatusChanged += Connection_ConnectionStatusChanged;
            _connection.ErrorOccurred += Connection_ErrorOccurred;
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            if (!_connection.IsConnected)
            {
                await ConnectToServer();
            }
            else
            {
                DisconnectFromServer();
            }
        }

        private async Task ConnectToServer()
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtServerIP.Text))
                {
                    MessageBox.Show("Please enter a server IP address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtPort.Text, out int port) || port <= 0 || port > 65535)
                {
                    MessageBox.Show("Please enter a valid port number (1-65535).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Please enter a username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Disable controls during connection
                SetConnectionControls(false);
                _currentUsername = txtUsername.Text.Trim();

                // Connect to server
                await _connection.ConnectAsync(txtServerIP.Text.Trim(), port);

                // Send join message
                var joinMessage = new ChatMessage("join", _currentUsername, "", "joined the chat");
                await _connection.SendMessageAsync(joinMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetConnectionControls(true);
            }
        }

        private void DisconnectFromServer()
        {
            try
            {
                _connection.Disconnect();
                SetConnectionControls(true);
                _onlineUsers.Clear();
                lstOnlineUsers.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error disconnecting: {ex.Message}", "Disconnect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetConnectionControls(bool enable)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SetConnectionControls(enable)));
                return;
            }

            txtServerIP.Enabled = enable;
            txtPort.Enabled = enable;
            txtUsername.Enabled = enable;
            btnConnect.Text = enable ? "Connect" : "Disconnect";
            btnSend.Enabled = !enable;
            txtMessage.Enabled = !enable;
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            await SendMessage();
        }

        private async void TxtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                await SendMessage();
            }
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text.Trim()))
                return;

            try
            {
                string message = txtMessage.Text.Trim();
                await _connection.SendBroadcastMessageAsync(_currentUsername, message);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send message: {ex.Message}", "Send Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LstOnlineUsers_DoubleClick(object sender, EventArgs e)
        {
            if (lstOnlineUsers.SelectedItem != null)
            {
                string selectedUser = lstOnlineUsers.SelectedItem.ToString();
                if (selectedUser != _currentUsername)
                {
                    string privateMessage = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Enter private message for {selectedUser}:",
                        "Private Message",
                        "");

                    if (!string.IsNullOrWhiteSpace(privateMessage))
                    {
                        SendPrivateMessage(selectedUser, privateMessage);
                    }
                }
            }
        }

        private async void SendPrivateMessage(string toUser, string message)
        {
            try
            {
                await _connection.SendPrivateMessageAsync(_currentUsername, toUser, message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send private message: {ex.Message}", "Send Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Connection_MessageReceived(object sender, ChatMessage message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => Connection_MessageReceived(sender, message)));
                return;
            }

            // Handle different message types
            switch (message.Type.ToLower())
            {
                case "userlist":
                    UpdateOnlineUsers(message.Message);
                    break;
                case "system":
                    AddSystemMessage(message.Message);
                    break;
                default:
                    AddChatMessage(message);
                    break;
            }
        }

        private void UpdateOnlineUsers(string userListJson)
        {
            try
            {
                var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(userListJson);
                lstOnlineUsers.Items.Clear();
                _onlineUsers.Clear();
                
                foreach (var user in users)
                {
                    if (user != _currentUsername)
                    {
                        lstOnlineUsers.Items.Add(user);
                        _onlineUsers.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                AddSystemMessage($"Error updating user list: {ex.Message}");
            }
        }

        private void AddChatMessage(ChatMessage message)
        {
            lstChatMessages.Items.Add(message.GetDisplayText());
            lstChatMessages.SelectedIndex = lstChatMessages.Items.Count - 1;
        }

        private void AddSystemMessage(string message)
        {
            var systemMessage = new ChatMessage("system", "", "", message);
            lstChatMessages.Items.Add(systemMessage.GetDisplayText());
            lstChatMessages.SelectedIndex = lstChatMessages.Items.Count - 1;
        }

        private void Connection_ConnectionStatusChanged(object sender, bool isConnected)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => Connection_ConnectionStatusChanged(sender, isConnected)));
                return;
            }

            lblStatus.Text = isConnected ? "Connected" : "Disconnected";
            SetConnectionControls(!isConnected);
        }

        private void Connection_ErrorOccurred(object sender, string error)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => Connection_ErrorOccurred(sender, error)));
                return;
            }

            AddSystemMessage($"Error: {error}");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _connection?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
