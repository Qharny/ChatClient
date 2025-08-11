using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatClient
{
    /// <summary>
    /// Manages the TCP connection to the chat server
    /// </summary>
    public class ClientConnection : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isConnected;

        /// <summary>
        /// Event fired when a message is received
        /// </summary>
        public event EventHandler<ChatMessage> MessageReceived;

        /// <summary>
        /// Event fired when connection status changes
        /// </summary>
        public event EventHandler<bool> ConnectionStatusChanged;

        /// <summary>
        /// Event fired when an error occurs
        /// </summary>
        public event EventHandler<string> ErrorOccurred;

        /// <summary>
        /// Gets whether the client is currently connected
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// Connects to the server asynchronously
        /// </summary>
        public async Task ConnectAsync(string serverIp, int port)
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(serverIp, port);
                
                _stream = _client.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8);
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };
                _cancellationTokenSource = new CancellationTokenSource();
                
                _isConnected = true;
                ConnectionStatusChanged?.Invoke(this, true);
                
                // Start listening for messages
                _ = Task.Run(() => ListenForMessagesAsync(_cancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                _isConnected = false;
                ErrorOccurred?.Invoke(this, $"Failed to connect: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        public void Disconnect()
        {
            try
            {
                _cancellationTokenSource?.Cancel();
                _isConnected = false;
                
                _writer?.Close();
                _reader?.Close();
                _stream?.Close();
                _client?.Close();
                
                ConnectionStatusChanged?.Invoke(this, false);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error during disconnect: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a message to the server asynchronously
        /// </summary>
        public async Task SendMessageAsync(ChatMessage message)
        {
            if (!_isConnected || _writer == null)
            {
                throw new InvalidOperationException("Not connected to server");
            }

            try
            {
                string json = message.ToJson();
                await _writer.WriteLineAsync(json);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Failed to send message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Sends a broadcast message to all users
        /// </summary>
        public async Task SendBroadcastMessageAsync(string from, string message)
        {
            var chatMessage = new ChatMessage("chat", from, "", message);
            await SendMessageAsync(chatMessage);
        }

        /// <summary>
        /// Sends a private message to a specific user
        /// </summary>
        public async Task SendPrivateMessageAsync(string from, string to, string message)
        {
            var chatMessage = new ChatMessage("private", from, to, message);
            await SendMessageAsync(chatMessage);
        }

        /// <summary>
        /// Listens for incoming messages from the server
        /// </summary>
        private async Task ListenForMessagesAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (_isConnected && !cancellationToken.IsCancellationRequested)
                {
                    string line = await _reader.ReadLineAsync();
                    
                    if (line == null)
                    {
                        // Server closed the connection
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        try
                        {
                            ChatMessage message = ChatMessage.FromJson(line);
                            MessageReceived?.Invoke(this, message);
                        }
                        catch (Exception ex)
                        {
                            ErrorOccurred?.Invoke(this, $"Failed to parse message: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    ErrorOccurred?.Invoke(this, $"Connection error: {ex.Message}");
                }
            }
            finally
            {
                _isConnected = false;
                ConnectionStatusChanged?.Invoke(this, false);
            }
        }

        /// <summary>
        /// Disposes the connection and releases resources
        /// </summary>
        public void Dispose()
        {
            Disconnect();
            _cancellationTokenSource?.Dispose();
        }
    }
} 