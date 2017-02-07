using System;

namespace Library.Models
{
    [Serializable]
    public class BookCheckoutPolicy: CheckoutPolicy
    {
        public const decimal DailyFineBasis = 0.10m;
        public override int MaximumCheckoutDays()
        {
            return 21;
        }

        public override decimal FineAmount(int daysLate)
        {
            return daysLate * DailyFineBasis;
        }
    }
}
