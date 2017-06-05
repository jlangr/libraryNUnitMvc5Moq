using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.POS
{
    public class Register
    {
        private IList<Purchase> purchases = new List<Purchase>();
        private decimal total;

        public void Purchase(string description, decimal amount)
        {
            purchases.Add(new Purchase { Description = description, Amount = amount });
        }

        public void CompleteSale()
        {
            total = 0.0m;
            foreach (Purchase purchase in purchases)
            {
                var message = "item:" + purchase.Description;
                total += purchase.Amount;
                DisplayDevice.AppendMessage(message);
            }
        }
    }
}