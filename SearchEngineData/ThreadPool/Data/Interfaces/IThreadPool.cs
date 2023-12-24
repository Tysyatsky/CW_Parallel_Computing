using Data.Models;
using SearchEngineData.ThreadPool.Data.Models;

namespace Data.Interfaces
{
    public interface IThreadPool
    {
        void Terminate();
        void AddTask(SearchTask task);
        bool Working();
        bool WorkingUnsave();
        void Print();
    }
}
