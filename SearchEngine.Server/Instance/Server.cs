using System.Net;
using System.Net.Sockets;
using SearchEngineData;
using SearchEngineData.InvertedIndex.Instance;
using SearchEngineServer.Helpers;

namespace SearchEngineServer.Instance;

public class Server
{
    private readonly InvertedIndex _invertedIndex;

    public Server(int port, InvertedIndex invertedIndex)
    {
        _invertedIndex = invertedIndex;
        var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, port));
        listener.Listen(10);

        Console.WriteLine($"Server is listening on port {port}. Press any key to exit.");

        while (true)
        {
            var clientSocket = listener.Accept();
            
            try
            {
                Thread clientThread = new(() => ClientHandler.HandleClient(clientSocket, invertedIndex));
                clientThread.Start(clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
