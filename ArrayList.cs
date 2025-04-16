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
        private int _count;
        private T[] _array;
        private readonly IComparer<T> _comparer;
        private int Capacity => _array.Length;

        public int Count => _count;
        public T this[int index] => _array[index];

        public ArrayList()
        {
            _count = 0;
            _array = Array.Empty<T>();
            _comparer = Comparer<T>.Default;
        }

        public ArrayList(IComparer<T> comparer)
        {
            _count = 0;
            _array = Array.Empty<T>();
            _comparer = comparer;
        }


        public ArrayList(StreamReader stream, Func<string, T> parser, IComparer<T> comparer)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (parser == null) throw new ArgumentNullException(nameof(parser));

            string line = stream.ReadLine();

            if (string.IsNullOrWhiteSpace(line)) return;

            _count = 0;
            _array = Array.Empty<T>();

            string[] elements = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            _comparer = comparer;

            foreach (string element in elements)
            {
                try
                {
                    T value = parser(element);
                    Add(value);
                }
                catch (Exception ex)
                {
                    throw new FormatException($"Ошибка парсинга элемента '{element}': {ex.Message}", ex);
                }
            }
        }

        public int Add(T value)
        {
            if (_count == Capacity)
                ExtendArray();
            int addingIndex = FindPosForAdding(value);
            ShiftToRight(addingIndex);
            _array[addingIndex] = value;
            _count++;
            return addingIndex;
        }

        public void Clear()
        {
            _array = Array.Empty<T>();
            _count = 0;
        }

        public bool Contains(T value)
        {
            return IndexOf(value) >= 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T value) // Binary Search
        {
            return Array.BinarySearch(_array, value, _comparer);

           /* int left = 0;
            int right = _count - 1;
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int comparison = _comparer.Compare(_array[mid], value);
                if (comparison == 0)
                {
                    return mid; // Элемент найден
                }
                else if (comparison < 0)
                {
                    left = mid + 1; // Искомый элемент находится в правой половине
                }
                else
                {
                    right = mid - 1; // Искомый элемент находится в левой половине
                }
            }
            return -1; // Элемент не найден*/
        }

        public void Remove(T value)
        {
            int index = IndexOf(value);
            RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Capacity)
            {
                throw new IndexOutOfRangeException("Индекс выходит за границы массива.");
            }

            for (int i = index; i < _count - 1; i++)
            {
                _array[i] = _array[i + 1];
            }
            _count--;
        }

        public IList<T> subList(int fromIndex, int toIndex)
        {
            ArrayList <T> list = new ArrayList<T>(_comparer);
            for (int i = fromIndex; i < toIndex; i++) 
            {
                list.Add(_array[i]);
            }
            return list;
        }

        public void DebugPrintToConsole()
        {
            for (int i = 0; i < _count; i++)
            {
                Console.Write(_array[i]);
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        private void ExtendArray()
        {
            if (_array == null)
            {
                throw new InvalidOperationException("Массив не инициализирован.");
            }

            T[] tmpArray = _array;
            
            if (Capacity == 0)
            {
                _array = new T[1];
            }
            else
            {
                _array = new T[Capacity * 2];
            }

            Array.Copy(tmpArray, _array, tmpArray.Length);
        }

        private void ShiftToRight(int index)
        {
            if (index < 0 || index >= Capacity)
                throw new IndexOutOfRangeException("Индекс выходит за границы массива.");

            for (int i = _count; i > index; i--)
                _array[i] = _array[i - 1];
        }

        private int FindPosForAdding(T value)
        {
            int i = 0;
            while (i < _count && _comparer.Compare(_array[i], value) <= 0)
                i++;
            return i;
        }
    }
}
