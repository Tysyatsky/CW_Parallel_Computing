using SearchEngineData.ThreadPool.Data.Models;

namespace SearchEngineData.ThreadPool.Data.Interfaces
{
    public interface IThreadPool
    {
        void Terminate();
        void AddTask(IndexingTask task);
        bool WorkingUnsafe();
    }
}
