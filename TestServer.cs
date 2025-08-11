using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatClient
{
    /// <summary>
    /// Simple test server for testing the chat client
    /// </summary>
    public class TestServer
    {
        private TcpListener _listener;
        private List<ClientInfo> _clients = new List<ClientInfo>();
        private bool _isRunning = false;
        private readonly object _lock = new object();

        public class ClientInfo
        {
            public TcpClient Client { get; set; }
            public StreamWriter Writer { get; set; }
            public string Username { get; set; }
        }

        public async Task StartAsync(int port = 8888)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            _isRunning = true;

            Console.WriteLine($"Test Server started on port {port}");
            Console.WriteLine("Waiting for connections...");

            while (_isRunning)
            {
                try
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClientAsync(client));
                }
                catch (Exception ex)
                {
                    if (_isRunning)
                        Console.WriteLine($"Error accepting client: {ex.Message}");
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            ClientInfo clientInfo = null;
            try
            {
                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                clientInfo = new ClientInfo { Client = client, Writer = writer };

                // Send welcome message
                var welcomeMsg = new ChatMessage("system", "", "", "Welcome to the test chat server!");
                await writer.WriteLineAsync(welcomeMsg.ToJson());

                // Send current user list
                await SendUserListAsync();

                while (client.Connected)
                {
                    string line = await reader.ReadLineAsync();
                    if (line == null) break;

                    try
                    {
                        ChatMessage message = ChatMessage.FromJson(line);
                        await ProcessMessageAsync(clientInfo, message);
                    }
                    catch (Exception ex)
                    {
                        var errorMsg = new ChatMessage("system", "", "", $"Error processing message: {ex.Message}");
                        await writer.WriteLineAsync(errorMsg.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                if (clientInfo != null)
                {
                    lock (_lock)
                    {
                        _clients.Remove(clientInfo);
                    }
                    await SendUserListAsync();
                }
                client.Close();
            }
        }

        private async Task ProcessMessageAsync(ClientInfo clientInfo, ChatMessage message)
        {
            switch (message.Type?.ToLower())
            {
                case "join":
                    clientInfo.Username = message.From;
                    lock (_lock)
                    {
                        _clients.Add(clientInfo);
                    }
                    
                    // Broadcast join message
                    var joinMsg = new ChatMessage("system", "", "", $"{message.From} joined the chat");
                    await BroadcastMessageAsync(joinMsg);
                    
                    // Send updated user list
                    await SendUserListAsync();
                    break;

                case "leave":
                    lock (_lock)
                    {
                        _clients.Remove(clientInfo);
                    }
                    
                    // Broadcast leave message
                    var leaveMsg = new ChatMessage("system", "", "", $"{message.From} left the chat");
                    await BroadcastMessageAsync(leaveMsg);
                    
                    // Send updated user list
                    await SendUserListAsync();
                    break;

                case "chat":
                case "broadcast":
                    // Broadcast to all clients
                    await BroadcastMessageAsync(message);
                    break;

                case "private":
                    // Send to specific user
                    await SendPrivateMessageAsync(message);
                    break;

                default:
                    // Echo back as system message
                    var echoMsg = new ChatMessage("system", "", "", $"Received: {message.Message}");
                    await clientInfo.Writer.WriteLineAsync(echoMsg.ToJson());
                    break;
            }
        }

        private async Task BroadcastMessageAsync(ChatMessage message)
        {
            List<ClientInfo> clientsCopy;
            lock (_lock)
            {
                clientsCopy = new List<ClientInfo>(_clients);
            }

            foreach (var client in clientsCopy)
            {
                try
                {
                    await client.Writer.WriteLineAsync(message.ToJson());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error broadcasting to {client.Username}: {ex.Message}");
                }
            }
        }

        private async Task SendPrivateMessageAsync(ChatMessage message)
        {
            List<ClientInfo> clientsCopy;
            lock (_lock)
            {
                clientsCopy = new List<ClientInfo>(_clients);
            }

            foreach (var client in clientsCopy)
            {
                if (client.Username == message.To || client.Username == message.From)
                {
                    try
                    {
                        await client.Writer.WriteLineAsync(message.ToJson());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending private message to {client.Username}: {ex.Message}");
                    }
                }
            }
        }

        private async Task SendUserListAsync()
        {
            List<string> usernames;
            lock (_lock)
            {
                usernames = _clients.ConvertAll(c => c.Username);
            }

            var userListMsg = new ChatMessage("userlist", "", "", JsonConvert.SerializeObject(usernames));
            await BroadcastMessageAsync(userListMsg);
        }

        public void Stop()
        {
            _isRunning = false;
            _listener?.Stop();
            
            lock (_lock)
            {
                foreach (var client in _clients)
                {
                    client.Client.Close();
                }
                _clients.Clear();
            }
        }
    }
} 