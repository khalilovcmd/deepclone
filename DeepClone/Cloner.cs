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
                    IList newList = (IList)Activator.CreateInstance(list.GetType());

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
}
