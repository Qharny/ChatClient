# Chat Client Testing Guide

## 🧪 **Testing Methods**

### **Method 1: Multiple Instances on Same Machine (Recommended)**

#### **Quick Start:**
1. **Build the project:**
   ```bash
   dotnet build
   ```

2. **Launch multiple clients:**
   - **Option A:** Run the batch file:
     ```bash
     test_multiple_users.bat
     ```
   
   - **Option B:** Use PowerShell:
     ```powershell
     .\test_chat.ps1 -NumberOfClients 4
     ```
   
   - **Option C:** Manual launch:
     - Open multiple command prompts
     - Navigate to your project directory
     - Run: `dotnet run` in each prompt

#### **Testing Steps:**
1. **Set up each client:**
   - Client 1: Username = "Alice"
   - Client 2: Username = "Bob" 
   - Client 3: Username = "Charlie"
   - Client 4: Username = "Diana"

2. **Connect to server:**
   - Server IP: `127.0.0.1`
   - Port: `8888` (or your server's port)
   - Click "Connect" on each client

3. **Test features:**
   - ✅ **Broadcast messages:** Type in message box and press Enter
   - ✅ **Private messages:** Double-click on a user in the online users list
   - ✅ **User list:** Check if all users appear in the online users list
   - ✅ **Disconnect/Reconnect:** Test the disconnect button and reconnection

### **Method 2: Network Testing (Multiple Computers)**

#### **Setup:**
1. **On Server Computer:**
   - Run your chat server
   - Note the IP address and port

2. **On Client Computers:**
   - Copy the built application to each computer
   - Run `ChatClient.exe`
   - Connect using the server's IP address

#### **Network Configuration:**
- **Firewall:** Allow the application through Windows Firewall
- **Port:** Ensure the server port is open and accessible
- **IP Address:** Use the actual server IP (not localhost)

### **Method 3: Using the Test Server**

If you don't have a chat server, use the included test server:

1. **Start the test server:**
   ```bash
   dotnet run --project TestServerLauncher.cs
   ```

2. **Launch multiple clients** using any method above

3. **Test with the test server** - it supports all the same features

## 🎯 **Test Scenarios**

### **Basic Functionality Tests:**
- [ ] **Connection:** Can connect to server
- [ ] **Disconnection:** Can disconnect properly
- [ ] **Reconnection:** Can reconnect after disconnection
- [ ] **Username:** Username is displayed correctly
- [ ] **Status:** Status bar shows correct connection state

### **Messaging Tests:**
- [ ] **Broadcast:** Messages sent to all users
- [ ] **Private:** Private messages work correctly
- [ ] **Message Display:** Messages show with correct formatting
- [ ] **Timestamps:** Timestamps are displayed correctly
- [ ] **Special Characters:** Messages with special characters work

### **User Management Tests:**
- [ ] **User List:** Online users are displayed
- [ ] **Join Notifications:** New users joining are announced
- [ ] **Leave Notifications:** Users leaving are announced
- [ ] **User Count:** User count updates correctly

### **Error Handling Tests:**
- [ ] **Invalid IP:** Error message for invalid server IP
- [ ] **Invalid Port:** Error message for invalid port
- [ ] **Server Down:** Graceful handling when server is down
- [ ] **Network Issues:** Handling of network interruptions

### **UI Tests:**
- [ ] **Responsive:** UI adapts to window resizing
- [ ] **Modern Look:** Modern styling is applied
- [ ] **Button States:** Buttons change state correctly
- [ ] **Hover Effects:** Hover effects work on buttons

## 🐛 **Common Issues & Solutions**

### **"Connection Refused" Error:**
- **Cause:** Server not running or wrong port
- **Solution:** Start your chat server or use the test server

### **"Invalid JSON Format" Messages:**
- **Cause:** Server sending non-JSON messages
- **Solution:** The client now handles this gracefully

### **"Unknown Message Type" Messages:**
- **Cause:** Server sending unexpected message types
- **Solution:** The client displays these as system messages

### **Multiple Clients Not Connecting:**
- **Cause:** Server not handling multiple connections
- **Solution:** Ensure your server supports multiple clients

## 📊 **Performance Testing**

### **Load Testing:**
- Test with 10+ simultaneous clients
- Monitor memory usage
- Check for message delays
- Test with long messages

### **Stress Testing:**
- Rapid connect/disconnect cycles
- High message frequency
- Large message content
- Network interruption simulation

## 🔧 **Debugging Tips**

### **Enable Debug Output:**
- Check the console output for error messages
- Monitor the status bar for connection state
- Look for system messages in the chat

### **Network Debugging:**
- Use `netstat -an` to check port usage
- Use `telnet` to test server connectivity
- Check Windows Firewall settings

### **Client Debugging:**
- Check the application logs
- Monitor memory usage in Task Manager
- Test with different network configurations

## 📝 **Test Checklist**

### **Pre-Testing:**
- [ ] Project builds successfully
- [ ] Server is running and accessible
- [ ] Network connectivity confirmed
- [ ] Firewall settings configured

### **During Testing:**
- [ ] All clients can connect
- [ ] Messages are delivered correctly
- [ ] User list updates properly
- [ ] Private messaging works
- [ ] Disconnect/reconnect works
- [ ] UI responds correctly

### **Post-Testing:**
- [ ] All clients disconnect cleanly
- [ ] No memory leaks detected
- [ ] Server handles all connections properly
- [ ] Error messages are helpful

## 🚀 **Advanced Testing**

### **Automated Testing:**
Consider creating automated tests for:
- Connection/disconnection cycles
- Message delivery verification
- User list synchronization
- Error condition handling

### **Cross-Platform Testing:**
If deploying to different environments:
- Test on different Windows versions
- Test with different .NET Framework versions
- Test with different network configurations

---

**Happy Testing! 🎉** 