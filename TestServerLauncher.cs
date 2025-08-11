using System;
using System.Threading.Tasks;

namespace ChatClient
{
    class TestServerLauncher
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Chat Client Test Server ===");
            Console.WriteLine("This server helps test the chat client functionality.");
            Console.WriteLine();

            int port = 8888;
            if (args.Length > 0 && int.TryParse(args[0], out int customPort))
            {
                port = customPort;
            }

            var server = new TestServer();
            
            Console.WriteLine($"Starting test server on port {port}...");
            Console.WriteLine("Press Ctrl+C to stop the server.");
            Console.WriteLine();

            try
            {
                await server.StartAsync(port);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error: {ex.Message}");
            }
            finally
            {
                server.Stop();
                Console.WriteLine("Server stopped.");
            }
        }
    }
} 