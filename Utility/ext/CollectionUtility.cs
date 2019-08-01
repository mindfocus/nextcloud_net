using System.Collections.Generic;

namespace ext
{
    public static class CollectionUtility
    {
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }
        public static bool IsNotEmpty<T>(this ICollection<T> collection)
        {
            return !collection.IsEmpty();
        }
        public static bool HasMethod(object objectToCheck, string methodName)
        {
            var a = new List<string>();
            var type = objectToCheck.GetType();
            return type.GetMethod(methodName) != null;
        }
    }
}