using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SortedList
{
    internal class ArrayList<T> : IList<T>
    {
        private struct Node
        {
            public T Value;
            public int NextIndex;
        }

        private int _count;
        private Node[] _array;
        private readonly IComparer<T> _comparer;
        private int _firstIndex = -1; // Индекс первого элемента в упорядоченном списке
        private int _freeIndex = -1;  // Индекс первого свободного элемента
        private int Capacity => _array.Length;

        public int Count => _count;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                int currentIndex = _firstIndex;
                for (int i = 0; i < index; i++)
                {
                    currentIndex = _array[currentIndex].NextIndex;
                }
                return _array[currentIndex].Value;
            }
        }

        public ArrayList()
        {
            _count = 0;
            _array = Array.Empty<Node>();
            _comparer = Comparer<T>.Default;
        }

        public ArrayList(IComparer<T> comparer)
        {
            _count = 0;
            _array = Array.Empty<Node>();
            _comparer = comparer;
        }

        public int Add(T value)
        {
            if (_count == Capacity)
                ExtendArray();

            int newIndex = AllocateNode();
            _array[newIndex].Value = value;

            if (_firstIndex == -1)
            {
                // Первый элемент в списке
                _firstIndex = newIndex;
                _array[newIndex].NextIndex = -1;
            }
            else
            {
                // Находим место для вставки
                int current = _firstIndex;
                int prev = -1;

                while (current != -1 && _comparer.Compare(_array[current].Value, value) <= 0)
                {
                    prev = current;
                    current = _array[current].NextIndex;
                }

                if (prev == -1)
                {
                    // Вставка в начало
                    _array[newIndex].NextIndex = _firstIndex;
                    _firstIndex = newIndex;
                }
                else
                {
                    // Вставка в середину или конец
                    _array[newIndex].NextIndex = _array[prev].NextIndex;
                    _array[prev].NextIndex = newIndex;
                }
            }

            _count++;
            return IndexOf(value);
        }

        public void Clear()
        {
            _array = Array.Empty<Node>();
            _count = 0;
            _firstIndex = -1;
            _freeIndex = -1;
        }

        public bool Contains(T value)
        {
            return IndexOf(value) >= 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            int current = _firstIndex;
            while (current != -1)
            {
                yield return _array[current].Value;
                current = _array[current].NextIndex;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T value)
        {
            int index = 0;
            int current = _firstIndex;

            while (current != -1)
            {
                if (_comparer.Compare(_array[current].Value, value) == 0)
                    return index;

                current = _array[current].NextIndex;
                index++;
            }

            return -1;
        }

        public void Remove(T value)
        {
            int index = IndexOf(value);
            if (index >= 0)
                RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException();

            int current = _firstIndex;
            int prev = -1;

            for (int i = 0; i < index; i++)
            {
                prev = current;
                current = _array[current].NextIndex;
            }

            if (prev == -1)
            {
                // Удаление первого элемента
                _firstIndex = _array[current].NextIndex;
            }
            else
            {
                // Удаление из середины или конца
                _array[prev].NextIndex = _array[current].NextIndex;
            }

            FreeNode(current);
            _count--;
        }

        public IList<T> subList(int fromIndex, int toIndex)
        {
            ArrayList<T> list = new ArrayList<T>(_comparer);
            int current = _firstIndex;
            int currentIndex = 0;

            while (current != -1 && currentIndex < toIndex)
            {
                if (currentIndex >= fromIndex)
                    list.Add(_array[current].Value);

                current = _array[current].NextIndex;
                currentIndex++;
            }

            return list;
        }

        public void DebugPrintToConsole()
        {
            Console.WriteLine("Array contents:");
            for (int i = 0; i < Capacity; i++)
            {
                Console.Write($"[{i}]: {_array[i].Value} -> {_array[i].NextIndex} | ");
            }
            Console.WriteLine();

            Console.WriteLine("Ordered list:");
            int current = _firstIndex;
            while (current != -1)
            {
                Console.Write(_array[current].Value + " ");
                current = _array[current].NextIndex;
            }
            Console.WriteLine();
        }

        private void ExtendArray()
        {
            int newCapacity = Capacity == 0 ? 1 : Capacity * 2;
            Node[] newArray = new Node[newCapacity];

            if (Capacity > 0)
                Array.Copy(_array, newArray, Capacity);

            // Инициализируем свободные элементы
            for (int i = Capacity; i < newCapacity; i++)
            {
                newArray[i].NextIndex = _freeIndex;
                _freeIndex = i;
            }

            _array = newArray;
        }

        private int AllocateNode()
        {
            if (_freeIndex == -1)
                ExtendArray();

            int index = _freeIndex;
            _freeIndex = _array[index].NextIndex;
            _array[index].NextIndex = -1; // На всякий случай
            return index;
        }

        private void FreeNode(int index)
        {
            _array[index].NextIndex = _freeIndex;
            _freeIndex = index;
        }
    }
}
