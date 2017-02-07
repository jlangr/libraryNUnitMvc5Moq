namespace Library.VideoStore
{
    public class Rental
    {
        public Rental(Movie movie, int daysRented)
        {
            Movie = movie;
            DaysRented = daysRented;
        }

        public int DaysRented { get; private set; }
        public Movie Movie { get; private set; }
    }
}
