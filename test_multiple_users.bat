@echo off
echo Starting multiple chat client instances for testing...
echo.

echo Starting Chat Client 1 (User1)...
start "Chat Client 1" "bin\Debug\net472\ChatClient.exe"

echo Starting Chat Client 2 (User2)...
start "Chat Client 2" "bin\Debug\net472\ChatClient.exe"

echo Starting Chat Client 3 (User3)...
start "Chat Client 3" "bin\Debug\net472\ChatClient.exe"

echo Starting Chat Client 4 (User4)...
start "Chat Client 4" "bin\Debug\net472\ChatClient.exe"

echo.
echo All chat clients launched!
echo.
echo Instructions:
echo 1. In each client, enter different usernames (e.g., User1, User2, User3, User4)
echo 2. Use server IP: 127.0.0.1 and port: 8888 (or your server's port)
echo 3. Click Connect on each client
echo 4. Start chatting to test the functionality
echo.
pause 