namespace SearchEngineData;

public class InvertedIndex
{
    private readonly Dictionary<string, List<int>> index;
    private readonly object _lockObject = new();

    public InvertedIndex()
    {
        index = new Dictionary<string, List<int>>();
    }

    public void AddDocument(int documentId, string[] terms)
    {
        lock (_lockObject)
        {
            foreach (var term in terms)
            {
                if (!index.ContainsKey(term))
                {
                    index[term] = new List<int>();
                }

                if (!index[term].Contains(documentId))
                {
                    index[term].Add(documentId);
                }
            }
        }
    }

    public List<int> Search(string term)
    {
        lock(_lockObject)
        {
            if (index.ContainsKey(term))
            {
                return index[term];
            }
            else
            {
                return new List<int>();
            }
        }
    }

    public void PrintIndex() // only for testing purposes
    {
        foreach (var term in index.Keys)
        {
            Console.Write($"{term}: ");
            foreach (var docId in index[term])
            {
                Console.Write($"{docId} ");
            }
            Console.WriteLine();
        }
    }
}
