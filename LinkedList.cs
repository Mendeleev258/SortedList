using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortedList
{
    internal class LinkedList<T> : IList<T>
    {
        private class Node
        {
            public T Data { get; set; }
            public Node Previous { get; set; }
            public Node Next { get; set; }

            public Node(T data)
            {
                Data = data;
            }
        }

        private Node _head;
        private Node _tail;
        private readonly IComparer<T> _comparer;
        public int _count {  get; private set; }

        public T this[int index] => GetNodeAt(index).Data;
        public int Count => _count;

        public LinkedList()
        {
            _head = null;
            _tail = null;
            _comparer = Comparer<T>.Default;
            _count = 0;
        }

        public LinkedList(IComparer<T> comparer)
        {
            _head = null;
            _tail = null;
            _comparer = comparer;
            _count = 0;
        }

        public LinkedList(StreamReader reader, Func<string, T> parser, IComparer<T> comparer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (parser == null) throw new ArgumentNullException(nameof(parser));

            string line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) return;
            
            string[] elements = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            _comparer = comparer;

            foreach (string element in elements)
            {
                try
                {
                    T value = parser(element);
                    Add(value);
                    _count++;
                }
                catch (Exception ex)
                {
                    throw new FormatException($"Ошибка парсинга элемента '{element}': {ex.Message}", ex);
                }
            }
        }

        public int Add(T data)
        {
            Node newNode = new Node(data);

            if (_head == null)
            {
                InsertToEmptyList(newNode);
                return 0;
            }

            (Node insertBeforeNode, int index) = FindInsertPosition(data);
            InsertNode(newNode, insertBeforeNode);
            return index;
        }

        public void Clear()
        {
            Node current = _head;
            while (current != null)
            {
                Node next = current.Next;
                current.Previous = null;
                current.Next = null;
                current = next;
            }

            _head = null;
            _tail = null;
            _count = 0;
        }

        public bool Contains(T value)
        {
            return IndexOf(value) >= 0;
        }

        public void DebugPrintToConsole()
        {
            Node current = _head;
            while (current != null)
            {
                Console.Write(current.Data);
                Console.Write(" ");
                current = current.Next;
            }
            Console.WriteLine();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var current = _head; current != null; current = current.Next)
            {
                yield return current.Data;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T value)
        {
            int index = 0;
            for (Node current = _head; current != null; current = current.Next, index++)
            {
                int cmp = _comparer.Compare(current.Data, value);
                if (cmp == 0) return index;
                if (cmp > 0) break; // Дальше искать не нужно - список отсортирован
            }
            return -1;
        }

        public void Remove(T value)
        {
            int index = IndexOf(value);
            RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            Node nodeToRemove = GetNodeAt(index);

            // Обновляем связи соседних узлов
            if (nodeToRemove.Previous != null)
                nodeToRemove.Previous.Next = nodeToRemove.Next;
            else
                _head = nodeToRemove.Next;

            if (nodeToRemove.Next != null)
                nodeToRemove.Next.Previous = nodeToRemove.Previous;
            else
                _tail = nodeToRemove.Previous;

            _count--;
        }

        public IList<T> subList(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || toIndex > Count || fromIndex > toIndex)
                throw new ArgumentOutOfRangeException();

            var subList = new LinkedList<T>(_comparer);
            Node current = GetNodeAt(fromIndex);

            for (int i = fromIndex; i < toIndex; i++)
            {
                subList.Add(current.Data);
                current = current.Next;
            }

            return subList;
        }

        private Node GetNodeAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException($"Индекс {index} выходит за границы диапазона [0, {Count - 1}]");
            }
            if (index < Count / 2)
            {
                var current = _head;
                for (var i = 0; i < index; i++) current = current.Next;
                return current;
            }
            else
            {
                var current = _tail;
                for (var i = Count - 1; i > index; i--) current = current.Previous;
                return current;
            }
        }

        private void InsertToEmptyList(Node newNode)
        {
            _head = newNode;
            _tail = newNode;
            _count = 1;
        }

        private (Node node, int index) FindInsertPosition(T data)
        {
            Node current = _head;
            int index = 0;

            while (current != null && _comparer.Compare(current.Data, data) < 0)
            {
                current = current.Next;
                index++;
            }

            return (current, index);
        }

        private void InsertNode(Node newNode, Node insertBeforeNode)
        {
            // Вставка в начало
            if (insertBeforeNode == _head)
            {
                InsertBeforeHead(newNode);
            }
            // Вставка в конец
            else if (insertBeforeNode == null)
            {
                InsertAfterTail(newNode);
            }
            // Вставка в середину
            else
            {
                InsertBeforeNode(newNode, insertBeforeNode);
            }

            _count++;
        }

        private void InsertBeforeHead(Node newNode)
        {
            newNode.Next = _head;
            _head.Previous = newNode;
            _head = newNode;
        }

        private void InsertAfterTail(Node newNode)
        {
            newNode.Previous = _tail;
            _tail.Next = newNode;
            _tail = newNode;
        }

        private void InsertBeforeNode(Node newNode, Node existingNode)
        {
            newNode.Previous = existingNode.Previous;
            newNode.Next = existingNode;
            existingNode.Previous.Next = newNode;
            existingNode.Previous = newNode;
        }
    }
}
