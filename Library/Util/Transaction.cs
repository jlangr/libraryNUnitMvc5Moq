using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Util
{
    public class Transaction
    {
        public DateTime Date { get; private set; }
        public int Shares { get; private set;}
        public string Symbol { get; private set; }

        public Transaction(string symbol, int shares, DateTime date)
        {
            Symbol = symbol;
            Shares = shares;
            Date = date;
        }
    }
}