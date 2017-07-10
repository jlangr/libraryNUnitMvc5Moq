﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Util
{
    public class Portfolio
    {
        public Portfolio() {
            Holdings = new Dictionary<string,int>();
            Transactions = new List<Transaction>();
        }
        private IDictionary<string,int> Holdings { get; set; }
        public bool IsEmpty { get { return UniqueSymbolCount == 0;  } }
        public int UniqueSymbolCount { get { return Holdings.Count; } }

        private IList<Transaction> Transactions { get; set; }

        public DateTime DateOfLastTransaction
        {
            get
            {
                return Transactions
                    .Reverse()
                    .First()
                    .Date;
            }
        }

        public DateTime LastTransactionDate(string symbol)
        {
            return Transactions
                .Reverse()
                .First(t => t.Symbol == symbol)
                .Date;
        }

        private void Transact(string symbol, int shares)
        {
            Holdings[symbol] = Shares(symbol) + shares;
            Transactions.Add(new Transaction(symbol, shares, TimeService.Now));
        }

        public void Purchase(string symbol, int shares)
        {
            ThrowWhenNonPositiveShares(shares);
            Transact(symbol, shares);
        }

        private void ThrowWhenNonPositiveShares(int shares)
        {
            if (shares <= 0) throw new InvalidTransactionException();
        }

        public void Sell(string symbol, int shares)
        {
            ThrowWhenSellingTooManyShares(symbol, shares);
            Transact(symbol, -shares);
        }

        private void ThrowWhenSellingTooManyShares(string symbol, int shares)
        {
            if (shares > Shares(symbol))
                throw new InvalidTransactionException("symbol not held");
        }

        public int Shares(string symbol)
        {
            return Transactions
                .Where(t => t.Symbol == symbol)
                .Select(t => t.Shares)
                .Sum();
        }
    }
}