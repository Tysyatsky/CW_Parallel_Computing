using System.Net.Sockets;
using System.Text;

namespace Client;

public class Client
{

    public static void SendMessage(Socket socket, string message)
    {
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        socket.Send(messageBytes);
    }

    public static string ReceiveMessage(Socket socket)
    {
        byte[] receivedBytes = new byte[4096];
        int bytesRead = socket.Receive(receivedBytes);
        return Encoding.ASCII.GetString(receivedBytes, 0, bytesRead);
    }
}
