namespace SearchEngineData.ThreadPool.Data.Interfaces
{
    public interface IQueue<T> : IEnumerable<T>
    {
        bool Empty();
        T Pop();
        void Push(T value);
    }
}
