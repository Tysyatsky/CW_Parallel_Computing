using Data.Interfaces;
using Data.Models;
using System.Collections;
using System.Collections.Generic;

namespace Instances
{
    public class CustomQueue : IQueue<FakeTask>, IEnumerable<FakeTask>
    {
        private readonly Queue<FakeTask> _tasks = new Queue<FakeTask>();
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public CustomQueue()
        {
            _tasks = new Queue<FakeTask>();
        }

        public CustomQueue(int capacity)
        {
            _tasks = new Queue<FakeTask>(capacity);
        }

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

        public FakeTask Pop()
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

        public int GetTotalTimeInQueue()
        {
            lock(_lock)
            {
                return !Empty() ? _tasks.Sum(task => task.ExecutionTime) : 0;
            }
        }

        public void Push(FakeTask value)
        {
            lock (_lock)
            {
                if (_tasks.Count >= 10)
                {
                    throw new InvalidOperationException("Too many elements in queue! " +
                        "Due to the variant it is imposible");
                }
                _tasks.Enqueue(value);
                Monitor.Pulse(_lock);
            }
        }

        public void Print()
        {
            foreach (var task in _tasks)
            {
                task.Print();
            }
        }

        public IEnumerator<FakeTask> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
