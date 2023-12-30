using System.Diagnostics;
using SearchEngineData.InvertedIndex.Helpers;
using SearchEngineData.InvertedIndex.Instance;
using SearchEngineServer.Interfaces;

namespace SearchEngineServer.Helpers.FIleFinders;

public class FileFinderThreads : IFileFinder
{
    private readonly InvertedIndex _invertedIndex;
    private readonly int _threadCount;

    public FileFinderThreads(InvertedIndex invertedIndex, int threadCount)
    {
        _invertedIndex = invertedIndex;
        _threadCount = threadCount;
    }

    public void FindFiles(string directoryPath)
    {
        Stopwatch stopwatch = new();
        
        var filePaths = Directory
            .EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
            .ToList();
        
        stopwatch.Start();
        
        var rowsPerThread = filePaths.Count / _threadCount; 

        var threads = new Thread[_threadCount]; 

        for (var t = 0; t < _threadCount; t++)
        {
            var threadIndex = t;

            threads[t] = new Thread(() =>
            {
                var perThreadStart = threadIndex * rowsPerThread;
                var perThreadEnd = (threadIndex == _threadCount - 1) ? filePaths.Count : (threadIndex + 1) * rowsPerThread;

                for (var i = perThreadStart; i < perThreadEnd; i++)
                {
                    var words = FileReader.ReadWordsFromFile(filePaths[i]);
                    var fileName = Path.GetFileName(filePaths[i]);
                    _invertedIndex.AddDocument(fileName, words);
                }
            });

            threads[t].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        stopwatch.Stop();
        Console.WriteLine($"Thread Count: {_threadCount}: {stopwatch.ElapsedMilliseconds}ms");
    }
}