# PowerShell script to test multiple chat clients
param(
    [int]$NumberOfClients = 4,
    [string]$ServerIP = "127.0.0.1",
    [int]$Port = 8888
)

Write-Host "=== Chat Client Multi-User Testing ===" -ForegroundColor Green
Write-Host "Starting $NumberOfClients chat client instances..." -ForegroundColor Yellow
Write-Host "Server: $ServerIP`:$Port" -ForegroundColor Cyan
Write-Host ""

# Build the project first
Write-Host "Building project..." -ForegroundColor Yellow
dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed! Please fix any errors first." -ForegroundColor Red
    exit 1
}

# Launch multiple instances
for ($i = 1; $i -le $NumberOfClients; $i++) {
    $username = "TestUser$i"
    Write-Host "Starting Chat Client $i ($username)..." -ForegroundColor Green
    
    # Start the process
    Start-Process -FilePath "bin\Debug\net472\ChatClient.exe" -WindowStyle Normal -ArgumentList @()
    
    # Small delay to prevent overwhelming the system
    Start-Sleep -Milliseconds 500
}

Write-Host ""
Write-Host "=== Testing Instructions ===" -ForegroundColor Magenta
Write-Host "1. In each client window, set the username to: TestUser1, TestUser2, etc." -ForegroundColor White
Write-Host "2. Set Server IP to: $ServerIP" -ForegroundColor White
Write-Host "3. Set Port to: $Port" -ForegroundColor White
Write-Host "4. Click Connect on each client" -ForegroundColor White
Write-Host "5. Test the following features:" -ForegroundColor White
Write-Host "   - Send broadcast messages" -ForegroundColor Gray
Write-Host "   - Send private messages (double-click users)" -ForegroundColor Gray
Write-Host "   - Check online users list" -ForegroundColor Gray
Write-Host "   - Test disconnection/reconnection" -ForegroundColor Gray
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") 