namespace Library.Models
{
    public class MovieCheckoutPolicy : CheckoutPolicy
    {
        public const decimal DailyFineBasis = 1.00m;
        public const decimal PenaltyAmount = 2.00m;
        public override int MaximumCheckoutDays()
        {
            return 7;
        }
        public override decimal FineAmount(int daysLate)
        {
            return PenaltyAmount + daysLate * DailyFineBasis;
        }
    }
}
