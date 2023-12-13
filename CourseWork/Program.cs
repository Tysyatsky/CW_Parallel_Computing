namespace CourseWork
{
    public class Program
    {
        // test documents
        readonly static string[] doc1 = { "apple", "banana", "orange" };
        readonly static string[] doc2 = { "apple", "pear", "grape" };
        readonly static string[] doc3 = { "banana", "kiwi", "orange" };

        static void Main(string[] args)
        {
            InitialTest();
        }

        static void InitialTest()
        {
            InvertedIndex invertedIndex = new();

            // Add documents to the index
            invertedIndex.AddDocument(1, doc1);
            invertedIndex.AddDocument(2, doc2);
            invertedIndex.AddDocument(3, doc3);

            // Search for documents containing a term
            string searchTerm = "banana";
            List<int> result = invertedIndex.Search(searchTerm);

            Console.WriteLine($"Documents containing '{searchTerm}':");
            foreach (var docId in result)
            {
                Console.WriteLine($"Document {docId}");
            }

            // Print the inverted index
            Console.WriteLine("\nInverted Index:");
            invertedIndex.PrintIndex();
        }

        static void SearchEngineTest()
        {

        }
    }
}