﻿using System.Net.Sockets;

namespace Client
{
    internal static class Program
    {
        private const string ServerIp = "127.0.0.1";
        private const int ServerPort = 6000;

        private static void Main()
        {
            var clientSocket = EstablishConnection();
            if (clientSocket is null)
            {
                return;
            }
            
            Console.WriteLine("Enter word to search in documents");
            try
            {
                var messageToSend = Console.ReadLine() ?? "";
                ClientMessenger.SendMessage(clientSocket, messageToSend);
                
                while (true)
                {
                     var receivedMessage = ClientMessenger.ReceiveMessage(clientSocket);
                     Console.WriteLine($"Server response:\n{receivedMessage}");
                     return;
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

        private static Socket? EstablishConnection()
        {
            Socket clientSocket = new(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            
            try
            {
                clientSocket.Connect(ServerIp, ServerPort);
                Console.WriteLine("Connection successful!");
                return clientSocket;
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection was not established, try again");
                return null;
            }
        }
    }
}