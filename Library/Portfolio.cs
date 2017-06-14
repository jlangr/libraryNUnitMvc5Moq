using System;

namespace Library
{
    public class Portfolio
    {
        public Portfolio() { IsEmpty = true; }
        public void Purchase(string symbol, int shares)
        {
            IsEmpty = false;
        }
        public bool IsEmpty { get; set; }
    }
}