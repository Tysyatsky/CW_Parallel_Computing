using SearchEngineData;

namespace SearchEngineServer
{
    public class Program
    {
        readonly static string[] doc1 = { "apple", "banana", "orange" };
        readonly static string[] doc2 = { "apple", "pear", "grape" };
        readonly static string[] doc3 = { "banana", "kiwi", "orange" };

        static void Main()
        {
            // InitialTest();
            // FilesTest();

            InvertedIndex invertedIndex = new();

            FilesFinder(invertedIndex, @"C:\Users\tysya\Desktop\test_n");

            int port = 6000; // Specify the port number you want the server to listen on
            _ = new Server(port, invertedIndex);

            Console.WriteLine($"Server is listening on port {port}. Press any key to exit.");
            Console.ReadKey();
        }

        static void InitialTest()
        {
            InvertedIndex invertedIndex = new();

            invertedIndex.AddDocument(1, doc1);
            invertedIndex.AddDocument(2, doc2);
            invertedIndex.AddDocument(3, doc3);

            string searchTerm = "banana";
            List<int> result = invertedIndex.Search(searchTerm);

            Console.WriteLine($"Documents containing '{searchTerm}':");
            foreach (var docId in result)
            {
                Console.WriteLine($"Document {docId}");
            }

            Console.WriteLine("\nInverted Index:");
            invertedIndex.PrintIndex();
        }

        static void FilesTest()
        {
            InvertedIndex invertedIndex = new();

            FilesFinder(invertedIndex, @"C:\Users\tysya\Desktop\test_n");

            var searchTerm = "word";

            // Too many words
            // invertedIndex.PrintIndex();

            List<int> result = invertedIndex.Search(searchTerm);

            Console.WriteLine($"Documents containing '{searchTerm}':");
            foreach (var docId in result)
            {
                Console.WriteLine($"Document {docId}");
            }
        }

        static void FilesFinder(InvertedIndex invertedIndex, string directoryPath)
        {
            try
            {
                string[] filePaths = Directory.GetFiles(directoryPath);

                foreach (string filePath in filePaths)
                {
                    var words = FileReader.ReadWordsFromFile(filePath);

                    _ = int.TryParse(Path.GetFileName(filePath).Split('_')[0], out int localDocId);

                    invertedIndex.AddDocument(localDocId, words);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}