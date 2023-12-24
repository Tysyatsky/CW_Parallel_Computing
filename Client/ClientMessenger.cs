using System.Net.Sockets;
using System.Text;

namespace Client;

public static class ClientMessenger
{
    private const int ByteLenght = 4096;
    public static void SendMessage(Socket socket, string message)
    {
        var messageBytes = Encoding.ASCII.GetBytes(message);
        socket.Send(messageBytes);
    }

    public static string ReceiveMessage(Socket socket)
    {
        var receivedBytes = new byte[ByteLenght];
        var bytesRead = socket.Receive(receivedBytes);
        return Encoding.ASCII.GetString(receivedBytes, 0, bytesRead);
    }
}
