# Chat Client

A TCP-based chat client built with C# Windows Forms (.NET Framework 4.7.2) that connects to a chat server for real-time messaging.

## Features

- **TCP Connection**: Connects to a chat server using IP address and port
- **Real-time Messaging**: Send and receive messages in real-time
- **User Management**: View list of online users
- **Private Messaging**: Send private messages to specific users (double-click on user in list)
- **Connection Status**: Visual status indicator showing connection state
- **Async Communication**: Non-blocking UI using async/await patterns
- **JSON Message Format**: Compatible with server using JSON serialization

## UI Components

- **Connection Panel**: Server IP, Port, Username inputs and Connect/Disconnect button
- **Chat Messages**: ListBox displaying all chat messages with timestamps
- **Online Users**: ListBox showing currently connected users
- **Message Input**: TextBox for typing messages with Send button
- **Status Bar**: Shows current connection status

## Message Types

The client supports the following message types:
- `chat`: Broadcast messages to all users
- `private`: Private messages to specific users
- `system`: System notifications (user join/leave, errors)
- `userlist`: Server-sent list of online users
- `join`: User joining notification

## Usage

1. Enter the server IP address (default: 127.0.0.1)
2. Enter the port number (default: 8888)
3. Enter your username (auto-generated if left empty)
4. Click "Connect" to join the chat
5. Type messages in the message box and press Enter or click Send
6. Double-click on a user in the online users list to send a private message
7. Click "Disconnect" to leave the chat

## Technical Details

- **Target Framework**: .NET Framework 4.7.2
- **UI Framework**: Windows Forms
- **Networking**: TcpClient with async/await
- **Serialization**: Newtonsoft.Json
- **Threading**: Background message processing with UI thread marshaling

## Dependencies

- Newtonsoft.Json (13.0.3)
- Microsoft.VisualBasic (for InputBox)

## Building

1. Open the solution in Visual Studio
2. Restore NuGet packages
3. Build the solution
4. Run the application

## Server Compatibility

This client is designed to work with a TCP-based chat server that:
- Accepts JSON-formatted messages
- Uses the same message structure (Type, From, To, Message, Timestamp)
- Sends user list updates
- Handles broadcast and private messaging