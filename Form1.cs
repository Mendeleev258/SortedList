using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SortedList.Person;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace SortedList
{
    public partial class Form1 : Form
    {
        private IList<int> intList;
        private IList<string> stringList;
        private IList<Person> personList;
        byte type; //1 - int, 2 - string, 3 - person
        Random rnd = new Random();

        public Form1()
        {
            intList = null;
            stringList = null;
            personList = null;
            type = 0;
            InitializeComponent();
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            TypeLists.Items.Clear();
            TypeLists.Items.Add("Array list");
            TypeLists.Items.Add("Linked list");
            TypeLists.SelectedIndex = 0;
            convertComboBox.Items.Clear();
            convertComboBox.Items.Add("Array list");
            convertComboBox.Items.Add("Linked list");
            convertComboBox.Items.Add("Unmutable list");
            convertComboBox.SelectedIndex = 0;
        }

        //------------------------------------------------------------------------------------
        //                                СОЗДАНИЕ
        //------------------------------------------------------------------------------------

        private void PrintToTextBox()
        {
            if (type == 1)
            {
                foreach (var item in intList)
                    textList.Text += (item.ToString() + ", ");
                textList.Text += "\r\n";
            }
            if (type == 2)
            {
                foreach (var item in stringList)
                    textList.Text += (item.ToString() + ", ");
                textList.Text += "\r\n";
            }
            if (type == 3)
            {
                foreach (var item in personList)
                    textList.Text += (item.ToString() + ", ");
                textList.Text += "\r\n";
            }
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            if (this.intRbtn.Checked) type = 1;
            if (this.stringRbtn.Checked) type = 2;
            if (this.personRbtn.Checked) type = 3;
            string selectedType = TypeLists.SelectedItem.ToString();

            if (type == 1)
            {
                if (selectedType == "Array list")
                    this.intList = new ArrayList<int>();
                else if (selectedType == "Linked list")
                    this.intList = new LinkedList<int>();
                else if (selectedType == "Unmutable list")
                    this.intList = new LinkedList<int>();

                for (int i = 0; i < rnd.Next(5, 15); i++)
                    intList.Add(rnd.Next(500));

                if (selectedType == "Unmutable list")
                    this.intList = new UnmutableList<int>(this.intList);

            }

            if (type == 2)
            {
                if (selectedType == "Array list")
                    this.stringList = new ArrayList<string>();
                else if (selectedType == "Linked list")
                    this.stringList = new LinkedList<string>();
                else if (selectedType == "Unmutable list")
                    this.stringList = new LinkedList<string>();

                for (int i = 0; i < rnd.Next(5, 15); i++)
                {
                    string res = "";
                    for (int j = 0; j < rnd.Next(5, 15); j++)
                        res += (char)('a' + rnd.Next(0, 25));
                    stringList.Add(res);
                }

                if (selectedType == "Unmutable list")
                    this.intList = new UnmutableList<int>(this.intList);
            }

            if (type == 3)
            {
                // var comparer = new PersonComparer();

                if (selectedType == "Array list")
                    this.personList = new ArrayList<Person>();
                else if (selectedType == "Linked list")
                    this.personList = new LinkedList<Person>();
                else if (selectedType == "Unmutable list")
                    this.personList = new LinkedList<Person>();

                string[] names = { "Bob", "Alexey", "Mykola", "Ostap", "Maria",
                                    "Stepan", "Antony", "Olesya", "Oleg", "Svetlana" };

                for (int i = 0; i < rnd.Next(5, 15); i++)
                {
                    string name = names[rnd.Next(0, 9)];
                    byte age = (byte)rnd.Next(100);
                    personList.Add(new Person(name, age));
                }
            }
            this.textList.Clear();
            PrintToTextBox();
        }

        //------------------------------------------------------------------------------------
        //                                 МЕТОДЫ СПИСКА
        //------------------------------------------------------------------------------------

        private void ContainsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string str = this.TextContains.Text;
                
                if (type == 1)
                {
                    int value = int.Parse(str);
                    if (intList.Contains(value))
                    {
                        int index = intList.IndexOf(value);
                        MessageBox.Show($"Элемент {value} существует и имеет индекс {index}");
                    }
                    else
                        MessageBox.Show("Элемент не найден");
                }

                if (type == 2)
                {
                    if (stringList.Contains(str))
                    {
                        int index = stringList.IndexOf(str);
                        MessageBox.Show($"Элемент {str} существует и имеет индекс {index}");
                    }
                    else
                        MessageBox.Show("Элемент не найден");
                }

                if (type == 3)
                {
                    Person value = new Person(str);
                    if (personList.Contains(value))
                    {
                        int index = personList.IndexOf(value);
                        MessageBox.Show($"Элемент {value} существует и имеет индекс {index}");
                    }
                    else
                        MessageBox.Show("Элемент не найден");
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string str = this.textAdd.Text;
                if (type == 1)
                {
                    int value = int.Parse(str);
                    intList.Add(value);
                }
                if (type == 2)
                {
                    stringList.Add(str);
                }
                if (type == 3)
                {
                    Person value = new Person(str);
                    personList.Add(value);
                }
                this.textList.Clear();
                PrintToTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveBnt_Click(object sender, EventArgs e)
        {
            try
            {
                string str = this.textRemove.Text;
                if (type == 1)
                {
                    int value = int.Parse(str);
                    intList.Remove(value);
                }
                if (type == 2)
                {
                    stringList.Remove(str);
                }
                if (type == 3)
                {
                    Person value = new Person(str);
                    personList.Remove(value);
                }
                this.textList.Clear();
                PrintToTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SublstBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int left = int.Parse(leftRangeBox.Text);
                int right = int.Parse(rightRangeBox.Text);
                if (type == 1)
                {
                    intList = intList.subList(left, right);
                }
                if (type == 2)
                {
                    stringList = stringList.subList(left, right);
                }
                if (type == 3)
                {
                    personList = personList.subList(left, right);
                }
                this.textList.Clear();
                PrintToTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            if (type == 1)
            {
                intList.Clear();
            }
            if (type == 2)
            {
                stringList.Clear();
            }
            if (type == 3)
            {
                personList.Clear();
            }
            this.textList.Clear();
        }

        //------------------------------------------------------------------------------------
        //                                    ОПЕРАЦИИ
        //------------------------------------------------------------------------------------

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            if (type == 1)
            {
                if (ListUtils<int>.CheckForAll(intList, (int elem) => { return elem % 2 == 0; }))
                {
                    MessageBox.Show("В списке все элементы четные");
                }
                else
                {
                    MessageBox.Show("В списке НЕ все элементы четные");
                }
            }
            if (type == 2)
            {
                if (ListUtils<string>.CheckForAll(stringList, (string elem) => { return elem.Length % 2 == 0; }))
                {
                    MessageBox.Show("В списке все элементы четной длины");
                }
                else
                {
                    MessageBox.Show("В списке НЕ все элементы четной длины");
                }
            }
            if (type == 3)
            {
                if (ListUtils<Person>.CheckForAll(personList, (Person elem) => { return elem.Age % 2 == 0; }))
                {
                    MessageBox.Show("В списке возраст всех людей делится на 2");
                }
                else
                {
                    MessageBox.Show("В списке возраст НЕ всех людей делится на 2");
                }

            }
        }

        private void ForEachBtn_Click(object sender, EventArgs e)
        {
            try
            {
                textList.Text += Environment.NewLine;
                if (type == 1)
                {
                    textList.Text += "Преобразованный список (значение увеличивается в 2 раза):" + Environment.NewLine;
                    intList = ListUtils<int>.ConvertAll(intList, elem => elem * 2, ListUtils<int>.ArrayListConstructor);
                    ListUtils<int>.ForEach(intList, elem => textList.Text += $"{elem}; ");
                }
                if (type == 2)
                {
                    textList.Text += "Преобразованный список (добавлена строка):" + Environment.NewLine;
                    stringList = ListUtils<string>.ConvertAll(stringList, elem => elem += "TEST", ListUtils<string>.ArrayListConstructor);
                    ListUtils<string>.ForEach(stringList, elem => textList.Text += $"{elem}; ");
                }
                if (type == 3)
                {
                    // var comparer = new PersonComparer();
                    textList.Text += "Преобразованный список (добавлен 1 год к возрасту):" + Environment.NewLine;
                    ListUtils<Person>.ForEach(personList, elem => textList.Text += $"{elem++}; ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FindAllBtn_Click(object sender, EventArgs e)
        {
            textList.Text += Environment.NewLine;

            if (type == 1)
            {
                IList<int> list = ListUtils<int>.FindAll(intList, (int elem) => { return elem % 2 == 0; }, ListUtils<int>.LinkedListConstructor);
                textList.Text += "Все четные элементы списка:" + Environment.NewLine;
                if (list.Count == 0)
                {
                    textList.Text += "Нет таких элементов";
                }
                else
                {
                    foreach (int elem in list)
                    {
                        textList.Text += elem.ToString() + "; ";
                    }
                }
            }
            if (type == 2)
            {
                IList<string> list = ListUtils<string>.FindAll(stringList, (string elem) => { return elem.Length % 2 == 0; }, ListUtils<string>.LinkedListConstructor);
                textList.Text += "Все элементы четной длины:" + Environment.NewLine;
                if (list.Count == 0)
                {
                    textList.Text += "Нет таких элементов";
                }
                else
                {
                    foreach (string elem in list)
                    {
                        textList.Text += elem + "; ";
                    }
                }
            }
            if (type == 3)
            {
                IList<Person> list = ListUtils<Person>.FindAll(personList, (Person elem) => { return elem.Age % 2 == 0; }, ListUtils<Person>.LinkedListConstructor);
                textList.Text += "Все люди, чей возраст делится на 2:" + Environment.NewLine;
                if (list.Count == 0)
                {
                    textList.Text += "Нет таких людей";
                }
                else
                {
                    foreach (Person elem in list)
                    {
                        textList.Text += elem.ToString() + "; ";
                    }
                }
            }
            textList.Text += Environment.NewLine;
        }

        private void ExistsBtn_Click(object sender, EventArgs e)
        {
            if (type == 1)
            {
                if (ListUtils<int>.Exists(intList, (int elem) => { return elem % 2 == 0; }))
                {
                    int first = ListUtils<int>.Find(intList, (int elem) => { return elem % 2 == 0; });
                    int firstIndex = ListUtils<int>.FindIndex(intList, (int elem) => { return elem % 2 == 0; });
                    int last = ListUtils<int>.FindLast(intList, (int elem) => { return elem % 2 == 0; });
                    int lastIndex = ListUtils<int>.FindLastIndex(intList, (int elem) => { return elem % 2 == 0; });
                    MessageBox.Show($"Первый четный элемент {first} с индексом {firstIndex}\n" +
                        $"Последний четный элемент {last} с индексом {lastIndex}");
                }
                else
                {
                    MessageBox.Show("В списке НЕ содержатся четные элементы");
                }
            }
            else if (type == 2)
            {
                if (ListUtils<string>.Exists(stringList, (string elem) => { return elem.Length % 2 == 0; }))
                {
                    string first = ListUtils<string>.Find(stringList, (string elem) => { return elem.Length % 2 == 0; });
                    int firstIndex = ListUtils<string>.FindIndex(stringList, (string elem) => { return elem.Length % 2 == 0; });
                    string last = ListUtils<string>.FindLast(stringList, (string elem) => { return elem.Length % 2 == 0; });
                    int lastIndex = ListUtils<string>.FindLastIndex(stringList, (string elem) => { return elem.Length % 2 == 0; });
                    MessageBox.Show($"Первый элемент четной длины {first} с индексом {firstIndex}\n" +
                       $"Последний элемент четной длины {last} с индексом {lastIndex}");
                }
                else
                {
                    MessageBox.Show("В списке НЕ содержатся элементы четной длины");
                }
            }
            else if (type == 3)
            {
                if (ListUtils<Person>.Exists(personList, (Person elem) => { return elem.Age % 2 == 0; }))
                {
                    Person first = ListUtils<Person>.Find(personList, (Person elem) => { return elem.Age % 2 == 0; });
                    int firstIndex = ListUtils<Person>.FindIndex(personList, (Person elem) => { return elem.Age % 2 == 0; });
                    Person last = ListUtils<Person>.FindLast(personList, (Person elem) => { return elem.Age % 2 == 0; });
                    int lastIndex = ListUtils<Person>.FindLastIndex(personList, (Person elem) => { return elem.Age % 2 == 0; });
                    MessageBox.Show($"Первый человек четного возраста {first} с индексом {firstIndex}\n" +
                       $"Последний человек четного возраста {last} с индексом {lastIndex}");
                }
                else
                {
                    MessageBox.Show("В списке НЕТ людей, чей возраст можно поделить на 2");
                }
            }
        }

        private void ConvertBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string newType = convertComboBox.SelectedItem.ToString();
                string oldType = TypeLists.SelectedItem.ToString();
                if (newType == "Array list")
                {
                    if (type == 1) // int
                    {
                        intList = ListUtils<int>.ConvertAll<int, int>(intList, x => x, ListUtils<int>.ArrayListConstructor);
                    }

                    if (type == 2) // string
                    {
                        stringList = ListUtils<string>.ConvertAll<string, string>(stringList, x => x, ListUtils<string>.ArrayListConstructor);
                    }

                    if (type == 3) // person
                    {
                        personList = ListUtils<Person>.ConvertAll<Person, Person>(personList, x => x, ListUtils<Person>.ArrayListConstructor);
                    }
                }
                if (newType == "Linked list")
                {
                    if (type == 1) // int
                    {
                        intList = ListUtils<int>.ConvertAll<int, int>(intList, x => x, ListUtils<int>.LinkedListConstructor);
                    }

                    if (type == 2) // string
                    {
                        stringList = ListUtils<string>.ConvertAll<string, string>(stringList, x => x, ListUtils<string>.LinkedListConstructor);
                    }

                    if (type == 3) // person
                    {
                        personList = ListUtils<Person>.ConvertAll<Person, Person>(personList, x => x, ListUtils<Person>.LinkedListConstructor);
                    }
                }
                
                if (newType == "Unmutable list")
                {
                    if (type == 1)
                        this.intList = new UnmutableList<int>(this.intList);
                    if (type == 2)
                        this.stringList = new UnmutableList<string>(this.stringList);
                    if (type == 3)
                        this.personList = new UnmutableList<Person>(this.personList);
                }

                textList.Text += Environment.NewLine;
                if (type == 1)
                    textList.Text += "Новый тип списка: " + newType + Environment.NewLine;
                if (type == 2)
                    textList.Text += "Новый тип списка: " + newType + Environment.NewLine;
                if (type == 3)
                    textList.Text += "Новый тип списка: " + newType + Environment.NewLine;

                PrintToTextBox();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        //------------------------------------------------------------------------------------
        //                                    ВЫХОД
        //------------------------------------------------------------------------------------

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
