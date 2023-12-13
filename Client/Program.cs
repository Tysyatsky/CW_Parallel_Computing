using System.Net.Sockets;

namespace Client
{
    internal class Program
    {
        static void Main()
        {
            string serverIp = "127.0.0.1"; // Replace with the actual IP address or hostname of the server
            int serverPort = 6000; // Replace with the port number the server is listening on

            Socket clientSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                clientSocket.Connect(serverIp, serverPort);
                Console.WriteLine("Connected to the server.");

                // Send a message to the server
                string messageToSend = "Hello, server!";
                Client.SendMessage(clientSocket, messageToSend);

                // Receive the server's response
                string receivedMessage = Client.ReceiveMessage(clientSocket);
                Console.WriteLine($"Server response: {receivedMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
            }
        }
    }
}