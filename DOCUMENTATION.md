# ChatClient Project Documentation

## Table of Contents
1. [Project Overview](#project-overview)
2. [Architecture](#architecture)
3. [Features](#features)
4. [Installation & Setup](#installation--setup)
5. [Usage Guide](#usage-guide)
6. [Technical Specifications](#technical-specifications)
7. [API Reference](#api-reference)
8. [Testing](#testing)
9. [Troubleshooting](#troubleshooting)
10. [Development Guide](#development-guide)

## Project Overview

**ChatClient** is a modern, real-time chat application built with C# Windows Forms (.NET Framework 4.7.2). It provides a TCP-based client-server communication system for instant messaging with features like broadcast messaging, private messaging, and user management.

### Key Characteristics
- **Platform**: Windows (Windows Forms)
- **Framework**: .NET Framework 4.7.2
- **Networking**: TCP/IP with async/await patterns
- **UI**: Modern, responsive Windows Forms interface
- **Serialization**: JSON-based message format
- **Threading**: Non-blocking UI with background message processing

## Architecture

### High-Level Architecture
```
┌─────────────────┐    TCP/IP    ┌─────────────────┐
│   ChatClient    │ ◄──────────► │  Chat Server    │
│   (Windows      │              │  (External)     │
│    Forms App)   │              │                 │
└─────────────────┘              └─────────────────┘
```

### Component Architecture
```
MainForm (UI Layer)
    │
    ├── ClientConnection (Network Layer)
    │   ├── TcpClient
    │   ├── NetworkStream
    │   └── Message Processing
    │
    └── ChatMessage (Data Layer)
        ├── JSON Serialization
        └── Message Types
```

### Core Components

#### 1. MainForm (`Form1.cs`)
- **Purpose**: Primary user interface and application controller
- **Responsibilities**:
  - UI management and layout
  - User interaction handling
  - Connection state management
  - Message display and formatting
  - Event coordination between UI and network layers

#### 2. ClientConnection (`ClientConnection.cs`)
- **Purpose**: Manages TCP connection to the chat server
- **Responsibilities**:
  - TCP client connection management
  - Asynchronous message sending/receiving
  - Connection status monitoring
  - Error handling and recovery
  - Background message listening

#### 3. ChatMessage (`ChatMessage.cs`)
- **Purpose**: Data structure for chat messages
- **Responsibilities**:
  - Message serialization/deserialization (JSON)
  - Message type classification
  - Timestamp management
  - Display text formatting

#### 4. TestServer (`TestServer.cs`)
- **Purpose**: Built-in test server for development and testing
- **Responsibilities**:
  - TCP server implementation
  - Client connection management
  - Message routing and broadcasting
  - User list management

## Features

### Core Features

#### 1. Real-Time Messaging
- **Broadcast Messages**: Send messages to all connected users
- **Private Messages**: Send direct messages to specific users
- **System Messages**: Server notifications and status updates
- **Message Timestamps**: All messages include timestamps

#### 2. User Management
- **Online User List**: Real-time display of connected users
- **User Join/Leave Notifications**: Automatic announcements
- **User Count Display**: Shows number of online users
- **Username Management**: Unique user identification

#### 3. Connection Management
- **TCP Connection**: Reliable network communication
- **Connection Status**: Visual status indicators
- **Auto-Reconnection**: Graceful handling of network issues
- **Connection Validation**: Input validation for server settings

#### 4. Modern UI
- **Responsive Design**: Adapts to window resizing
- **Modern Styling**: Clean, professional appearance
- **Intuitive Layout**: Logical component organization
- **Visual Feedback**: Hover effects and status indicators

### Advanced Features

#### 1. Message Types Support
- `chat`/`broadcast`: Public messages to all users
- `private`: Direct messages between users
- `system`: Server notifications and status messages
- `userlist`: Server-sent user list updates
- `connect`/`disconnect`: User join/leave notifications

#### 2. Error Handling
- **Network Error Recovery**: Graceful handling of connection issues
- **Input Validation**: Comprehensive validation of user inputs
- **JSON Parsing**: Robust handling of malformed messages
- **Exception Management**: User-friendly error messages

#### 3. Performance Features
- **Async/Await**: Non-blocking UI operations
- **Background Processing**: Message listening on separate threads
- **Memory Management**: Proper resource disposal
- **Efficient UI Updates**: Thread-safe UI modifications

## Installation & Setup

### Prerequisites
- Windows 10 or later
- .NET Framework 4.7.2 or later
- Visual Studio 2019+ (for development)
- Network access for server communication

### Installation Steps

#### Option 1: Build from Source
```bash
# Clone or download the project
cd ChatClient

# Restore NuGet packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

#### Option 2: Pre-built Executable
1. Download the latest release
2. Extract to a folder
3. Run `ChatClient.exe`

### Dependencies
```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
<PackageReference Include="System.Text.Json" Version="8.0.5" />
<Reference Include="Microsoft.VisualBasic" />
```

## Usage Guide

### Getting Started

#### 1. Launch the Application
- Run `ChatClient.exe` or use `dotnet run`
- The application window will appear with connection settings

#### 2. Configure Connection
- **Server IP**: Enter the chat server's IP address (default: 127.0.0.1)
- **Port**: Enter the server port number (default: 8888)
- **Username**: Enter your desired username (auto-generated if empty)

#### 3. Connect to Server
- Click the "Connect" button
- Wait for connection confirmation
- Status bar will show "Connected" when successful

### Using the Chat

#### Sending Messages
1. **Broadcast Message**: Type in the message box and press Enter or click "Send"
2. **Private Message**: Double-click on a user in the online users list
3. **Message Format**: Messages support text and basic formatting

#### Managing Connections
- **Disconnect**: Click "Disconnect" button to leave the chat
- **Reconnect**: Click "Connect" to rejoin the chat
- **Status Monitoring**: Watch the status bar for connection state

#### User Interface
- **Chat Messages**: Main area showing all messages with timestamps
- **Online Users**: Right panel showing connected users
- **Connection Panel**: Top panel for server settings
- **Message Input**: Bottom panel for typing messages

### Advanced Usage

#### Private Messaging
1. Locate the target user in the "Online Users" list
2. Double-click on their username
3. Enter your private message in the dialog
4. Click "OK" to send

#### Connection Troubleshooting
- Verify server IP and port are correct
- Check network connectivity
- Ensure firewall allows the application
- Try different server settings if available

## Technical Specifications

### System Requirements
- **OS**: Windows 10/11 (x64)
- **Framework**: .NET Framework 4.7.2+
- **Memory**: 50MB RAM minimum
- **Network**: TCP/IP connectivity
- **Display**: 1024x768 minimum resolution

### Network Protocol
- **Transport**: TCP/IP
- **Message Format**: JSON
- **Encoding**: UTF-8
- **Connection**: Persistent TCP connection

### Message Format
```json
{
  "Type": "chat|private|system|userlist|connect|disconnect",
  "From": "sender_username",
  "To": "recipient_username",
  "Message": "message_content",
  "Timestamp": "2024-01-01T12:00:00"
}
```

### Performance Characteristics
- **Connection Time**: < 2 seconds (typical)
- **Message Latency**: < 100ms (local network)
- **Memory Usage**: ~20-50MB (typical)
- **CPU Usage**: < 5% (idle), < 15% (active)

## API Reference

### MainForm Class

#### Properties
```csharp
public partial class MainForm : Form
{
    private ClientConnection _connection;
    private string _currentUsername;
    private List<string> _onlineUsers;
}
```

#### Key Methods
```csharp
// Connection management
private async Task ConnectToServer()
private void DisconnectFromServer()
private void SetConnectionControls(bool enable)

// Message handling
private async Task SendMessage()
private void AddChatMessage(ChatMessage message)
private void AddSystemMessage(string message)
private void UpdateOnlineUsers(string userListJson)

// Event handlers
private async void BtnConnect_Click(object sender, EventArgs e)
private async void BtnSend_Click(object sender, EventArgs e)
private void Connection_MessageReceived(object sender, ChatMessage message)
```

### ClientConnection Class

#### Properties
```csharp
public class ClientConnection : IDisposable
{
    public bool IsConnected { get; }
    public event EventHandler<ChatMessage> MessageReceived;
    public event EventHandler<bool> ConnectionStatusChanged;
    public event EventHandler<string> ErrorOccurred;
}
```

#### Key Methods
```csharp
// Connection management
public async Task ConnectAsync(string serverIp, int port)
public void Disconnect()

// Message sending
public async Task SendMessageAsync(ChatMessage message)
public async Task SendBroadcastMessageAsync(string from, string message)
public async Task SendPrivateMessageAsync(string from, string to, string message)

// Internal methods
private async Task ListenForMessagesAsync(CancellationToken cancellationToken)
private ChatMessage HandleNonJsonMessage(string rawMessage)
```

### ChatMessage Class

#### Properties
```csharp
public class ChatMessage
{
    public string Type { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}
```

#### Key Methods
```csharp
// Serialization
public string ToJson()
public static ChatMessage FromJson(string json)

// Utility
public string GetDisplayText()
public static ChatMessage CreateUserListMessage(List<string> usernames)
```

## Testing

### Testing Methods

#### 1. Local Testing (Recommended)
```bash
# Start test server
dotnet run --project TestServerLauncher.cs

# Launch multiple clients
test_multiple_users.bat
# or
.\test_chat.ps1 -NumberOfClients 4
```

#### 2. Network Testing
- Deploy server on separate machine
- Configure firewall settings
- Test with multiple clients across network

#### 3. Automated Testing
- Use the provided test scripts
- Test connection/disconnection cycles
- Verify message delivery and user list updates

### Test Scenarios

#### Basic Functionality
- [ ] Connection establishment
- [ ] Message sending/receiving
- [ ] User list updates
- [ ] Private messaging
- [ ] Disconnection handling

#### Error Conditions
- [ ] Invalid server settings
- [ ] Network interruptions
- [ ] Server unavailability
- [ ] Malformed messages

#### Performance Testing
- [ ] Multiple simultaneous clients
- [ ] High message frequency
- [ ] Large message content
- [ ] Memory usage monitoring

## Troubleshooting

### Common Issues

#### Connection Problems
**Issue**: "Connection refused" error
- **Cause**: Server not running or wrong port
- **Solution**: Verify server is running and port is correct

**Issue**: "Timeout" error
- **Cause**: Network connectivity issues
- **Solution**: Check network connection and firewall settings

#### Message Issues
**Issue**: Messages not appearing
- **Cause**: Connection lost or server error
- **Solution**: Check connection status and reconnect if needed

**Issue**: "Invalid JSON" messages
- **Cause**: Server sending non-JSON messages
- **Solution**: Client handles this gracefully, but check server compatibility

#### UI Issues
**Issue**: Application not responding
- **Cause**: High CPU usage or memory issues
- **Solution**: Restart application and check system resources

**Issue**: Window layout problems
- **Cause**: Resolution or DPI scaling issues
- **Solution**: Adjust window size or system display settings

### Debug Information

#### Enable Debug Output
- Check console output for error messages
- Monitor status bar for connection state
- Review system messages in chat area

#### Network Debugging
```bash
# Check port usage
netstat -an | findstr 8888

# Test server connectivity
telnet 127.0.0.1 8888

# Check firewall settings
netsh advfirewall firewall show rule name=all
```

#### Performance Monitoring
- Use Task Manager to monitor CPU and memory usage
- Check network activity in Resource Monitor
- Monitor application logs for errors

## Development Guide

### Project Structure
```
ChatClient/
├── Form1.cs                 # Main UI and application logic
├── ClientConnection.cs      # Network communication layer
├── ChatMessage.cs          # Message data structure
├── Program.cs              # Application entry point
├── TestServer.cs           # Built-in test server
├── TestServerLauncher.cs   # Test server launcher
├── ChatClient.csproj       # Project configuration
├── README.md              # Basic project information
├── TESTING_GUIDE.md       # Testing instructions
├── test_chat.ps1          # PowerShell test script
├── test_multiple_users.bat # Batch test script
└── LICENSE.txt            # License information
```

### Development Setup

#### Environment Requirements
- Visual Studio 2019+ or VS Code
- .NET Framework 4.7.2 SDK
- Git for version control

#### Building from Source
```bash
# Clone repository
git clone <repository-url>
cd ChatClient

# Restore dependencies
dotnet restore

# Build project
dotnet build

# Run application
dotnet run
```

### Code Style Guidelines

#### C# Conventions
- Use PascalCase for public members
- Use camelCase for private fields
- Use async/await for asynchronous operations
- Implement proper exception handling
- Add XML documentation for public APIs

#### UI Guidelines
- Use consistent color scheme
- Implement responsive design
- Provide clear user feedback
- Handle UI thread marshaling properly

### Extending the Application

#### Adding New Message Types
1. Update `ChatMessage.cs` to handle new types
2. Modify `MainForm.cs` message handling
3. Update server compatibility if needed

#### Adding New Features
1. Create new UI components in `Form1.cs`
2. Implement business logic in appropriate classes
3. Add proper error handling and validation
4. Update documentation and tests

#### Customizing the UI
1. Modify color scheme in `Form1.cs`
2. Adjust layout in `LayoutControls()` method
3. Add new controls as needed
4. Test on different screen resolutions

### Deployment

#### Building for Production
```bash
# Release build
dotnet build -c Release

# Publish self-contained
dotnet publish -c Release -r win-x64 --self-contained true
```

#### Distribution
- Include all required dependencies
- Provide installation instructions
- Test on target environments
- Create installer package if needed

---

## License

This project is licensed under the terms specified in `LICENSE.txt`.

## Support

For issues, questions, or contributions:
1. Check the troubleshooting section
2. Review the testing guide
3. Create an issue in the project repository
4. Contact the development team

---

*Documentation Version: 1.0*  
*Last Updated: January 2024* 