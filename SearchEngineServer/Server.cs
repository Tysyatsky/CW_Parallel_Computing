using System.Net.Sockets;
using System.Net;
using System.Text;
using SearchEngineData;

namespace SearchEngineServer;

public class Server
{
    private readonly Socket _listener;
    private readonly InvertedIndex _invertedIndex;

    public Server(int port, InvertedIndex invertedIndex)
    {
        _invertedIndex = invertedIndex;
        _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _listener.Bind(new IPEndPoint(IPAddress.Any, port));
        _listener.Listen(10);

        Console.WriteLine($"Server is listening on port {port}. Press any key to exit.");

        while (true)
        {
            Socket clientSocket = _listener.Accept();
            Thread clientThread = new(new ParameterizedThreadStart(HandleClientComm));
            clientThread.Start(clientSocket);
        }
    }

    private void HandleClientComm(object clientObj)
    {
        Socket clientSocket = (Socket)clientObj;
        NetworkStream networkStream = new(clientSocket);
        byte[] message = new byte[4096];
        int bytesRead;

        while (true)
        {
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

            string clientMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
            Console.WriteLine($"Received: {clientMessage}");

            List<int> result = _invertedIndex.Search(clientMessage);

            byte[] sendBytes = Encoding.ASCII.GetBytes("Server received your message.");
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            // networkStream.Flush();

            Console.WriteLine($"Documents containing '{clientMessage}':");
            foreach (var docId in result)
            {
                sendBytes = Encoding.ASCII.GetBytes($"Doc #{docId}");
                networkStream.Write(sendBytes, 0, sendBytes.Length);
            }
        }

        clientSocket.Close();
    }
}
