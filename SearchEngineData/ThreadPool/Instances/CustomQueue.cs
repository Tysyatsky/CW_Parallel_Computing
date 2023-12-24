using System.Collections;
using Data.Interfaces;
using Data.Models;
using SearchEngineData.ThreadPool.Data.Models;

namespace SearchEngineData.ThreadPool.Instances
{
    public class CustomQueue : IQueue<SearchTask>
    {
        private readonly Queue<SearchTask> _tasks = new();
        private readonly ReaderWriterLockSlim _lock = new();

        public void Clear() 
        {
            lock (_lock)
            {
                _tasks.Clear();
            }
        }

        public bool Empty()
        {
            lock (_lock)
            {
                return _tasks.Count == 0;
            }
        }
        public int Size()
        {
            lock (_lock) 
            {
                return _tasks.Count;
            }
        }

        public SearchTask Pop()
        {   
            lock (_lock)
            {
                while (Empty())
                {
                    Monitor.Wait(_lock);
                }
                var task = _tasks.Dequeue();
                return task;
            }
        }

        public bool GetTotalTimeInQueue()
        {
            lock(_lock)
            {
                return !Empty();
            }
        }

        public void Push(SearchTask value)
        {
            lock (_lock)
            {
                _tasks.Enqueue(value);
                Monitor.Pulse(_lock);
            }
        }

        public void Print()
        {
            foreach (var task in _tasks)
            {
                // task.Print();
            }
        }

        public IEnumerator<SearchTask> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
