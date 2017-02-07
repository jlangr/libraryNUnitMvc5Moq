using System.Collections.Generic;

namespace Library.Util
{
    public static class CollectionUtil
    {
        public delegate bool Match(object obj, object value);

        public static T Find<T>(ICollection<T> elements, Match match, object value)
        {
            foreach (var each in elements)
                if (match(each, value))
                    return each;
            return default(T);
        }

        public static T[] ToArray<T>(ICollection<T> collection)
        {
            var array = new T[collection.Count];
            collection.CopyTo(array, 0);
            return array;
        }
    }
}
