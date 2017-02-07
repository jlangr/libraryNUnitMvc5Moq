using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Util
{
    public static class ListUtil
    {
        // this could be done with a join but not (easily?) for the generic version
        public static string ToString<T>(IList<T> list)
        {
            var builder = new StringBuilder();
            foreach (var each in list)
            {
                if (builder.Length > 0)
                    builder.Append(",");
                builder.Append(each);
            }
            return builder.ToString();
        }
    }
}
