using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IQueue<T> : IEnumerable<T>
    {
        bool Empty();
        int Size();
        void Clear();
        T Pop();
        void Push(T value);

        void Print();

        int GetTotalTimeInQueue();
    }
}
