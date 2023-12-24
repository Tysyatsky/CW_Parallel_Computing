using SearchEngineData;

namespace Data.Models;

public class IndexingTask
{
    private readonly int _minIndex;
    private readonly int _maxIndex;
    private readonly string _searchString;
    private readonly string _directoryPath;
    private readonly InvertedIndex _invertedIndex;

    public IndexingTask(int minIndex, int maxIndex, string searchString, string directoryPath, InvertedIndex invertedIndex)
    {
        _minIndex = minIndex;
        _maxIndex = maxIndex;
        _searchString = searchString;
        _directoryPath = directoryPath;
        _invertedIndex = invertedIndex;
    }

    public void Execute()
    {
        
    }
}