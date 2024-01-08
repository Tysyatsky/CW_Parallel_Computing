using System.Net.Sockets;
using System.Text;
using SearchEngineData;
using SearchEngineData.InvertedIndex.Instance;
using SearchEngineData.ThreadPool.Data.Models;

namespace SearchEngineServer.Helpers;

public class ClientHandler
{
    public void HandleClient(object? clientObj, InvertedIndex invertedIndex)
    {
        var clientSocket = (Socket)clientObj!;
        NetworkStream networkStream = new(clientSocket ?? throw new InvalidOperationException());
        
        var sendConnected = Encoding.ASCII.GetBytes("Connected!");
        networkStream.Write(sendConnected, 0, sendConnected.Length);
        
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
            var result = new SearchTask(clientMessage, invertedIndex).Execute();
            var sendBytes = Encoding.ASCII.GetBytes(result);
            networkStream.Write(sendBytes, 0, sendBytes.Length);
        }

        clientSocket.Close();
    }
}