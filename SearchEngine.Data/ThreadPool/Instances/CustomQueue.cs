using System.Collections;
using SearchEngineData.ThreadPool.Data.Interfaces;
using SearchEngineData.ThreadPool.Data.Models;

namespace SearchEngineData.ThreadPool.Instances
{
    public class CustomQueue : IQueue<IndexingTask>
    {
        private readonly Queue<IndexingTask> _tasks = new();
        private readonly ReaderWriterLockSlim _lock = new();

        public void Clear() 
        {
            _lock.EnterWriteLock(); 
            _tasks.Clear();
            _lock.ExitWriteLock();
        }

        public bool Empty()
        {
            _lock.EnterReadLock(); 
            var isEmpty = _tasks.Count == 0;
            _lock.ExitReadLock();
            return isEmpty;
        }
        public int Size()
        {
            _lock.EnterReadLock(); 
            var size = _tasks.Count;
            _lock.ExitReadLock();
            return size;
        }

        public IndexingTask Pop()
        {
            _lock.EnterWriteLock();
            var task = _tasks.Dequeue();
            _lock.ExitWriteLock();
            return task;
        }

        public void Push(IndexingTask value)
        {
            _lock.EnterWriteLock();
            _tasks.Enqueue(value);
            _lock.ExitWriteLock();
        }

        public IEnumerator<IndexingTask> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
