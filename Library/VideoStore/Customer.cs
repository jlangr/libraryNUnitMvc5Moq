using System;
using System.Collections.Generic;

namespace Library.VideoStore
{
    public class Customer
    {
        private readonly string name;
        private readonly IList<Rental> rentals = new List<Rental>();
        private int frequentRenterPoints;

        public Customer(string name)
        {
            this.name = name;
        }

        public void Add(Rental rental)
        {
            rentals.Add(rental);
        }

        public string Name
        {
            get { return name; }
        }

        public string Statement()
        {
            var totalAmount = 0.0m;
            var result = "Rental Record for " + Name + Environment.NewLine;

            foreach (var each in rentals)
            {
                decimal thisAmount = 0;

                // determines the amount for each line
                switch (each.Movie.PriceCode)
                {
                    case Movie.Regular:
                        thisAmount += 2;
                        if (each.DaysRented > 2)
                            thisAmount += (each.DaysRented - 2) * 1.5m;
                        break;
                    case Movie.NewRelease:
                        thisAmount += each.DaysRented * 3m;
                        break;
                    case Movie.Childrens:
                        thisAmount += 1.5m;
                        if (each.DaysRented > 3)
                            thisAmount += (each.DaysRented - 3) * 1.5m;
                        break;
                }

                frequentRenterPoints++;

                if (each.Movie.PriceCode == Movie.NewRelease
                        && each.DaysRented > 1)
                    frequentRenterPoints++;

                result += "\t" + each.Movie.Title + "\t"
                                    + thisAmount.ToString("N2") + Environment.NewLine;
                totalAmount += thisAmount;
            }

            result += "You owed " + totalAmount.ToString("N2") + 
                Environment.NewLine;
            result += "You earned " + 
                frequentRenterPoints + " frequent renter points" + 
                Environment.NewLine;

            return result;
        }
    }
}
