using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

namespace Application.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        ///     Helper method to get constant values of a class
        /// </summary>
        public static string[] GetConstants(this Type type)
        {
            ArrayList constants = new ArrayList();

            FieldInfo[] fieldInfos = type.GetFields(
                // Gets all public and static fields and non public e.g internal

                BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic |
                // This tells it to get the fields from all base types as well

                BindingFlags.FlattenHierarchy);

            // Go through the list and only pick out the constants
            foreach (FieldInfo fi in fieldInfos)
                // IsLiteral determines if its value is written at 
                //   compile time and not changeable
                // IsInitOnly determine if the field can be set 
                //   in the body of the constructor
                // for C# a field which is readonly keyword would have both true 
                //   but a const field would have only IsLiteral equal to true
                if (fi.IsLiteral && !fi.IsInitOnly)
                    constants.Add(fi.GetValue(null));

            // Return an array of FieldInfos
            return (string[])constants.ToArray(typeof(string));
        }

        // Ref: https://stackoverflow.com/questions/972307/how-to-loop-through-all-enum-values-in-c
        // https://stackoverflow.com/questions/67401524/get-enum-constant-values-as-list-of-integers
        public static List<int> GetEnumDataTypeValues(this Type type)
        {
            return Enum.GetValues(type).Cast<int>().ToList();
        }
        //public static List<int> GetEnumDataTypeValues<T>()
        //      {
        //          return Enum.GetValues(typeof(T)).Cast<int>().ToList();
        //      }

        // Ref: https://stackoverflow.com/questions/14971631/convert-an-enum-to-liststring
        public static List<string> GetEnumDataTypes<T>()
        {
            return Enum.GetNames(typeof(T)).ToList();
        }

        // Ref: https://stackoverflow.com/questions/40639126/list-of-enum-values-from-long-bit-mask
        public static List<int> UnfoldBitmask<T>(this int bitMask)
        {
            return Enum.GetValues(typeof(T)).Cast<int>().Where(m => (bitMask & m) > 0).Cast<int>().ToList();
        }

        public static object GetInstanceOf(this Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type)!;

            // Type without parameterless constructor
            return FormatterServices.GetUninitializedObject(type);
        }

        // Ref: https://code-maze.com/csharp-get-list-of-properties/
        public static List<string> GetPropertyNames(this Type type, BindingFlags binding = BindingFlags.Public | BindingFlags.Instance)
        {
            return type.GetProperties(binding)?.Select(propertyInfo => propertyInfo.Name)?.ToList() ?? new();
        }

        public static bool CheckIfPropertyNameExist(this Type type, string Name, BindingFlags binding = BindingFlags.Public | BindingFlags.Instance)
        {
            return type.GetPropertyNames(binding).Exists(x => x == Name);
        }

        public static object? GetPropertyValByName(this Type type, object src, string Name)
        {
            return type.GetType()?.GetProperty(Name)?.GetValue(src, null);
        }
    }
}
