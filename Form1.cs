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
        private Panel pnlConnection;
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
        private Panel pnlChat;
        private Panel pnlUsers;
        private Panel pnlInput;

        // Modern colors
        private Color PrimaryColor = Color.FromArgb(52, 152, 219);
        private Color SecondaryColor = Color.FromArgb(41, 128, 185);
        private Color BackgroundColor = Color.FromArgb(236, 240, 241);
        private Color PanelColor = Color.White;
        private Color TextColor = Color.FromArgb(44, 62, 80);
        private Color SuccessColor = Color.FromArgb(46, 204, 113);
        private Color ErrorColor = Color.FromArgb(231, 76, 60);

        public MainForm()
        {
            InitializeComponent();
            InitializeConnection();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Modern Chat Client";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = BackgroundColor;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Create controls
            CreateControls();
            LayoutControls();

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void CreateControls()
        {
            // Connection panel
            pnlConnection = new Panel
            {
                BackColor = PanelColor,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15),
                Height = 80
            };

            lblServerIP = new Label
            {
                Text = "Server IP:",
                Location = new Point(15, 20),
                Size = new Size(70, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextColor
            };

            txtServerIP = new TextBox
            {
                Location = new Point(90, 17),
                Size = new Size(120, 25),
                Text = "127.0.0.1",
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblPort = new Label
            {
                Text = "Port:",
                Location = new Point(225, 20),
                Size = new Size(40, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextColor
            };

            txtPort = new TextBox
            {
                Location = new Point(270, 17),
                Size = new Size(60, 25),
                Text = "8888",
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(345, 20),
                Size = new Size(70, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextColor
            };

            txtUsername = new TextBox
            {
                Location = new Point(420, 17),
                Size = new Size(120, 25),
                Text = "User" + new Random().Next(1000, 9999),
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnConnect = new Button
            {
                Text = "Connect",
                Location = new Point(555, 15),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };

            // Chat panel
            pnlChat = new Panel
            {
                BackColor = PanelColor,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15)
            };

            lblChat = new Label
            {
                Text = "Chat Messages",
                Location = new Point(15, 15),
                Size = new Size(150, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = TextColor
            };

            lstChatMessages = new ListBox
            {
                Location = new Point(15, 45),
                Size = new Size(600, 400),
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = TextColor
            };

            // Users panel
            pnlUsers = new Panel
            {
                BackColor = PanelColor,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15)
            };

            lblOnlineUsers = new Label
            {
                Text = "Online Users",
                Location = new Point(15, 15),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = TextColor
            };

            lstOnlineUsers = new ListBox
            {
                Location = new Point(15, 45),
                Size = new Size(250, 400),
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = TextColor
            };

            // Input panel
            pnlInput = new Panel
            {
                BackColor = PanelColor,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15),
                Height = 80
            };

            lblMessage = new Label
            {
                Text = "Message:",
                Location = new Point(15, 20),
                Size = new Size(70, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextColor
            };

            txtMessage = new TextBox
            {
                Location = new Point(90, 17),
                Size = new Size(500, 25),
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = false
            };

            btnSend = new Button
            {
                Text = "Send",
                Location = new Point(600, 15),
                Size = new Size(80, 30),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = SuccessColor,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Enabled = false
            };

            // Status strip
            statusStrip = new StatusStrip
            {
                BackColor = PanelColor,
                ForeColor = TextColor
            };
            lblStatus = new ToolStripStatusLabel("Disconnected")
            {
                Font = new Font("Segoe UI", 9F)
            };
            statusStrip.Items.Add(lblStatus);

            // Add controls to panels
            pnlConnection.Controls.AddRange(new Control[]
            {
                lblServerIP, txtServerIP,
                lblPort, txtPort,
                lblUsername, txtUsername,
                btnConnect
            });

            pnlChat.Controls.AddRange(new Control[]
            {
                lblChat, lstChatMessages
            });

            pnlUsers.Controls.AddRange(new Control[]
            {
                lblOnlineUsers, lstOnlineUsers
            });

            pnlInput.Controls.AddRange(new Control[]
            {
                lblMessage, txtMessage, btnSend
            });

            // Add panels to form
            this.Controls.AddRange(new Control[]
            {
                pnlConnection, pnlChat, pnlUsers, pnlInput, statusStrip
            });

            // Wire up events
            btnConnect.Click += BtnConnect_Click;
            btnSend.Click += BtnSend_Click;
            txtMessage.KeyPress += TxtMessage_KeyPress;
            lstOnlineUsers.DoubleClick += LstOnlineUsers_DoubleClick;

            // Add hover effects
            btnConnect.MouseEnter += (s, e) => btnConnect.BackColor = SecondaryColor;
            btnConnect.MouseLeave += (s, e) => btnConnect.BackColor = PrimaryColor;
            btnSend.MouseEnter += (s, e) => btnSend.BackColor = Color.FromArgb(39, 174, 96);
            btnSend.MouseLeave += (s, e) => btnSend.BackColor = SuccessColor;
        }

        private void LayoutControls()
        {
            // Set form minimum size
            this.MinimumSize = new Size(1000, 700);

            // Layout panels
            pnlConnection.Location = new Point(0, 0);
            pnlConnection.Width = this.ClientSize.Width;
            pnlConnection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            pnlChat.Location = new Point(0, 80);
            pnlChat.Width = this.ClientSize.Width - 300;
            pnlChat.Height = this.ClientSize.Height - 200;
            pnlChat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            pnlUsers.Location = new Point(this.ClientSize.Width - 280, 80);
            pnlUsers.Width = 280;
            pnlUsers.Height = this.ClientSize.Height - 200;
            pnlUsers.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            pnlInput.Location = new Point(0, this.ClientSize.Height - 120);
            pnlInput.Width = this.ClientSize.Width;
            pnlInput.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            statusStrip.Location = new Point(0, this.ClientSize.Height - 22);
            statusStrip.Width = this.ClientSize.Width;
            statusStrip.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
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

                // Wait a moment for connection to stabilize
                await Task.Delay(500);

                // Send join message
                try
                {
                    var connectMessage = new ChatMessage("connect", _currentUsername, "", "");
                    await _connection.SendMessageAsync(connectMessage);
                    
                    // Request current user list
                    await RequestUserList();
                }
                catch (Exception ex)
                {
                    AddSystemMessage($"Warning: Could not send connect message: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetConnectionControls(true);
            }
        }

        private async void DisconnectFromServer()
        {
            try
            {
                // Send leave message if connected
                if (_connection.IsConnected)
                {
                    try
                    {
                        var disconnectMessage = new ChatMessage("disconnect", _currentUsername, "", "");
                        await _connection.SendMessageAsync(disconnectMessage);
                        
                        // Wait a moment for the message to be sent
                        await Task.Delay(200);
                    }
                    catch (Exception ex)
                    {
                        AddSystemMessage($"Warning: Could not send disconnect message: {ex.Message}");
                    }
                }

                _connection.Disconnect();
                SetConnectionControls(true);
                _onlineUsers.Clear();
                lstOnlineUsers.Items.Clear();
                AddSystemMessage("Disconnected from server");
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
            btnConnect.BackColor = enable ? PrimaryColor : ErrorColor;
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

        private async Task RequestUserList()
        {
            try
            {
                var userListRequest = new ChatMessage("userlist", _currentUsername, "", "");
                await _connection.SendMessageAsync(userListRequest);
            }
            catch (Exception ex)
            {
                AddSystemMessage($"Warning: Could not request user list: {ex.Message}");
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
            switch (message.Type?.ToLower())
            {
                case "userlist":
                    UpdateOnlineUsers(message.Message);
                    break;
                case "system":
                    AddSystemMessage(message.Message);
                    break;
                case "chat":
                case "broadcast":
                    AddChatMessage(message);
                    break;
                case "private":
                    AddPrivateMessage(message);
                    break;
                case "connect":
                    AddSystemMessage($"{message.From} joined the chat");
                    break;
                case "disconnect":
                    AddSystemMessage($"{message.From} left the chat");
                    break;
                case null:
                case "":
                    // Handle messages without a type
                    if (!string.IsNullOrEmpty(message.Message))
                    {
                        AddSystemMessage(message.Message);
                    }
                    break;
                default:
                    // Handle unknown message types
                    AddSystemMessage($"Unknown message type '{message.Type}': {message.Message}");
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
                
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        if (!string.IsNullOrWhiteSpace(user) && user != _currentUsername)
                        {
                            lstOnlineUsers.Items.Add(user);
                            _onlineUsers.Add(user);
                        }
                    }
                }
                
                // Update the online users label to show count
                lblOnlineUsers.Text = $"Online Users ({lstOnlineUsers.Items.Count})";
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

        private void AddPrivateMessage(ChatMessage message)
        {
            string displayText = $"[{message.Timestamp:HH:mm:ss}] {message.From} -> {message.To}: {message.Message}";
            lstChatMessages.Items.Add(displayText);
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
