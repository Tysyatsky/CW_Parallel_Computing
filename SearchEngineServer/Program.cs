using System.Diagnostics;
using SearchEngineData;

namespace SearchEngineServer
{
    internal class Program
    {
        private static readonly string[] Doc1 = { "apple", "banana", "orange" };
        private static readonly string[] Doc2 = { "apple", "pear", "grape" };
        private static readonly string[] Doc3 = { "banana", "kiwi", "orange" };

        private static void Main()
        {
            // InitialTest();
            // FilesTest();

            InvertedIndex invertedIndex = new();
            SearchEngineData.ThreadPool.Instances.ThreadPool threadPool = new(4);

            FilesFinder(invertedIndex, @"C:\Uni\CourseWork\CW_Parallel_Computing\Docs\test_neg");
            invertedIndex.PrintIndex();

            const int port = 6000; // Specify the port number you want the server to listen on
            _ = new Server(port, invertedIndex, threadPool);

            Console.WriteLine($"Server is listening on port {port}. Press any key to exit.");
            Console.ReadKey();
        }

        private static void InitialTest()
        {
            InvertedIndex invertedIndex = new();

            invertedIndex.AddDocument(1, Doc1);
            invertedIndex.AddDocument(2, Doc2);
            invertedIndex.AddDocument(3, Doc3);

            const string searchTerm = "banana";
            var result = invertedIndex.Search(searchTerm);

            Console.WriteLine($"Documents containing '{searchTerm}':");
            foreach (var docId in result)
            {
                Console.WriteLine($"Document {docId}");
            }

            Console.WriteLine("\nInverted Index:");
            invertedIndex.PrintIndex();
        }

        private static void FilesTest()
        {
            InvertedIndex invertedIndex = new();

            FilesFinder(invertedIndex, "./train_u");

            const string searchTerm = "word";

            // Too many words
            // invertedIndex.PrintIndex();

            var result = invertedIndex.Search(searchTerm);

            Console.WriteLine($"Documents containing '{searchTerm}':");
            foreach (var docId in result)
            {
                Console.WriteLine($"Document {docId}");
            }
        }

        private static void FilesFinder(InvertedIndex invertedIndex, string directoryPath)
        {
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();
            
            try
            {
                var filePaths = Directory.GetFiles(directoryPath);

                const int threadCount = 12;
                var threads = new Thread[threadCount];
                
                for (int t = 0; t < threadCount; t++)
                {
                    int threadIndex = t;
                    int rowsPerThread = filePaths.Length / threadCount;

                    threads[t] = new Thread(() =>
                    {
                        int startRow = threadIndex * rowsPerThread;
                        int endRow = (threadIndex == threadCount - 1) ? filePaths.Length : (threadIndex + 1) * rowsPerThread;

                        for (int i = startRow; i < endRow; i++)
                        {
                            var words = FileReader.ReadWordsFromFile(filePaths[i]);

                            _ = int.TryParse(Path.GetFileName(filePaths[i]).Split('_')[0], out var localDocId);

                            invertedIndex.AddDocument(localDocId, words);
                        }
                    });
                    threads[t].Start();
                }

                foreach (var thread in threads)
                {
                    thread.Join();
                }
                
                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds / 1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}