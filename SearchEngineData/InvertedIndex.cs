namespace SearchEngineData;

public class InvertedIndex
{
    private readonly Dictionary<string, List<int>> _index = new();
    private readonly object _lockObject = new();

    public void AddDocument(int documentId, IEnumerable<string> terms)
    {
        lock (_lockObject)
        {
            foreach (var term in terms)
            {
                if (!_index.ContainsKey(term))
                {
                    _index[term] = new List<int>();
                }

                if (!_index[term].Contains(documentId))
                {
                    _index[term].Add(documentId);
                }
            }
        }
    }

    public List<int> Search(string term)
    {
        lock(_lockObject)
        {
            return _index.TryGetValue(term, out var search) ? search : new List<int>();
        }
    }

    public void PrintIndex() // only for testing purposes
    {
        lock (_lockObject)
        {
            foreach (var term in _index.Keys)
            {
                Console.Write($"{term}: ");
                foreach (var docId in _index[term])
                {
                    Console.Write($"{docId} ");
                }
                Console.WriteLine();
            }
        }
    }
}
