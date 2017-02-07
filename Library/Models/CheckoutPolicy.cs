using System;

namespace Library.Models
{
    [Serializable]
    public abstract class CheckoutPolicy
    {
        public abstract int MaximumCheckoutDays();
        public abstract decimal FineAmount(int daysLate);

        public int DaysLate(DateTime checkoutDate, DateTime checkinDate)
        {
            return DaysLate(checkoutDate, checkinDate, this.MaximumCheckoutDays());
        }

        public int DaysLate(DateTime checkoutDate, DateTime checkinDate, int maxCheckoutDays)
        {
            var span = checkinDate - checkoutDate;
            if (span.Days <= maxCheckoutDays)
                return 0;
            return span.Days - maxCheckoutDays;
        }

        public decimal FineAmount(DateTime checkoutDate, DateTime checkinDate)
        {
            return FineAmount(DaysLate(checkoutDate, checkinDate, MaximumCheckoutDays()));
        }
    }
}
