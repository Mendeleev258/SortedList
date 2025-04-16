using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortedList
{
    internal class UnmutableList<T> : IList<T>
    {
        private readonly IList<T> _list;

        public T this[int index] => _list[index];

        public int Count => _list.Count;

        public UnmutableList(IList<T> list)
        {
            _list = list;
        }

        public int Add(T value)
        {
            throw new UnmutableListException();
        }

        public void Clear()
        {
            throw new UnmutableListException();
        }

        public bool Contains(T value)
        {
            return _list.Contains(value);
        }

        public void DebugPrintToConsole()
        {
            _list.DebugPrintToConsole();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public int IndexOf(T value)
        {
            return _list.IndexOf(value);
        }

        public void Remove(T value)
        {
            throw new UnmutableListException();
        }

        public void RemoveAt(int index)
        {
            throw new UnmutableListException();
        }

        public IList<T> subList(int fromIndex, int toIndex)
        {
            return _list.subList(fromIndex, toIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
