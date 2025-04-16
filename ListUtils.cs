using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SortedList
{
    internal class ListUtils<T>
    {
        public delegate IList<T> ListConstructorDelegate<T>(IEnumerable<T> collection);
        public delegate bool CheckDelegate<T>(T date);
        public delegate void ActionDelegate<T>(T date);
        public delegate TO ConvertDelegate<TI, TO>(TI elem);

        public static bool Exists<T>(IList<T> list, CheckDelegate<T> check)
        {
            foreach (var item in list)
            {
                if (check(item))
                {
                    return true;
                }
            }
            return false;
        }

        public static T Find<T>(IList<T> list, CheckDelegate<T> check)
        {
            foreach (var item in list)
            {
                if (check(item))
                {
                    return item;
                }
            }
            throw new Exception("Нет элементов, удовлетворяющих условию");
        }

        public static T FindLast<T>(IList<T> list, CheckDelegate<T> check)
        {
            T result = default;
            if (!Exists<T>(list, check))
                throw new Exception("Нет элементов, удовлетворяющих условию");
            foreach (var item in list)
            {
                if (check(item))
                {
                    result = item;
                }
            }
            return result;
        }

        public static int FindIndex<T>(IList<T> list, CheckDelegate<T> check)
        {
            int index = 0;
            foreach(var item in list)
            {
                if (check(item))
                {
                    return index;
                }
                index++;
            }
            throw new Exception("Нет элементов, удовлетворяющих условию");
        }

        public static int FindLastIndex<T>(IList<T> list, CheckDelegate<T> check)
        {
            T item = FindLast(list, check);
            return list.IndexOf(item);
        }

        public static IList<T> FindAll<T>(IList<T> list, CheckDelegate<T> check, ListConstructorDelegate<T> constructor)
        {
            LinkedList <T> resList = new LinkedList<T>();
            foreach(var item in list)
            {
                if (check(item))
                { 
                    resList.Add(item); 
                }
            }
            return constructor(resList);
        }

        public static IList<TO> ConvertAll<TI, TO>(IList<TI> inputList, ConvertDelegate<TI, TO> converter, ListConstructorDelegate<TO> constructor)
        {
            LinkedList<TO> convertedList = new LinkedList<TO>();
            foreach(var item in inputList)
            {
                convertedList.Add(converter(item));
            }
            return constructor(convertedList);
        }

        public static void ForEach<T>(IList<T> list, ActionDelegate<T> action)
        {
            foreach (var item in list)
                action(item);
        }

        public static bool CheckForAll<T>(IList<T> list, CheckDelegate<T> check)
        {
            foreach (var item in list)
            {
                if (!check(item))
                {
                    return false;
                }
            }
            return true;
        }

        public static readonly ListConstructorDelegate<T> ArrayListConstructor = (collection) =>
        {
            ArrayList<T> res = new ArrayList<T>();
            foreach (var item in collection)
                res.Add(item);
            return res;
        };

        public static readonly ListConstructorDelegate<T> LinkedListConstructor = (collection) =>
        {
            LinkedList<T> res = new LinkedList<T>();
            foreach (var item in collection)
                res.Add(item);
            return res;
        };
    }
}
