using System;
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

        public int Shares
        {
            get
            {
                return Transactions
                    .Select(t => t.Shares)
                    .Sum();
            }
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