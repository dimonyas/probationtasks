using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace CloneObjectExtension
{
    public static class CloneExtension
    {
        public static object Clone(this object objSource)
        {
            return DoCopy(objSource, new Dictionary<object, object>());
        }

        private static object DoCopy(object objSource, Dictionary<object, object> circularDictionary)
        {
            if (objSource == null)
                return null;
            Type type = objSource.GetType();

            if (type.IsValueType || type == typeof(string))
            {
                return objSource;
            }
            else if (type.IsArray)
            {
                if (circularDictionary.ContainsKey(objSource))
                    return circularDictionary[objSource];
                Type elementType = Type.GetType(
                    type.FullName.Replace("[]", string.Empty));
                var array = objSource as Array;
                Array copiedArray = Array.CreateInstance(elementType, array.Length);
                circularDictionary.Add(objSource, copiedArray);
                for (int i = 0; i < array.Length; i++)
                {
                    object element = array.GetValue(i);
                    object objectToBeCopied;
                    if (element != null && circularDictionary.ContainsKey(element))
                        objectToBeCopied = circularDictionary[element];
                    else
                        objectToBeCopied = DoCopy(element, circularDictionary);
                    copiedArray.SetValue(objectToBeCopied, i);
                }
                return Convert.ChangeType(copiedArray, objSource.GetType());
            }
            else
            {
                if (circularDictionary.ContainsKey(objSource))
                    return circularDictionary[objSource];
                object objTarget = Activator.CreateInstance(objSource.GetType());
                circularDictionary.Add(objSource,objTarget);
                FieldInfo[] fields = type.GetFields(BindingFlags.Public |
                                                    BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    object fieldValue = field.GetValue(objSource);
                    if (fieldValue == null)
                        continue;
                    object objectToBeCopied = circularDictionary.ContainsKey(fieldValue) ? circularDictionary[fieldValue] : DoCopy(fieldValue, circularDictionary);
                    field.SetValue(objTarget, objectToBeCopied);
                }
                return objTarget;
            }
        }
    }
}
