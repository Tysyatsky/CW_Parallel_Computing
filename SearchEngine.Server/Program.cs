using System.Diagnostics;
using SearchEngineData;
using SearchEngineData.InvertedIndex.Instance;
using SearchEngineServer.Helpers;
using SearchEngineServer.Instance;
using static System.Int32;

namespace SearchEngineServer
{
    internal class Program
    {
        private const string Path = @"C:\Uni\CourseWork\CW_Parallel_Computing\Docs\";
        private const int Port = 6000;

        private static void Main()
        {
            InvertedIndex invertedIndex = new();
            Console.WriteLine("Enter thread count: ");
            var isParsed = TryParse(Console.ReadLine(), out var threadCount);

            if (!isParsed)
            {
                Console.WriteLine("Threads was not instantiated");
                return;
            }
            
            SearchEngineData.ThreadPool.Instances.ThreadPool threadPool = new(threadCount);
            
            FileFinder.FindFiles(invertedIndex, threadPool, Path);
            
            threadPool.Terminate();

            _ = new Server(Port, invertedIndex);

            Console.WriteLine($"Server is listening on port {Port}. Press any key to exit.");
            Console.ReadKey();
        }
    }
}