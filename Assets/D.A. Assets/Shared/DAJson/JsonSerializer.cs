//
// JsonSerializer.cs
//
// Author:
//       Michael Ganss <michael@ganss.org>
//
// Copyright (c) 2011 Michael Ganss
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DA_Assets.Shared
{
    /// <summary>
    /// Serializes and deserializes JSON.
    /// </summary>
    internal class JsonSerializer
    {
        /// <summary>
        /// Serializes an object into JSON text.
        /// </summary>
        /// <returns>
        /// The JSON text that represents the object.
        /// </returns>
        /// <param name='obj'>
        /// The object to serialize.
        /// </param>
        public static string SerializeObject(object obj)
        {
            string result = new JsonSerializer().Serialize(obj);

            if (result[result.Length - 1] == '\"')
            {
                result = result.Remove(result.Length - 1);
            }

            if (result[0] == '\"')
            {
                result = result.Substring(1);
            }


            return result;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public JsonSerializer()
        {
            TypeInfoPropertyName = "@type";
        }

        /// <summary>
        /// <para>
        /// Gets or sets a value indicating whether to serialize and deserialize type information
        /// for derived classes.
        /// </para>
        /// <para>
        /// When a derived class is serialized and no additional type information is serialized
        /// a deserializer does not know the derived class the object originated from. Setting this property to true emits
        /// type information in an additional property whose name is indicated by <see cref="TypeInfoPropertyName"/> and
        /// deserializing honors this property. Type information is emitted only for types which are derived classes or implement
        /// an interface.
        /// </para>
        /// <para>
        /// The type information includes only the class name (no namespace, assembly information etc.) in order to be potentially compatible
        /// with other implementations. When deserializing, the type indicated by the type information is searched only in the assembly where the base
        /// type is located.
        /// </para>
        /// The default is false.
        /// </summary>
        public bool UseTypeInfo { get; set; }

        /// <summary>
        /// Gets or sets a property name where additional type information is serialized to and deserialized from.
        /// The default is "@type".
        /// </summary>
        public string TypeInfoPropertyName { get; set; }

        static Regex DateTimeRegex = new Regex(@"^/Date\((-?\d+)\)/$");

        private Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();

        class SetterMember
        {
            public Type Type { get; set; }
            public Action<object, object> Set { get; set; }
        }

        private Dictionary<string, SetterMember> MemberCache = new Dictionary<string, SetterMember>();

        /// <summary>
        /// Serialize the specified object into JSON.
        /// </summary>
        /// <param name='obj'>
        /// The object to serialize.
        /// </param>
        public string Serialize(object obj)
        {
            if (obj == null) return "null";

            IList list = obj as IList;
            if (list != null && !(obj is IEnumerable<KeyValuePair<string, object>>))
            {
                StringBuilder sb = new StringBuilder("[");
                if (list.Count > 0)
                {
                    sb.Append(string.Join(",", list.Cast<object>().Select(i => Serialize(i)).ToArray()));
                }
                sb.Append("]");
                return sb.ToString();
            }

            string str = obj as string;
            if (str != null)
            {
                return @"""" + EscapeString(str) + @"""";
            }

            if (obj is int)
            {
                return obj.ToString();
            }

            bool? b = obj as bool?;
            if (b.HasValue)
            {
                return b.Value ? "true" : "false";
            }

            if (obj is decimal)
            {
                return ((IFormattable)obj).ToString("G", NumberFormatInfo.InvariantInfo);
            }

            if (obj is double || obj is float)
            {
                return ((IFormattable)obj).ToString("R", NumberFormatInfo.InvariantInfo);
            }

            if (obj is Enum)
            {
                return @"""" + EscapeString(obj.ToString()) + @"""";
            }

            if (obj is char)
            {
                return @"""" + obj + @"""";
            }

            if (obj.GetType().IsPrimitive)
            {
                return (string)Convert.ChangeType(obj, typeof(string), CultureInfo.InvariantCulture);
            }

            if (obj is DateTime)
            {
                return SerializeDateTime(obj);
            }

            if (obj is Guid)
            {
                return @"""" + obj + @"""";
            }

            if (obj is Uri)
            {
                return @"""" + obj + @"""";
            }

            return SerializeComplexType(obj);
        }

        private static string SerializeDateTime(object o)
        {
            DateTime d = (DateTime)o;
            long ticks = (d.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            return @"""\/Date(" + ticks + @")\/""";
        }


        private static string EscapeString(string src) => src;

        private string SerializeComplexType(object o)
        {
            StringBuilder s = new StringBuilder("{");

            if (o is IDictionary || o is IEnumerable<KeyValuePair<string, object>>)
            {
                SerializeDictionary(o, s);
            }
            else
            {
                SerializeProperties(o, s);
            }

            s.Append("}");

            return s.ToString();
        }

        private void SerializeProperties(object o, StringBuilder s)
        {
            Type type = o.GetType();
            IEnumerable<GetterMember> members = GetMembers(type);

            if (UseTypeInfo && ((type.BaseType != typeof(object) && type.BaseType != null) || type.GetInterfaces().Any()))
            {
                // emit type info
                s.Append(@"""");
                s.Append(TypeInfoPropertyName);
                s.Append(@""":""");
                s.Append(type.Name);
                s.Append(@""",");
            }

            foreach (var member in members)
            {
                object val = null;

                try
                {
                    val = member.Get(o);
                }
                catch
                {

                }

                if (val != null && (member.DefaultValue == null || !val.Equals(member.DefaultValue)))
                {
                    string v = Serialize(val);
                    s.Append(@"""");
                    s.Append(member.Name);
                    s.Append(@""":");
                    s.Append(v);
                    s.Append(",");
                }
            }

            if (s.Length > 0 && s[s.Length - 1] == ',') s.Remove(s.Length - 1, 1);
        }

        private void SerializeDictionary(object o, StringBuilder s)
        {
            IEnumerable<KeyValuePair<string, object>> kvps;
            IDictionary dict = o as IDictionary;
            if (dict != null)
                kvps = dict.Keys.Cast<object>().Select(k => new KeyValuePair<string, object>(k.ToString(), dict[k]));
            else
                kvps = (IEnumerable<KeyValuePair<string, object>>)o;

            // work around MonoTouch Full-AOT issue
            List<KeyValuePair<string, object>> kvpList = kvps.ToList();
            kvpList.Sort((e1, e2) => string.Compare(e1.Key, e2.Key, StringComparison.OrdinalIgnoreCase));

            foreach (KeyValuePair<string, object> kvp in kvpList)
            {
                s.Append(@"""");
                s.Append(kvp.Key);
                s.Append(@""":");
                s.Append(Serialize(kvp.Value));
                s.Append(",");
            }

            if (s.Length > 0 && s[s.Length - 1] == ',')
                s.Remove(s.Length - 1, 1);
        }

        class GetterMember
        {
            public string Name { get; set; }
            public Func<object, object> Get { get; set; }
            public object DefaultValue { get; set; }
        }

        private Dictionary<Type, GetterMember[]> MembersCache = new Dictionary<Type, GetterMember[]>();

        private GetterMember[] GetMembers(Type type)
        {
            GetterMember[] members;

            if (!MembersCache.TryGetValue(type, out members))
            {
                IEnumerable<GetterMember> props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
                    .Where(p => p.CanWrite)
                    .Select(p => BuildGetterMember(p));

                IEnumerable<GetterMember> fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
                    .Select(f => BuildGetterMember(f));

                members = props.Concat(fields).OrderBy(g => g.Name, StringComparer.OrdinalIgnoreCase).ToArray();

                MembersCache[type] = members;
            }

            return members;
        }

        private static GetterMember BuildGetterMember(PropertyInfo p)
        {
            DefaultValueAttribute defaultAttribute = p.GetCustomAttributes(typeof(DefaultValueAttribute), true).FirstOrDefault() as DefaultValueAttribute;

            return new GetterMember
            {
                Name = p.Name,
                Get = (Func<object, object>)(o => p.GetValue(o, null)),
                DefaultValue = defaultAttribute != null ? defaultAttribute.Value : GetDefaultValueForType(p.PropertyType)
            };
        }

        private static GetterMember BuildGetterMember(FieldInfo f)
        {
            DefaultValueAttribute defaultAttribute = f.GetCustomAttributes(typeof(DefaultValueAttribute), true).FirstOrDefault() as DefaultValueAttribute;
            return new GetterMember
            {
                Name = f.Name,
                Get = (o => f.GetValue(o)),
                DefaultValue = defaultAttribute != null ? defaultAttribute.Value : GetDefaultValueForType(f.FieldType)
            };
        }

        private static object GetDefaultValueForType(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}