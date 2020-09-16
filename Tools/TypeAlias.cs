using System;
using System.Collections.Generic;
using System.Linq;

namespace Nerdomat.Tools
{
    public static class TypeAlias
    {
        static Dictionary<Type, string> _typeAlias = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            { typeof(object), "object" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(string), "string" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            { typeof(void), "void" }
        };

        public static string TypeNameOrAlias(this Type type)
        {
            // Handle nullable value types
            var nullbase = Nullable.GetUnderlyingType(type);
            if (nullbase != null)
                return TypeNameOrAlias(nullbase) + "?";

            // Handle arrays
            if (type.BaseType == typeof(System.Array))
                return TypeNameOrAlias(type.GetElementType()) + "[]";

            // Lookup alias for type
            if (_typeAlias.TryGetValue(type, out string alias))
                return alias;

            // Handle generic types
            if (type.IsGenericType)
            {
                string name = type.Name.Split('`').FirstOrDefault();
                IEnumerable<string> parms =
                    type.GetGenericArguments()
                    .Select(a => type.IsConstructedGenericType ? TypeNameOrAlias(a) : a.Name);
                return $"{name}<{string.Join(",", parms)}>";
            }

            // Default to CLR type name
            return type.Name;
        }
    }
}