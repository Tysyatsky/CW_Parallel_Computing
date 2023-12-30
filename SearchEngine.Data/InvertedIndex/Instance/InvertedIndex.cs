using System.Collections.Concurrent;

namespace SearchEngineData.InvertedIndex.Instance;

public class InvertedIndex
{
    private readonly ConcurrentDictionary<string, List<string>> _index = new();

    public void AddDocument(string documentId, IEnumerable<string> terms)
    {
        foreach (var term in terms)
        {
            if (!_index.ContainsKey(term))
            {
                _index[term] = new List<string>();
            }

            if (!_index[term].Contains(documentId))
            {
                _index[term].Add(documentId);
            }
        }
    }

    public List<string> Search(string term)
    {
        return _index.TryGetValue(term, out var search) ? search : new List<string>();
    }

    public void PrintIndex() // only for testing purposes
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
