using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortedList
{
    internal class ListException : Exception
    {
        public ListException() { }
        public ListException(string message) : base(message) { }
    }

    internal class UnmutableListException : ListException 
    {
        public UnmutableListException() : base("Изменение списка запрещено") { }
    }
}
