using System.Collections.Generic;

namespace Library.Models
{
    public class MasterClassificationService : IClassificationService
    {
        private static readonly IDictionary<string, Material> Materials = new Dictionary<string, Material>();

        public void DeleteAllBooks()
        {
            Materials.Clear();
        }

        public virtual string Classification(string isbn)
        {
            return ""; // would require external lookup
        }

        public void AddMovie(string classification, string title, string director, string year)
        {
            Materials[classification] = new Material
            {
                Classification = classification,
                Title = title,
                Author = director,
                Year = year,
                CheckoutPolicy = CheckoutPolicies.MovieCheckoutPolicy
            };
        }

        public void AddBook(string classification, string title, string author, string year)
        {
            Materials[classification] = new Material
            {
                Classification = classification,
                Title = title,
                Author = author,
                Year = year,
                CheckoutPolicy = CheckoutPolicies.BookCheckoutPolicy
            };
        }

        public Material Retrieve(string classification)
        {
            if (!Materials.ContainsKey(classification))
                return null;
            return Materials[classification];
        }
    }
}
