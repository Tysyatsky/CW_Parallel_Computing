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

                Console.WriteLine("Enter word to search in documents");

                // Send a message to the server
                string messageToSend = Console.ReadLine() ?? "";
                ClientMessanger.SendMessage(clientSocket, messageToSend);

                while (true)
                {
                    // Receive the server's response
                    string receivedMessage = ClientMessanger.ReceiveMessage(clientSocket);
                    Console.WriteLine($"Server response: {receivedMessage}");
                }
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