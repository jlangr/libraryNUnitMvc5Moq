using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Util
{
    public class Portfolio
    {
        public Portfolio() {
            Holdings = new Dictionary<string,int>();
        }
        private IDictionary<string,int> Holdings { get; set; }
        public bool IsEmpty { get { return UniqueSymbolCount == 0;  } }
        public int UniqueSymbolCount { get { return Holdings.Count; } }

        public DateTime DateOfLastTransaction { get; private set; }

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

        private void Transact(string symbol, int shares)
        {
            DateOfLastTransaction = TimeService.Now;
            Holdings[symbol] = Shares(symbol) + shares;
        }

        public int Shares(string symbol)
        {
            if (!Holdings.ContainsKey(symbol)) return 0;
            return Holdings[symbol];
        }
    }
}