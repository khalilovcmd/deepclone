using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DeepClone
{
    public class Cloner
    {
        public object Clone(object value)
        {
            if (value == null)
                return value;

            if (value.GetType().IsValueType || value.GetType() == typeof(String))
                return value;

            if (value.GetType().GetInterface("IEnumerable") != null)
            {
                IEnumerable enumerable = (IEnumerable)value;
                
                if (value.GetType().GetInterface("IList") != null)
                {
                    IList list = (IList)enumerable;
                    IList newList = (IList) Activator.CreateInstance(list.GetType());

                    foreach (var l in list)
                        newList.Add(Clone(l));

                    return newList;
                }

                if (value.GetType().GetInterface("IDictionary") != null)
                {
                    IDictionary dictionary = (IDictionary)enumerable;
                    IDictionary newDictionary = (IDictionary)Activator.CreateInstance(dictionary.GetType());

                    foreach (DictionaryEntry d in dictionary)
                        newDictionary.Add(Clone(d.Key), Clone(d.Value));

                    return newDictionary;
                }
            }

            object returnValue = Activator.CreateInstance(value.GetType());

            IEnumerable<MemberInfo> propertyMembers =
                value
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                .Cast<MemberInfo>();

            IEnumerable<MemberInfo> fieldMembers =
                value
                .GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                .Cast<MemberInfo>();

            IEnumerable<MemberInfo> members =
                propertyMembers.Union(fieldMembers);


            foreach (MemberInfo member in members)
            {
                if (member is FieldInfo)
                {
                    FieldInfo memberField = member as FieldInfo;
                    Console.WriteLine(memberField.Name);

                    if (memberField.GetValue(value) == value)
                        return value;

                    memberField.SetValue(returnValue, Clone(memberField.GetValue(value)));
                }
            }

            return returnValue;
        }
    }


    public class TestClass
    {
        public int apple { set; get; }
        public int orange { set; get; }
        public String color { set; get; }
        public DateTime timeToCut { set; get; }
        public List<TestSubClass> subs { set; get; }
        public List<TestStruct> structs { set; get; }
        public Dictionary<int, TestSubClass> subsDictionary { set; get; }
        public static String standStill = "asdasd";

        public TestClass testClass { set; get; }
    }

    public class TestSubClass
    {
        public String name {set;get;}
        public ConsoleColor color { set; get; }
        public ConsoleColor color1 { set; get; }
    }

    public struct TestStruct
    {
        public int a { set; get; }
        public int b { set; get; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Cloner cloner = new Cloner();
            Console.ReadKey();
        }
    }
}
