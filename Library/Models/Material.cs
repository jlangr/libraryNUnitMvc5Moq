namespace Library.Models
{
    public class Material
    {
        public string Director { get { return Author; } }

        public CheckoutPolicy CheckoutPolicy { get; set; }
        public string Title { get; set; }
        public string Classification { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }
    }
}
