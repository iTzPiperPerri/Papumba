using DA_Assets.Shared.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace DA_Assets.Shared
{
    public class DAJson
    {
        public static bool debug = false;

        public static IEnumerator FromJson<T>(string json, Return<T, Exception> callback)
        {
            var @return = new RoutineResult<T, Exception>();

            int progressPercent = 0;
            int max = 100;

            new Thread(() =>
            {
                try
                {
                    @return.Result = FromJson<T>(json, ref progressPercent);
                    @return.Success = true;
                }
                catch (ThreadAbortException ex)
                {
                    @return.Success = false;
                    @return.Error = ex;
                }
                catch (Exception ex)
                {
                    @return.Success = false;
                    @return.Error = ex;

                    if (debug)
                    {
                        DALogger.LogError(ex.ToString());
                    }
                }

            }).Start();

            int tempCount = -1;
            while (DALogger.WriteLogBeforeEqual(
                ref progressPercent,
                ref max,
                $"Processing json: {progressPercent}%",
                ref tempCount))
            {
                yield return WaitFor.Delay1();
            }

            callback.Invoke(@return);
        }

        public static T FromJson<T>(string json)
        {
            int progressPercent = 0;
            return FromJson<T>(json, ref progressPercent);
        }

        public static T FromJson<T>(string json, ref int progressPercent)
        {
            JFResult jfr = DAFormatter.Format(json);

            if (jfr.IsValid == false)
            {
                progressPercent = 100;
                throw new Exception("Not valid json.");
            }

            string[] jsonLines = jfr.Json.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<Type, Dictionary<string, MemberInfo>> cache = new Dictionary<Type, Dictionary<string, MemberInfo>>();

            List<ParsedObject> objects = new List<ParsedObject>();

            object root = Activator.CreateInstance(typeof(T));

            objects.Add(new ParsedObject
            {
                Object = root,
                ParentType = ParentType.Root,
            });

            int skipArrayCount = 0;
            int skipObjectCount = 0;

            bool matchesTargetType = false;

            for (int i = 1; i < jsonLines.Length; i++)
            {
                string trimmedLine = jsonLines[i].Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine))
                {
                    continue;
                }

                if (trimmedLine.NeedSkipObject(ref skipArrayCount, ref skipObjectCount))
                {
                    continue;
                }

                string[] keyValue = jsonLines[i].Split(new[] { "\": " }, StringSplitOptions.None);

                if (debug)
                    Debug.Log($"{trimmedLine} | objects.Count: {objects.Count()} | keyValue.Lenght: {keyValue.Length}");

                if (keyValue.Length == 2)//field with key
                {
                    string key = Trimmer(keyValue[0]);
                    string value = Trimmer(keyValue[1]);

                    ParsedObject last = objects.Last();

                    GetMemberInfo(last.Object.GetType(), key, out MemberInfo memberInfo, ref cache);

                    if (debug)
                        Debug.Log($"GetMemberInfo | {key} | {last.Object.GetType()} | {memberInfo}");

                    if (memberInfo == null && (last.Object.GetType().IsDictionary() && value.IsFieldObjectStart() == false) == false)
                    {
                        if (debug)
                            Debug.Log($"missing '{key}' in '{last.Object}'");

                        if (value.IsFieldObjectStart())
                        {
                            skipObjectCount++;
                        }
                        else if (value.IsFieldArrayStart())
                        {
                            skipArrayCount++;
                        }

                        continue;
                    }
                    else if (last.Object.GetType().IsDictionary())
                    {
                        Type[] args = last.Object.GetType().GetGenericArguments();

                        Type keyType = args[0];
                        Type valueType = args[1];

                        object keyObj = GetValue(key, keyType);
                        object valueObj;

                        if (value.IsFieldObjectStart())
                        {
                            valueObj = Activator.CreateInstance(valueType);
                            AddObject(ref objects, key, valueObj, ParentType.Dictionary);
                        }
                        else
                        {
                            valueObj = GetValue(value, valueType);
                            AddToDictionary(last.Object, keyObj, valueObj);
                        }

                        if (debug)
                            Debug.Log($"last.Object IsDictionary | {keyObj} | {valueObj}");
                    }
                    else
                    {
                        matchesTargetType = true;

                        Type type = memberInfo.GetMemberType();

                        if (type.IsList())
                        {
                            if (debug)
                                Debug.Log($"type.IsList | {key} | 2 | {last.Object}");

                            Type loType = GetListType(type);
                            object newObj = Activator.CreateInstance(typeof(List<>).MakeGenericType(loType));

                            AddObject(ref objects, key, newObj, ParentType.Object);
                        }
                        else if (type.IsDictionary())
                        {
                            if (debug)
                                Debug.Log($"type.IsDictionary | {key}");

                            Type[] args = type.GetGenericArguments();

                            Type keyType = args[0];
                            Type valueType = args[1];

                            object newObj = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, valueType));

                            AddObject(ref objects, key, newObj, ParentType.Object);
                        }
                        else if (value.IsFieldObjectStart())
                        {
                            if (debug)
                                Debug.Log($"value.IsFieldObjectStart | {key} | 1 | {type} | {last.Object}");

                            object newObj = Activator.CreateInstance(type);

                            AddObject(ref objects, key, newObj, ParentType.Object);
                        }
                        else
                        {
                            if (debug)
                                Debug.Log($"before {key} | {value} | 3 | {last.Object}");

                            object valueObj = GetValue(value, type);

                            if (debug)
                                Debug.Log($"after {key} | {valueObj}");

                            memberInfo.SetValue(last.Object, valueObj);
                            matchesTargetType = true;
                        }
                    }
                }
                else if (trimmedLine.IsInnerObjectStart())//object without key inside list
                {
                    ParsedObject last = objects.Last();

                    Type ll = GetListType(last.Object.GetType());

                    if (debug)
                        Debug.Log($"trimmedLine.IsInnerObjectStart | {ll} | {last.Object}");

                    object newObj = Activator.CreateInstance(ll);

                    AddObject(ref objects, null, newObj, ParentType.List);
                }
                else if (trimmedLine.IsInnerArrayStart())//list without key inside list
                {
                    ParsedObject last = objects.Last();
                    Type ll = GetListType(last.Object.GetType());

                    if (debug)
                        Debug.Log($"trimmedLine.IsInnerArrayStart | {ll} | {last.Object}");

                    object newObj = Activator.CreateInstance(typeof(List<>).MakeGenericType(GetListType(ll)));

                    AddObject(ref objects, null, newObj, ParentType.List);
                }
                else if (trimmedLine.IsBlockEnd())
                {
                    ParsedObject last = objects.Last();

                    if (debug)
                        Debug.Log($"trimmedLine.IsBlockEnd | {last.FieldKey} | {last.Object}");

                    if (last.ParentType == ParentType.Object)
                    {
                        GetMemberInfo(objects[last.ParentIndex].Object.GetType(), last.FieldKey, out MemberInfo fieldInfo, ref cache);

                        fieldInfo.SetValue(objects[last.ParentIndex].Object, last.Object);
                        objects.RemoveAt(objects.Count - 1);
                    }
                    else if (last.ParentType == ParentType.List)
                    {
                        AddToList(objects[last.ParentIndex].Object, last.Object);
                        objects.RemoveAt(objects.Count - 1);
                    }
                    else if (last.ParentType == ParentType.Dictionary)
                    {
                        AddToDictionary(objects[last.ParentIndex].Object, last.FieldKey, last.Object);
                        objects.RemoveAt(objects.Count - 1);
                    }
                }
                else
                {
                    matchesTargetType = true;

                    ParsedObject last = objects.Last();

                    string value = Trimmer(trimmedLine);

                    if (debug)
                        Debug.Log($"else | {value} | {trimmedLine} | {last.Object}");

                    Type ll = GetListType(last.Object.GetType());

                    var cvalue = GetValue(value, ll);

                    AddToList(last.Object, cvalue);
                }

                progressPercent = Convert.ToInt32(((float)i / jsonLines.Length) * 100);
            }

            objects.Clear();

            progressPercent = 100;

            if (matchesTargetType == false)
            {
                throw new InvalidCastException("The input json does not match the target type.");
            }

            return (T)root;
        }

        private static void AddObject(ref List<ParsedObject> list, string key, object newObj, ParentType parentType)
        {
            list.Add(new ParsedObject
            {
                FieldKey = key,
                Object = newObj,
                ParentIndex = list.Count - 1,
                ParentType = parentType
            });
        }

        private static object GetValue(string value, Type type)
        {
            if (type == typeof(string))
            {
                return value;
            }
            else if (type == typeof(bool))
            {
                if (bool.TryParse(value, out bool result))
                {
                    return result;
                }
                else
                {
                    return false;
                }
            }
            else if (type == typeof(int))
            {
                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            else if (type == typeof(float))
            {
                if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
                {
                    return result;
                }
                else
                {
                    return 0f;
                }
            }
            else if (type == typeof(DateTime))
            {
                if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (debug)
                    Debug.Log($"parse {value}");

                Type nullableType = type.GetGenericArguments()[0];

                return GetValue(value, nullableType);
            }
            else
            {
                try
                {
                    return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    //if (debug)
                    Debug.LogError($"Convert.ChangeType() | {type} | {value} | {ex}");
                    return null;
                }

            }
        }

        private static string Trimmer(string value)
        {
            string result = value.Trim();

            int beginRemoveCount = 0;
            int endRemoveCount = 0;

            if (result.EndsWith("\","))
            {
                endRemoveCount = 2;
            }
            else if (result.EndsWith("\""))
            {
                endRemoveCount = 1;
            }
            else if (result.EndsWith(","))
            {
                endRemoveCount = 1;
            }

            if (result.StartsWith("\""))
            {
                beginRemoveCount = 1;
            }

            result = result.Substring(0, result.Length - endRemoveCount);
            result = result.Substring(beginRemoveCount);

            return result;
        }

        private static Type GetListType(Type type)
        {
            var args = type.GetGenericArguments();

            if (args.Length == 0)
            {
                Debug.LogError(type);
                return type;
            }

            return args[0];
        }

        private static void AddToList(object targetList, object valueToAdd)
        {
            Type listType = targetList.GetType();
            MethodInfo mi = listType.GetMethod("Add");

            if (debug)
                Debug.Log($"AddToList | {listType} | {mi}");

            mi.Invoke(targetList, new object[] { valueToAdd });
        }

        private static void AddToDictionary(object targetList, object keyToAdd, object valueToAdd)
        {
            Type listType = targetList.GetType();
            MethodInfo mi = listType.GetMethod("Add", new[] { keyToAdd.GetType(), valueToAdd.GetType() });

            if (debug)
                Debug.Log($"AddToDictionary\nkeyToAdd:{keyToAdd}\nvalueToAdd:{valueToAdd}\nlistType:{listType}\nmi:{mi}");

            mi.Invoke(targetList, new object[] { keyToAdd, valueToAdd });
        }

        private static void GetMemberInfo(Type type, string key, out MemberInfo memberInfo, ref Dictionary<Type, Dictionary<string, MemberInfo>> cache)
        {
            if (cache.TryGetValue(type, out Dictionary<string, MemberInfo> cache2))
            {
                if (cache2.TryGetValue(key, out MemberInfo value))
                {
                    memberInfo = value;
                    return;
                }
            }
            else
            {
                cache[type] = new Dictionary<string, MemberInfo>();
            }

            List<MemberInfo> mis = new List<MemberInfo>();

            FieldInfo[] fis = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            PropertyInfo[] pis = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            mis.AddRange(fis);
            mis.AddRange(pis);

            foreach (var mi in mis)
            {
                if (mi.IsIgnoredField())
                    continue;

                cache[type].TryAddValue(mi.Name, mi);

                string customName = mi.GetDataMemberName();

                if (customName != null && customName == key || mi.Name == key)
                {
                    memberInfo = mi;
                    return;
                }
            }

            memberInfo = null;
            return;
        }

        enum ParentType
        {
            Root,
            Object,
            List,
            Dictionary
        }

        struct ParsedObject
        {
            public string FieldKey { get; set; }
            public object Object { get; set; }
            public int ParentIndex { get; set; }
            public ParentType ParentType { get; set; }
        }
    }
}