using SearchEngineData.InvertedIndex.Helpers;

namespace SearchEngineData.ThreadPool.Data.Models;

public class IndexingTask
{
    private readonly string _filePath;
    private readonly InvertedIndex.Instance.InvertedIndex _invertedIndex;

    public IndexingTask(string filePath, InvertedIndex.Instance.InvertedIndex invertedIndex)
    {
        _filePath = filePath;
        _invertedIndex = invertedIndex;
    }

    public void Execute()
    {
        var words = FileReader.ReadWordsFromFile(_filePath);
        var fileName = Path.GetFileName(_filePath);
        _invertedIndex.AddDocument(fileName, words);
    }
}