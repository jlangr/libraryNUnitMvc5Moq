using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Util
{
    public interface IStockService
    {
        decimal Price(string symbol);
    }
}