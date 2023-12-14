using Data.Models;

namespace Data.Interfaces
{
    public interface IThreadPool
    {
        void Terminate();
        void AddTask(FakeTask task);
        bool Working();
        bool WorkingUnsave();
        void Print();
    }
}
