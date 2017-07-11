using System;
using System.Collections.Generic;

namespace Library.VideoStore
{
    public class Customer
    {
        private readonly string name;
        private readonly IList<Rental> rentals = new List<Rental>();
        readonly string EOL = Environment.NewLine;
        private int frequentRenterPoints;
        private decimal totalPrice;

        public Customer(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        public void Add(Rental rental)
        {
            rentals.Add(rental);
            CacheIntoOtherwiseCalculatedTotals(rental);
        }

        private void CacheIntoOtherwiseCalculatedTotals(Rental rental)
        {
            // this is optimized caching for performance purposes!
            frequentRenterPoints += FrequentRenterPoints(rental);
            totalPrice += Price(rental);
        }

        public string Statement()
        {
            var result = Header();
            foreach (var rental in rentals)
                result += Detail(rental);
            result += TotalPriceFooter();
            result += TotalFreqRenterPointsFooter();
            return result;
        }
        private int TotalFrequentRenterPoints()
        {
            return frequentRenterPoints;
        }
        private decimal CalculateTotalPrice()
        {
            return totalPrice;
        }

        private string TotalFreqRenterPointsFooter()
        {
            return "You earned " + TotalFrequentRenterPoints() + " frequent renter points" + EOL;
        }
        private string TotalPriceFooter()
        {
            return "You owed " + CalculateTotalPrice().ToString("N2") + EOL;
        }

        private string Detail(Rental rental)
        {
            return "\t" + rental.Movie.Title + "\t" + Price(rental).ToString("N2") + Environment.NewLine;
        }

        private string Header()
        {
            return "Rental Record for " + Name + Environment.NewLine;
        }

        private int FrequentRenterPoints(Rental rental)
        {
            var frequentRenterPoints = 1;
            if (rental.Movie.PriceCode == Movie.NewRelease && rental.DaysRented > 1)
                frequentRenterPoints++;
            return frequentRenterPoints;
        }

        private decimal Price(Rental rental)
        {
            var price = 0m;
            switch (rental.Movie.PriceCode)
            {
                case Movie.Regular:
                    price += 2;
                    if (rental.DaysRented > 2)
                        price += (rental.DaysRented - 2) * 1.5m;
                    break;
                case Movie.NewRelease:
                    price += rental.DaysRented * 3m;
                    break;
                case Movie.Childrens:
                    price += 1.5m;
                    if (rental.DaysRented > 3)
                        price += (rental.DaysRented - 3) * 1.5m;
                    break;
            }
            return price;
        }
    }
}
