﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace Library.Util
{
    public class StockHolding
    {
        public StockHolding(string symbol)
        {
            Symbol = symbol;
            Transactions = new List<Transaction>();
        }

        public string Symbol { get; set; }
        private IList<Transaction> Transactions { get; set; }

        public void Add(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        internal DateTime LastTransactionDate()
        {
            return Transactions
                .Reverse()
                .First()
                .Date;
        }
    }
}