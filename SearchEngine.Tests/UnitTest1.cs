using SearchEngineData;
using SearchEngineData.InvertedIndex.Instance;
using SearchEngineServer;

namespace SearchEngineTests
{
    public class Tests
    {
        private static readonly string[] Doc1 = { "apple", "banana", "orange" };
        private static readonly string[] Doc2 = { "apple", "pear", "grape" };
        private static readonly string[] Doc3 = { "banana", "kiwi", "orange" };
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
        
        private static void InitialTest()
        {
            InvertedIndex invertedIndex = new();

            invertedIndex.AddDocument("1", Doc1);
            invertedIndex.AddDocument("2", Doc2);
            invertedIndex.AddDocument("3", Doc3);

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

            // FileFinder.FindFiles(invertedIndex, "./train_u");
            //
            // const string searchTerm = "word";
            //
            // var result = invertedIndex.Search(searchTerm);
            //
            // Console.WriteLine($"Documents containing '{searchTerm}':");
            // foreach (var docId in result)
            // {
            //     Console.WriteLine($"Document {docId}");
            // }
        }
    }
}