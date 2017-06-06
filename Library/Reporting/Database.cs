using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.Models;

namespace Library.Reporting
{
    public class Database
    {
        internal static IList<Material> SelectMany(string sql)
        {
            throw new Exception("invalid database configuration");
        }
    }
}