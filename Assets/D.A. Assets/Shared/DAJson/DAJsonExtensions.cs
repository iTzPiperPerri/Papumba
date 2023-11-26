using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace DA_Assets.Shared
{
    public static class DAJsonExtensions
    {
        public static bool debug = false;

        internal static bool IsDictionary(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        internal static bool IsList(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        internal static bool IsBlockEnd(this string str) => str.IsArrayEnd() || str.IsObjectEnd();
        internal static bool IsBlockStart(this string str) => str.IsFieldObjectStart() || str.IsFieldArrayStart() || str.IsInnerObjectStart() || str.IsInnerArrayStart();

        internal static bool IsArrayEnd(this string str) => str == "]," || str == "]";
        internal static bool IsObjectEnd(this string str) => str == "}," || str == "}";

        internal static bool IsInnerObjectStart(this string str) => str == "{";
        internal static bool IsInnerArrayStart(this string str) => str == "[";

        internal static bool IsFieldObjectStart(this string str) => str.EndsWith("{");
        internal static bool IsFieldArrayStart(this string str) => str.EndsWith("[");

        internal static bool NeedSkipObject(this string trimmedLine, ref int skipArrayCount, ref int skipObjectCount)
        {
            if (skipArrayCount > 0 || skipObjectCount > 0)
            {
                if (debug)
                    Debug.Log($"NeedSkipObject | before | skipArrayCount: {skipArrayCount} | skipObjectCount: {skipObjectCount} | {trimmedLine}");

                if (trimmedLine.IsFieldObjectStart() || trimmedLine.IsInnerObjectStart())
                {
                    if (debug)
                        Debug.Log($"NeedSkipObject | 1");
                    skipObjectCount++;
                }
                else if (trimmedLine.IsFieldArrayStart() || trimmedLine.IsInnerArrayStart())
                {
                    if (debug)
                        Debug.Log($"NeedSkipObject | 2");
                    skipArrayCount++;
                }
                else if (trimmedLine.IsObjectEnd())
                {
                    if (debug)
                        Debug.Log($"NeedSkipObject | 3");
                    skipObjectCount--;
                }
                else if (trimmedLine.IsArrayEnd())
                {
                    if (debug)
                        Debug.Log($"NeedSkipObject | 4");
                    skipArrayCount--;
                }
                if (debug)
                    Debug.Log($"NeedSkipObject | after | skipArrayCount: {skipArrayCount} | skipObjectCount: {skipObjectCount} | {trimmedLine}");

                return true;
            }
            else
            {
                return false;
            }
        }

        internal static string GetDataMemberName(this MemberInfo member)
        {
            if (member.IsDefined(typeof(DataMemberAttribute), true) == false)
                return null;

            DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(member, typeof(DataMemberAttribute), true);

            if (string.IsNullOrEmpty(dataMemberAttribute.Name))
                return null;

            return dataMemberAttribute.Name;
        }

        internal static bool IsIgnoredField(this MemberInfo member)
        {
            return member.IsDefined(typeof(IgnoreDataMemberAttribute), true);
        }

        internal static void SetValue(this MemberInfo member, object obj, object value)
        {
            if (member.MemberType == MemberTypes.Property)
            {
                ((PropertyInfo)member).SetValue(obj, value);
            }
            else if (member.MemberType == MemberTypes.Field)
            {
                ((FieldInfo)member).SetValue(obj, value);
            }
        }

        internal static Type GetMemberType(this MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)member).PropertyType;
            }
            else if (member.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)member).FieldType;
            }

            return null;
        }
    }
}
