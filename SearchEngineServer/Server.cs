using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SearchEngineServer;

public class Server
{
    private readonly Socket _listener;

    public Server(int port)
    {
        _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _listener.Bind(new IPEndPoint(IPAddress.Any, port));
        _listener.Listen(10);

        Console.WriteLine($"Server is listening on port {port}. Press any key to exit.");

        while (true)
        {
            Socket clientSocket = _listener.Accept();
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
            clientThread.Start(clientSocket);
        }
    }

    private void HandleClientComm(object clientObj)
    {
        Socket clientSocket = (Socket)clientObj;
        NetworkStream networkStream = new NetworkStream(clientSocket);
        byte[] message = new byte[4096];
        int bytesRead;

        while (true)
        {
            bytesRead = networkStream.Read(message, 0, message.Length);
            if (bytesRead == 0)
                break;

            string clientMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
            Console.WriteLine($"Received: {clientMessage}");

            byte[] sendBytes = Encoding.ASCII.GetBytes("Server received your message.");
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            networkStream.Flush();
        }

        clientSocket.Close();
    }
}
