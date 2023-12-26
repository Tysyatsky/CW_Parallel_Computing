using System.Diagnostics;
using SearchEngineData.ThreadPool.Data.Interfaces;
using SearchEngineData.ThreadPool.Data.Models;

namespace SearchEngineData.ThreadPool.Instances
{
    public sealed class ThreadPool : IThreadPool, IDisposable
    {
        private readonly IQueue<IndexingTask> _queue;
        private Thread[] _threads;
        private bool _terminated;
        private bool _init;
        private bool _disposed;
        private readonly int _threadCount;
        private readonly Stopwatch _stopwatch;
        private readonly AutoResetEvent _newWorkItemsAvailableWait = new AutoResetEvent(false);

        public ThreadPool(int threadCount)
        {
            _queue = new CustomQueue();
            _threads = new Thread[threadCount];
            _init = false;
            _threadCount = threadCount;
            _stopwatch = new Stopwatch();
            for (var i = 0; i < threadCount; i++)
            {
                _threads[i] = new Thread(Routine);
            }
            StartIteration();
        }

        private void StartIteration()
        {
            if (!_init) 
            {
                for (var i = 0; i < _threadCount; i++)
                {
                    _threads[i].Start();
                    Console.WriteLine($"Thread {i} started routine!");
                }
            }
            _init = true;
        }

        public void AddTask(IndexingTask task)
        {
            if (_disposed)
            {
                return;
            }

            _queue.Push(task);
            _newWorkItemsAvailableWait.Set();
            // Console.WriteLine($"Task was added to the queue");

        }

        private void Routine()
        {
            while (true)
            {
                if (_queue.Empty())
                {
                    Console.WriteLine($"Wait started on thread: {Environment.CurrentManagedThreadId}");
                    if (_disposed || _terminated)
                    {
                        return;
                    }
                    _newWorkItemsAvailableWait.WaitOne();
                    Console.WriteLine($"Wait finished on thread: {Environment.CurrentManagedThreadId}");
                }

                try
                {
                    var task = _queue.Pop();
                    task.Execute();
                    Console.WriteLine($"{task.GetHashCode()} task executed");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        

        public bool WorkingUnsafe()
        {
            return !_terminated;
        }

        public void Print()
        {
            throw new NotImplementedException();
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (!disposing || !WorkingUnsafe()) 
                return;
            
            _disposed = true;

            foreach (var thread in _threads)
            {
                thread.Join();
            }

            _terminated = true;

            _threads = null;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Terminate()
        {
            Dispose();
        }
    }
}
