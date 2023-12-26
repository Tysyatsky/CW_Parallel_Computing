using SearchEngineData;
using SearchEngineData.InvertedIndex.Instance;
using SearchEngineData.ThreadPool.Data.Models;
using ThreadPool = SearchEngineData.ThreadPool.Instances.ThreadPool;

namespace SearchEngineServer.Helpers;

public static class FileFinder
{
    public static void FindFiles(InvertedIndex invertedIndex, ThreadPool threadPool, string directoryPath)
    {
        try
        {
            var filePaths = Directory
                .EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .ToList();

            foreach (var filePath in filePaths)
            {
                threadPool.AddTask(new IndexingTask(filePath, invertedIndex));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}