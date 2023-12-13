using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SearchEngineServer
{
    internal class Program
    {
        static void Main()
        {
            int port = 6000; // Specify the port number you want the server to listen on
            _ = new Server(port);

            Console.WriteLine($"Server is listening on port {port}. Press any key to exit.");
            Console.ReadKey();
        }
    }
}