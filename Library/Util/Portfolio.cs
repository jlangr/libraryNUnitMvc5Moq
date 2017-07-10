using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Util
{
    public class Portfolio
    {
        public Portfolio()
        {
            Transactions = new List<Transaction>();
            Holdings = new Dictionary<string, StockHolding>();
        }

        public bool IsEmpty { get { return UniqueSymbolCount == 0; } }

        private IDictionary<string, StockHolding> Holdings { get; set; }

        public int UniqueSymbolCount
        {
            get { return Holdings.Count(); }
        }

        private IList<Transaction> Transactions { get; set; }

        public DateTime DateOfLastTransaction
        {
            get; private set;
        }

        public DateTime LastTransactionDate(string symbol)
        {
            return HoldingForSymbol(symbol).LastTransactionDate();
        }

        private void Transact(string symbol, int shares)
        {
            var now = TimeService.Now;
            DateOfLastTransaction = now;
            var transaction = new Transaction(symbol, shares, now);
            // TODO delete me ASAP
            Transactions.Add(transaction);

            HoldingForSymbol(symbol).Add(transaction);
        }

        private StockHolding HoldingForSymbol(string symbol)
        {
            if (Holdings.ContainsKey(symbol))
                return Holdings[symbol];

            var holding = new StockHolding(symbol);
            Holdings[symbol] = holding;
            return holding;
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
            return HoldingForSymbol(symbol).Shares;
        }
    }
}