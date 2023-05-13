using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Beryl.Util;

namespace Beryl.Refmatic
{
    public static class RefmaticParams
    {
        public const BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static class GenericKey
        {
            const string Empty = "";
            const string Pattern = @".{1,}[^0-9]";
            public static string Replace(string input)
            {
                return Regex.Replace(input, Pattern, Empty);
            }
        }

        public static readonly List<Type> TupleTypes = new List<Type>
        {
            typeof(SerializableTuple<,>),
            typeof(SerializableTuple<,,>),
            typeof(SerializableTuple<,,,>),
        };
    }
}
