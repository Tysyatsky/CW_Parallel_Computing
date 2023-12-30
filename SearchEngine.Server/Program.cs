using SearchEngineData.InvertedIndex.Instance;
using SearchEngineServer.Helpers.FIleFinders;
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
            InvertedIndex invertedIndexThreadPool = new();
            InvertedIndex invertedIndexThreads = new();
            InvertedIndex invertedIndexSq = new();
            
            var fileFinderSq = new FileFinderSequential(invertedIndexSq);
            fileFinderSq.FindFiles(Path);
            
            Console.WriteLine("Enter thread count: ");
            var isParsed = TryParse(Console.ReadLine(), out var threadCount);

            if (!isParsed)
            {
                Console.WriteLine("Threads was not instantiated");
                return;
            }
            
            var fileFinderThreads = new FileFinderThreads(invertedIndexThreads, threadCount);
            fileFinderThreads.FindFiles(Path);
            
            SearchEngineData.ThreadPool.Instances.ThreadPool threadPool = new(threadCount);
            var fileFinderThreadPool = new FileFinderThreadPool(invertedIndexThreadPool, threadPool);
            fileFinderThreadPool.FindFiles(Path);

            _ = new Server(Port, invertedIndexThreadPool);
            Console.WriteLine($"Server is listening on port {Port}");
            Console.ReadKey();
            
        }
    }
}