using System.Net;
using System.Net.Sockets;
using System.Text;
using SearchEngineData.InvertedIndex.Instance;
using SearchEngineServer.Helpers;

namespace SearchEngineServer.Instance;

public class Server
{
    public Server(int port, InvertedIndex invertedIndex)
    {
        var clientHandler = new ClientHandler();
        var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, port));
        listener.Listen(10);

        Console.WriteLine($"Server is listening on port {port}. Press any key to exit.");

        while (true)
        {
            var clientSocket = listener.Accept();
            try
            {
                Thread clientThread = new(() => clientHandler.HandleClient(clientSocket, invertedIndex));
                clientThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
