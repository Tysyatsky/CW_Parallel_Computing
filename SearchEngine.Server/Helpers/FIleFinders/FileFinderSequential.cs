using System.Diagnostics;
using SearchEngineData.InvertedIndex.Helpers;
using SearchEngineData.InvertedIndex.Instance;
using SearchEngineServer.Interfaces;

namespace SearchEngineServer.Helpers.FIleFinders;

public class FileFinderSequential : IFileFinder
{
    private readonly InvertedIndex _invertedIndex;

    public FileFinderSequential(InvertedIndex invertedIndex)
    {
        _invertedIndex = invertedIndex;
    }

    public void FindFiles(string directoryPath)
    {
        Stopwatch stopwatch = new();
        var filePaths = Directory
            .EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
            .ToList();
        stopwatch.Start();
        
        foreach (var filePath in filePaths)
        {
            var words = FileReader.ReadWordsFromFile(filePath);
            var fileName = Path.GetFileName(filePath);
            _invertedIndex.AddDocument(fileName, words);
        }

        stopwatch.Stop();
        Console.WriteLine($"Sequential: {stopwatch.ElapsedMilliseconds}ms");
    }
}