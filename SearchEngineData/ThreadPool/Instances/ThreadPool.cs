using Data.Interfaces;
using Data.Models;
using System.Diagnostics;

namespace Instances
{
    public class ThreadPool : IThreadPool, IDisposable
    {
        private IQueue<FakeTask> _queue;
        private Thread[] _threads;
        private bool _working;
        private bool _terminated;
        private bool _init;
        private bool _disposed;
        private int _threadCount;

        private volatile int _rejectedTaskInQueue;
        private volatile int _rejectedTaskInBuffer;

        private List<long> _timeForThreadWaiting;
        private Stopwatch _stopwatch;

        private readonly object _lock = new object();
        public bool IsDisposed { get; private set; }
        public bool IsWorking { get; private set; }
        public ThreadPool(int threadCount)
        {   
            _stopwatch = new Stopwatch();
            _timeForThreadWaiting = new List<long>();
            _rejectedTaskInBuffer = 0;
            _rejectedTaskInQueue = 0;
            _queue = new CustomQueue();
            _threads = new Thread[threadCount];
            IsDisposed = false;
            _working = false;
            _init = false;
            _threadCount = threadCount;

            for (int i = 0; i < threadCount; i++)
            {
                _threads[i] = new Thread(() => { Routine(); });
            }
        }

        public void StartIteration()
        {
            _working = true;
            if (!_init) 
            {
                for (int i = 0; i < _threadCount; i++)
                {
                    _threads[i].Start();
                    Console.WriteLine($"Thread {i} started routine!");
                }
            }
            _init = true;
        }

        public void AddTask(FakeTask task)
        {
            lock (_lock)
            {
                if (_disposed)
                {
                    return;
                }
                if (!_working && (_queue.GetTotalTimeInQueue() + task.ExecutionTime) >= 60)
                {
                    Console.WriteLine($"Task {task.Id} was declined!");
                    _rejectedTaskInQueue++;
                    return;
                }
                if(!_working && (_queue.GetTotalTimeInQueue() + task.ExecutionTime) < 60)
                {
                    _queue.Push(task);
                    Monitor.Pulse(_lock);
                    Console.WriteLine($"Task #{task.Id} was added to the queue");
                }
            }
        }
        public void Routine()
        {
            while (true)
            {   
                FakeTask task;
                lock (_lock)
                {
                    if (!_queue.Empty() && !_stopwatch.IsRunning)
                    {
                        _stopwatch.Start();
                    }
                    if (_queue.Empty())
                    {   
                        if(_stopwatch.IsRunning)
                        {
                            _stopwatch.Stop();
                            long time = _stopwatch.ElapsedMilliseconds;
                            _timeForThreadWaiting.Add(time);
                            _stopwatch.Reset();
                        }
                        Console.WriteLine($"Wait started on thread: {Environment.CurrentManagedThreadId}");
                        Monitor.Wait(_lock);
                        Console.WriteLine($"Wait finished on thread: {Environment.CurrentManagedThreadId}");
                    }
                    task = _queue.Pop();
                }
                if (task != null)
                {
                    task.Execute();
                    if (_disposed || _terminated)
                    {
                        return;
                    }
                }
            }


        }
        public bool Working()
        {
            lock (_lock)
            {
                return WorkingUnsave();
            }
        }

        public bool WorkingUnsave()
        {
            return !_terminated && _init;
        }

        public void Print()
        {
            throw new NotImplementedException();
        }
        protected virtual void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (_disposed)
                {
                    return;
                }

                if (disposing && WorkingUnsave())
                {
                    // _terminated = true;
                    _queue.Clear();
                    Console.WriteLine("Queue cleared!");
                    _buffer.Clear();
                    Console.WriteLine("Buffer cleared!");
                    _disposed = true;
                    Monitor.PulseAll(_lock);

                    foreach (var thread in _threads)
                    {
                        thread.Join();
                    }
                    Console.WriteLine("Thread cleared!");
                    Console.WriteLine($"Rejected for queue: {_rejectedTaskInQueue}");
                    Console.WriteLine($"Rejected for buffer: {_rejectedTaskInBuffer}");
                    Console.WriteLine($"Min time for queue empty: {_timeForThreadWaiting.Min()}");
                    Console.WriteLine($"Max time for queue empty: {_timeForThreadWaiting.Max()}");
                    _terminated = true;
                    _threads = null;
                }
                else
                {
                    _threads = null;
                    _terminated = false;
                    return;
                }
            }
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
