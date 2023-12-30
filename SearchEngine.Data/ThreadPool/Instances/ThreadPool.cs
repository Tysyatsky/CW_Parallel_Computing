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
        private readonly Dictionary<int, Stopwatch> _stopwatch;
        private static readonly AutoResetEvent NewTasksAvailableWait = new(false);

        public ThreadPool(int threadCount)
        {
            _queue = new CustomQueue();
            _threads = new Thread[threadCount];
            _init = false;
            _threadCount = threadCount;
            _stopwatch = new Dictionary<int, Stopwatch>();
            for (var i = 0; i < threadCount; i++)
            {
                _threads[i] = new Thread(Routine);
                _stopwatch[_threads[i].ManagedThreadId] = new Stopwatch();
            }
        }

        public void Start()
        {
            if (!_init)
            {
                for (var i = 0; i < _threadCount; i++)
                {
                    _threads[i].Start();
                    // Console.WriteLine($"Thread {i} started routine!");
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
            NewTasksAvailableWait.Set();
        }

        private void Routine()
        {
            while (true)
            {
                if (_stopwatch[Environment.CurrentManagedThreadId].IsRunning && _queue.Empty())
                {
                    _stopwatch[Environment.CurrentManagedThreadId].Stop();
                    Console.WriteLine(
                        $"Thread {Environment.CurrentManagedThreadId} was working for {_stopwatch[Environment.CurrentManagedThreadId].ElapsedMilliseconds}");
                }

                if (_queue.Empty() && (_disposed || _terminated))
                {
                    return;
                }

                if (_queue.Empty())
                {
                    NewTasksAvailableWait.WaitOne();

                    // Console.WriteLine($"Wait finished on thread: {Environment.CurrentManagedThreadId}");
                }

                if (!_stopwatch[Environment.CurrentManagedThreadId].IsRunning)
                {
                    _stopwatch[Environment.CurrentManagedThreadId].Start();
                }

                try
                {
                    var task = _queue.Pop();
                    task.Execute();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public bool WorkingUnsafe()
        {
            return !_terminated;
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
                NewTasksAvailableWait.Set();
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