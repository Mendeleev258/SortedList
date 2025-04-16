using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortedList
{
	interface IList<T> : IEnumerable<T>
	{
		int Add(T value);
		void Clear();
		bool Contains(T value);
		int IndexOf(T value);
		// void Insert(int index, T value); не имеет смысла, т.к. список упорядоченный
		void Remove(T value);
		void RemoveAt(int index);
		IList<T> subList(int fromIndex, int toIndex);
		void DebugPrintToConsole();
        int Count { get; }
		T this[int index] { get; }
    }
}
