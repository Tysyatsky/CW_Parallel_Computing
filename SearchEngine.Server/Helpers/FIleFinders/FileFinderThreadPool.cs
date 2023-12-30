using System.Diagnostics;
using SearchEngineData.InvertedIndex.Helpers;
using SearchEngineData.InvertedIndex.Instance;
using SearchEngineData.ThreadPool.Data.Models;
using SearchEngineServer.Interfaces;
using ThreadPool = SearchEngineData.ThreadPool.Instances.ThreadPool;

namespace SearchEngineServer.Helpers.FIleFinders;

public class FileFinderThreadPool : IFileFinder
{
    private readonly InvertedIndex _invertedIndex;
    private readonly ThreadPool _threadPool;
    public FileFinderThreadPool(InvertedIndex invertedIndex, ThreadPool threadPool)
    {
        _invertedIndex = invertedIndex;
        _threadPool = threadPool;
    }
    
    public void FindFiles(string directoryPath)
    {
        try
        {
            var filePaths = Directory
                .EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .ToList();
            
            _threadPool.Start();
            
            foreach (var filePath in filePaths)
            {
                _threadPool.AddTask(new IndexingTask(filePath, _invertedIndex));
            }
            
            _threadPool.Terminate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}