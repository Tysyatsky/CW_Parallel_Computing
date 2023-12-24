using System.Net.Sockets;
using System.Net;
using System.Text;
using Data.Models;
using SearchEngineData;
using SearchEngineData.ThreadPool.Data.Models;

namespace SearchEngineServer;

public class Server
{
    private readonly InvertedIndex _invertedIndex;
    private readonly SearchEngineData.ThreadPool.Instances.ThreadPool _threadPool;

    public Server(int port, InvertedIndex invertedIndex, SearchEngineData.ThreadPool.Instances.ThreadPool threadPool)
    {
        _invertedIndex = invertedIndex;
        _threadPool = threadPool ?? throw new ArgumentNullException(nameof(threadPool));
        var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, port));
        listener.Listen(10);

        Console.WriteLine($"Server is listening on port {port}. Press any key to exit.");

        while (true)
        {
            var clientSocket = listener.Accept();
            try
            {
                Thread clientThread = new(HandleClientComm);
                clientThread.Start(clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    private void HandleClientComm(object? clientObj)
    {
        var clientSocket = (Socket)clientObj!;
        NetworkStream networkStream = new(clientSocket ?? throw new InvalidOperationException());
        var message = new byte[4096];

        while (true)
        {
            int bytesRead;
            try
            {
                bytesRead = networkStream.Read(message, 0, message.Length);
                if (bytesRead == 0)
                    break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            var clientMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
            Console.WriteLine($"Received: {clientMessage}");

            Thread.Sleep(3000);
            var result = new SearchTask(0,0, clientMessage, "", _invertedIndex).Execute();
            
            Thread.Sleep(3000);
            byte[] sendBytes = Encoding.ASCII.GetBytes(result);
            networkStream.Write(sendBytes, 0, sendBytes.Length);
        }

        clientSocket.Close();
    }
}
